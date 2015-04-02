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
    public class BossEnemy : NotAimingEnemy
    {
        #region składowe klasy
        [XmlIgnore]
        public WhereIGo Where = WhereIGo.movingLeft;
        [XmlIgnore]
        public WhereIGo WhereVertical = WhereIGo.movingDown;
        public int TimeFromPreviousShot = 0;
        public int Epson = 15;                          // to jest tolerancja pozycji, chodzi o to żeby nie było "drgań" w takich krytycznych momentach
        public int BaseHP;
        #endregion


        public BossEnemy() { BaseHP = this.HP; }

        public BossEnemy(int PositionX, int PositionY, int SizeX, int SizeY)
            : base(PositionX, PositionY, SizeX, SizeY)
        {
            BaseHP = this.HP;
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(texture_path);
        }
        public override void Update(GameTime gameTime, GameWindow gameWindow, Platforms platforms, ref int shift, Player Player, ContentManager Content)
        {
            this.UpdateTime(gameTime);
            Rectangle EnemyPlusHisRange = new Rectangle((int)(this.position.X) - Range, (int)(this.position.Y), (int)(this.size.X) + 2 * (int)(Range), (int)(this.size.Y));

            if (!(Math.Abs(Player.position.X - this.position.X) < Range) && Where == WhereIGo.movingLeft)
            {
                if (!(Math.Abs(position.X - Player.position.X) < Epson))
                {
                    if (this.canMoveLeft(gameWindow, platforms, shift))
                    {
                        position.X -= velocity.X;
                    }
                    if (position.X < Player.position.X)
                    {
                        Where = WhereIGo.movingRight;
                    }
                }
            }
            else if (!(Math.Abs(Player.position.X - this.position.X) < Range) && Where == WhereIGo.movingRight)
            {
                if (!(Math.Abs(position.X - Player.position.X) < Epson))
                {
                    if (this.canMoveRight(gameWindow, platforms, shift))
                    {
                        position.X += velocity.X;
                    }
                    if (position.X > Player.position.X)
                    {
                        Where = WhereIGo.movingLeft;
                    }
                }
            }

            if ((Math.Abs(Player.position.Y - this.position.Y) < Range))
            {
                return;
            }
            else if (WhereVertical == WhereIGo.movingDown)
            {
                if (Math.Abs(position.Y - Player.position.Y) < Epson)
                {
                    return;
                }
                if (this.canMoveDown(gameWindow, platforms, shift))
                {
                    position.Y += velocity.Y;
                }
                if (position.Y > Player.position.Y)
                {
                    WhereVertical = WhereIGo.movingUp;
                }
                return;
            }
            else if (WhereVertical == WhereIGo.movingUp)
            {
                if (Math.Abs(position.Y - Player.position.Y) < Epson)
                {
                    return;
                }
                if (this.canMoveUp(gameWindow, platforms, shift))
                {
                    position.Y -= velocity.Y;
                }
                if (position.Y < Player.position.Y)
                {
                    WhereVertical = WhereIGo.movingDown;
                }
                return;
            }
        }
        public void AddFewBulletsToBullets(Bullets bullets, ContentManager Content)
        {
            bullets.bullets.Add(GenerateBullet((int)this.position.X, (int)this.position.Y + (int)this.size.Y / 4, (int)BulletSize.X, (int)BulletSize.Y, (int)BulletVelocity.X, (int)BulletVelocity.Y, BulletDamage, this.GetType(), this.bulletTexture, Content));
            bullets.bullets.Add(GenerateBullet((int)this.position.X, (int)this.position.Y + (int)this.size.Y / 4, (int)BulletSize.X, (int)BulletSize.Y, (int)BulletVelocity.X, -(int)BulletVelocity.Y, BulletDamage, this.GetType(), this.bulletTexture, Content));
            bullets.bullets.Add(GenerateBullet((int)this.position.X, (int)this.position.Y + (int)this.size.Y / 4, (int)BulletSize.X, (int)BulletSize.Y, -(int)BulletVelocity.X, -(int)BulletVelocity.Y, BulletDamage, this.GetType(), this.bulletTexture, Content));
            bullets.bullets.Add(GenerateBullet((int)this.position.X, (int)this.position.Y + (int)this.size.Y / 4, (int)BulletSize.X, (int)BulletSize.Y, -(int)BulletVelocity.X, (int)BulletVelocity.Y, BulletDamage, this.GetType(), this.bulletTexture, Content));
            return;
        }
        public override void Attack(GameTime gameTime, Player player, Bullets bullets, ContentManager Content)
        {
            Rectangle EnemyPlusHisRange = new Rectangle((int)(this.position.X) - Range, (int)(this.position.Y), (int)(this.size.X) + 2 * Range, (int)(this.size.Y));
            if (this.HP >= 0.5 * BaseHP)
            {
                if (TimeFromPreviousShot == 0)
                {
                    TimeFromPreviousShot = gameTime.ElapsedGameTime.Seconds;
                }
                System.TimeSpan delay = new TimeSpan(0, 0, Delay);
                if ((Math.Abs((int)(gameTime.TotalGameTime.Seconds) - this.TimeFromPreviousShot) >= (int)(delay.Seconds)))
                {
                    System.Console.WriteLine("Omg I shoot around! Watch out, derp!");

                    AddFewBulletsToBullets(bullets, Content);
                    TimeFromPreviousShot = (int)(gameTime.TotalGameTime.Seconds);
                }
                    return;
                }

            else if (this.HP < 0.5 * BaseHP)
            {
                if (TimeFromPreviousShot == 0)
                {
                    TimeFromPreviousShot = gameTime.ElapsedGameTime.Seconds;
                }
                System.TimeSpan delay = new TimeSpan(0, 0, Delay/2);
                if ((Math.Abs((int)(gameTime.TotalGameTime.Seconds) - this.TimeFromPreviousShot) >= (int)(delay.Seconds)))
                {
                    System.Console.WriteLine("Omg I shoot around! Watch out, derp!");

                    AddFewBulletsToBullets(bullets, Content);

                    TimeFromPreviousShot = (int)(gameTime.TotalGameTime.Seconds);

                }

            }

        }
    }
}

