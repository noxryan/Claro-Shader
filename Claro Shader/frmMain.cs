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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using AForge;
using AForge.Imaging.Filters;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Claro_Shader
{
    public partial class frmMain : Form
    {
        string[] paths = new string[]{"images", @"form\images", @"layout\images"};
        string pathToLess = "variables.less";
        string excludeFile = @"Resources\excludes.txt";
        string demoImage = @"form\images\checkboxRadioButtonStates.png";
        List<string> excludes = new List<string>();
        Bitmap orginBmp;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            using (StreamReader sr = new StreamReader(excludeFile))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    excludes.Add(line);
                }
            }
        }

        private void log(string msg)
        {
            txtLog.AppendText(msg + Environment.NewLine);
            txtLog.ScrollToCaret();
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            dirDialog.ShowDialog();
            txtFolder.Text = dirDialog.SelectedPath;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            log("Starting...");
            foreach(string path in paths)
            {
                string fullPath = Path.Combine(txtFolder.Text, path);
                log(Environment.NewLine + "Path: " + fullPath);
                string[] files = Directory.GetFiles(fullPath);
                foreach (string file in files)
                {
                    bool excluded = false;
                    foreach (string exclude in excludes)
                    {
                        if (Path.GetFileName(file) == exclude)
                        {
                            log("xxx " + Path.GetFileName(file) + " excluded.");
                            excluded = true;
                        }
                    }
                    if (!excluded)
                    {
                        log("=== " + Path.GetFileName(file));
                        FileStream fs = new FileStream(file, FileMode.Open);
                        Image imgPhoto = Image.FromStream(fs);
                        Bitmap bmp = new Bitmap(fs);
                        fs.Close();
                        bmp = processBitmap(bmp);
                        bmp.Save(file);
                    }
                }
            }
            editLess();
            log(pathToLess + " has been updated.");
            buildClaro();
            this.Enabled = true;
        }

        private Bitmap processBitmap(Bitmap bmp)
        {
            return processBitmap(bmp, trkH.Value, trkS.Value, trkL.Value, chkInvert.Checked, chkBW.Checked, chkGray.Checked, (int)numGrayTolerance.Value);
        }

        private Bitmap processBitmap(Bitmap bmp, int h, double s, double l, bool invert, bool keepBW, bool keepGray, int grayTolerance)
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
                using (StreamReader sr = new StreamReader(Path.Combine(txtFolder.Text, pathToLess)))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Match m = Regex.Match(line, "#.*;");
                        foreach (Match match in m.Groups)
                        {
                            string oldRGB = match.ToString().Trim(new char[]{';'});
                            if (oldRGB != "")
                            {
                                Bitmap tmp = new Bitmap(1, 1);
                                tmp.SetPixel(0, 0, ColorTranslator.FromHtml(oldRGB));
                                Color newColor = processBitmap(tmp).GetPixel(0, 0);
                                log(oldRGB + " - " + ColorTranslator.ToHtml(newColor));
                                line = line.Replace(oldRGB, ColorTranslator.ToHtml(newColor));
                            }
                        }
                        newData += line + "\n";
                    }
                }
                using (StreamWriter outfile = new StreamWriter(Path.Combine(txtFolder.Text, pathToLess)))
                {
                    outfile.Write(newData);
                }
                if (chkLess.Checked)
                {
                    frmEditor editor = new frmEditor(Path.Combine(txtFolder.Text, pathToLess));
                    editor.ShowDialog();
                }
            }
            catch (Exception e)
            {
                log(e.Message);
            }
        }

        private void buildClaro()
        {
            log(".less Compile Started.");
            File.Copy("Resources\\compile.js", Path.Combine(txtFolder.Text, "compile.js"), true);
            ProcessStartInfo psi = new ProcessStartInfo(@"Resources\node\bin\node.exe", "compile.js \"" + Path.Combine(Application.StartupPath, @"Resources\node\lib\node_modules\less").Replace('\\', '/') + "\"");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            #if !DEBUG
                psi.ErrorDialog = false;
            #endif
            psi.RedirectStandardOutput = true;
            psi.WorkingDirectory = txtFolder.Text;
			psi.RedirectStandardError = true;

            Process p = Process.Start(psi);
            
            StreamReader oReader2 = p.StandardOutput;
            string sRes = oReader2.ReadToEnd();

			StreamReader oReader3 = p.StandardError;
			string sRes1 = oReader3.ReadToEnd();

            oReader2.Close();
			oReader3.Close();
            log(sRes);
			log(sRes1);

            using (StreamWriter outfile = new StreamWriter(Path.Combine(txtFolder.Text, "Claro-Shader_Colors.txt")))
            {
                outfile.Write("Hue: " + trkH.Value.ToString() + Environment.NewLine);
                outfile.Write("Saturation: " + trkS.Value.ToString() + Environment.NewLine);
                outfile.Write("Luminosity: " + trkL.Value.ToString() + Environment.NewLine);
                outfile.Write("Invert: " + chkInvert.Checked.ToString() + Environment.NewLine);
                outfile.Write("Keep Blacks: " + chkBW.Checked.ToString() + Environment.NewLine);
                outfile.Write("Keep Grays: " + chkGray.Checked.ToString() + Environment.NewLine);
                outfile.Write("Gray Tolerance: " + numGrayTolerance.Value.ToString());
            }

            resetSliders();

            p.WaitForExit(10000);
            log("Compile complete.");
        }

        private void resetSliders()
        {
            trkH.Value = 0;
            trkS.Value = 0;
            trkL.Value = 0;
            numH.Value = 0;
            numS.Value = 0;
            numL.Value = 0;
            chkInvert.Checked = false;
            chkBW.Checked = true;
            chkGray.Checked = false;
            FileStream fs = new FileStream(Path.Combine(txtFolder.Text, "form/images/checkboxRadioButtonStates.png"), FileMode.Open);
            Image imgPhoto = Image.FromStream(fs);
            pictureBox1.Image = new Bitmap(fs);
            fs.Close();
            orginBmp = new Bitmap(pictureBox1.Image);
        }

        private void trkH_Scroll(object sender, EventArgs e)
        {
            pictureBox1.Image = (Image)processBitmap((Bitmap)orginBmp.Clone());
            numH.Value = trkH.Value;
        }

        private void trkS_Scroll(object sender, EventArgs e)
        {
            pictureBox1.Image = (Image)processBitmap((Bitmap)orginBmp.Clone()); 
            numS.Value = trkS.Value;
        }

        private void trkL_Scroll(object sender, EventArgs e)
        {
            pictureBox1.Image = (Image)processBitmap((Bitmap)orginBmp.Clone()); 
            numL.Value = trkL.Value;
        }

        private void txtFolder_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(txtFolder.Text, demoImage)))
            {
                trkH.Enabled = true;
                trkS.Enabled = true;
                trkL.Enabled = true;
                numH.Enabled = true;
                numS.Enabled = true;
                numL.Enabled = true;
                chkInvert.Enabled = true;
                chkBW.Enabled = true;
                chkGray.Enabled = true;
                btnStart.Enabled = true;
                resetSliders();
            }
            else
            {
                pictureBox1.Image = null;
                trkH.Enabled = false;
                trkS.Enabled = false;
                trkL.Enabled = false;
                numH.Enabled = false;
                numS.Enabled = false;
                numL.Enabled = false;
                chkInvert.Enabled = false;
                chkBW.Enabled = false;
                chkGray.Enabled = false;
                btnStart.Enabled = false;
            }
        }

        private void chkInvert_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = (Image)processBitmap((Bitmap)orginBmp.Clone());
        }

        private void chkBW_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = (Image)processBitmap((Bitmap)orginBmp.Clone());
        }

        private void chkGray_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGray.Checked)
                numGrayTolerance.Enabled = true;
            else
                numGrayTolerance.Enabled = false;
            pictureBox1.Image = (Image)processBitmap((Bitmap)orginBmp.Clone());
        }

        private void numGrayTolerance_ValueChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = (Image)processBitmap((Bitmap)orginBmp.Clone());
        }

        private void numH_ValueChanged(object sender, EventArgs e)
        {
            trkH.Value = (int) numH.Value;
            trkH_Scroll(sender, e);
        }

        private void numS_ValueChanged(object sender, EventArgs e)
        {
            trkS.Value = (int) numS.Value;
            trkS_Scroll(sender, e);
        }

        private void numL_ValueChanged(object sender, EventArgs e)
        {
            trkL.Value = (int) numL.Value;
            trkL_Scroll(sender, e);
        }
    }
}
