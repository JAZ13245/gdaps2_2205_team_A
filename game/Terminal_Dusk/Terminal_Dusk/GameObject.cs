using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Terminal_Dusk
{
    abstract class GameObject
    {
        private Rectangle position;
        protected Texture2D image;

        protected GameObject(Texture2D image, Rectangle position)
        {
            this.image = image;
            this.position = position;
        }
    }
}
