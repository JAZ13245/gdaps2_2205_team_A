﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Terminal_Dusk.PlayerComponents
{
    class Health : GameObject
    {

        //health of the player
        private int playerHealth;
        public int PlayerHealth
        {
            get
            {
                return this.playerHealth;
            }
            set
            {
                this.playerHealth = value;
            }
        }
        public Health(Texture2D image, Rectangle position) :base(image,position)
        {
            this.image = image;
            this.position = position;
        }

        //hypothetically this would load in and save the current player health
        public override void Load(string filename)
        {
            throw new NotImplementedException();
        }

        public override void Save(string filename)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        //sets the player health
        public void Update(GameTime gameTime, int health)
        {
            playerHealth = health;
        }

        //for as many pieces of health the player has, draws a heart to represent it
        public override void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < playerHealth; i++)
            {
                sb.Draw(image, new Rectangle(Position.X-((int)(image.Width*1.25)*i),Position.Y,Position.Width,Position.Height), Color.White);
            }        
        }
    }
}
