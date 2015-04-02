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
    public class Platforms
    {
        [XmlArrayItem(typeof(IcyPlatform))]
        [XmlArrayItem(typeof(GluePlatform))]
        [XmlArrayItem(typeof(LeakyPlatform))]
        [XmlArrayItem(typeof(CrustyPlatform))]
        [XmlArrayItem(typeof(CommonPlatform))]
        [XmlArrayItem(typeof(MovingPlatform))]
        public List<Platform> platforms;
        [XmlArrayItem(typeof(GluePlatform))]
        [XmlArrayItem(typeof(IcyPlatform))]
        [XmlArrayItem(typeof(LeakyPlatform))]
        [XmlArrayItem(typeof(CrustyPlatform))]
        [XmlArrayItem(typeof(CommonPlatform))]
        [XmlArrayItem(typeof(MovingPlatform))]
        public List<Platform> stats;

        public Platforms() { ;}

        //Metoda żeby można było wpisać ścieżkę do danego obiektu tylko raz.
        public void LoadStats()
        {
            foreach (var statsPlatform in stats)
            {
                foreach (var platform in platforms)
                {
                    if (statsPlatform.GetType() == platform.GetType())
                    {
                        platform.SetStats(statsPlatform);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameWindow gameWindow, int shift)
        {
            foreach (var platform in platforms)
            {
                if (platform.isOnScreen(gameWindow,shift))
                    platform.Draw(spriteBatch, shift);
            }
        }

        public void LoadContent(ContentManager content)
        {
            LoadStats();
            foreach (var platform in platforms)
            {
                platform.LoadContent(content);
            }
        }

        public void Update(GameTime gameTime, GameWindow gameWindow, Unit Unit, int shift)
        {
            List<Platform> platformsToRemove = new List<Platform>();
            foreach (var platform in this.platforms)
            {
                platform.Update(gameTime);
                if (platform.ShouldBeRemoved(this, Unit, gameWindow, gameTime, shift))
                    platformsToRemove.Add(platform);
            }
            foreach (var platform in platformsToRemove)
            {
                this.platforms.Remove(platform);
            }
        }

        public Platform CurrentPlatform(Unit Unit, GameWindow gameWindow, int shift)
        {
            foreach (var platform in this.platforms)
            {
                if (platform.isOnScreen(gameWindow, shift)
                    && Collision.Intersect(platform.convertToRectangle(), Unit.convertToRectangle()))
                    return platform;
            }
            return null;
        }

    }
}
