using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Terminal_Dusk
{
    class Background
    {
        private Texture2D backgroundImg;
        private Rectangle position;
        
        //constructor for background
        public Background(Texture2D backImg, Rectangle position)
        {
            // Save copies/references to the info we'll need later
            this.position = position;
            this.backgroundImg = backImg;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the background
            spriteBatch.Draw(backgroundImg, position, Color.White);
        }
    }
    
}
