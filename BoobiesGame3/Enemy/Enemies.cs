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
    public class Enemies
    {
        [XmlArrayItem(typeof(NotSmartEnemy))]
        [XmlArrayItem(typeof(DamnEnemy))]
        [XmlArrayItem(typeof(FlyingEnemy))]
        [XmlArrayItem(typeof(BossEnemy))]
        [XmlArrayItem(typeof(JumpingEnemy))]
        [XmlArrayItem(typeof(StaticEnemy))]
        public List<Enemy> enemies;

        [XmlArrayItem(typeof(NotSmartEnemy))]
        [XmlArrayItem(typeof(DamnEnemy))]
        [XmlArrayItem(typeof(FlyingEnemy))]
        [XmlArrayItem(typeof(BossEnemy))]
        [XmlArrayItem(typeof(JumpingEnemy))]
        [XmlArrayItem(typeof(StaticEnemy))]
        public List<Enemy> enemiesStats;

        public Enemies() { ;}

        public void Generate(ContentManager Content, int count)
        {
            // nie wiem jak ty chcesz Sławek ich dodawac, wiecej szczegolow potrzebuje
            Random random = new Random();

            for (int i = 0; i < count; i++)
            {
                if (i % 4 == 0)
                {
                    NotSmartEnemy x = new NotSmartEnemy(i * 500 + 50, (int)(50 + 150 * Math.Abs(Math.Sin(i)) - 90), 70, 90);
                    enemies.Add(x);
                }
                else if (i % 4 == 1)
                {
                    JumpingEnemy x = new JumpingEnemy(i * 500 + 50, (int)(50 + 150 * Math.Abs(Math.Sin(i)) - 90), 40, 60);
                    enemies.Add(x);
                }
                else if (i % 4 == 2)
                {
                    FlyingEnemy x = new FlyingEnemy(i * 500 + 50, (int)(50 + 150 * Math.Abs(Math.Sin(i)) - 90), 70, 60);
                    enemies.Add(x);
                }
                else if (i % 4 == 3)
                {
                    DamnEnemy x = new DamnEnemy(i * 500 + 50, (int)(50 + 150 * Math.Abs(Math.Sin(i)) - 90), 40, 60);
                    enemies.Add(x);
                }

                // do zmienienia, to losowanie jest bez sensu, to jest tylko żeby można było na coś popatrzeć !!!
            }
            BossEnemy t = new BossEnemy(3 * 200 + 50, (int)(50 + 150 * Math.Abs(Math.Sin(3)) - 100), 60, 60);
            enemies.Add(t);
            StaticEnemy w = new StaticEnemy(3 * 200 + 50, 230, 80, 80);
            enemies.Add(w);
        }

        public void LoadStats()
        {
            foreach (var statEnemy in enemiesStats)
            {
                foreach (var enemy in enemies)
                {
                    if (enemy.GetType() == statEnemy.GetType())
                    {
                        enemy.SetStats(statEnemy);
                    }
                }
            }
        }

        public int DamageDealt(Player Player, Bullets Bullets)
        {
            int damage = 0;
            foreach (var item in enemies)
            {
                damage += item.DamageDealt(Player);  // to zlicza tylko dmg od melee enemy, bullety żyją swoim życiem i mają swoją analogiczną metodę
            }
            damage += Bullets.DamageFromBullets(Player);
            return damage;
        }
        public void LoadContent(ContentManager content)
        {
            LoadStats();
            foreach (var item in enemies)
            {
                item.LoadContent(content);
            }
        }
        public void Update(GameTime gameTime, GameWindow gameWindow, Platforms platforms, int shift, Player Player, PlayerBullets bullets, ContentManager content)
        {
            GetHurt(bullets, Player);
            List<Enemy> enemiesToDelete = new List<Enemy>();
            foreach (var item in enemies)
            {
                if (item.isOnScreen(gameWindow, shift))
                {
                    item.Update(gameTime, gameWindow, platforms, ref shift, Player, content);
                }
                if ((!item.isOnScreen(gameWindow, shift) && item.appeared) || !item.IsAlive())
                {
                    enemiesToDelete.Add(item);
                }
            }
            foreach (var item in enemiesToDelete)
            {
                enemies.Remove(item);
            }
        }
        public void Attack(GameTime gameTime, Player player, Bullets bullets, ContentManager Content)
        {
            foreach (var item in enemies)
            {
                item.Attack(gameTime, player, bullets, Content);
            }
        }
        public void Draw(GameWindow gameWindow, SpriteBatch spriteBatch, int shift)
        {
            foreach (var item in enemies)
            {
                if (item.isOnScreen(gameWindow, shift))
                {
                    item.Draw(spriteBatch, shift);
                    item.appeared = true;
                }
            }
        }
        public void GetHurt(PlayerBullets bullets, Player player)
        {
            foreach (var item in enemies)
            {
                item.HP -= player.DamageDealt(bullets, item);
            }
        }

    }
}
