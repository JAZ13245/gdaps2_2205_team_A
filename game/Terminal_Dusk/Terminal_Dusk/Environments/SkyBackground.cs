using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Terminal_Dusk.Environments
{
    class SkyBackground : Environment
    {
        private Texture2D sprite;
        private Rectangle location;

        public SkyBackground(Texture2D sprite, Rectangle location) : base(sprite, location)
        {
            this.sprite = sprite;
            this.location = location;
        }
    }
}
