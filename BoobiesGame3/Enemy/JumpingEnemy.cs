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
    public class JumpingEnemy : NotAimingEnemy
    {
        #region składowe klasy
        [XmlIgnore]
        public JumpManager Jumper;
        

        public int HowHighJumped = 0;
        public int TimeFromPreviousAttack = 0;
        public bool DidJumpEnd = true;
        #endregion

        #region TO LOAD FROM XML

        [XmlAttribute]
        public int MaxJumpHeight;
        [XmlAttribute("Direction")]
        public WhereIGo Where;
        #endregion

        public JumpingEnemy() 
        {
            Jumper = new JumpManager(this);
        }

        public JumpingEnemy(int PositionX, int PositionY, int SizeX, int SizeY) : base(PositionX, PositionY, SizeX, SizeY)
        {
        }

        public override void SetStats(Enemy enemy)
        {
            base.SetStats(enemy);
            this.MaxJumpHeight = ((JumpingEnemy)enemy).MaxJumpHeight;

        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(texture_path);
        }
        public override void Update(GameTime gameTime, GameWindow gameWindow, Platforms platforms, ref int shift, Player Player, ContentManager Content)
        {
            this.UpdateTime(gameTime);
            bool CouldMoveDown = canMoveDown(gameWindow, platforms, shift);
            if (Where == WhereIGo.movingLeft)
            {
                if (canMoveLeft(gameWindow, platforms, shift))
                    this.position.X -= velocity.X;
                if (!Jumper.Jump(gameWindow, platforms, shift, MaxJumpHeight) && !canMoveLeft(gameWindow, platforms, shift))
                    Jumper.jumpState = JumpManager.JumpState.rising;
            }
            else
            {
                if (canMoveRight(gameWindow, platforms, shift))
                    this.position.X += velocity.X;
                if (!Jumper.Jump(gameWindow, platforms, shift, MaxJumpHeight) && !canMoveRight(gameWindow, platforms, shift))
                    Jumper.jumpState = JumpManager.JumpState.rising;
            }
            if (!CouldMoveDown && canMoveDown(gameWindow, platforms, shift))
                Jumper.jumpState = JumpManager.JumpState.rising;
        }

        public override void Attack(GameTime gameTime, Player player, Bullets bullets, ContentManager Content)
        {
            Rectangle EnemyPlusHisRange = new Rectangle((int)(this.position.X) - Range, (int)(this.position.Y), (int)(this.size.X) + 2*Range, (int)(this.size.Y));
            if (!Rectangle.Intersect(EnemyPlusHisRange, convertToRectangle(player.position, player.size)).IsEmpty)
            {
                if (TimeFromPreviousAttack == 0)
                {
                    TimeFromPreviousAttack = gameTime.ElapsedGameTime.Seconds;
                }
                System.TimeSpan delay = new TimeSpan(0, 0, Delay);
                if (((int)(gameTime.TotalGameTime.Seconds) - this.TimeFromPreviousAttack >= (int)(delay.Seconds)) && player.position.X<this.position.X)
                {
                    System.Console.WriteLine("Omg I shoot! Watch out, derp!");
                    Bullet BulletToAdd = GenerateBullet((int)this.position.X, (int)this.position.Y + (int)this.size.Y / 4, (int)BulletSize.X, (int)BulletSize.Y, -(int)BulletVelocity.X, (int)BulletVelocity.Y, BulletDamage, this.GetType(), this.bulletTexture, Content);
                    bullets.bullets.Add(BulletToAdd);
                    TimeFromPreviousAttack = (int)(gameTime.TotalGameTime.Seconds);
                }
                else if (((int)(gameTime.TotalGameTime.Seconds) - this.TimeFromPreviousAttack >= (int)(delay.Seconds)) && player.position.X > this.position.X)
                {
                    System.Console.WriteLine("Omg I shoot! Watch out, derp!");
                    Bullet BulletToAdd = GenerateBullet((int)this.position.X + (int)this.size.X, (int)this.position.Y + (int)this.size.Y / 4, (int)BulletSize.X, (int)BulletSize.Y, (int)BulletVelocity.X, (int)BulletVelocity.Y, BulletDamage, this.GetType(), this.bulletTexture, Content);
                    bullets.bullets.Add(BulletToAdd);
                    TimeFromPreviousAttack = (int)(gameTime.TotalGameTime.Seconds);
                }

            }
        }

    }
}

