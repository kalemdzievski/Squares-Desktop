using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Igrica
{
    public class RandomBrush
    {
        public SolidBrush sb { get; set; }
        public Random r = new Random();
        public RandomBrush()
        {
            sb = new SolidBrush(Color.White);
        }

        public SolidBrush getRandomBrush(int number)
        {
            int n = r.Next(number);
            if (n == 0)
                sb = new SolidBrush(Color.Red);
            else if (n == 1)
                sb = new SolidBrush(Color.Blue);
            else if (n == 2)
                sb = new SolidBrush(Color.Green);
            else if (n == 3)
                sb = new SolidBrush(Color.Yellow);
            else if (n == 4)
                sb = new SolidBrush(Color.Orange);
            else if (n == 5)
                sb = new SolidBrush(Color.HotPink);
            else if (n == 6)
                sb = new SolidBrush(Color.Purple);
            else if (n == 7)
                sb = new SolidBrush(Color.LightSkyBlue);
            return sb; ;
        }
    }
}
