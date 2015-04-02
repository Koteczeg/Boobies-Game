using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Xml.Serialization;




namespace BoobiesGame3
{
    [Serializable]
    public class NotSmartEnemy : Enemy, IAnimation
    {
        #region składowe klasy
        public enum StateOfMoving { notfalling, falling, attacking };
        private AnimatedSprite animatedSprite;

        [XmlIgnore]
        public StateOfMoving stateOfMoving = StateOfMoving.falling;
        public StateOfMoving PreviousStateOfMoving = StateOfMoving.falling;

        public TimeSpan DelayForRisingAndFalling = new TimeSpan(0, 0, 0, 0, 150);
        public TimeSpan DelayForShooting = new TimeSpan(0, 0, 0, 0, 75);
        public TimeSpan DelayToGenerateBulletSinceShootingStarted = new TimeSpan(0, 0, 0, 0, 540);
        public TimeSpan Timer1 = new TimeSpan(0);
        public TimeSpan Timer2 = new TimeSpan(0);
        public int TimeFromPreviousAttack = 0;
        public int dmg = 0;
        #endregion
       
        #region TO LOAD FROM XML
        [XmlAttribute]
        public int AttackDamage;
        [XmlAttribute]
        public int Range;
        [XmlAttribute("Direction")]
        public WhereIGo Where;
        #endregion

        #region BASIC
        public NotSmartEnemy() { ;}

        public NotSmartEnemy(int PositionX, int PositionY, int SizeX, int SizeY)
            : base(PositionX, PositionY, SizeX, SizeY)
        {
        }

        public override void SetStats(Enemy enemy)
        {
            base.SetStats(enemy);
            this.AttackDamage = ((NotSmartEnemy)enemy).AttackDamage;
            this.Range = ((NotSmartEnemy)enemy).Range;

        }
        #endregion

        #region IMPLEMENTATION OF IANIMATION

        public void LoadContentForAnimation(ContentManager content)
        {
            switch (stateOfMoving)
            {
                case StateOfMoving.notfalling:
                    Texture2D texture = content.Load<Texture2D>("AnimationSprites/NotSmartEnemy/notsmartenemy_basic");
                    animatedSprite = new AnimatedSprite(texture, 1, 6);
                    break;
                case StateOfMoving.falling:
                    Texture2D texture2 = content.Load<Texture2D>("AnimationSprites/NotSmartEnemy/notsmartenemy_basic");
                    animatedSprite = new AnimatedSprite(texture2, 1, 6);
                    break;
                case StateOfMoving.attacking:
                    Texture2D texture3 = content.Load<Texture2D>("AnimationSprites/NotSmartEnemy/notsmartenemy_attacking");
                    animatedSprite = new AnimatedSprite(texture3, 1, 7);
                    break;
        }
        }

        public void DrawAnimation(SpriteBatch spriteBatch, int shift)
        {
            switch (stateOfMoving)
            {
                case StateOfMoving.notfalling:
                    switch (Where)
                    {
                        case WhereIGo.movingLeft:
                            animatedSprite.Draw(spriteBatch, new Vector2(this.position.X - shift, this.position.Y));
                            break;
                        case WhereIGo.movingRight:
                            animatedSprite.DrawMirror(spriteBatch, new Vector2(this.position.X - shift, this.position.Y));
                            break;
                    }
                    break;
                case StateOfMoving.falling:
                    switch (Where)
                    {
                        case WhereIGo.movingLeft:
                            animatedSprite.Draw(spriteBatch, new Vector2(this.position.X - shift, this.position.Y));
                            break;
                        case WhereIGo.movingRight:
                            animatedSprite.DrawMirror(spriteBatch, new Vector2(this.position.X - shift, this.position.Y));
                            break;
                    }                    break;
                case StateOfMoving.attacking:
                    switch (Where)
                    {
                        case WhereIGo.movingLeft:
                            animatedSprite.Draw(spriteBatch, new Vector2(this.position.X - shift, this.position.Y - 55));
                            break;
                        case WhereIGo.movingRight:
                            animatedSprite.DrawMirror(spriteBatch, new Vector2(this.position.X - shift, this.position.Y - 55));
                            break;
            }
                    break;
            }
        }

