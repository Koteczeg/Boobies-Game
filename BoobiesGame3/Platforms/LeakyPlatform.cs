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
    public class LeakyPlatform : Platform
    {
        public LeakyPlatform(int x = 0, int y = 0, int width = 0, int height = 0)
            : base(x, y, width, height) { }
        public LeakyPlatform() { ;}

        public override bool BlocksToMoveUp(GameObject obj)
        {
            return false;
        }

        public override bool BlocksToMoveLeft(GameObject obj)
        {
            return false;
        }

        public override bool BlocksToMoveRight(GameObject obj)
        {
            return false;
        }
    }
}
