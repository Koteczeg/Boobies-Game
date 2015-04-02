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
    class Collision
    {
        public const int EPS = 10; // tyle mogą być zanurzone w sobie obiekty żeby metoda FixOvelap usunęła nachodzenie się - 
                                    //jak będzie więcej to nic nie zrobi (potrzebne do LeakyPlatform)

        public static bool Intersect(Rectangle rec1, Rectangle rec2)
        {
            if (rec1.Left <= rec2.Left)
            {
                if (rec1.Top <= rec2.Top)
                    if (rec1.Right >= rec2.Left && rec1.Bottom >= rec2.Top)
                    {
                        if (rec1.Right == rec2.Left && rec1.Bottom == rec2.Top) //jeżeli stykają się rogami
                            return false;
                        return true;
                    }
                if (rec1.Top >= rec2.Top)
                    if (rec1.Right >= rec2.Left && rec1.Top <= rec2.Bottom)
                    {
                        if (rec1.Right == rec2.Left && rec1.Top == rec2.Bottom) //jeżeli stykają się rogami
                            return false;
                        return true;
                    }
            }
            if (rec1.Left >= rec2.Left)
            {
                if (rec1.Top <= rec2.Top)
                    if (rec1.Left <= rec2.Right && rec1.Bottom >= rec2.Top)
                    {
                        if (rec2.Right == rec1.Left && rec1.Bottom == rec2.Top) //jeżeli stykają się rogami
                            return false;
                        return true;
                    }
                if (rec1.Top >= rec2.Top)
                    if (rec1.Left <= rec2.Right && rec1.Top <= rec2.Bottom)
                    {
                        if (rec2.Right == rec1.Left && rec2.Bottom == rec1.Top) //jeżeli stykają się rogami
                            return false;
                        return true;
                    }
            }
            return false;
        }

        public static void FixOvelap(GameObject movingObject, GameObject staticObject)
        {
            Rectangle rec1 = new Rectangle((int)movingObject.position.X, (int)movingObject.position.Y, (int)movingObject.size.X, (int)movingObject.size.Y);
            Rectangle rec2 = new Rectangle((int)staticObject.position.X, (int)staticObject.position.Y, (int)staticObject.size.X, (int)staticObject.size.Y);
            if (rec1.Left <= rec2.Left)
            {
                if (rec1.Top <= rec2.Top)
                    if (rec1.Right > rec2.Left && rec1.Bottom > rec2.Top)
                    {
                        if (rec1.Bottom - rec2.Top <= rec1.Right - rec2.Left)
                        {
                            if (Math.Abs(movingObject.position.Y - staticObject.position.Y + movingObject.size.Y) < EPS)
                            {
                                movingObject.position.Y = staticObject.position.Y - movingObject.size.Y;
                                return;
                            }
                        }
                        else
                            if (Math.Abs(movingObject.position.X - staticObject.position.X + movingObject.size.X) < EPS)
                            {
                                movingObject.position.X = staticObject.position.X - movingObject.size.X;
                                return;
                            }                     

                    }
                if (rec1.Top >= rec2.Top)
                    if (rec1.Right > rec2.Left && rec1.Top < rec2.Bottom)
                    {
                        if (rec2.Bottom - rec1.Top <= rec1.Right - rec2.Left)
                        {
                            if (Math.Abs(movingObject.position.Y - staticObject.position.Y - staticObject.size.Y) < EPS)
                            {
                                movingObject.position.Y = staticObject.position.Y + staticObject.size.Y;
                                return;
                            } 
                        }
                        else
                            if (Math.Abs(movingObject.position.X - staticObject.position.X + movingObject.size.X) < EPS)
                            {
                                movingObject.position.X = staticObject.position.X - movingObject.size.X;
                                return;
                            }
                       
                    }
            }
            if (rec1.Left >= rec2.Left)
            {
                if (rec1.Top <= rec2.Top)
                    if (rec1.Left < rec2.Right && rec1.Bottom > rec2.Top)
                    {
                        if (rec1.Bottom - rec2.Top < rec2.Right - rec1.Left)
                        {
                            if (Math.Abs(movingObject.position.Y - staticObject.position.Y + movingObject.size.Y) < EPS)
                            {
                                movingObject.position.Y = staticObject.position.Y - movingObject.size.Y;
                                return;
                            }
                        }
                        else
                            if (Math.Abs(movingObject.position.X - staticObject.position.X - staticObject.size.X) < EPS)
                            {
                                movingObject.position.X = staticObject.position.X + staticObject.size.X;
                                return;
                            }

                    }
                if (rec1.Top >= rec2.Top)
                    if (rec1.Left < rec2.Right && rec1.Top < rec2.Bottom)
                    {
                        if (rec2.Bottom - rec1.Top < rec2.Right - rec1.Left)
                        {
                            if (Math.Abs(movingObject.position.Y - staticObject.position.Y - staticObject.size.Y) < EPS)
                            {
                                movingObject.position.Y = staticObject.position.Y + staticObject.size.Y;
                                return;
                            }
                        }
                        else
                            if (Math.Abs(movingObject.position.X - staticObject.position.X - staticObject.size.X) < EPS)
                            {
                                movingObject.position.X = staticObject.position.X + staticObject.size.X;
                                return;
                            }

                    }
            }

        }
    }
}
