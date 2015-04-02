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

namespace BoobiesGame3
{
    public class IcyPlatform : Platform
    {
        [XmlAttribute]
        public float speedFactor;

        public IcyPlatform(int x = 0, int y = 0, int width = 0, int height = 0)
            : base(x, y, width, height) { }
        public IcyPlatform() { ;}

        public override bool ChangeVelocity(ref Vector2 velocity)
        {
            velocity.X *= speedFactor;
            return true;
        }

        public override void SetStats(Platform statsPlatform)
        {
            this.speedFactor = ((IcyPlatform)statsPlatform).speedFactor;
            base.SetStats(statsPlatform);
        }
    }
}
