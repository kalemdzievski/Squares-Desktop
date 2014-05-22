using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Igrica.Properties;
using System.Drawing.Imaging;

namespace Igrica
{
    public class Square
    {

        private Bitmap imageBtmp;
        private Bitmap imageSelectedBtmp;
        private int width;
        public float X { get; set; }
        public float Y { get; set; }
        public Image imageDefault { get; set; }
        public Image image { get; set; }
        public Image imageSelected { get; set; }
        public bool isSelected { get; set; }
        public bool isPainted { get; set; }

        public Square(float y, float x, int w)
        {
            this.X = x;
            this.Y = y;
            width = w - 5;
            imageDefault = Resources._11;
            image = imageDefault;
            isSelected = false;
            isPainted = false;
        }

        public void drawSquare(Graphics g)
        {
            if (!isSelected)
            {
                imageBtmp = new Bitmap(image, new Size(width, width));
                g.DrawImage(imageBtmp, X, Y);
            }
            else
            {
                imageSelectedBtmp = new Bitmap(imageSelected, new Size(width, width));
                g.DrawImage(imageSelectedBtmp, X, Y);
            }
        }

        public bool isHit(float x, float y)
        {
            return Math.Abs((20 + X) - x) <= width / 2 && Math.Abs((20 + Y) - y) <= width / 2;
        }

    }
}
