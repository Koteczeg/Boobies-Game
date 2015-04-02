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
using System.ComponentModel;



namespace BoobiesGame3
{
    public class FlyingEnemy : AimingEnemy, IAnimation
    {
        #region składowe klasy
        private AnimatedSprite animatedSprite;

        public enum FlyingEnemyState { rising, falling, shooting };
        [XmlIgnore]
        public FlyingEnemyState StateOfMoving = FlyingEnemyState.rising;
        public FlyingEnemyState PreviousStateOfMoving = FlyingEnemyState.rising;
        
        public TimeSpan DelayForRisingAndFalling = new TimeSpan(0, 0, 0, 0, 150);
        public TimeSpan DelayForShooting = new TimeSpan(0, 0, 0, 0, 45);
        public TimeSpan DelayToGenerateBulletSinceShootingStarted = new TimeSpan(0, 0, 0, 0, 540);
        public TimeSpan Timer1 = new TimeSpan(0);
        public TimeSpan Timer2 = new TimeSpan(0);

        public int HowHighFromStartLevel = 0;
        public int TimeFromPreviousShot = 0;
        public const int DeltaX = 10;
        #endregion

        #region TO LOAD FROM XML
        [XmlAttribute]
        public int MaxHeight;
        [XmlAttribute("Direction")]
        public WhereIGo Where;
        #endregion

        #region BASIC
        public FlyingEnemy() {}

        public FlyingEnemy(int PositionX, int PositionY, int SizeX, int SizeY) : base(PositionX, PositionY, SizeX, SizeY) { }

        public override void SetStats(Enemy enemy)
        {
            base.SetStats(enemy);
            this.MaxHeight = ((FlyingEnemy)enemy).MaxHeight;
        }
        #endregion

        #region IMPLEMENTATION OF IANIMATION
        public void LoadContentForAnimation(ContentManager content)
        {
            switch (StateOfMoving)
            {
                case FlyingEnemyState.shooting:
                    Texture2D texture = content.Load<Texture2D>("AnimationSprites/FlyingEnemy/flyingenemy_shooting");
                    animatedSprite = new AnimatedSprite(texture, 1, 12);
                    break;
                case FlyingEnemyState.falling:
                    Texture2D texture2 = content.Load<Texture2D>("AnimationSprites/FlyingEnemy/flyingenemy_basic");
                    animatedSprite = new AnimatedSprite(texture2, 1, 6);
                    break;
                case FlyingEnemyState.rising:
                    Texture2D texture3 = content.Load<Texture2D>("AnimationSprites/FlyingEnemy/flyingenemy_basic");
                    animatedSprite = new AnimatedSprite(texture3, 1, 6);
                    break;
            }
        }

        public void UpdateAnimation(GameTime gameTime, ContentManager content)
        {
            switch (StateOfMoving)
            {
                case FlyingEnemyState.rising:
                    Timer1 += gameTime.ElapsedGameTime;
                    if (Timer1 > DelayForRisingAndFalling)
                    {
                        Timer1 -= DelayForRisingAndFalling;
                        animatedSprite.Update();
                    }
                    break;
                case FlyingEnemyState.falling:
                    Timer1 += gameTime.ElapsedGameTime;
                    if (Timer1 > DelayForRisingAndFalling)
                    {
                        Timer1 -= DelayForRisingAndFalling;
                        animatedSprite.Update();
                    }
                    break;
                case FlyingEnemyState.shooting:
                    Timer1 += gameTime.ElapsedGameTime;
                    if (Timer1 > DelayForShooting)
                    {
                        Timer1 -= DelayForShooting;
                        int pomocnicze_klatki = animatedSprite.currentFrame;
                        animatedSprite.Update();
                        if (pomocnicze_klatki + 1 >= animatedSprite.totalFrames)
                        {
                            this.StateOfMoving = this.PreviousStateOfMoving;
                            this.LoadContent(content);
                        }
                    }
                    break;
            }
        }

        public void DrawAnimation(SpriteBatch spriteBatch, int shift)
        {
            animatedSprite.Draw(spriteBatch, new Vector2(this.position.X - shift, this.position.Y));
        }
        #endregion

        #region IMPLEMENTACJA METOD Z KLASY BAZOWEJ
        public override void LoadContent(ContentManager content)
        {
            LoadContentForAnimation(content);
        }

        public override void Update(GameTime gameTime, GameWindow gameWindow, Platforms platforms, ref int shift, Player Player, ContentManager content)
        {
            UpdateAnimation(gameTime, content);

            this.UpdateTime(gameTime);
            #region poruszanie sie
            if (StateOfMoving == FlyingEnemyState.rising)
            {
                if (this.canMoveLeft(gameWindow, platforms, shift) && Where==WhereIGo.movingLeft)
                {
                    position.X -= (int)(velocity.X);
                }
                if (this.canMoveRight(gameWindow, platforms, shift) && Where == WhereIGo.movingRight)
                {
                    position.X += (int)(velocity.X);
                }
                if (this.canMoveUp(gameWindow, platforms, shift))
                {
                    position.Y -= (int)(velocity.Y);
                }
                HowHighFromStartLevel += (int)(velocity.Y);
                if (HowHighFromStartLevel >= MaxHeight)
                {
                    StateOfMoving = FlyingEnemyState.falling;
                }
            }
            if (StateOfMoving == FlyingEnemyState.falling)
            {
                if (this.canMoveLeft(gameWindow, platforms, shift) && Where == WhereIGo.movingLeft)
                {
                    position.X -= (int)(velocity.X);
                }
                if (this.canMoveRight(gameWindow, platforms, shift) && Where == WhereIGo.movingRight)
                {
                    position.X += (int)(velocity.X);
                }
                if (this.canMoveDown(gameWindow, platforms, shift))
                {
                    position.Y += (int)(velocity.Y);
                }
                HowHighFromStartLevel -= (int)(velocity.Y);
                if (HowHighFromStartLevel <= 0)
                {
                    StateOfMoving = FlyingEnemyState.rising;
                }
            }
            #endregion
                }

