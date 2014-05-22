using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using Igrica.Properties;

namespace Igrica
{
    public class Sounds
    {

        private static SoundPlayer score;
        private static SoundPlayer mainButtons;
        private static SoundPlayer noPath;
        private static SoundPlayer placeSquare;
        private static SoundPlayer bombSound;
        private static SoundPlayer swordSound;

        public bool mute { get; set; }

        public Sounds()
        {
            score = new SoundPlayer(Resources.score);
            mainButtons = new SoundPlayer(Resources.main_menu_buttons);
            noPath = new SoundPlayer(Resources.noPath);
            placeSquare = new SoundPlayer(Resources.place_square);
            bombSound = new SoundPlayer(Resources.bombSound);
            swordSound = new SoundPlayer(Resources.swordSound);
            mute = false;
        }

        public void playScore() 
        {
            if(!mute)
                score.Play();
        }

        public void playNoPath()
        {
            if (!mute)
                noPath.Play();
        }

        public void playButtons()
        {
            if (!mute)
                mainButtons.Play();
        }

        public void playPlaceSquare()
        {
            if (!mute)
                placeSquare.Play();
        }

        public void playBomb()
        {
            if (!mute)
            {
                bombSound.Play();
            }
        }

        public void playSword()
        {
            if (!mute)
            {
                swordSound.Play();
            }
        }

    }
}
