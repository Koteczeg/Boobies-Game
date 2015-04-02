using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BoobiesGame3
{
    public class HpBar
    {
        public int MaxHP;
        public int CurrentHp;
        public Color Color;
        public Vector2 size;
        public Vector2 currentSize;
        public Texture2D pixel;
        
        public void Initialize(Player Player, GraphicsDevice graphicsDevice)
        {
            MaxHP = Player.HP;
            size = new Vector2(Player.size.X, 10);
            pixel = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });
            currentSize = size;
        }

        public void Update(Player Player)
        {
            CurrentHp = Player.HP;
            float ratio = (float)CurrentHp / MaxHP;
            if (ratio > 0.8)
                Color = Color.Green;
            else if (ratio > 0.5)
                Color = Color.Yellow;
            else if (ratio > 0.2)
                Color = Color.Orange;
            else
                Color = Color.Red;
            currentSize.X = ratio * size.X;
        }

        public void Draw(SpriteBatch spriteBatch, Player player, int shift)
        {
            spriteBatch.Draw(pixel, new Rectangle((int)player.position.X - shift, (int)player.position.Y-(int)size.Y, (int)currentSize.X, (int)currentSize.Y), Color);
        }
    }
}
