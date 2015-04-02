using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace BoobiesGame3
{
    public class PlayerAnimation : IAnimation
    {
        
        private Texture2D[] texturePlayerRight=new Texture2D[6];
        private Texture2D[] texturePlayerLeft = new Texture2D[6];
        private Texture2D[] texturePlayerJump = new Texture2D[4];
        private Texture2D texturePlayerStanding, texturePlayerStandingLeft,texturePlayerStandingUp;
        
        private Player player;
        private int current = 1, current2=1;
        private TimeSpan delay = new TimeSpan(0, 0, 0, 0, 150);
        private TimeSpan time = new TimeSpan(0);

        public PlayerAnimation(Player player)
        {
            this.player = player;
        }

        public void LoadContent(ContentManager content)
        {
            texturePlayerRight[0] = content.Load<Texture2D>("Player/P1");
            texturePlayerRight[1] = content.Load<Texture2D>("Player/P2");
            texturePlayerRight[2] = content.Load<Texture2D>("Player/P3");
            texturePlayerRight[3] = content.Load<Texture2D>("Player/P4");
            texturePlayerRight[4] = content.Load<Texture2D>("Player/P5");
            texturePlayerRight[5] = content.Load<Texture2D>("Player/P6");
            texturePlayerLeft[0] = content.Load<Texture2D>("Player/L1");
            texturePlayerLeft[1] = content.Load<Texture2D>("Player/L2");
            texturePlayerLeft[2] = content.Load<Texture2D>("Player/L3");
            texturePlayerLeft[3] = content.Load<Texture2D>("Player/L4");
            texturePlayerLeft[4] = content.Load<Texture2D>("Player/L5");
            texturePlayerLeft[5] = content.Load<Texture2D>("Player/L6");
            texturePlayerStanding = content.Load<Texture2D>("Player/Standing");
            texturePlayerStandingLeft = content.Load<Texture2D>("Player/StandingLeft");
            texturePlayerStandingUp = content.Load<Texture2D>("Player/StandingUp");
            texturePlayerJump[0] = content.Load<Texture2D>("Player/Jump1");
            texturePlayerJump[1] = content.Load<Texture2D>("Player/Jump2");
            texturePlayerJump[2] = content.Load<Texture2D>("Player/Jump3");
            texturePlayerJump[3] = content.Load<Texture2D>("Player/Jump4");
        }

        public void Update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime;
            if (time > delay)
            {
                time -= delay;
                current++;
                current2++;
                if (current >= 6)
                    current = 0;
                if (current2 >= 4)
                    current2 = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, int shift)
        {
            Vector2 onWindowPositon = new Vector2(player.position.X - shift, player.position.Y);
            if (player.Jumper.jumpState == JumpManager.JumpState.rising || (player.Jumper.jumpState == JumpManager.JumpState.falling))
            {
                spriteBatch.Draw(texturePlayerJump[current2], player.convertToRectangle(onWindowPositon, player.size), Color.White);
            }
            else if (player.playerDirection == Player.PlayerDirection.right)
            {
                if(player.oldState.IsKeyDown(Keys.Right)==true)
                    spriteBatch.Draw(texturePlayerRight[current], player.convertToRectangle(onWindowPositon, player.size), Color.White);
                else
                    spriteBatch.Draw(texturePlayerStanding, player.convertToRectangle(onWindowPositon, player.size), Color.White);
            }
            else if (player.playerDirection == Player.PlayerDirection.left)
            {
                if (player.oldState.IsKeyDown(Keys.Left) == true)
                    spriteBatch.Draw(texturePlayerLeft[current], player.convertToRectangle(onWindowPositon, player.size), Color.White);
                else
                    spriteBatch.Draw(texturePlayerStandingLeft, player.convertToRectangle(onWindowPositon, player.size), Color.White);
            }
            else if(player.playerDirection==Player.PlayerDirection.up)
            {
                if (player.oldState.IsKeyDown(Keys.Left) == true)   //running with gun up (no texture so far)
                    spriteBatch.Draw(texturePlayerStandingUp, player.convertToRectangle(onWindowPositon, player.size), Color.White);
                else if (player.oldState.IsKeyDown(Keys.Right) == true) //running with gun up (no texture so far)
                    spriteBatch.Draw(texturePlayerStandingUp, player.convertToRectangle(onWindowPositon, player.size), Color.White);
                else
                    spriteBatch.Draw(texturePlayerStandingUp, player.convertToRectangle(onWindowPositon, player.size), Color.White);
            }
        }




        public void LoadContentForAnimation(ContentManager content)
        {
            throw new NotImplementedException();
        }

        public void UpdateAnimation(GameTime gameTime, ContentManager content)
        {
            throw new NotImplementedException();
        }

        public void DrawAnimation(SpriteBatch spriteBatch, int shift)
        {
            throw new NotImplementedException();
        }
    }
}