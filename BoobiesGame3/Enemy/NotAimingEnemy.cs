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
    public class NotAimingEnemy : RangeEnemy
    {
        
        public Vector2 BulletVelocity;

        [XmlAttribute(AttributeName = "BulletVelocity")]
        [EditorBrowsable(EditorBrowsableState.Never)] // IDE nie będzie podpowiadać, że takie pole istnieje
        public string BulletVelocityAttribute
        {
            get { return this.BulletVelocity.X + "," + this.BulletVelocity.Y; }
            set
            {
                var parts = value.Split(',');
                this.BulletVelocity = new Vector2(float.Parse(parts[0]), float.Parse(parts[1]));
            }
        }

        public NotAimingEnemy(int PositionX, int PositionY, int SizeX, int SizeY) : base(PositionX, PositionY, SizeX, SizeY) { }
        public NotAimingEnemy() { }

        public override void SetStats(Enemy enemy)
        {
            base.SetStats(enemy);
            this.BulletVelocity = ((NotAimingEnemy)enemy).BulletVelocity;
        }
    }
}
