using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.IO;


namespace BoobiesGame3
{
    public class Wyniki
    {
        public string Name;
        public int? Score;
        public Wyniki(string Name, int? Score)
        {
            this.Name = Name;
            this.Score = Score;
        }
    }
}