using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Xml.Serialization;

namespace BoobiesGame3
{
    public class PlayerBonus : Unit
    {
        public PlayerBonus() { ;}
        public bool appeared = true;
        public PlayerBonus(int position_x, int position_y, int size_x, int size_y) 
            : base(position_x, position_y, size_x, size_y) { }
        public virtual void LoadContent(ContentManager content) 
        {
            texture = content.Load<Texture2D>(texture_path);
        }
        public virtual void Update(GameTime gameTime, GameWindow gameWindow, Platforms platforms, ref int shift, Player Player) { }
        
    }
}
