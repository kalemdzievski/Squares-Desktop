using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Igrica.Properties;

namespace Igrica
{
    public partial class EnterName : Form
    {
        private Fonts font;
        private Image square;
        private int offsetX, offsetY;
        public String name { get; set; }
        public int score { get; set; }

        public EnterName()
        {
            InitializeComponent();
            this.BackColor = Color.Black;
            this.Width = 350;
            this.Height = 300;
            this.CenterToScreen();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            square = Resources._11;
            offsetX = 45;
            offsetY = 60;
            nameTxt.Location = new Point(120 + offsetX, 25 + offsetY);
            btnOk.Location = new Point(120 + offsetX, 90 + offsetY);
            font = new Fonts("lgs.ttf", 28);
            Invalidate();
        }

        private void EnterName_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString("Name: ", font.getFont(), new SolidBrush(Color.White), 10 + offsetX, 20 + offsetY);
            e.Graphics.DrawString("Score:  " + score.ToString(), font.getFont(), new SolidBrush(Color.White), 10 + offsetX, 50 + offsetY);
            e.Graphics.DrawImage(new Bitmap(square, new Size(30, 30)), 5, 5);
            e.Graphics.DrawImage(new Bitmap(square, new Size(30, 30)), 40, 5);
            e.Graphics.DrawImage(new Bitmap(square, new Size(30, 30)), 5, 40);
            e.Graphics.DrawImage(new Bitmap(square, new Size(30, 30)), this.Width - 45, this.Height - 65);
            e.Graphics.DrawImage(new Bitmap(square, new Size(30, 30)), this.Width - 80, this.Height - 65);
            e.Graphics.DrawImage(new Bitmap(square, new Size(30, 30)), this.Width - 45, this.Height - 100);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            name = nameTxt.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void nameTxt_Validating(object sender, CancelEventArgs e)
        {
            if (nameTxt.Text.Length > 5 || nameTxt.Text.Trim().Length == 0)
            {
                e.Cancel = true;
                errorProviderName.SetError(nameTxt, "Imeto ne smee da bide prazno nitu pak pogolemo od 5 bukvi!");
            }
            else
                errorProviderName.SetError(nameTxt, null);
        }

    }
}
