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

namespace BoobiesGame3
{
    public class PlayerBonuses
    {
        [XmlArrayItem(typeof(PlayerBonusChangeBulletsType))]
        [XmlArrayItem(typeof(PlayerBonusIncreaseHP))]
        [XmlArrayItem(typeof(PlayerBonusIncreaseVelocity))]
        [XmlArrayItem(typeof(PlayerBonusAddPoints))]
        public List<PlayerBonus> playerBonuses;
        public PlayerBonuses()
        {
            ;
           
        }
     
        public void LoadContent(ContentManager content)
        {
            foreach (var item in playerBonuses)
            {
                item.LoadContent(content);
            }
        }
        public void Update(GameTime gameTime, GameWindow gameWindow, Platforms platforms, int shift, Player Player, ContentManager content)
        {
           
            List<PlayerBonus> playerBonusesToDelete = new List<PlayerBonus>();
            foreach (var item in playerBonuses)
            {
                if (item.isOnScreen(gameWindow, shift))
                {
                    item.Update(gameTime, gameWindow, platforms, ref shift, Player);
                }
                if ((!item.appeared) )
                {
                    playerBonusesToDelete.Add(item);
                }
            }
            foreach (var item in playerBonusesToDelete)
            {
                playerBonuses.Remove(item);
            }
        }
        
        public void Draw(GameWindow gameWindow, SpriteBatch spriteBatch, int shift)
        {
            foreach (var item in playerBonuses)
            {
             if (item.isOnScreen(gameWindow, shift))
              {
                   item.Draw(spriteBatch, shift);
                   item.appeared = true;
               }
           }
        }
    }
}
