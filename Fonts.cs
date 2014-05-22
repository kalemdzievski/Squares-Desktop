using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Text;
using System.Drawing;

namespace Igrica
{
    public class Fonts
    {
        private PrivateFontCollection pfc;
        private Font font { get; set; }
       
        public Fonts(string fileName, int size)
        {
            pfc = new PrivateFontCollection();
            pfc.AddFontFile(fileName);
            font = new Font(pfc.Families[0], size, FontStyle.Regular);
            
        }

        public Font getFont()
        {
            return font;
        }
    }
}
