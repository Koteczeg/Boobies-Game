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

    public class StartMenu
    {
        public bool GameStarted { get; set; }
        public Texture2D Texture;
        public SpriteFont SpriteFont;
        private Keys[] LastPressedKeys = new Keys[15];
        public static string MyName = string.Empty;
        public static string prevname;
        public List<Tekst> TextToDisplay = new List<Tekst>();
        public int VelocityOfMenu = 0;
        public enum MenuState { MainMenu, NewGame, Credits, Settings, Scores, GameShouldStart, Nothing, Moving };

        public MenuState State = MenuState.MainMenu;
        public MenuState NextState = MenuState.NewGame;
        public ButtonState PreviousStateOfMouse = ButtonState.Released;
        public ButtonState CurrentStateOfMouse = ButtonState.Released;

        public string fontFilePath;
        public FontFile fontFile;
        public Texture2D fontTexture;
        public FontRenderer FontRenderer;

        public StartMenu(ContentManager Content)
        {
            GameStarted = false;
            GameOver.AlreadySorted = false;
            fontFilePath = Path.Combine(Content.RootDirectory, "Fonts/Font1.fnt");
            fontFile = FontLoader.Load(fontFilePath);
            fontTexture = Content.Load<Texture2D>("Fonts/Font1_0.png");
            FontRenderer = new FontRenderer(fontFile, fontTexture);
        }

        public void DrawPrettyStrings(List<Tekst> TextToDisplay, FontRenderer FontRenderer, MouseState MouseState, SpriteBatch SpriteBatch)
        {
            PreviousStateOfMouse = CurrentStateOfMouse;
            CurrentStateOfMouse = MouseState.LeftButton;
            foreach (var item in TextToDisplay)
            {
                if (MouseState.LeftButton == ButtonState.Pressed && State != MenuState.Moving && item.Rect.Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1)) && item.ShouldBeColoured)
                {
                    FontRenderer.DrawText(SpriteBatch, item.Rect.X, item.Rect.Y, item.Text, Color.Red);
                    if (PreviousStateOfMouse == ButtonState.Released && CurrentStateOfMouse == ButtonState.Pressed)
                    {
                        if (item.Text == "Start game" && MyName.Length <= 0)
                        {
                            continue;
                        }
                        else
                        {
                        State = item.Stat;
                        NextState = item.NextStat;
                    }
                }
                }
                else if (item.Rect.Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1)) && item.ShouldBeColoured && State != MenuState.Moving)
                {
                    FontRenderer.DrawText(SpriteBatch, item.Rect.X, item.Rect.Y, item.Text, Color.Red);
                }
                else
                {
                    FontRenderer.DrawText(SpriteBatch, item.Rect.X, item.Rect.Y, item.Text, Color.Black);
                }
            }
        }

        public void GenerateMainMenu()
        {
            TextToDisplay.Clear();
            TextToDisplay.Add(new Tekst("New Game", new Rectangle(40, 110, 190, 40), MenuState.Moving, MenuState.NewGame, true));
            TextToDisplay.Add(new Tekst("Credits", new Rectangle(40, 210, 140, 40), MenuState.Moving, MenuState.Credits, true));
            TextToDisplay.Add(new Tekst("Settings", new Rectangle(40, 310, 150, 40), MenuState.Moving, MenuState.Settings, true));
            TextToDisplay.Add(new Tekst("High Scores", new Rectangle(40, 410, 215, 40), MenuState.Moving, MenuState.Scores, true));
        }

        public void GenerateNewGame()
        {
            TextToDisplay.Clear();
            TextToDisplay.Add(new Tekst("Type your name:", new Rectangle(40, 110, 190, 40), MenuState.Nothing, MenuState.Nothing, false));
            TextToDisplay.Add(new Tekst("Back to main menu", new Rectangle(40, 310, 400, 40), MenuState.Moving, MenuState.MainMenu, true));
            TextToDisplay.Add(new Tekst("Start game", new Rectangle(40, 410, 240, 40), MenuState.Moving, MenuState.GameShouldStart, true));
        }

        public void GenerateCredits()
        {
            TextToDisplay.Clear();
            TextToDisplay.Add(new Tekst("Created by:", new Rectangle(40, 290, 240, 40), MenuState.Nothing, MenuState.Nothing, false));
            TextToDisplay.Add(new Tekst("soivek", new Rectangle(90, 330, 240, 40), MenuState.Nothing, MenuState.Nothing, false));
            TextToDisplay.Add(new Tekst("tygryseq", new Rectangle(90, 370, 240, 40), MenuState.Nothing, MenuState.Nothing, false));
            TextToDisplay.Add(new Tekst("wozniakb2", new Rectangle(90, 420, 240, 40), MenuState.Nothing, MenuState.MainMenu, false));
            TextToDisplay.Add(new Tekst("Back to main menu", new Rectangle(40, 540, 400, 40), MenuState.Moving, MenuState.MainMenu, true));
        }


        public void Draw(GameTime gameTime, SpriteBatch SpriteBatch, ContentManager Content, GameWindow gameWindow)
        {
            Rectangle Window = new Rectangle(0, 0, gameWindow.ClientBounds.Width, gameWindow.ClientBounds.Height);
            this.Texture = Content.Load<Texture2D>("robobg"); // w pewnym momencie cały czas krzyczało że jest nullem, niech na razie tak będzie
            SpriteBatch.Draw(Texture, Window, Color.White);

            var MouseState = Mouse.GetState();
            if (State == MenuState.MainMenu)
            {
                VelocityOfMenu = 0;
                MyName = "";
                DrawPrettyStrings(TextToDisplay, FontRenderer, MouseState, SpriteBatch);
            }
            if (State == MenuState.Moving)
            {
                // update will update position
                DrawPrettyStrings(TextToDisplay, FontRenderer, MouseState, SpriteBatch);
                if (TextToDisplay.Count == 0)
                    State = NextState;
            }
            if (State == MenuState.NewGame)
            {
                VelocityOfMenu = 0;
                FontRenderer.DrawText(SpriteBatch, 40, 210, MyName, Color.Aqua);
                DrawPrettyStrings(TextToDisplay, FontRenderer, MouseState, SpriteBatch);
            }
            if (State == MenuState.GameShouldStart)
            {
                if (MyName.Length > 0)
                {
                    GameStarted = true;
                    prevname = MyName;
                }
                else State = MenuState.NewGame;
            }
            if (State == MenuState.Credits || State == MenuState.Scores || State == MenuState.Settings)
            {
                VelocityOfMenu = 0;
                DrawPrettyStrings(TextToDisplay, FontRenderer, MouseState, SpriteBatch);
            }
        }

        public void GetKeys()
        {
            KeyboardState kbState = Keyboard.GetState();
            Keys[] pressedKeys = kbState.GetPressedKeys();
            foreach (Keys key in LastPressedKeys)
            {
                if (!pressedKeys.Contains(key))
                {
                    OnKeyUp(key);
                }
            }
            foreach (Keys key in pressedKeys)
            {
                if (key == Keys.Back)
                {
                    OnKeyDown(key);
                    continue;
                }
                if (!LastPressedKeys.Contains(key))
                {
                    OnKeyDown(key);
                }
            }
            LastPressedKeys = pressedKeys;
        }

        public void OnKeyUp(Keys key)
        {
        }

        public static bool IsKeyADigit(Keys key)
        {
            return (key >= Keys.D0 && key <= Keys.D9) || (key >= Keys.NumPad0 && key <= Keys.NumPad9);
        }

        private int GetKeyValue(int keyValue)
        {
            if (keyValue >= 48 && keyValue <= 57)
            {
                return keyValue - 48;
            }
            else if (keyValue >= 96 && keyValue <= 105)
            {
                return keyValue - 96;
            }
            else
            {
                return -1; // Not a number... do whatever...
            }
        }

        public void OnKeyDown(Keys key)
        {
            if (key == Keys.Back && MyName.Length > 0)
            {
                MyName = MyName.Remove(MyName.Length - 1);
                return;
            }
            else
            {
                if (MyName.Length < 10)
                {
                    if (IsKeyADigit(key))
                        MyName += GetKeyValue((int)key).ToString();
                    else if (key < Keys.A || key > Keys.Z)
                        return;
                    else
                        MyName += key.ToString();
                }
            }
        }

        public void GenerateCurrentMenu(GameWindow gameWindow)
        {
            List<Tekst> TemporaryText = new List<Tekst>();
            TemporaryText.Clear();
            IEnumerator<Tekst> ie = TextToDisplay.GetEnumerator();
            VelocityOfMenu += 2;
            int currrentTextVelocity = VelocityOfMenu;
            while (ie.MoveNext())
            {
                currrentTextVelocity = currrentTextVelocity - 5;
                if (ie.Current.Rect.X + VelocityOfMenu <= gameWindow.ClientBounds.Width)
                {
                    TemporaryText.Add(new Tekst(ie.Current.Text, new Rectangle(ie.Current.Rect.X + currrentTextVelocity, ie.Current.Rect.Y, ie.Current.Rect.Width, ie.Current.Rect.Height), ie.Current.Stat, ie.Current.NextStat, ie.Current.ShouldBeColoured));
                }
            }
            TextToDisplay = TemporaryText;
        }

        public void GenerateScores()
        {
            TextToDisplay.Clear();
            string[] lines = File.ReadAllLines(@"Content/Scores.txt");
            int i = 50;
            int licznik = 1;
            foreach (string line in lines)
            {
                TextToDisplay.Add(new Tekst(licznik + ". " + line, new Rectangle(40, i, 100, 100), MenuState.Nothing, MenuState.Nothing, false));
                i += 50;
                licznik++;
            }
            TextToDisplay.Add(new Tekst("Back to main menu", new Rectangle(40, 540, 400, 40), MenuState.Moving, MenuState.MainMenu, true));
            
        }

        public void GenerateSettings()
        {
            TextToDisplay.Clear();
            TextToDisplay.Add(new Tekst("Under construction.", new Rectangle(40, 230, 240, 40), MenuState.Nothing, MenuState.MainMenu, false));
            TextToDisplay.Add(new Tekst("Back to main menu", new Rectangle(40, 540, 400, 40), MenuState.Moving, MenuState.MainMenu, true));
        }

        public void Update(GameTime gameTime, GameWindow gameWindow)
        {
            if (State == MenuState.MainMenu)
            {
                GenerateMainMenu();
            }
            if (State == MenuState.Moving)
            {
                GenerateCurrentMenu(gameWindow);
            }
            if (State == MenuState.NewGame)
            {
                GetKeys();
                GenerateNewGame();
            }
            if (State == MenuState.Credits)
            {
                GenerateCredits();
            }
            if (State == MenuState.Scores)
            {
                GenerateScores();
            }
            if (State == MenuState.Settings)
            {
                GenerateSettings();
            }
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("robobg");
            SpriteFont = content.Load<SpriteFont>("Fonts/ffont");
        }
    }
}