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
    public class PlayerBonusIncreaseHP : PlayerBonus
    {
        public PlayerBonusIncreaseHP() { }
        public PlayerBonusIncreaseHP(int PositionX, int PositionY, int SizeX, int SizeY) 
            : base(PositionX, PositionY, SizeX, SizeY) { }
       
        public override void Update(GameTime gameTime, GameWindow gameWindow, Platforms platforms, ref int shift, Player player)
        {
            if (this.canMoveDown(gameWindow, platforms, shift))
            {
                position.Y += 5;
            }
            else if (Collision.Intersect(player.convertToRectangle(), this.convertToRectangle())==true)
            {
                if (player.HP <= player.fullHP)
                    player.HP = player.fullHP;
                appeared = false;
            }
        }
    }
}
