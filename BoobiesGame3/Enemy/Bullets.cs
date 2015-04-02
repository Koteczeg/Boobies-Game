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
    public class Bullets
    {
        public List<Bullet> bullets;

        public Bullets()
        {
            bullets = new List<Bullet>();
        }

        public void LoadContent(ContentManager content)
        {
            foreach (var item in bullets)
            {
                item.LoadContent(content);
            }
        }
        public void Update(GameTime gameTime, GameWindow gameWindow, Platforms platforms, int shift)
        {
            List<Bullet> bulletsToDelete = new List<Bullet>();
            foreach (var item in bullets)
            {
                item.Update(gameTime,gameWindow,platforms, shift);
                if ( (!item.isOnScreen(gameWindow, shift) && item.Appeared) || item.DidTheBulletShotPlayer)
                {
                    bulletsToDelete.Add(item);
                    
                }
            }
            foreach (var item in bulletsToDelete)
            {
                    bullets.Remove(item);
            }
        }
        public Rectangle convertToRectangle(Vector2 position, Vector2 size)
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }
        public int DamageFromBullets(Player player)
        {
            int DamageFromBullets = 0;
            foreach (var item in bullets)
            {
                if (item.DidTheBulletShotPlayer == true)
                {
                    continue;
                }
                Rectangle TheBullet = new Rectangle((int)(item.position.X), (int)(item.position.Y), (int)(item.size.X), (int)(item.size.Y));
                if (!Rectangle.Intersect(TheBullet, convertToRectangle(player.position, player.size)).IsEmpty)
                {
                        item.DidTheBulletShotPlayer = true;
                        DamageFromBullets += item.Shoot;
                }
            }
            return DamageFromBullets;
        }
        public void Draw(GameWindow gameWindow ,SpriteBatch spriteBatch, int shift)
        {
            foreach (var item in bullets)
            {
                if (item.isOnScreen(gameWindow, shift))
                {

                    item.Draw(spriteBatch, shift);
                    item.Appeared = true;
                }
            }
        }
    }
}