        public void UpdateAnimation(GameTime gameTime, ContentManager content)
            {
            switch (stateOfMoving)
            {
                case StateOfMoving.falling:
                    Timer1 += gameTime.ElapsedGameTime;
                    if (Timer1 > DelayForRisingAndFalling)
                    {
                        Timer1 -= DelayForRisingAndFalling;
                        animatedSprite.Update();
                    }
                    break;
                case StateOfMoving.notfalling:
                    Timer1 += gameTime.ElapsedGameTime;
                    if (Timer1 > DelayForRisingAndFalling)
                    {
                        Timer1 -= DelayForRisingAndFalling;
                        animatedSprite.Update();
                }
                    break;
                case StateOfMoving.attacking:
                    Timer1 += gameTime.ElapsedGameTime;
                    if (Timer1 > DelayForShooting)
                    {
                        Timer1 -= DelayForShooting;
                        int pomocnicze_klatki = animatedSprite.currentFrame;
                        animatedSprite.Update();
                        if (pomocnicze_klatki + 1 >= animatedSprite.totalFrames)
                        {
                            this.stateOfMoving = StateOfMoving.notfalling;
                            this.LoadContent(content);
                        }
                    }
                    break;
            }
        }

        #endregion

        #region IMPLEMENTACJA METOD Z KLASY BAZOWEJ

        public override void LoadContent(ContentManager content)
                {
            LoadContentForAnimation(content);
                }

        public override void Draw(SpriteBatch spriteBatch, int shift)
        {
            DrawAnimation(spriteBatch, shift);
        }

        public override void Update(GameTime gameTime, GameWindow gameWindow, Platforms platforms, ref int shift, Player Player, ContentManager Content)
        {
            UpdateAnimation(gameTime, Content);

            if (Where == WhereIGo.movingLeft)
            {
                if (this.canMoveLeft(gameWindow, platforms, shift))
                    this.position.X -= velocity.X;
                else
                    Where = WhereIGo.movingRight;
            }
            if (Where == WhereIGo.movingRight)
            {
                if (this.canMoveRight(gameWindow, platforms, shift))
                    this.position.X += velocity.X;
                else
                    Where = WhereIGo.movingLeft;
            }
            if (this.stateOfMoving == StateOfMoving.attacking)
                return;
            if (this.canMoveDown(gameWindow, platforms, shift))
            {
                this.position.Y += velocity.Y;
                this.stateOfMoving = StateOfMoving.falling;
            }
            else
                this.stateOfMoving = StateOfMoving.notfalling;
            }
        #endregion

        #region POMOCNICZE METODY

        public override void Attack(GameTime gameTime, Player player, Bullets bullets, ContentManager Content)
        {
            Rectangle EnemyPlusHisRange = new Rectangle((int)(this.position.X) - Range, (int)(this.position.Y), (int)(this.size.X) + Range, (int)(this.size.Y));
            if (!Rectangle.Intersect(EnemyPlusHisRange, convertToRectangle(player.position, player.size)).IsEmpty && this.position.X > player.position.X)
            {
                if (TimeFromPreviousAttack == 0)
                {
                    TimeFromPreviousAttack = gameTime.ElapsedGameTime.Seconds;
                }
                System.TimeSpan delay = new TimeSpan(0, 0, Delay);
                if (((int)(gameTime.TotalGameTime.Seconds) - this.TimeFromPreviousAttack >= (int)(delay.Seconds)))
                {
                    System.Console.WriteLine("I'm assassin! Watch my razor.");
                    stateOfMoving = StateOfMoving.attacking;
                    Texture2D texture = Content.Load<Texture2D>("AnimationSprites/NotSmartEnemy/notsmartenemy_attacking");
                    animatedSprite = new AnimatedSprite(texture, 1, 7);
                    dmg += AttackDamage;
                    TimeFromPreviousAttack = (int)(gameTime.TotalGameTime.Seconds);
                }
            }
        }
        public override int DamageDealt(Player player)
        {
            int temporary = dmg;
            dmg = 0;
            return temporary;
        }
        #endregion

    }
}
