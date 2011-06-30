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
        string[] paths = new string[]{"images", "form\\images", "layout\\images"};
        string[] excludes = new string[] { "error.png", "dnd.png", "Thumbs.db"};
        string pathToLess = "variables.less";
        string demoImage = "form/images/checkboxRadioButtonStates.png";
        Bitmap orginBmp;

        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            txtFolder.Text = folderBrowserDialog1.SelectedPath;
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
                    string[] splitPath = file.Split('\\');
                    bool excluded = false;
                    try
                    {
                        string[] extension = file.Split('.');
                        if (extension[extension.Length - 1] == "gif")
                        {
                            log("xxx GIF File: " + splitPath[splitPath.Length - 1] + " - excluding.");
                            excluded = true;
                        }
                    }
                    catch(Exception)
                    {
                        excluded = true;
                        log("xxx Unknown file type: " + splitPath[splitPath.Length - 1] + " - excluding.");
                    }
                    foreach (string exclude in excludes)
                    {
                        if (splitPath[splitPath.Length - 1] == exclude)
                        {
                            log("xxx " + splitPath[splitPath.Length - 1] + " excluded.");
                            excluded = true;
                        }
                    }
                    if (!excluded)
                    {
                        log("=== " + splitPath[splitPath.Length - 1]);
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

        private void log(string msg)
        {
            txtLog.AppendText(msg + Environment.NewLine);
            txtLog.ScrollToCaret();
        }

        private Bitmap processBitmap(Bitmap bmp)
        {
            return processBitmap(bmp, trkH.Value, trkS.Value, trkL.Value, chkInvert.Checked);
        }

        private Bitmap processBitmap(Bitmap bmp, int h, double s, double l, bool invert)
        {
            bmp = changeHSL(bmp, h, s, l);
            if (invert)
                bmp = invertColors(bmp);
            return bmp;
        }

        private Bitmap changeHSL(Bitmap bmp, int h, double s, double l)
        {
            s = Math.Round(s / 100, 2);
            l = Math.Round(l / 100, 2);
            HueModifierRelative hue = new HueModifierRelative(h);
            SaturationCorrection saturation = new SaturationCorrection(s);
            BrightnessCorrection bright = new BrightnessCorrection(l);
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
            p.Disposed += new EventHandler(Process_Exited);
            StreamReader oReader2 = p.StandardOutput;
            string sRes = oReader2.ReadToEnd();

			StreamReader oReader3 = p.StandardError;
			string sRes1 = oReader3.ReadToEnd();

            oReader2.Close();
			oReader3.Close();
            log(sRes);
			log(sRes1);
            resetSliders();
        }

        void Process_Exited(object sender, EventArgs e)
        {
            log("Compile complete.");
        }

        private void resetSliders()
        {
            trkH.Value = 0;
            trkS.Value = 0;
            trkL.Value = 0;
            txtH.Text = "0";
            txtS.Text = "0";
            txtL.Text = "0";
            chkInvert.Checked = false;
            FileStream fs = new FileStream(Path.Combine(txtFolder.Text, "form/images/checkboxRadioButtonStates.png"), FileMode.Open);
            Image imgPhoto = Image.FromStream(fs);
            pictureBox1.Image = new Bitmap(fs);
            fs.Close();
            orginBmp = new Bitmap(pictureBox1.Image);
        }

        private void trkH_Scroll(object sender, EventArgs e)
        {
            pictureBox1.Image = (Image)processBitmap((Bitmap)orginBmp.Clone());
            txtH.Text = trkH.Value.ToString();
        }

        private void trkS_Scroll(object sender, EventArgs e)
        {
            pictureBox1.Image = (Image)processBitmap((Bitmap)orginBmp.Clone()); 
            txtS.Text = trkS.Value.ToString();
        }

        private void trkL_Scroll(object sender, EventArgs e)
        {
            pictureBox1.Image = (Image)processBitmap((Bitmap)orginBmp.Clone()); 
            txtL.Text = trkL.Value.ToString();
        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                trkL.Value = Int32.Parse(txtL.Text);
                trkL_Scroll(sender, e);
            }
            catch (Exception) { }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                trkS.Value = Int32.Parse(txtS.Text);
                trkS_Scroll(sender, e);
            }
            catch (Exception) { }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                trkH.Value = Int32.Parse(txtH.Text);
                trkH_Scroll(sender, e);
            }
            catch (Exception) { }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void chkLess_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtFolder_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(txtFolder.Text, demoImage)))
            {
                trkH.Enabled = true;
                trkS.Enabled = true;
                trkL.Enabled = true;
                txtH.Enabled = true;
                txtS.Enabled = true;
                txtL.Enabled = true;
                chkInvert.Enabled = true;
                btnStart.Enabled = true;
                resetSliders();
            }
            else
            {
                pictureBox1.Image = null;
                trkH.Enabled = false;
                trkS.Enabled = false;
                trkL.Enabled = false;
                txtH.Enabled = false;
                txtS.Enabled = false;
                txtL.Enabled = false;
                chkInvert.Enabled = false;
                btnStart.Enabled = false;
            }
        }

        private void chkInvert_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = (Image)processBitmap((Bitmap)orginBmp.Clone());
        }
    }
}
