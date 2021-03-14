﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Terminal_Dusk
{
    abstract class GameObject
    {
        protected Rectangle position;
        protected Texture2D image;

        //propertie(s) for the position  field
        public Rectangle Position { get { return position; } }

        //protected constructor for the GameObject class
        protected GameObject(Texture2D image, Rectangle position)
        {
            this.image = image;
            this.position = position;
        }

        //method to use spritebatch to draw the object
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(image, Position, Color.White);
        }
        
        //abstract method for child classes to update as needed
        public abstract void Update(GameTime gameTime);

        //abstract method for save (file IO)
        public abstract void Save(string filename);
        
        //abstract method for load (file IO)
        public abstract void Load(string filename);
    }
}
