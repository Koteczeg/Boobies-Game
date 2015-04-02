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
    public class PlayerBullet : GameObject
    {
        private Vector2 velocity;
        public int shoot;
        public bool DidTheBulletShotEnemy = false;
        public bool appeared = false;
        public PlayerBullet(int positionX, int positionY, int velocityX, int velocityY, int shoot, string texture_path, 
            int sizeX=15, int sizeY=15)
            : base(positionX, positionY, sizeX, sizeY)
        {
            this.texture_path = texture_path;
            velocity.X = velocityX;
            velocity.Y = velocityY;
            this.shoot = shoot;
        }
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(texture_path);
        }
        public void Update()
        {
            position.X += velocity.X;
            position.Y += velocity.Y;
        }
    }
}
