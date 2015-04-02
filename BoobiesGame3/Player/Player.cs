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

namespace BoobiesGame3
{
    public class Player : Unit
    {
        #region TO LOAD FROM XML
        [XmlAttribute]
        public int bonusVelocityDuration;
        [XmlAttribute]
        public int bonusVelocityAcceleration;
        [XmlAttribute(AttributeName = "HP")]
        public int fullHP;
        [XmlAttribute]
        public int attackDamage;
        [XmlAttribute]
        public int maxJumpHeight;
        [XmlAttribute]
        public int baseVelocity;
        [XmlAttribute]
        public string bulletTexturePath;
        #endregion
        //----------------------------------------------------------
        [XmlIgnore]
        public JumpManager Jumper;
        private int jumpHeight = 0;
        private PlayerAnimation playerAnimation;
        public enum PlayerDirection { right, left, up };
        public PlayerDirection playerDirection;
        public KeyboardState oldState = Keyboard.GetState();
        
        public Platform currentPlatform = new GluePlatform();
        public enum PlayerBonusShoot { one, three, five}
        public PlayerBonusShoot playerBonusShoot=PlayerBonusShoot.one;
        public bool IncreaseVelocityBonusChecker = false;
        public TimeSpan velocityBonusChecker = new TimeSpan(0);
        public TimeSpan velocityBonusTotalTime;
        public int HP;
        private const int playerScreenPosition = 400;
        public static int points=0;
        //-------------------------------------------------------------
        public Player() { ;}
        public Player(int x = 0, int y = 0, int width = 50, int height = 50, int hp = 1000, int damage = 100, int baseVelocity = 5)
            : base(x, y, height, width)
        {
            this.HP = hp;
            this.attackDamage = damage;
            this.baseVelocity = baseVelocity;
        }
        public void SetVelocity(GameWindow gameWindow, Platforms platforms, ref int shift, GameTime gameTime)
        {
            velocity.X = baseVelocity;
            velocity.Y = baseVelocity + 5;
            Platform platform = platforms.CurrentPlatform(this, gameWindow, shift);
            if (platform != null)
            {
                currentPlatform = platform;
                platform.MoveUnit(this, gameWindow, platforms, shift);
                changeShift(ref shift, gameWindow);
            }
            currentPlatform.ChangeVelocity(ref velocity);
            if (IncreaseVelocityBonusChecker==true)
            {
                velocity.X += bonusVelocityAcceleration;
                velocityBonusChecker += gameTime.ElapsedGameTime;
                if (velocityBonusChecker >= velocityBonusTotalTime)
                    IncreaseVelocityBonusChecker = false;
            }

        }
        private void PlayerJump(KeyboardState oldstate, KeyboardState newState, GameWindow gameWindow, Platforms platforms, int shift)
        {
            if (!Jumper.Jump(gameWindow, platforms, shift, jumpHeight) && newState.IsKeyDown(Keys.Space) && !oldstate.IsKeyDown(Keys.Space))
            {
                jumpHeight = 0;
                Jumper.jumpState = JumpManager.JumpState.rising;
            }
            if (newState.IsKeyDown(Keys.Space) && jumpHeight < maxJumpHeight)
                jumpHeight += 20;

        }
        public void SetPlayerDirection(KeyboardState oldState, KeyboardState newState)
        {
            if (newState.IsKeyDown(Keys.Up))
                playerDirection = PlayerDirection.up;
            else if (newState.IsKeyDown(Keys.Right))
                playerDirection = PlayerDirection.right;
            else if (newState.IsKeyDown(Keys.Left))
                playerDirection = PlayerDirection.left;
            else if (oldState.IsKeyDown(Keys.Up))
                playerDirection = PlayerDirection.right;
        }
        private void Shoot(KeyboardState oldstate, KeyboardState newState, PlayerBullets playerBullets, ContentManager content)
        {
            if (newState.IsKeyDown(Keys.LeftControl))
                if (!oldState.IsKeyDown(Keys.LeftControl))
                {
                    if (playerBonusShoot == PlayerBonusShoot.five)
                    playerBullets.InitializeFiveBulletsShoot(this, content);
                    else if (playerBonusShoot == PlayerBonusShoot.three)
                        playerBullets.InitializeThreeBulletsShoot(this, content);
                    else
                        playerBullets.InitializeOneBulletShoot(this, content);
                }
                    
        }
        public bool IsAlive(GameWindow gameWindow) 
        { 
            return (this.HP > 0 && this.position.Y < gameWindow.ClientBounds.Bottom + this.size.Y);
        }

        public void LoadContent(ContentManager content)
        {
            playerAnimation.LoadContent(content);
        }
        public new void Draw(SpriteBatch spriteBatch, int shift)
        {
            playerAnimation.Draw(spriteBatch, shift);
            
        }

        private void GetHurt(Enemies enemies, Bullets bullets)
        {
            this.HP -= enemies.DamageDealt(this, bullets);
        }

        public int DamageDealt(PlayerBullets bullets, Enemy enemy)
        {
            return bullets.DamageDealt(enemy);
        }

        public void Update(GameTime gameTime, GameWindow gameWindow, Platforms platforms, ref int shift, PlayerBullets playerBullets,
            ContentManager content, Enemies enemies, Bullets bullets)
        {
            playerAnimation.Update(gameTime);
            GetHurt(enemies, bullets);
            KeyboardState newState = Keyboard.GetState();
            SetPlayerDirection(oldState, newState);
            PlayerJump(oldState, newState, gameWindow, platforms, shift);
            SetVelocity(gameWindow, platforms, ref shift, gameTime);
            Shoot(oldState, newState, playerBullets, content);
            if (canMoveRight(gameWindow, platforms, shift) && newState.IsKeyDown(Keys.Right))
            {
                position.X += velocity.X;
                changeShift(ref shift, gameWindow);
            }
            if (canMoveLeft(gameWindow, platforms, shift) && newState.IsKeyDown(Keys.Left))
            {
                position.X -= velocity.X;
                changeShift(ref shift, gameWindow);
            }
            oldState = newState;
            EndGame();
        }

        public void changeShift(ref int shift, GameWindow gameWindow)
        {
            int onScreenX = (int)position.X - shift;
            if (onScreenX > playerScreenPosition)
                shift += onScreenX - playerScreenPosition;
        }

        public void Initialize()
        {
            velocityBonusTotalTime = new TimeSpan(0, 0, bonusVelocityDuration);
            this.HP = this.fullHP;
            playerAnimation = new PlayerAnimation(this); 
            Jumper = new JumpManager(this);
        }

        public void EndGame()
        {
            if (this.position.X > 20800)
                this.HP = 0;
        }
    }
}










