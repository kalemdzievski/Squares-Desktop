using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Igrica.Properties;
using System.Drawing.Text;

namespace Igrica
{
    public partial class Form1 : Form
    {
        private Square s1, s2, s3, s4;

        private Sounds mainSound;

        private static Image main;
        private static Image playBtn;
        private static Image highScoresBtn;
        private static Image helpBtn;
        private static Image exitBtn;
        private static Image playBtnHover;
        private static Image exitBtnHover;
        private static Image scoresBtnHover;
        private static Image helpBtnHover;

        private Bitmap scoresHoverBtmp;
        private Bitmap helpHoverBtmp;
        private Bitmap exitHoverBtmp;
        private Bitmap playHoverBtmp;
        private Bitmap exitBitmap;
        private Bitmap helpBitmap;
        private Bitmap highScoreBitmap;
        private Bitmap playBitmap;
        private Bitmap mainBitmap;

        private HighScores hs;
        private Help h;
        private Play p;

        private bool play, exit, highScores, help;

        public Form1()
        {
            InitializeComponent();
            mainSound = new Sounds();
            this.CenterToScreen();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Width = Resources.squares.Width + 15;
            this.Height = Resources.squares.Width + 15;
            this.BackColor = System.Drawing.Color.Black;
            this.DoubleBuffered = true;

            main = Resources.squares;
            playBtn = Resources.Play2;
            highScoresBtn = Resources.Score1;
            helpBtn = Resources.HelpBtn;
            exitBtn = Resources.Exit;
            playBtnHover = Resources.Play_hover;
            exitBtnHover = Resources.Exit_hover;
            scoresBtnHover = Resources.Score_hover;
            helpBtnHover = Resources.Help_hover;

            scoresHoverBtmp = new Bitmap(scoresBtnHover, new Size(210, 80));
            helpHoverBtmp = new Bitmap(helpBtnHover, new Size(210, 80));
            exitHoverBtmp = new Bitmap(exitBtnHover, new Size(210, 80));
            playHoverBtmp = new Bitmap(playBtnHover, new Size(210, 80));
            exitBitmap = new Bitmap(exitBtn, new Size(210, 80));
            helpBitmap = new Bitmap(helpBtn, new Size(210, 80));
            highScoreBitmap = new Bitmap(highScoresBtn, new Size(210, 80));
            playBitmap = new Bitmap(playBtn, new Size(210, 80));
            mainBitmap = new Bitmap(main);

            s1 = new Square(220, 410, 40);
            s1.image = Resources._1;
            s2 = new Square(290, 35, 40);
            s2.image = Resources._9;
            s3 = new Square(350, 220, 40);
            s3.image = Resources._5;
            s4 = new Square(410, 200, 40);
            s4.image = Resources._7;

            hs = new HighScores();

            animationTimer.Start();
        }

        private void animationTimer_Tick(object sender, EventArgs e)
        {            
            s1.X -= 1;
            s2.X += 1;
            s3.X += 1;
            s4.X -= 1;
            if (s1.X < -30)
                s1.X = this.Width + 30;
            if (s2.X > this.Width + 30)
                s2.X = -30;
            if (s3.X > this.Width + 30)
                s3.X = -30;
            if (s4.X < -30)
                s4.X = this.Width + 30;
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            s1.drawSquare(e.Graphics);
            s2.drawSquare(e.Graphics);
            s3.drawSquare(e.Graphics);
            s4.drawSquare(e.Graphics);
            e.Graphics.DrawImage(mainBitmap, 0, 0);

            if (play)
                e.Graphics.DrawImage(playHoverBtmp, 140, 250);
            else e.Graphics.DrawImage(playBitmap, 140, 250);
            if (highScores)
                e.Graphics.DrawImage(scoresHoverBtmp, 140, 295);
            else e.Graphics.DrawImage(highScoreBitmap, 140, 295);
            if (help)
                e.Graphics.DrawImage(helpHoverBtmp, 140, 340);
            else e.Graphics.DrawImage(helpBitmap, 140, 340);
            if (exit)
                e.Graphics.DrawImage(exitHoverBtmp, 140, 385);
            else e.Graphics.DrawImage(exitBitmap, 140, 385);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.X > 160 && e.X < 120 + 210 && e.Y >= 260 && e.Y < 230 + 80)
            {
                mainSound.playButtons();
                this.Hide();
                p = new Play();
                if (p.ShowDialog() == DialogResult.Cancel)
                {
                    if (hs.scores.Count == 0 || (p.sMatrix.score > hs.scores.ElementAt(hs.scores.Count - 1)))
                        if (p.en != null && DialogResult.OK == p.en.ShowDialog())
                        {
                            hs.addScore(p.en.score, p.en.name);
                            hs.serializeHighScores();
                        }
                    if (hs.ShowDialog() == DialogResult.Cancel)
                        this.Show();
                }
            }
            else if (e.X > 160 && e.X < 120 + 210 && e.Y >= 300 && e.Y < 275 + 80)
            {
                mainSound.playButtons();
                this.Hide();
                if (hs.ShowDialog() == DialogResult.Cancel)
                    this.Show();
            }
            else if (e.X > 160 && e.X < 120 + 210 && e.Y >= 340 && e.Y < 320 + 80)
            {
                mainSound.playButtons();
                this.Hide();
                h = new Help();
                if (h.ShowDialog() == DialogResult.Cancel)
                    this.Show();
            }
            else if (e.X > 160 && e.X < 120 + 210 && e.Y >= 385 && e.Y < 370 + 80)
            {
                Close();
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X > 160 && e.X < 120 + 210 && e.Y >= 260 && e.Y < 230 + 80)
            {
                play = true;
                exit = false;
                help = false;
                highScores = false;
                s1.X -= 1;
                s2.X += 1;
                s3.X += 1;
                s4.X -= 1;
                Invalidate();
            }
            else if (e.X > 160 && e.X < 120 + 210 && e.Y >= 300 && e.Y < 275 + 80)
            {
                play = false;
                exit = false;
                help = false;
                highScores = true;
                s1.X -= 1;
                s2.X += 1;
                s3.X += 1;
                s4.X -= 1;
                Invalidate();
            }
            else if (e.X > 160 && e.X < 120 + 210 && e.Y >= 340 && e.Y < 320 + 80)
            {
                play = false;
                exit = false;
                help = true;
                highScores = false;
                s1.X -= 1;
                s2.X += 1;
                s3.X += 1;
                s4.X -= 1;
                Invalidate();
            }
            else if (e.X > 160 && e.X < 120 + 210 && e.Y >= 385 && e.Y < 375 + 80)
            {
                play = false;
                exit = true;
                help = false;
                highScores = false;
                s1.X -= 1;
                s2.X += 1;
                s3.X += 1;
                s4.X -= 1;
                Invalidate();
            }
            else
            {
                play = false;
                exit = false;
                help = false;
                highScores = false;
            }
        }

    }
}
