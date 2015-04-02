using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;
using System.Xml;
using System.ComponentModel;

namespace BoobiesGame3
{
    public class GameElements
    {
        int shift = 0;
        public Vector2 Resolution;
        public Background Background;
        public Player Player;
        public Platforms Platforms;
        public Enemies Enemies;
        public PlayerBonuses PlayerBonuses;

        [XmlIgnore]
        public HpBar HpBar;
        [XmlIgnore]
        public bool newGame = false;        
        [XmlIgnore]
        public StartMenu startMenu;
        [XmlIgnore]
        public GameOver gameOver;
        [XmlIgnore]
        public PlayerBullets playerBullets;
        
        [XmlIgnore]
        public Bullets Bullets;

        [XmlIgnore]
        public SpriteFont Font;

        public GameElements() { ;}

        public void Initialize(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            startMenu = new StartMenu(Content);
            gameOver = new GameOver(Content);
            playerBullets = new PlayerBullets();
            Bullets = new Bullets();
            HpBar = new HpBar();
            Player.Initialize();
            HpBar.Initialize(Player, graphicsDevice);
            Background.Initialize(this.Resolution);
            Font = Content.Load<SpriteFont>("Font1");
        }


        public void Draw(GameTime gameTime ,SpriteBatch spriteBatch, GameWindow gameWindow, ContentManager Content)
        {

            if (!startMenu.GameStarted)
                startMenu.Draw(gameTime,spriteBatch,Content, gameWindow);
            else if (gameOver.isGameOver)
                gameOver.Draw(spriteBatch,gameWindow);
            else
            {
                Background.Draw(spriteBatch, shift);
                Platforms.Draw(spriteBatch, gameWindow, shift);
                playerBullets.Draw(gameWindow, spriteBatch, shift);
                Player.Draw(spriteBatch, shift);
               
                Platforms.Draw(spriteBatch, gameWindow, shift);
                Enemies.Draw(gameWindow, spriteBatch, shift);
                PlayerBonuses.Draw(gameWindow, spriteBatch, shift);
                Bullets.Draw(gameWindow, spriteBatch, shift);
                HpBar.Draw(spriteBatch, Player, shift);
                spriteBatch.DrawString(Font, " Points: " + Player.points, new Vector2(0, 0), Color.Bisque);
                //spriteBatch.End();
                //spriteBatch.Begin();
                // nie wiem czemu ale dość kłopotliwe jest pisanie tego tekstu (tzn. to gdzie znajduje się linijka
                // z "DrawString" jest bardzo kluczowe. Doszedłem do wniosku że jeśli umieszcza się ją na końcu funkcji rysującej cokolwiek
                // to powinno być ok.
                // na youtubie wyczytalem że kluczowe jest otwieranie i zamykanie spritebatcha przy pisaniu tekstu, to działało ale
                // Sławek chyba do rysowania otwiera spritebatcha w jakims innym trybie, bo jak go pozniej otworze na nowo to pozostałe rzeczy
                // jakos dziwnie się rysują.

                // ogolna zasada dodawania nowej czcionki: tworzymy nowy projekt w projekcie, dodajemy tam item ktory jest typu SpriteFont (zakładka c#), odpowiednio modyfikujemy dodany tam plik,
                // następnie zapisujemy i budujemy projekt. Teraz w folderze z projektem odnajdź plik nazwa_pliku_ktory_dodales.xnb i załącz go do głównego projektu. Nastepnie skojarz SpriteFont z tym plikiem
                // i można już pisać.
            }
        }

        public void Update(GameTime gameTime, GameWindow gameWindow, ContentManager content, GraphicsDevice graphicsDevice)
        {
            gameOver.Update(Player, startMenu, gameWindow,this,content, graphicsDevice);
            if (!startMenu.GameStarted)
                startMenu.Update(gameTime, gameWindow);
            else if (!gameOver.isGameOver)
            {
                Player.Update(gameTime, gameWindow, Platforms, ref shift, playerBullets, content, Enemies, Bullets);
                playerBullets.Update(gameTime, gameWindow, Platforms, ref shift);
                Platforms.Update(gameTime, gameWindow, Player, shift);
                Enemies.Update(gameTime, gameWindow, Platforms, shift, Player, playerBullets, content);
                PlayerBonuses.Update(gameTime, gameWindow, Platforms, shift, Player,  content);
                Bullets.Update(gameTime, gameWindow, Platforms, shift);
                Enemies.Attack(gameTime, Player, Bullets, content);
                HpBar.Update(Player);
            }
        }

        public void LoadContent(ContentManager content)
        {
            gameOver.LoadContent(content);
            startMenu.LoadContent(content);
            Background.LoadContent(content);
            Player.LoadContent(content);
            Platforms.LoadContent(content);
            playerBullets.LoadContent(content);
            Enemies.LoadContent(content);
            PlayerBonuses.LoadContent(content);
            Bullets.LoadContent(content);
        }
    }
}
