using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;


namespace BoobiesGame3
{
    public class GameOver
    {

        public bool isGameOver { get; private set; }
        public static bool AlreadySorted = false;
        public Texture2D Texture;
        public SpriteFont SpriteFont;
        public string fontFilePath;
        public FontFile fontFile;
        public Texture2D fontTexture;
        public FontRenderer FontRenderer;
        public List<Tekst2> TextToDisplay = new List<Tekst2>();
        public int VelocityOfMenu = 0;    

        public enum MenuStates { MainMenu, NewGame, Credits, Settings, Scores, GameShouldStart, EndGame, Nothing, Moving };

        public MenuStates State = MenuStates.EndGame;
        public MenuStates NextState = MenuStates.Nothing;
        public ButtonState PreviousStateOfMouse = ButtonState.Released;
        public ButtonState CurrentStateOfMouse = ButtonState.Released;



        public GameOver(ContentManager Content)
        {
            this.fontFilePath = Path.Combine(Content.RootDirectory, "Fonts/Font1.fnt");
            using (var stream = TitleContainer.OpenStream(fontFilePath))
            {
                fontFile = FontLoader.Load(fontFilePath);
                fontTexture = Content.Load<Texture2D>("Fonts/Font1_0.png");
                FontRenderer = new FontRenderer(fontFile, fontTexture);
                stream.Close();
            }
        }

