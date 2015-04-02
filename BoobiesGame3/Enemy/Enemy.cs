using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Xml.Serialization;



namespace BoobiesGame3
{
    public class Enemy : Unit
    {
        public enum WhereIGo { [XmlEnum("Left")] movingLeft,[XmlEnum("Right")] movingRight, movingUp, movingDown };
        #region składowe klasy
        public bool appeared = false;
        #endregion

        #region dane do wczytania z pliku
        [XmlAttribute]
        public int HP;
        [XmlAttribute]
        public int Delay;
        #endregion

        public Enemy() { }

        public Enemy(int position_x, int position_y, int size_x, int size_y):base(position_x,position_y,size_x,size_y){}

        public virtual void SetStats(Enemy enemy)
        {
            this.size = enemy.size;
            this.HP = enemy.HP;
            this.texture_path = enemy.texture_path;
            this.velocity = enemy.velocity;
            this.Delay = enemy.Delay;
        }

        public bool IsAlive()
        {
            if (this.HP < 0)
            {
                Player.points += 10;
            }
                
            return this.HP > 0;
        }

        public virtual int DamageDealt(Player player) { return 0; }
        public virtual void LoadContent(ContentManager Content){}
        public virtual void Update(GameTime gameTime, GameWindow gameWindow, Platforms platforms, ref int shift, Player Player, ContentManager content){}
        public virtual void Attack(GameTime gameTime, Player player, Bullets bullets, ContentManager Content){}
        
    }
}
