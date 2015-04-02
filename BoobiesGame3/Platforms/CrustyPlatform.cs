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
    public class CrustyPlatform : Platform
    {
        [XmlAttribute]
        public int time; // in miliseconds

        public bool active = false;
        public TimeSpan timeSinceActivation = TimeSpan.Zero;
        public TimeSpan maxTime
        {
            get
            {
                return new TimeSpan(0, 0, 0, 0, time);
            }
        }
        [XmlIgnore]
        public CrustyPlatformAnimation Animation;

        public CrustyPlatform(int x = 0, int y = 0, int width = 0, int height = 0)
            : base(x, y, width, height) { }
        public CrustyPlatform() 
        {
            Animation = new CrustyPlatformAnimation(this);
        }

        public override bool ShouldBeRemoved(Platforms platforms,Unit Unit,GameWindow gameWindow,GameTime gameTime, int shift)
        {
            if(!active)
                active = (this == platforms.CurrentPlatform(Unit, gameWindow, shift));
            if (active)
            {
                timeSinceActivation += gameTime.ElapsedGameTime;
            }
            return timeSinceActivation > maxTime;
        }

        public override void Draw(SpriteBatch spriteBatch, int shift)
        {
            Animation.Draw(spriteBatch, shift);
        }

        public override void Update(GameTime gameTime) 
        {
            Animation.Update(gameTime);
        }

        public override void LoadContent(ContentManager content)
        {
            Animation.LoadContent(content);
        }

        public override void SetStats(Platform statsPlatform)
        {
            this.time = ((CrustyPlatform)statsPlatform).time;
            base.SetStats(statsPlatform);
        }

    }
}
