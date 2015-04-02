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
using System.ComponentModel;

namespace BoobiesGame3
{
    public class GameObject
    {
        public Vector2 position;
        public Vector2 size;
        protected Texture2D texture;
        [XmlAttribute]
        public string texture_path = null;

        [XmlAttribute(AttributeName = "position")]
        [EditorBrowsable(EditorBrowsableState.Never)] // IDE nie będzie podpowiadać, że takie pole istnieje
        public string PositionAttribute
        {
            get { return this.position.X + "," + this.position.Y; }
            set
            {
                var parts = value.Split(',');
                this.position = new Vector2(float.Parse(parts[0]), float.Parse(parts[1]));
            }
        }

        [XmlAttribute(AttributeName = "size")]
        [EditorBrowsable(EditorBrowsableState.Never)] // IDE nie będzie podpowiadać, że takie pole istnieje
        public string SizeAttribute
        {
            get { return this.size.X + "," + this.size.Y; }
            set
            {
                var parts = value.Split(',');
                this.size = new Vector2(float.Parse(parts[0]), float.Parse(parts[1]));
            }
        }

        public GameObject(int x = 0, int y = 0, int width = 0, int height = 0)
        {
            this.position.X = x;
            this.position.Y = y;
            this.size.X = width;
            this.size.Y = height;
        }

        public virtual void Draw(SpriteBatch spriteBatch, int shift)
        {
            Vector2 onWindowPositon = new Vector2(position.X - shift, position.Y);
            spriteBatch.Draw(texture, convertToRectangle(onWindowPositon, size), Color.White);
        }

        public virtual void Update(GameTime gameTime) { ;}

        public Rectangle convertToRectangle(Vector2 position, Vector2 size)
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }

        public Rectangle convertToRectangle()
        {
            return new Rectangle((int)this.position.X, (int)this.position.Y, (int)this.size.X, (int)this.size.Y);
        }

        public bool isOnScreen(GameWindow gameWindow, int shift)
        {
            var screen = new Rectangle(shift, 0, gameWindow.ClientBounds.Width, gameWindow.ClientBounds.Height);
            return Rectangle.Intersect(this.convertToRectangle(), screen) != Rectangle.Empty;
        }

    }
}