        public void DrawPrettyStrings(List<Tekst2> TextToDisplay, FontRenderer FontRenderer, MouseState MouseState, SpriteBatch SpriteBatch)
        {
            PreviousStateOfMouse = CurrentStateOfMouse;
            CurrentStateOfMouse = MouseState.LeftButton;
            foreach (var item in TextToDisplay)
            {
                if (MouseState.LeftButton == ButtonState.Pressed && State != MenuStates.Moving && item.Rect.Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1)) && item.ShouldBeColoured)
                {
                    FontRenderer.DrawText(SpriteBatch, item.Rect.X, item.Rect.Y, item.Text, Color.Red);
                    if (PreviousStateOfMouse == ButtonState.Released && CurrentStateOfMouse == ButtonState.Pressed)
                    {
                        State = item.Stat;
                        NextState = item.NextStat;
                    }
                }
                else if (item.Rect.Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1)) && item.ShouldBeColoured && State != MenuStates.Moving)
                {
                    FontRenderer.DrawText(SpriteBatch, item.Rect.X, item.Rect.Y, item.Text, Color.Red);
                }
                else
                {
                    FontRenderer.DrawText(SpriteBatch, item.Rect.X, item.Rect.Y, item.Text, Color.Silver);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameWindow gameWindow)
        {
            var MouseState = Mouse.GetState();
            Rectangle Window = new Rectangle(0, 0, gameWindow.ClientBounds.Width, gameWindow.ClientBounds.Height);
            spriteBatch.Draw(Texture, Window, Color.White);
            VelocityOfMenu = 0;
            if (State == MenuStates.Moving)
            {
                // update will update position
                DrawPrettyStrings(TextToDisplay, FontRenderer, MouseState, spriteBatch);
                if (TextToDisplay.Count == 0)
                    State = NextState;
                return;
            }
            VelocityOfMenu = 0;
            DrawPrettyStrings(TextToDisplay, FontRenderer, MouseState, spriteBatch);


        }

        public void SortListIfNeeded(List<KeyValuePair<string, int>> ScoreList)
        {
            if (!AlreadySorted)
            {
                ScoreList.Sort(delegate(KeyValuePair<string, int> x, KeyValuePair<string, int> y)
                {
                    if (x.Value < y.Value)
                        return 1;
                    if (x.Value == y.Value)
                        return 0;
                    else return -1;
                });
                AlreadySorted = true;
            }
        }

        public void WriteListToFile(List<KeyValuePair<string, int>> ScoreList)
        {

            StreamWriter sw = new StreamWriter(@"Content/Scores.txt");
            sw.Flush();
            int i = 0;
            foreach (var item in ScoreList)
            {
                sw.WriteLine(item.Key + " " + item.Value);
                i++;
                if (i == 6 || i == ScoreList.Count)
                    break;
            }
           sw.Close();
        }

        public void ReadListFromFile(List<KeyValuePair<string, int>> ScoreList)
        {
            StringBuilder sb = new StringBuilder();
            StreamReader sr = new StreamReader(@"Content/Scores.txt");
            string s = sr.ReadLine();
            while (s != null)
            {
                ScoreList.Add(new KeyValuePair<string, int>(s.Split(' ')[0], Convert.ToInt32(s.Split(' ')[1])));
                s = sr.ReadLine();
            }
            sr.Close();
        }

        public void GenerateCredits()
        {
            TextToDisplay.Clear();
            TextToDisplay.Add(new Tekst2("Created by:", new Rectangle(40, 290, 240, 40), MenuStates.Nothing, MenuStates.Nothing, false));
            TextToDisplay.Add(new Tekst2("soivek", new Rectangle(90, 330, 240, 40), MenuStates.Nothing, MenuStates.Nothing, false));
            TextToDisplay.Add(new Tekst2("tygryseq", new Rectangle(90, 370, 240, 40), MenuStates.Nothing, MenuStates.Nothing, false));
            TextToDisplay.Add(new Tekst2("wozniakb2", new Rectangle(90, 420, 240, 40), MenuStates.Nothing, MenuStates.MainMenu, false));
            TextToDisplay.Add(new Tekst2("Back", new Rectangle(40, 540, 400, 40), MenuStates.Moving, MenuStates.EndGame, true));
        }

        public void GenerateEndMenu()
        {
            TextToDisplay.Clear();
            TextToDisplay.Add(new Tekst2("Retry", new Rectangle(90, 590, 240, 40), MenuStates.GameShouldStart, MenuStates.GameShouldStart, true));
            TextToDisplay.Add(new Tekst2("High scores", new Rectangle(90, 630, 240, 40), MenuStates.Moving, MenuStates.Scores, true));
            TextToDisplay.Add(new Tekst2("Back to main menu", new Rectangle(90, 670, 240, 40), MenuStates.Moving, MenuStates.MainMenu, true));
        }

        public void GenerateCurrentMenu(GameWindow gameWindow)
        {
            List<Tekst2> TemporaryText = new List<Tekst2>();
            TemporaryText.Clear();
            IEnumerator<Tekst2> ie = TextToDisplay.GetEnumerator();
            VelocityOfMenu =10;
            int currrentTextVelocity = VelocityOfMenu;
            while (ie.MoveNext())
            {
                currrentTextVelocity = currrentTextVelocity + 5;
                if (ie.Current.Rect.X + VelocityOfMenu <= gameWindow.ClientBounds.Width)
                {
                    TemporaryText.Add(new Tekst2(ie.Current.Text, new Rectangle(ie.Current.Rect.X + currrentTextVelocity, ie.Current.Rect.Y, ie.Current.Rect.Width, ie.Current.Rect.Height), ie.Current.Stat, ie.Current.NextStat, ie.Current.ShouldBeColoured));
                }
            }
            TextToDisplay = TemporaryText;
        }

        public void Update(Player Player, StartMenu startMenu, GameWindow gameWindow, GameElements gameElements, ContentManager Content, GraphicsDevice graphicsDevice)
        {
            if (!isGameOver)
            {
                if (!Player.IsAlive(gameWindow))
                    isGameOver = true;
            }
            if (State == MenuStates.MainMenu)
            {
                VelocityOfMenu = 0;
                isGameOver = false;
                gameElements.newGame = true;
            }
            if (State == MenuStates.Moving)
            {
                GenerateCurrentMenu(gameWindow);
            }
            if (State == MenuStates.Scores)
            {
                VelocityOfMenu = 0;
                GenerateCredits();
            }
            if (State == MenuStates.EndGame)
            {
                VelocityOfMenu = 0;
                GenerateEndMenu();
            }
            if (State == MenuStates.GameShouldStart)
            {
                VelocityOfMenu = 0;
                gameElements.newGame = true;
                XmlManager xml = new XmlManager("Game.xml");
                gameElements = xml.Load();
                StartMenu.MyName = StartMenu.prevname;
                startMenu.State = StartMenu.MenuState.GameShouldStart;
                startMenu.GameStarted = true;
                Player.points = 0;
            }
            string name = StartMenu.MyName;
            int score = Player.points;
            List<KeyValuePair<string, int>> ScoreList = new List<KeyValuePair<string, int>>();

            ReadListFromFile(ScoreList);
            ScoreList.Add(new KeyValuePair<string, int>(name, score));

            SortListIfNeeded(ScoreList);
            WriteListToFile(ScoreList);
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("Other/gameover");
            SpriteFont = content.Load<SpriteFont>("Fonts/ffont");
        }
    }
}
