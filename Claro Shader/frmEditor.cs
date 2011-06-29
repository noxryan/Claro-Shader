using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Claro_Shader
{
    public partial class frmEditor : Form
    {
        string file = "";

        public frmEditor(string file)
        {
            InitializeComponent();
            this.file = file;
        }

        private void frmEditor_Load(object sender, EventArgs e)
        {
            rtbEdit.LoadFile(file, RichTextBoxStreamType.PlainText);
        }

        private void btnCompile_Click(object sender, EventArgs e)
        {
            rtbEdit.SaveFile(file);
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
