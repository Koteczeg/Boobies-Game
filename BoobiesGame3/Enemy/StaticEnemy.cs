using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel;
using System.Xml.Serialization;



namespace BoobiesGame3
{
    public class StaticEnemy : AimingEnemy
    {
        #region składowe klasy
        public int TimeFromPreviousShot = 0;
        public float rotation = 0;
        private StaticEnemyAnimation animation;
        public bool JustShot = false;
        #endregion

        public StaticEnemy() 
        {
            animation = new StaticEnemyAnimation(this);
        }

        public StaticEnemy(int PositionX, int PositionY, int SizeX, int SizeY) : base(PositionX, PositionY, SizeX, SizeY) { }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(texture_path);
            animation.LoadContent(content);
        }
        public override void Update(GameTime gameTime, GameWindow gameWindow, Platforms platforms, ref int shift, Player Player, ContentManager Content)
        {
            this.UpdateTime(gameTime);
            animation.Update(gameTime);
            return;
        }

        public Bullet GenerateBullet(int BulletSizeX, int BulletSizeY, float BulletVelocity, int BulletDamage, Type TypeOfBullet, string BulletTexture, ContentManager Content, Player Player)
        {
            float v = BulletVelocity;
            float BulletVelocityXModule = (float)(v * Math.Abs(this.position.X - Player.position.X - Player.size.X / 2) / (Math.Sqrt(Math.Pow((this.position.Y + this.size.Y / 2 - Player.position.Y - Player.size.Y / 2), 2) + Math.Pow((this.position.X - Player.position.X - Player.size.X / 2), 2))));
            float BulletVelocityYModule = (float)(v * Math.Abs(this.position.Y + this.size.Y / 2 - Player.position.Y - Player.size.Y / 2) / (Math.Sqrt(Math.Pow((this.position.Y + this.size.Y / 2 - Player.position.Y - Player.size.Y / 2), 2) + Math.Pow((this.position.X - Player.position.X - Player.size.X / 2), 2))));
            rotation = (float)Math.Atan((double)(BulletVelocityYModule / BulletVelocityXModule));
            if (position.X > Player.position.X)
            {
                rotation = (float)Math.PI - rotation;
                if (position.Y > Player.position.Y)
                {
                    rotation *= -1;
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
                    rotation *= -1;
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

        public override void Attack(GameTime gameTime, Player player, Bullets bullets, ContentManager Content)
        {
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
                    System.Console.WriteLine("Omg I shoot from rocket! Watch out, derp!");

                    Bullet BulletToAdd = GenerateBullet((int)this.BulletSize.X, (int)this.BulletSize.Y, v, this.BulletDamage, this.GetType(), this.bulletTexture, Content, player);
                    bullets.bullets.Add(BulletToAdd);
                    JustShot = true;
                    TimeFromPreviousShot = (int)(gameTime.TotalGameTime.Seconds);
                }
                else
                    JustShot = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, int shift)
        {
            animation.Draw(spriteBatch, shift);
        }
    }
}