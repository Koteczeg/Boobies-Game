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
    public abstract class RangeEnemy : Enemy
    {
        #region LOAD FROM XML
        [XmlAttribute]
        public string bulletTexture = null;
        [XmlAttribute]
        public int BulletDamage;
        [XmlAttribute]
        public int Range;

        public Vector2 BulletSize;

        [XmlAttribute(AttributeName = "BulletSize")]
        [EditorBrowsable(EditorBrowsableState.Never)] // IDE nie będzie podpowiadać, że takie pole istnieje
        public string BulletSizeAttribute
        {
            get { return this.BulletSize.X + "," + this.BulletSize.Y; }
            set
            {
                var parts = value.Split(',');
                this.BulletSize = new Vector2(float.Parse(parts[0]), float.Parse(parts[1]));
            }
        }

        #endregion

        TimeSpan TimeSinceLastMeeleHit = new TimeSpan(0);
        TimeSpan MeeleHitDelay = new TimeSpan(0, 0, 3);

        public RangeEnemy(int PositionX, int PositionY, int SizeX, int SizeY) : base(PositionX, PositionY, SizeX, SizeY) { }
        public RangeEnemy() { }

        public void UpdateTime(GameTime gameTime)
        {
            TimeSinceLastMeeleHit += gameTime.ElapsedGameTime;
        }

        public override int DamageDealt(Player player)
        {
            if (TimeSinceLastMeeleHit > MeeleHitDelay && !Rectangle.Intersect(player.convertToRectangle(), this.convertToRectangle()).IsEmpty)
            {
                TimeSinceLastMeeleHit = new TimeSpan(0);
                return 2 * BulletDamage;
            }
            return 0;
        }

        public override void SetStats(Enemy enemy)
        {
            base.SetStats(enemy);
            this.bulletTexture = ((RangeEnemy)enemy).bulletTexture;
            this.BulletDamage = ((RangeEnemy)enemy).BulletDamage;
            this.Range = ((RangeEnemy)enemy).Range;
            this.BulletSize = ((RangeEnemy)enemy).BulletSize;
        }
        public virtual Bullet GenerateBullet(int BulletPositionX, int BulletPositionY, int BulletSizeX, int BulletSizeY, float BulletVelocityX, float BulletVelocityY, int BulletDamage, Type TypeOfBullet, string BulletTexture, ContentManager Content)
        {
            Bullet BulletToAdd = new Bullet(BulletPositionX, BulletPositionY, BulletSizeX, BulletSizeY, BulletVelocityX, BulletVelocityY, BulletDamage);
            BulletToAdd.TypeOfBullet = TypeOfBullet;
            BulletToAdd.texture_path = BulletTexture;
            BulletToAdd.LoadContent(Content);
            return BulletToAdd;
        }
    }
}
