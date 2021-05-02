using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Terminal_Dusk.Environments
{
    class Sun : Environment
    {
        private Game1 game = new Game1();
        private Texture2D sprite;
        private Rectangle location;
        GameState state;

        public override GameState State
        {
            set { state = value; }
        }

        public Sun(Texture2D sprite, Rectangle location, GameState state) : base(sprite, location)
        {
            this.sprite = sprite;
            this.location = location;
            //timer = 0;
            this.state = state;
        }

        public override void Update(GameTime gameTime)
        {
            if (state == GameState.GamePlayState)
            {
                //Slows scroll speed
                double timer = (double)gameTime.TotalGameTime.Ticks;
                if (timer % (10 / 3) == 0)
                {
                    location.Y++;
                }
            }
            if (state == GameState.MainMenu)
            {
                Reset();
            }

        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, location, Color.White);
        }

        public void Reset()
        {
            location.X = 40 * game.Scale;
            location.Y = -20 * game.Scale;
        }
    }
}
