using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Igrica.Properties;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Igrica
{
    public partial class HighScores : Form
    {
        public List<int> scores { get; set; }
        public List<String> names { get; set; }

        private static Image scoresMain;
        private static Image back;
        private static Image backHover;
        private Bitmap scoresBitmap;
        private Bitmap backBtmp;
        private Bitmap backHoverBtmp;
        private Fonts fonts;

        private bool buttonHovered;

        public HighScores()
        {
            InitializeComponent();
            this.CenterToScreen();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.BackColor = System.Drawing.Color.Black;
            this.DoubleBuffered = true;
            this.Width = Resources.highscores.Width + 15;
            this.Height = Resources.highscores.Width + 15;
            buttonHovered = false;

            try
            {
                FileStream str = File.OpenRead("scores.bin");
            }
            catch (FileNotFoundException)
            {
                scores = new List<int>();
                serializeScores(scores);
            }
            try
            {
                FileStream str = File.OpenRead("names.bin");
            }
            catch (FileNotFoundException)
            {
                names = new List<string>();
                serializeNames(names);
            }
            scores = deserializeScores();
            names = deserializeNames();

            scoresMain = Resources.highscores;
            back = Resources.Back;
            backHover = Resources.Back_hover;
            scoresBitmap = new Bitmap(scoresMain);
            backBtmp = new Bitmap(back, new Size(210, 80));
            backHoverBtmp = new Bitmap(backHover, new Size(210, 80));
            fonts = new Fonts("lgs.ttf", 28);
        }

        private void HighScores_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(scoresBitmap, 0, 0);

            if (buttonHovered)
                e.Graphics.DrawImage(backHoverBtmp, 0, this.Height - 120);
            else e.Graphics.DrawImage(backBtmp, 0, this.Height - 120);
            if (scores.Count != 0)
            {
                for (int i = 0; i < scores.Count; i++)
                {
                    if (i == 0)
                        e.Graphics.DrawString(" 1. " + names.ElementAt(i).ToString(), fonts.getFont(), new SolidBrush(Color.White), 73, (i * 24) + 200);
                    else e.Graphics.DrawString(i + 1 + ". " + names.ElementAt(i).ToString(), fonts.getFont(), new SolidBrush(Color.White), 70, (i * 24) + 200);
                    e.Graphics.DrawString("Score: " + scores.ElementAt(i).ToString(), fonts.getFont(), new SolidBrush(Color.White), 230, (i * 24) + 200);
                }
            }
        }

        private void HighScores_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X > 20 && e.X <= 190 && e.Y > this.Height - 120 && e.Y <= this.Height - 60)
                Close();
        }

        private void HighScores_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X > 20 && e.X <= 190 && e.Y > this.Height - 120 && e.Y <= this.Height - 60)
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

        public void addScore(int s, string n)
        {
            int i;
            for (i = 0; i < scores.Count; i++)
            {
                if (s > scores.ElementAt(i))
                    break;
            }
            scores.Insert(i, s);
            names.Insert(i, n);
            if (scores.Count == 6)
            {
                scores.RemoveAt(5);
                names.RemoveAt(5);
            }
        }

        private static void serializeScores(List<int> list)
        {
            using (FileStream str = File.Create("scores.bin"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(str, list);
            }
        }

        private static List<int> deserializeScores()
        {
            List<int> scores = null;
            using (FileStream str = File.OpenRead("scores.bin"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                scores = (List<int>)bf.Deserialize(str);
            }
            return scores;
        }

        private static void serializeNames(List<String> list)
        {
            using (FileStream str = File.Create("names.bin"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(str, list);
            }
        }

        private static List<String> deserializeNames()
        {
            List<String> names = null;
            using (FileStream str = File.OpenRead("names.bin"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                names = (List<String>)bf.Deserialize(str);
            }
            return names;
        }

        public void serializeHighScores()
        {
            serializeScores(scores);
            serializeNames(names);
        }

        private void HighScores_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
