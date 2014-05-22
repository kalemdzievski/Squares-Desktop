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
    public partial class Help : Form
    {
        private Image help;
        private Bitmap helpBitmap;
        private Fonts font1;
        private Fonts font2;

        private static Image back;
        private static Image backHover;
        private Bitmap backBtmp;
        private Bitmap backHoverBtmp;
        private String text;

        private bool buttonHovered;

        public Help()
        {
            InitializeComponent();
            this.CenterToScreen();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.BackColor = System.Drawing.Color.Black;
            this.DoubleBuffered = true;
            this.Width = Resources.help1.Width + 15;
            this.Height = Resources.help1.Width + 15;
            help = Resources.help1;
            helpBitmap = new Bitmap(help);
            font1 = new Fonts("lgs.ttf", 20);
            font2 = new Fonts("lgs.ttf", 28);
            text = "Squares is a simple game with 100 squares\n"
                 + "placed in a matrix 10x10. On the start\n"
                 + "there are 5 squares random placed and\n"
                 + "with random color.The goal is to connect\n"
                 + "a line of three squares and get points.\n"
                 + "But look for a path to the destination!\n"
                 + "If there is no path, there will be no move!\n"
                 + "If you don't connect a line of three squares\n"
                 + "new five random squares are added. After\n"
                 + "two minutes the number of random generated\n"
                 + "squares is increased by one. Maximum\n"
                 + "number of colors added is eight. When\n"
                 + "the matrix is full the game is over.";

            back = Resources.Back;
            backHover = Resources.Back_hover;
            backBtmp = new Bitmap(back, new Size(210, 80));
            backHoverBtmp = new Bitmap(backHover, new Size(210, 80));
            buttonHovered = false;
        }

        private void Help_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(helpBitmap, 0, 0);
            e.Graphics.DrawString(text, font1.getFont(), new SolidBrush(Color.White), 30, 160);
            if (buttonHovered)
                e.Graphics.DrawImage(backHoverBtmp, -20, this.Height - 112);
            else e.Graphics.DrawImage(backBtmp, -20, this.Height - 112);
        }

        private void Help_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X > 0 && e.X <= 190 && e.Y > this.Height - 112 && e.Y <= this.Height - 70)
            {
                buttonHovered = true;
                Invalidate();
            }
            else
            {
                buttonHovered = false;
            }
            Invalidate();
        }

        private void Help_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X > 0 && e.X <= 190 && e.Y > this.Height - 112 && e.Y <= this.Height - 70)
                Close();
        }
    }
}
