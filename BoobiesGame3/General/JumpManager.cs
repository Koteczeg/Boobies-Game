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
    public class JumpManager
    {
        public Unit unit;
        public enum JumpState { notJumping, rising, falling };
        public JumpState jumpState = JumpState.falling;
        public int howHighJumped = 0;
        
        public int currentVelocityY;

        public JumpManager(Unit unit)
        {
            this.unit = unit;
        }

        public bool Jump(GameWindow gameWindow, Platforms platforms, int shift, int jumpHeight)
        //returns true if Unit is Jumping 
        {
            int toPeak = jumpHeight - howHighJumped;
            int root = (int)(jumpHeight / unit.velocity.Y);
            if (toPeak != 0)
                currentVelocityY = (int)unit.velocity.Y - jumpHeight / (toPeak + root);
            if (currentVelocityY < 1)
                currentVelocityY = 1;

            if (jumpState == JumpState.rising && unit.canMoveUp(gameWindow, platforms, shift))
            {
                unit.position.Y -= currentVelocityY;
                howHighJumped += currentVelocityY;
                if (howHighJumped > jumpHeight)
                    jumpState = JumpState.falling;
                return true;
            }
            if (unit.canMoveDown(gameWindow, platforms, shift))
            {
                unit.position.Y += currentVelocityY;
                howHighJumped -= currentVelocityY;
                if (!unit.canMoveDown(gameWindow, platforms, shift))
                {
                    jumpState = JumpState.notJumping;
                    howHighJumped = 0;
                    return false;
                }
                else
                    jumpState = JumpState.falling;
                return true;
            }

            if (!unit.canMoveDown(gameWindow, platforms, shift))
            {
                howHighJumped = 0;
                jumpState = JumpState.notJumping;
                return false;
            }
            if (!unit.canMoveUp(gameWindow, platforms, shift))
            {
                jumpState = JumpState.falling;
                return true;
            }
            return false;


        }
    }
}
