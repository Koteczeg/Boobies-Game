using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;

namespace BoobiesGame3
{
    public class FontRenderer
    {
        public FontRenderer(FontFile fontFile, Texture2D fontTexture)
        {
            FFile = fontFile;
            FTexture = fontTexture;
            CharMap = new Dictionary<char, FontChar>();

            foreach (var fontCharacter in FFile.Chars)
            {
                char c = (char)fontCharacter.ID;
                CharMap.Add(c, fontCharacter);
            }
        }

        private Dictionary<char, FontChar> CharMap;
        private FontFile FFile;
        private Texture2D FTexture;

        public void DrawText(SpriteBatch spriteBatch, int x, int y, string text, Color kolor)
        {
            int dx = x;
            int dy = y;
            foreach (char c in text)
            {
                FontChar fc;
                if (CharMap.TryGetValue(c, out fc))
                {
                    var sourceRectangle = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);
                    var position = new Vector2(dx + fc.XOffset, dy + fc.YOffset);

                    spriteBatch.Draw(FTexture, position, sourceRectangle, kolor);
                    dx += fc.XAdvance;
                }
            }
        }
    }
}