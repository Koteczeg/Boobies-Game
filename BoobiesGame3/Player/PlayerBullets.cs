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
    public class PlayerBullets
    {
        public List<PlayerBullet> bullets;
        public PlayerBullets()
        {
            bullets = new List<PlayerBullet>();
        }
        public void InitializeFiveBulletsShoot(Player player, ContentManager content)
        {
            Vector2[] velocity = new Vector2[5];
            if (player.playerDirection == Player.PlayerDirection.right)
            {
                for (int i = 0; i < 5; ++i)
                {
                    if (i < 3)
                        velocity[i].X = 6 + i * 2;
                    else
                        velocity[i].X = 10 - (i - 2) * 2;
                    velocity[i].Y = -4 + i * 2;
                }
            }
            if (player.playerDirection == Player.PlayerDirection.left)
            {
                for (int i = 0; i < 5; ++i)
                {
                    if (i < 3)
                        velocity[i].X = -8 - i * 1;
                    else
                        velocity[i].X = -10 + (i - 2) * 1;
                    velocity[i].Y = -2 + i * 1;
                }
            }
            if (player.playerDirection == Player.PlayerDirection.up)
            {
                for (int i = 0; i < 5; ++i)
                {
                    velocity[i].X = -4 + i * 2;
                    if (i < 3)
                        velocity[i].Y = -6 - i * 2;
                    else
                        velocity[i].Y = -10 + (i - 2) * 2;
                }
            }
            for (int i = 0; i < 5; ++i)
            {
                PlayerBullet playerBullet = new PlayerBullet((int)player.position.X, (int)player.position.Y + (int)player.size.Y / 4,
                    (int)velocity[i].X, (int)velocity[i].Y, player.attackDamage, player.bulletTexturePath);
                playerBullet.LoadContent(content);
                bullets.Add(playerBullet);
            }
        }
        public void InitializeThreeBulletsShoot(Player player, ContentManager content)
        {
            Vector2[] velocity = new Vector2[3];
            if (player.playerDirection == Player.PlayerDirection.right)
            {
                for (int i = 0; i < 3; ++i)
                {
                    if (i < 1)
                        velocity[i].X = 7;
                    else
                        velocity[i].X = 10 - (i - 1) * 3;
                    velocity[i].Y = -3 + i * 3;
                }
            }
            if (player.playerDirection == Player.PlayerDirection.left)
            {
                for (int i = 0; i < 3; ++i)
                {
                    if (i < 1)
                        velocity[i].X = -7;
                    else
                        velocity[i].X = -10 + (i - 1) * 3;
                    velocity[i].Y = -3 + i * 3;
                }
            }
            if (player.playerDirection == Player.PlayerDirection.up)
            {
                for (int i = 0; i < 3; ++i)
                {
                    velocity[i].X = -3 + i * 3;
                    if (i < 1)
                        velocity[i].Y = -7 - i * 3;
                    else
                        velocity[i].Y = -10 + (i - 1) * 3;
                }
            }
            for (int i = 0; i < 3; ++i)
            {
                PlayerBullet playerBullet = new PlayerBullet((int)player.position.X, (int)player.position.Y + (int)player.size.Y / 4,
                    (int)velocity[i].X, (int)velocity[i].Y, player.attackDamage, player.bulletTexturePath);
                playerBullet.LoadContent(content);
                bullets.Add(playerBullet);
            }
        }
        public void InitializeOneBulletShoot(Player player, ContentManager content)
        {
            Vector2 velocity;
            velocity.X = velocity.Y = 0;
            if (player.playerDirection == Player.PlayerDirection.right)
            {
                velocity.X = 10 ;
                velocity.Y = 0 ;
            }
            if (player.playerDirection == Player.PlayerDirection.left)
            {
                velocity.X = -10;
                velocity.Y = 0;
            }
            if (player.playerDirection == Player.PlayerDirection.up)
            {
                velocity.X = 0 ;
                velocity.Y = -10 ;
            }
            PlayerBullet playerBullet = new PlayerBullet((int)player.position.X, (int)player.position.Y + (int)player.size.Y / 4,
                (int)velocity.X, (int)velocity.Y, player.attackDamage, player.bulletTexturePath);
            playerBullet.LoadContent(content);
            bullets.Add(playerBullet);
        }
        public void LoadContent(ContentManager content)
        {
            foreach (var item in bullets)
                item.LoadContent(content);
        }
        public void Update(GameTime gameTime, GameWindow gameWindow, Platforms platforms, ref int shift)
        {
            List<PlayerBullet> bulletsToDelete = new List<PlayerBullet>();
            foreach (var item in bullets)
            {
                item.Update();
                if ((!item.isOnScreen(gameWindow, shift) && item.appeared)||item.DidTheBulletShotEnemy == true)
                    bulletsToDelete.Add(item);
            }
            foreach (var item in bulletsToDelete)
                bullets.Remove(item);
        }
        public int DamageDealt(Enemy enemy)
        {
            int dmg = 0;
            foreach (var bullet in bullets)
            {
                if (bullet.DidTheBulletShotEnemy == true)
                    continue;
                else if (bullet.convertToRectangle().Intersects(enemy.convertToRectangle()))
                {
                    bullet.DidTheBulletShotEnemy = true;
                    dmg += bullet.shoot;
                }
            }
            return dmg;
        }
        public void Draw(GameWindow gameWindow, SpriteBatch spriteBatch, int shift)
        {
            foreach (var item in bullets)
                if (item.isOnScreen(gameWindow, shift))
                {
                    item.Draw(spriteBatch, shift);
                    item.appeared = true;
                }
        }
    }
}
