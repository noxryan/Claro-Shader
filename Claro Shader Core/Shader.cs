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
    public class Shader
    {
        string[] paths = new string[] { "images", Path.Combine("form", "images"), Path.Combine("layout", "images") };
        string pathToLess = "variables.less";
        private List<string> imgExcludes = new List<string>();
        private List<string> cssExcludes = new List<string>();
        private string claroPath = "";
        private int h = 0;
        private int s = 0;
        private int l = 0;
        private bool invert = false;
        private bool keepBlacks = true;
        private bool keepGrays = false;
        private int grayTolerance = 0;

#region Public Properties

        public List<string> ImgExcludes
        {
            set { this.imgExcludes= value; }
            get { return this.imgExcludes; }
        }

        public List<string> CssExcludes
        {
            set { this.cssExcludes = value; }
            get { return this.cssExcludes; }
        }

        public string ClaroPath
        {
            set { this.claroPath = value; }
            get { return this.claroPath; }
        }

        public int H
        {
            set { this.h = value; }
            get { return this.h; }
        }

        public int S
        {
            set { this.s = value; }
            get { return this.s; }
        }

        public int L
        {
            set { this.l = value; }
            get { return this.l; }
        }

        public bool Invert
        {
            set { this.invert = value; }
            get { return this.invert; }
        }

        public bool KeepBlacks
        {
            set { this.keepBlacks = value; }
            get { return this.keepBlacks; }
        }

        public bool KeepGrays
        {
            set { this.keepGrays = value; }
            get { return this.keepGrays; }
        }

        public int GrayTolerance
        {
            set { this.grayTolerance = value; }
            get { return this.grayTolerance; }
        }

#endregion

        public delegate void LogEventHandler(string msg);
        public event LogEventHandler Log;

        public Shader()
        {

        }

       /* public Shader(string claroPath, int h, int s, int l, bool keepBlacks, bool keepGrays, int grayTolerance) : this()
        {
            this.claroPath = claroPath;
            this.h = h;
            this.s = s;
            this.l = l;
            this.keepBlacks = keepBlacks;
            this.keepGrays = keepGrays;
            this.grayTolerance = grayTolerance;
        }*/

        public void Start()
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
            Log(pathToLess + " has been updated.");
            buildClaro();
        }
       
        public Bitmap ProcessBitmap(Bitmap bmp)
        {
            return ProcessBitmap(bmp, h, s, l, invert, keepBlacks, keepGrays, grayTolerance);
        }

        public Bitmap ProcessBitmap(Bitmap bmp, int h, double s, double l, bool invert, bool keepBW, bool keepGray, int grayTolerance)
        {
            bmp = changeHSL(bmp, h, s, l, keepBW, keepGray, grayTolerance);
            if (invert)
                bmp = invertColors(bmp);
            return bmp;
        }

        private Bitmap changeHSL(Bitmap bmp, int h, double s, double l, bool keepBW, bool keepGray, int grayTolerance)
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

        private Bitmap invertColors(Bitmap bmp)
        {
            Invert filter = new Invert();
            filter.ApplyInPlace(bmp);
            return bmp;
        }

        private void editLess()
        {
            string newData = "";
            try
            {
                using (StreamReader sr = new StreamReader(Path.Combine(claroPath, pathToLess)))
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
                using (StreamWriter outfile = new StreamWriter(Path.Combine(claroPath, pathToLess)))
                {
                    outfile.Write(newData);
                }
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        private void buildClaro()
        {
            Log(".less Compile Started.");
            File.Copy("Resources\\compile.js", Path.Combine(claroPath, "compile.js"), true);
            ProcessStartInfo psi = new ProcessStartInfo(@"Resources\node\bin\node.exe", "compile.js \"" + Path.Combine(Environment.CurrentDirectory, @"Resources\node\lib\node_modules\less").Replace('\\', '/') + "\"");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            #if !DEBUG
                psi.ErrorDialog = false;
            #endif
            psi.RedirectStandardOutput = true;
            psi.WorkingDirectory = claroPath;
            psi.RedirectStandardError = true;

            Process p = Process.Start(psi);

            StreamReader oReader2 = p.StandardOutput;
            string sRes = oReader2.ReadToEnd();

            StreamReader oReader3 = p.StandardError;
            string sRes1 = oReader3.ReadToEnd();

            oReader2.Close();
            oReader3.Close();
            Log(sRes);
            Log(sRes1);

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

            p.WaitForExit(10000);
            Log("Compile complete.");
        }
    }
}
