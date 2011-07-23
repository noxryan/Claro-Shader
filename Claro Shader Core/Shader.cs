/*Copyright 2011 Ryan Schlesinger. All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are
permitted provided that the following conditions are met:

   1. Redistributions of source code must retain the above copyright notice, this list of
      conditions and the following disclaimer.

   2. Redistributions in binary form must reproduce the above copyright notice, this list
      of conditions and the following disclaimer in the documentation and/or other materials
      provided with the distribution.

THIS SOFTWARE IS PROVIDED BY Ryan Schlesinger ``AS IS'' AND ANY EXPRESS OR IMPLIED
WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL Ryan Schlesinger OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those of the
authors and should not be interpreted as representing official policies, either expressed
or implied, of Ryan Schlesinger.*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AForge;
using AForge.Imaging.Filters;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Claro_Shader_Core
{
    public static class Shader
    {
        private static string[] paths = new string[] { "images", Path.Combine("form", "images"), Path.Combine("layout", "images") };
        private static string pathToVariables = "variables.less";
        private static List<string> imgExcludes = new List<string>();
        private static List<string> cssExcludes = new List<string>();
        private static string claroPath = "";
        private static int h = 0;
        private static int s = 0;
        private static int l = 0;
        private static bool invert = false;
        private static bool keepBlacks = true;
        private static bool keepGrays = false;
        private static int grayTolerance = 0;

#region Public Properties

        /// <summary>
        /// List of images that will be excluded when recoloring.
        /// </summary>
        public static List<string> ImgExcludes
        {
            set { imgExcludes= value; }
            get { return imgExcludes; }
        }

        /// <summary>
        /// List of CSS .less variables that will be excluded when recoloring.
        /// </summary>
        public static List<string> CssExcludes
        {
            set { cssExcludes = value; }
            get { return cssExcludes; }
        }

        /// <summary>
        /// File path to the Claro folder.
        /// </summary>
        public static string ClaroPath
        {
            set { claroPath = value; }
            get { return claroPath; }
        }

        /// <summary>
        /// Hue adjustment. Default is 0 (No Change).
        /// </summary>
        public static int H
        {
            set { h = value; }
            get { return h; }
        }

        /// <summary>
        /// Saturation adjustment. Default is 0 (No Change).
        /// </summary>
        public static int S
        {
            set { s = value; }
            get { return s; }
        }

        /// <summary>
        /// Luminosity adjustment. Default is 0 (No Change).
        /// </summary>
        public static int L
        {
            set { l = value; }
            get { return l; }
        }

        /// <summary>
        /// Invert all colors. Default is false (No Change).
        /// </summary>
        public static bool Invert
        {
            set { invert = value; }
            get { return invert; }
        }

        /// <summary>
        /// Ignore black and white from HSL adjustments. This should generally be used when adjusting luminosity. Default is true.
        /// </summary>
        public static bool KeepBlacks
        {
            set { keepBlacks = value; }
            get { return keepBlacks; }
        }

        /// <summary>
        /// Ignore all shades of gray from HSL adjustments. This should generally be used when adjusting luminosity. Default is false.
        /// </summary>
        public static bool KeepGrays
        {
            set { keepGrays = value; }
            get { return keepGrays; }
        }

        /// <summary>
        /// Setting this above 0 will also ignore off shades of gray. For Claro generally keep this at 0. Default is 0.
        /// </summary>
        public static int GrayTolerance
        {
            set { grayTolerance = value; }
            get { return grayTolerance; }
        }

#endregion

        /// <summary>
        /// Delegate for log messages.
        /// </summary>
        /// <param name="msg">Log message</param>
        public delegate void LogEventHandler(string msg);

        /// <summary>
        /// Event handler for log messages.
        /// </summary>
        public static event LogEventHandler Log;

        /// <summary>
        /// Starts the recoloring process. Edits all images and variables.less.
        /// </summary>
        public static void Start()
        {
            Log("Starting...");
            foreach (string path in paths)
            {
                string fullPath = Path.Combine(claroPath, path);
                Log(Environment.NewLine + "Path: " + fullPath);
                string[] files = Directory.GetFiles(fullPath);
                foreach (string file in files)
                {
                    bool excluded = false;
                    foreach (string imgExclude in imgExcludes)
                    {
                        if (Path.GetFileName(file) == imgExclude)
                        {
                            Log("xxx " + Path.GetFileName(file) + " excluded.");
                            excluded = true;
                        }
                    }
                    if (!excluded)
                    {
                        Log("=== " + Path.GetFileName(file));
                        FileStream fs = new FileStream(file, FileMode.Open);
                        Image imgPhoto = Image.FromStream(fs);
                        Bitmap bmp = new Bitmap(fs);
                        fs.Close();
                        bmp = ProcessBitmap(bmp);
                        bmp.Save(file);
                    }
                }
            }
            editLess();
            using (StreamWriter outfile = new StreamWriter(Path.Combine(claroPath, "Claro-Shader_Colors.txt"), true))
            {
                outfile.Write("Date: " + DateTime.Now + Environment.NewLine);
                outfile.Write("Hue: " + h.ToString() + Environment.NewLine);
                outfile.Write("Saturation: " + s.ToString() + Environment.NewLine);
                outfile.Write("Luminosity: " + l.ToString() + Environment.NewLine);
                outfile.Write("Invert: " + invert.ToString() + Environment.NewLine);
                outfile.Write("Keep Blacks: " + keepBlacks.ToString() + Environment.NewLine);
                outfile.Write("Keep Grays: " + keepGrays.ToString() + Environment.NewLine);
                outfile.Write("Gray Tolerance: " + grayTolerance.ToString() + Environment.NewLine);
            }
            Log(pathToVariables + " has been updated.");
        }

        /// <summary>
        /// Runs a bitmap through all filters.
        /// </summary>
        /// <param name="bmp">Bitmap to be processed.</param>
        /// <returns>Processed bitmap.</returns>
        public static Bitmap ProcessBitmap(Bitmap bmp)
        {
            return ProcessBitmap(bmp, h, s, l, invert, keepBlacks, keepGrays, grayTolerance);
        }

        /// <summary>
        /// Runs a bitmap through all filters.
        /// </summary>
        /// <param name="bmp">Bitmap for processing.</param>
        /// <param name="h">Hue adjustment.</param>
        /// <param name="s">Saturation adjustment.</param>
        /// <param name="l">Luminosity adjustment</param>
        /// <param name="invert">Invert all colors.</param>
        /// <param name="keepBW">Ignore black and white.</param>
        /// <param name="keepGray">Ignore Shades of gray.</param>
        /// <param name="grayTolerance">Tolerance for off shades of gray.</param>
        /// <returns>Processed bitmap.</returns>
        public static Bitmap ProcessBitmap(Bitmap bmp, int h, double s, double l, bool invert, bool keepBW, bool keepGray, int grayTolerance)
        {
            bmp = changeHSL(bmp, h, s, l, keepBW, keepGray, grayTolerance);
            if (invert)
                bmp = invertColors(bmp);
            return bmp;
        }

        private static Bitmap changeHSL(Bitmap bmp, int h, double s, double l, bool keepBW, bool keepGray, int grayTolerance)
        {
            s = Math.Round(s / 100, 2);
            l = Math.Round(l / 100, 2);
            HueModifierRelative hue = new HueModifierRelative(h, keepBW);
            SaturationCorrection saturation = new SaturationCorrection(s, keepBW, keepGray, grayTolerance);
            BrightnessCorrection bright = new BrightnessCorrection(l, keepBW, keepGray, grayTolerance);
            hue.ApplyInPlace(bmp);
            saturation.ApplyInPlace(bmp);
            bright.ApplyInPlace(bmp);
            return bmp;
        }

        private static Bitmap invertColors(Bitmap bmp)
        {
            Invert filter = new Invert();
            filter.ApplyInPlace(bmp);
            return bmp;
        }

        private static void editLess()
        {
            string newData = "";
            try
            {
                using (StreamReader sr = new StreamReader(Path.Combine(claroPath, pathToVariables)))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        bool excluded = false;
                        foreach (string cssExclude in cssExcludes)
                            if (line.Contains(cssExclude))
                                excluded = true;
                        if (!excluded)
                        {
                            Match m = Regex.Match(line, "#.*;");
                            while (m.Success)
                            {
                                string oldRGB = m.ToString().Trim(new char[] { ';' });
                                if (oldRGB != "")
                                {
                                    Bitmap tmp = new Bitmap(1, 1);
                                    tmp.SetPixel(0, 0, ColorTranslator.FromHtml(oldRGB));
                                    Color newColor = ProcessBitmap(tmp).GetPixel(0, 0);
                                    Log(oldRGB + " - " + ColorTranslator.ToHtml(newColor));
                                    line = line.Replace(oldRGB, ColorTranslator.ToHtml(newColor));
                                }
                                m = m.NextMatch();
                            }
                        }
                        newData += line + "\n";
                    }
                }
                using (StreamWriter outfile = new StreamWriter(Path.Combine(claroPath, pathToVariables)))
                {
                    outfile.Write(newData);
                }
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        /// <summary>
        /// Executes NodeJS and runs compile.js
        /// </summary>
        /// <returns></returns>
        public static bool BuildClaro()
        {
            return BuildClaro(new ProcessStartInfo("node", "compile.js"));
        }

       /// <summary>
       /// Executes NodeJS and runs compile.js
       /// </summary>
       /// <param name="nodeJS">ProcessStartInfo for NodeJS</param>
       /// <returns></returns>
        public static bool BuildClaro(ProcessStartInfo nodeJS)
        {
            Log(".less Compile Started.");
            nodeJS.CreateNoWindow = true;
            nodeJS.UseShellExecute = false;
            #if !DEBUG
                nodeJS.ErrorDialog = false;
            #endif
            nodeJS.RedirectStandardOutput = true;
            nodeJS.WorkingDirectory = claroPath;
            nodeJS.RedirectStandardError = true;

            Process p = Process.Start(nodeJS);

            StreamReader oReader2 = p.StandardOutput;
            string sRes = oReader2.ReadToEnd();

            StreamReader oReader3 = p.StandardError;
            string sRes1 = oReader3.ReadToEnd();

            oReader2.Close();
            oReader3.Close();
            Log(sRes);
            Log(sRes1);

            if (p.WaitForExit(5000))
            {
                Log("Compile complete.");
                return true;
            }
            else
            {
                Log("Compile Failed.");
                return false;
            }
        }
    }
}
