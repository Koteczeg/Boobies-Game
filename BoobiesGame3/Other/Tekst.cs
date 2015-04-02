using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.IO;


namespace BoobiesGame3
{
    public class Tekst
    {
        public string Text;
        public Rectangle Rect;
        public StartMenu.MenuState Stat;
        public StartMenu.MenuState NextStat;
        public bool ShouldBeColoured;


        public Tekst(string Text, Rectangle Rect, StartMenu.MenuState Stat, StartMenu.MenuState NextStat, bool ShouldBeColoured)
        {
            this.Text = Text;
            this.Rect = Rect;
            this.Stat = Stat;
            this.ShouldBeColoured = ShouldBeColoured;
            this.NextStat = NextStat;
        }
        public bool isOnScreen(GameWindow gameWindow, int shift)
        {
            var screen = new Rectangle(shift, 0, gameWindow.ClientBounds.Width, gameWindow.ClientBounds.Height);
            return Rectangle.Intersect(this.Rect, screen) != Rectangle.Empty;
        }
    }

    public class Tekst2
    {
        public string Text;
        public Rectangle Rect;
        public GameOver.MenuStates Stat;
        public GameOver.MenuStates NextStat;
        public bool ShouldBeColoured;


        public Tekst2(string Text, Rectangle Rect, GameOver.MenuStates Stat, GameOver.MenuStates NextStat, bool ShouldBeColoured)
        {
            this.Text = Text;
            this.Rect = Rect;
            this.Stat = Stat;
            this.ShouldBeColoured = ShouldBeColoured;
            this.NextStat = NextStat;
        }
        public bool isOnScreen(GameWindow gameWindow, int shift)
        {
            var screen = new Rectangle(shift, 0, gameWindow.ClientBounds.Width, gameWindow.ClientBounds.Height);
            return Rectangle.Intersect(this.Rect, screen) != Rectangle.Empty;
        }
    }
}