        public override void Draw(SpriteBatch spriteBatch, int shift)
        {
            DrawAnimation(spriteBatch, shift);
            }
        #endregion

        #region POMOCNICZE METODY
        [XmlIgnore]
        public Bullet BulletToAdd;
        [XmlIgnore]
        public bool IWillShootInTheNearestFuture = false;

        public override void Attack(GameTime gameTime, Player player, Bullets bullets, ContentManager Content)
        {
            Timer2 += gameTime.ElapsedGameTime;
            if (IWillShootInTheNearestFuture && Timer2 > DelayToGenerateBulletSinceShootingStarted)
            {
                bullets.bullets.Add(BulletToAdd);
                BulletToAdd = null;
                IWillShootInTheNearestFuture = false;
            }
            float v = this.BulletVelocity;
            Rectangle EnemyPlusHisRange = new Rectangle((int)(this.position.X) - Range, (int)(this.position.Y) - Range, (int)(this.size.X) + 2 * Range, (int)(this.size.Y) + 2 * Range);
            if (!Rectangle.Intersect(EnemyPlusHisRange, convertToRectangle(player.position, player.size)).IsEmpty)
            {
                if (TimeFromPreviousShot == 0)
                {
                    TimeFromPreviousShot = gameTime.ElapsedGameTime.Seconds;
                }
                System.TimeSpan delay = new TimeSpan(0, 0, Delay);
                if (((int)(gameTime.TotalGameTime.Seconds) - this.TimeFromPreviousShot >= (int)(delay.Seconds)))
                {
                    if (StateOfMoving != FlyingEnemyState.shooting)
                    {
                        this.PreviousStateOfMoving = StateOfMoving;
                    }
                    StateOfMoving = FlyingEnemyState.shooting;
                    Timer2 = new TimeSpan(0);
                    Texture2D texture = Content.Load<Texture2D>("AnimationSprites/FlyingEnemy/flyingenemy_shooting");
                    animatedSprite = new AnimatedSprite(texture, 1, 12);
                    BulletToAdd = GenerateBullet((int)this.BulletSize.X, (int)this.BulletSize.Y, v, this.BulletDamage, this.GetType(), this.bulletTexture, Content, player);
                    IWillShootInTheNearestFuture = true;
                    TimeFromPreviousShot = (int)(gameTime.TotalGameTime.Seconds);
                }
            }
        }

        public Bullet GenerateBullet(int BulletSizeX, int BulletSizeY, float BulletVelocity, int BulletDamage, Type TypeOfBullet, string BulletTexture, ContentManager Content, Player Player)
        {
            float v = BulletVelocity;
            // tu są obliczenia żeby celowało dokładnie w playera, sorry ale ładniej sie nie da.
            float BulletVelocityXModule = (float)(v * Math.Abs(this.position.X - Player.position.X - Player.size.X / 2) / (Math.Sqrt(Math.Pow((this.position.Y + this.size.Y / 2 - Player.position.Y - Player.size.Y / 2), 2) + Math.Pow((this.position.X - Player.position.X - Player.size.X / 2), 2))));
            float BulletVelocityYModule = (float)(v * Math.Abs(this.position.Y + this.size.Y / 2 - Player.position.Y - Player.size.Y / 2) / (Math.Sqrt(Math.Pow((this.position.Y + this.size.Y / 2 - Player.position.Y - Player.size.Y / 2), 2) + Math.Pow((this.position.X - Player.position.X - Player.size.X / 2), 2))));
            if (position.X > Player.position.X)
            {
                if (position.Y > Player.position.Y)
                {
                    Bullet BulletToAdd = GenerateBullet((int)this.position.X, (int)this.position.Y + (int)this.size.Y / 2, (int)BulletSize.X, (int)BulletSize.Y, (float)(-BulletVelocityXModule), (float)(-BulletVelocityYModule), BulletDamage, this.GetType(), this.bulletTexture, Content);
                    return BulletToAdd;
                }
                else
                {
                    Bullet BulletToAdd = GenerateBullet((int)this.position.X, (int)this.position.Y + (int)this.size.Y / 2, (int)BulletSize.X, (int)BulletSize.Y, (float)(-BulletVelocityXModule), (float)(BulletVelocityYModule), BulletDamage, this.GetType(), this.bulletTexture, Content);
                    return BulletToAdd;
                }
            }
            else
            {
                if (position.Y > Player.position.Y)
                {
                    Bullet BulletToAdd = GenerateBullet((int)this.position.X, (int)this.position.Y + (int)this.size.Y / 2, (int)BulletSize.X, (int)BulletSize.Y, (float)(BulletVelocityXModule), (float)(-BulletVelocityYModule), BulletDamage, this.GetType(), this.bulletTexture, Content);
                    return BulletToAdd;
                }
                else
                {
                    Bullet BulletToAdd = GenerateBullet((int)this.position.X, (int)this.position.Y + (int)this.size.Y / 2, (int)BulletSize.X, (int)BulletSize.Y, (float)(BulletVelocityXModule), (float)(BulletVelocityYModule), BulletDamage, this.GetType(), this.bulletTexture, Content);
                    return BulletToAdd;
                }
            }
                }
#endregion

    }
}
