using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;


namespace BoobiesGame3
{
    public class MovingPlatform : Platform
    {
        #region TO LOAD FROM XML
        public Vector2 velocity;
        public Vector2 range;

        [XmlAttribute(AttributeName = "velocity")]
        [EditorBrowsable(EditorBrowsableState.Never)] // IDE nie będzie podpowiadać, że takie pole istnieje
        public string velocityAttribute
        {
            get { return this.velocity.X + "," + this.velocity.Y; }
            set
            {
                var parts = value.Split(',');
                this.velocity = new Vector2(float.Parse(parts[0]), float.Parse(parts[1]));
            }
        }

        [XmlAttribute(AttributeName = "range")]
        [EditorBrowsable(EditorBrowsableState.Never)] // IDE nie będzie podpowiadać, że takie pole istnieje
        public string rangeAttribute
        {
            get { return this.range.X + "," + this.range.Y; }
            set
            {
                var parts = value.Split(',');
                this.range = new Vector2(float.Parse(parts[0]), float.Parse(parts[1]));
            }
        }
        #endregion

        public Vector2 move = new Vector2(0, 0);
        public enum Direction { Left, Right, Up, Down };
        public Direction HoricontalDirection = Direction.Right;
        public Direction VerticalDirection = Direction.Down;

        public MovingPlatform(){}

        public override void Update(GameTime gameTime)
        {
            if (HoricontalDirection == Direction.Right)
            {
                move.X += velocity.X;
                this.position.X += velocity.X;
                if (move.X > range.X )
                    HoricontalDirection = Direction.Left;
            }
            else if (HoricontalDirection == Direction.Left)
            {
                move.X -= velocity.X;
                this.position.X -= velocity.X;
                if (move.X < 0 )
                    HoricontalDirection = Direction.Right;
            }
            if (VerticalDirection == Direction.Down)
            {
                move.Y += velocity.Y;
                this.position.Y += velocity.Y;
                if (move.Y > range.Y)
                    VerticalDirection = Direction.Up;
            }
            else if (VerticalDirection == Direction.Up)
            {
                move.Y -= velocity.Y;
                this.position.Y -= velocity.Y;
                if (move.Y < 0)
                    VerticalDirection = Direction.Down;
            }
        }

        public override void MoveUnit(Unit unit, GameWindow gameWindow, Platforms platforms, int shift)
        {
            if (this.HoricontalDirection == Direction.Right && unit.canMoveRight(gameWindow,platforms,shift))
                unit.position.X += this.velocity.X;
            else if (this.HoricontalDirection == Direction.Left && unit.canMoveLeft(gameWindow, platforms, shift))
                unit.position.X -= this.velocity.X;
        }
    }
}
