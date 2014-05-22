using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Igrica.Properties;
using System.Diagnostics;

namespace Igrica
{
    public partial class Play : Form
    {
        public EnterName en { get; set; }

        private Fonts font;
        public SquaresMatrix sMatrix { get; set; }
        private Square bomb, cut, freeze, mute;
        private static String Score = "Score: ", Time = "Time: ", Combo = "Combo: ", Squares = "Squares: ", Weapons = "Weapons: ";
        private int rows, columns, offset, squareWidth, comboTimerTicks, freezeSeconds, time;
        private bool bombWeaponSelected, cutWeaponSelected, freezeWeaponSelected, bombUsed, cutUsed, freezeUsed, comboScore;
         
        public Play()
        {
            InitializeComponent();

            rows = 10;
            columns = 10;
            offset = 20;
            comboTimerTicks = 0;
            time = 120;
            freezeSeconds = 0;
            squareWidth = Resources._1.Width + 5;

            this.CenterToScreen();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Height = 8 * offset + rows * squareWidth;
            this.Width = 3 * offset + columns * squareWidth - 15;
            this.BackColor = System.Drawing.Color.Black;
            this.DoubleBuffered = true;

            comboScore = false;
            bombWeaponSelected = false;
            cutWeaponSelected = false;
            freezeWeaponSelected = false;
            bombUsed = false;
            cutUsed = false;
            freezeUsed = false;

            nosTimer.Start();

            font = new Fonts("lgs.ttf", 28);

            sMatrix = new SquaresMatrix(rows, columns, squareWidth, offset);

            bomb = new Square(80 + rows * squareWidth, offset + 150, squareWidth);
            bomb.image = Resources.bomb;
            bomb.imageSelected = Resources.bomb_selected;
            bomb.imageDefault = Resources.no_bomb;
            cut = new Square(80 + rows * squareWidth, offset + 195, squareWidth);
            cut.image = Resources.cut_sword;
            cut.imageSelected = Resources.cut_sword_selected;
            cut.imageDefault = Resources.no_sword;
            freeze = new Square(80 + rows * squareWidth, offset + 240, squareWidth);
            freeze.image = Resources.freeze_time;
            freeze.imageSelected = Resources.no_freeze_time;
            mute = new Square(80 + rows * squareWidth, this.Width - squareWidth - offset - 5, squareWidth);
            mute.image = Resources.volume;
            mute.imageSelected = Resources.volume_off;
        }

        private void Play_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
            sMatrix.drawMatrix(e.Graphics);
            if (comboScore)
                e.Graphics.DrawString(Score + sMatrix.score.ToString() + " +" + sMatrix.getComboScore.ToString(), font.getFont(), new SolidBrush(Color.Beige), offset, 10 + rows * squareWidth);
            else e.Graphics.DrawString(Score + sMatrix.score.ToString(), font.getFont(), new SolidBrush(Color.Beige), offset, 10 + rows * squareWidth);
            e.Graphics.DrawString(Time + time.ToString(), font.getFont(), new SolidBrush(Color.Beige), this.Width - 152, 10 + rows * squareWidth);
            e.Graphics.DrawString(Combo + sMatrix.combo.ToString(), font.getFont(), new SolidBrush(Color.Beige), offset, 40 + rows * squareWidth);
            e.Graphics.DrawString(Squares + sMatrix.numberOfSquares.ToString(), font.getFont(), new SolidBrush(Color.Beige), this.Width - 215, 40 + rows * squareWidth);
            e.Graphics.DrawString(Weapons, font.getFont(), new SolidBrush(Color.Beige), offset, 75 + rows * squareWidth);
            bomb.drawSquare(e.Graphics);
            cut.drawSquare(e.Graphics);
            freeze.drawSquare(e.Graphics);
            mute.drawSquare(e.Graphics);
        }

        private void click(int X, int Y)
        {
            if (bombWeaponSelected)
            {
                bombUsed = true;
                bomb.image = bomb.imageDefault;
                bomb.imageSelected = bomb.imageDefault;
                bombWeaponSelected = false;
                sMatrix.bombThem(X, Y);
            }
            else if (cutWeaponSelected)
            {
                cutUsed = true;
                cut.image = cut.imageDefault;
                cut.imageSelected = cut.imageDefault;
                cutWeaponSelected = false;
                sMatrix.cutThem(X, Y);
            }
            else sMatrix.selectAndMove(X, Y);

            if (sMatrix.getComboScore != 0)
            {
                comboScore = true;
                comboTimer.Start();
            }
        }

        private void checkWeapon(int X, int Y)
        {
            if (bomb.isHit(X, Y) && !bombUsed)
            {
                bomb.isSelected = !bomb.isSelected;
                bombWeaponSelected = !bombWeaponSelected;
                if (!cutUsed)
                {
                    cutWeaponSelected = false;
                    cut.isSelected = false;
                }
            }
            else if (cut.isHit(X, Y) && !cutUsed)
            {
                cut.isSelected = !cut.isSelected;
                cutWeaponSelected = !cutWeaponSelected;
                if (!bombUsed)
                {
                    bomb.isSelected = false;
                    bombWeaponSelected = false;
                }
            }
            else if (freeze.isHit(X, Y) && !freezeUsed)
            {
                freezeUsed = true;
                freeze.isSelected = true;
                freezeWeaponSelected = true;
                if (!bombUsed)
                {
                    bomb.isSelected = false;
                    bombWeaponSelected = false;
                }
                if (!cutUsed)
                {
                    cut.isSelected = false;
                    cutWeaponSelected = false;
                }
            }
            else if (mute.isHit(X, Y))
            {
                mute.isSelected = !mute.isSelected;
                sMatrix.gameSound.mute = !sMatrix.gameSound.mute;
            }
        }

        private void Play_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.X <= offset + columns * squareWidth && e.Y <= offset + rows * squareWidth && e.X > offset && e.Y > offset)
                click(e.X, e.Y);
            else
                checkWeapon(e.X, e.Y);
            Invalidate();
        }

        private void Play_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.X <= offset + columns * squareWidth && e.Y <= offset + rows * squareWidth && e.X > offset && e.Y > offset)
                click(e.X, e.Y);
            else
                checkWeapon(e.X, e.Y);
            Invalidate();
        }

        private void nosTimer_Tick(object sender, EventArgs e)
        {
            if (freezeWeaponSelected && freezeSeconds <= 10)
            {
                freezeSeconds++;
            }
            else
            {
                freezeWeaponSelected = false;
                time -= 1;
            }
            if (time == 0)
            {
                sMatrix.numberOfSquares++;
                time = 120;
            }
            Invalidate();
            if (sMatrix.gameOver)
            {
                nosTimer.Stop();
                en = new EnterName();
                en.score = sMatrix.score;
                this.Close();
            }
        }

        private void comboTimer_Tick(object sender, EventArgs e)
        {
            comboTimerTicks++;
            comboScore = !comboScore;
            Invalidate();
            if (comboTimerTicks == 10)
            {
                comboScore = false;
                sMatrix.getComboScore = 0;
                comboTimerTicks = 0;
                comboTimer.Stop();
            }
        }

        private void Play_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!sMatrix.gameOver)
            {
                nosTimer.Stop();
                DialogResult r = MessageBox.Show(" Are you sure you want to exit?\n Your current play will be lost! ", " Are you sure? ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                    e.Cancel = false;
                else
                {
                    e.Cancel = true;
                    nosTimer.Start();
                }
            }
            else e.Cancel = false;
        }

    }
}
