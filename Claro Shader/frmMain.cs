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
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Claro_Shader_Core;

namespace Claro_Shader
{
    public partial class frmMain : Form
    {
        Shader shader;

        string imgExcludeFile = Path.Combine("Resources", "img_excludes.txt");
        string cssExcludeFile = Path.Combine("Resources", "css_excludes.txt");
        string pathToLess = "variables.less";
        string demoImage = @"form\images\checkboxRadioButtonStates.png";
        Bitmap orginBmp;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            shader = new Shader();
            shader.Log += new Shader.LogEventHandler(shader_Log);

            using (StreamReader sr = new StreamReader(imgExcludeFile))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    shader.ImgExcludes.Add(line);
                }
            }
            using (StreamReader sr = new StreamReader(cssExcludeFile))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    shader.CssExcludes.Add(line);
                }
            }
        }

        void shader_Log(string msg)
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
            shader.Start();
            resetSliders();
            this.Enabled = true;
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
            shader.H = trkH.Value;
            pictureBox1.Image = (Image)shader.ProcessBitmap((Bitmap)orginBmp.Clone());
            numH.Value = trkH.Value;
        }

        private void trkS_Scroll(object sender, EventArgs e)
        {
            shader.S = trkS.Value;
            pictureBox1.Image = (Image)shader.ProcessBitmap((Bitmap)orginBmp.Clone()); 
            numS.Value = trkS.Value;
        }

        private void trkL_Scroll(object sender, EventArgs e)
        {
            shader.L = trkL.Value;
            pictureBox1.Image = (Image)shader.ProcessBitmap((Bitmap)orginBmp.Clone()); 
            numL.Value = trkL.Value;
        }

        private void txtFolder_TextChanged(object sender, EventArgs e)
        {
            shader.ClaroPath = dirDialog.SelectedPath;
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
            shader.Invert = chkInvert.Checked;
            pictureBox1.Image = (Image)shader.ProcessBitmap((Bitmap)orginBmp.Clone());
        }

        private void chkBW_CheckedChanged(object sender, EventArgs e)
        {
            shader.KeepBlacks = chkBW.Checked;
            pictureBox1.Image = (Image)shader.ProcessBitmap((Bitmap)orginBmp.Clone());
        }

        private void chkGray_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGray.Checked)
                numGrayTolerance.Enabled = true;
            else
                numGrayTolerance.Enabled = false;
            shader.KeepGrays = chkGray.Checked;
            pictureBox1.Image = (Image)shader.ProcessBitmap((Bitmap)orginBmp.Clone());
        }

        private void numGrayTolerance_ValueChanged(object sender, EventArgs e)
        {
            shader.GrayTolerance = (int) numGrayTolerance.Value;
            pictureBox1.Image = (Image)shader.ProcessBitmap((Bitmap)orginBmp.Clone());
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
