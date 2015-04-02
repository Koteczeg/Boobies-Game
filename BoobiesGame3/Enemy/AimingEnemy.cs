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
    public class AimingEnemy : RangeEnemy
    {
        [XmlAttribute]
        public float BulletVelocity;

        public AimingEnemy(int PositionX, int PositionY, int SizeX, int SizeY) : base(PositionX, PositionY, SizeX, SizeY) { }
        public AimingEnemy() { }

        public override void SetStats(Enemy enemy)
        {
            base.SetStats(enemy);
            this.BulletVelocity = ((AimingEnemy)enemy).BulletVelocity;
        }
    }
}
