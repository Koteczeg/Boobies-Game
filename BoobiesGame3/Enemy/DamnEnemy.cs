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
    public class DamnEnemy : NotAimingEnemy
    {
        #region składowe klasy
        [XmlIgnore]
        public WhereIGo Where = WhereIGo.movingLeft;
        public int TimeFromPreviousShot = 0;
        public int Epson = 15;                          // to jest tolerancja pozycji, chodzi o to żeby nie było "drgań" w takich krytycznych momentach
        #endregion

        public DamnEnemy() { }

        public DamnEnemy(int PositionX, int PositionY, int SizeX, int SizeY) : base(PositionX, PositionY, SizeX, SizeY) { }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(texture_path);
        }
        public override void Update(GameTime gameTime, GameWindow gameWindow, Platforms platforms, ref int shift, Player Player, ContentManager Content)
        {
            this.UpdateTime(gameTime);
            Rectangle EnemyPlusHisRange = new Rectangle((int)(this.position.X) - Range, (int)(this.position.Y), (int)(this.size.X) + 2 * (int)(Range), (int)(this.size.Y));
            if (this.canMoveDown(gameWindow, platforms, shift))
            {
                position.Y += velocity.Y;
                return;
            }
            else if ((Math.Abs(Player.position.X - this.position.X) < Range))
            {
                return;
            }
            else if (Where == WhereIGo.movingLeft && !this.canMoveDown(gameWindow, platforms, shift))
            {
                if (Math.Abs(position.X - Player.position.X) < Epson)
                {
                    return;
                }
                if (this.canMoveLeft(gameWindow, platforms, shift))
                {
                    position.X -= velocity.X;
                }
                if (position.X < Player.position.X)
                {
                    Where = WhereIGo.movingRight;
                }
                return;
            }
            else if (Where == WhereIGo.movingRight && !this.canMoveDown(gameWindow, platforms, shift))
            {
                if (Math.Abs(position.X - Player.position.X) < Epson)
                {
                    return;
                }
                if (this.canMoveRight(gameWindow, platforms, shift))
                {
                    position.X += velocity.X;
                }
                if (position.X > Player.position.X)
                {
                    Where = WhereIGo.movingLeft;
                }
                return;
            }
            else if (Where == WhereIGo.movingLeft && this.canMoveDown(gameWindow, platforms, shift))
            {
                if (Math.Abs(position.X - Player.position.X) < Epson)
                {
                    return;
                }
                if (this.canMoveLeft(gameWindow, platforms, shift))
                {
                    position.X -= velocity.X;
                }
                position.Y += velocity.Y;
                if (position.X < Player.position.X)
                {
                    Where = WhereIGo.movingRight;
                }
                return;
            }
            else if (Where == WhereIGo.movingRight && this.canMoveDown(gameWindow, platforms, shift))
            {
                if (Math.Abs(position.X - Player.position.X) < Epson)
                {
                    return;
                }
                if (this.canMoveRight(gameWindow, platforms, shift))
                {
                    position.X += velocity.X;
                }
                position.Y += velocity.Y;
                if (position.X > Player.position.X)
                {
                    Where = WhereIGo.movingLeft;
                }
                return;
            }
        }
        public override void Attack(GameTime gameTime, Player player, Bullets bullets, ContentManager Content)
        {
            Rectangle EnemyPlusHisRange = new Rectangle((int)(this.position.X) - Range, (int)(this.position.Y), (int)(this.size.X) + 2 * Range, (int)(this.size.Y));
            if (!Rectangle.Intersect(EnemyPlusHisRange, convertToRectangle(player.position, player.size)).IsEmpty)
            {
                if (TimeFromPreviousShot == 0)
                {
                    TimeFromPreviousShot = gameTime.ElapsedGameTime.Seconds;
                }
                System.TimeSpan delay = new TimeSpan(0, 0, Delay);
                if (((int)(gameTime.TotalGameTime.Seconds) - this.TimeFromPreviousShot >= (int)(delay.Seconds)))
                {
                    System.Console.WriteLine("Omg I shoot! Watch out, derp!");
                    if (position.X > player.position.X)
                    {
                        Bullet BulletToAdd = GenerateBullet((int)this.position.X, (int)this.position.Y + (int)this.size.Y / 4, (int)BulletSize.X, (int)BulletSize.Y, -(int)BulletVelocity.X, (int)BulletVelocity.Y, BulletDamage, this.GetType(), this.bulletTexture, Content);
                        bullets.bullets.Add(BulletToAdd);
                        TimeFromPreviousShot = (int)(gameTime.TotalGameTime.Seconds);
                    }
                    else
                    {
                        Bullet BulletToAdd = GenerateBullet((int)this.position.X + (int)this.size.X, (int)this.position.Y + (int)this.size.Y / 4, (int)BulletSize.X, (int)BulletSize.Y, (int)BulletVelocity.X, (int)BulletVelocity.Y, BulletDamage, this.GetType(), this.bulletTexture, Content);
                        bullets.bullets.Add(BulletToAdd);
                        TimeFromPreviousShot = (int)(gameTime.TotalGameTime.Seconds);
                    }
                }
            }
        }
    }
}
