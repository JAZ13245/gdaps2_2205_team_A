using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Terminal_Dusk
{
    class Health : GameObject
    {
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

        public void Update(GameTime gameTime, int health)
        {
            playerHealth = health;
        }

        public override void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < playerHealth; i++)
            {
                sb.Draw(image, new Rectangle(Position.X-((int)(image.Width*1.25)*i),Position.Y,Position.Width,Position.Height), Color.White);
            }        
        }
    }
}
