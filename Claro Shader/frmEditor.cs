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
using System.Text.RegularExpressions;

namespace Claro_Shader
{
    public partial class frmEditor : Form
    {
        private string file = "";

        public frmEditor(string file)
        {
            InitializeComponent();
            this.file = file;
        }

        private void frmEditor_Load(object sender, EventArgs e)
        {
            rtbEdit.LoadFile(file, RichTextBoxStreamType.PlainText);
            highlight();
            rtbEdit.Select(0, 0);
        }

        private void btnCompile_Click(object sender, EventArgs e)
        {
            rtbEdit.SaveFile(file, RichTextBoxStreamType.PlainText);
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void highlight()
        {
            Regex regx = new Regex("#.*;", RegexOptions.IgnoreCase);
            Match m = regx.Match(rtbEdit.Text);
            while (m.Success)
            {
                string RGB = m.Value.Trim(new char[] { ';' });
                try
                {
                    rtbEdit.Select(m.Index, m.Length);
                    rtbEdit.SelectionColor = ColorTranslator.FromHtml(RGB);
                }
                catch (Exception e) { }
                m = m.NextMatch();
            }
            rtbEdit.DeselectAll();
        }

        private void highlight(int index)
        {
            if (index == 0)
                return;
            int lineIndex = rtbEdit.GetLineFromCharIndex(index);
            Regex regx = new Regex("#.*;", RegexOptions.IgnoreCase);
            Match m = regx.Match(rtbEdit.Lines[lineIndex]);
            while (m.Success)
            {
                string RGB = m.Value.Trim(new char[] { ';' });
                try
                {
                    rtbEdit.Select(rtbEdit.GetFirstCharIndexFromLine(lineIndex) + m.Index, m.Length);
                    rtbEdit.SelectionColor = ColorTranslator.FromHtml(RGB);
                }
                catch (Exception e) { }
                m = m.NextMatch();
            }
            rtbEdit.DeselectAll();
        }

        private void rtbEdit_TextChanged(object sender, EventArgs e)
        {
            if (!chkHighlight.Checked)
                return;
            int selected = rtbEdit.SelectionStart;
            int selectedLength = rtbEdit.SelectionLength;
            highlight(selected);
            rtbEdit.Select(selected, selectedLength);
        }

        private void chkHighlight_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHighlight.Checked)
                highlight();
            else
            {
                int selected = rtbEdit.SelectionStart;
                int selectedLength = rtbEdit.SelectionLength;
                rtbEdit.SelectAll();
                rtbEdit.SelectionColor = Color.Black;
                rtbEdit.DeselectAll();
                rtbEdit.Select(selected, selectedLength);
            }
        }
    }
}
