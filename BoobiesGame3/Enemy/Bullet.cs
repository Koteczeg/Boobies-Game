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
    public class Bullet : Unit
    {
        // klasa bazowa dla wszystkich bulletów. bullet będzie zawsze tworzony za pośrednictwem danego enemy i tam niech odbywają się potrzebne do wczytania rzeczy?
        #region składowe klasy
        public Vector2 Velocity;
        public int Shoot;
        public Type TypeOfBullet;
        public float rotation = 0;
        public bool DidTheBulletShotPlayer = false;
        public bool Appeared = false;
        #endregion

        public Bullet(int PositionX, int PositionY, int SizeX, int SizeY,float VelocityX,float VelocityY,int Shoot):base(PositionX,PositionY,SizeX,SizeY)
        {
            Velocity.X=VelocityX;
            Velocity.Y=VelocityY;
            this.Shoot=Shoot;

            rotation = (float)Math.Atan((double)(Math.Abs(Velocity.Y / Velocity.X)));
            if (Velocity.Y < 0)
                rotation *= -1;
            if (Velocity.X < 0)
                rotation = (float)Math.PI - rotation;
        }
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(texture_path);
        }
        public void Update(GameTime gameTime, GameWindow gameWindow, Platforms platforms, int shift)
        {
            position.X += Velocity.X;
            position.Y += Velocity.Y;

        }

        public override void Draw(SpriteBatch spriteBatch, int shift)
        {
            Vector2 onWindowPositon = new Vector2(position.X - shift, position.Y);
            spriteBatch.Draw(texture, this.convertToRectangle(onWindowPositon, this.size), null, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
