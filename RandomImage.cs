using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Igrica.Properties;

namespace Igrica
{
    class RandomImage
    {
        private List<Image> listImages;
        private Random r;

        public RandomImage()
        {
            r = new Random();
            listImages = new List<Image>();

            listImages.Add(Resources._1);
            listImages.Add(Resources._2);
            listImages.Add(Resources._3);
            listImages.Add(Resources._5);
            listImages.Add(Resources._9);
            listImages.Add(Resources._6);
            listImages.Add(Resources._7);
            listImages.Add(Resources._10);

            listImages.Add(Resources._1_copy);
            listImages.Add(Resources._2_copy);
            listImages.Add(Resources._3_copy);
            listImages.Add(Resources._5_copy);
            listImages.Add(Resources._9_copy);
            listImages.Add(Resources._6_copy);
            listImages.Add(Resources._7_copy);
            listImages.Add(Resources._10_copy);
        }

        public void getImages(int i, out Image image, out Image imageSelected)
        {
            int n = r.Next(i);
            image = listImages.ElementAt(n);
            imageSelected = listImages.ElementAt(n+8);
        }
    }
}
