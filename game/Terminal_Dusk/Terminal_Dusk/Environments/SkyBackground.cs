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
        //private double timer;
        GameState state;

        public override GameState State
        {
            set { state = value; }
        }

        public SkyBackground(Texture2D sprite, Rectangle location, GameState state) : base(sprite, location)
        {
            this.sprite = sprite;
            this.location = location;
            //timer = 0;
            this.state = state;
        }

        public override void Update(GameTime gameTime)
        {
            if(state == GameState.GamePlayState)
            {
                if (state == GameState.MainMenu)
                {
                    Reset();
                }
                //timer = gameTime.ElapsedGameTime.TotalSeconds;
                if (location.Y < 1350*3 - 2012*3) //to sunset
                {
                    location.Y += 1;
                }
                else if(location.Y >= 1350 * 3 - 2012 * 3 && location.Y < 1422*3 - 2012 * 3) //-651 -579
                {
                    location.Y = 1512 * 3 - 2012 * 3; //-489
                }
                else if(location.Y >= 1512 * 3 - 2012 * 3 && !(location.Y >= 0))
                {
                    location.Y += 1;
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, location, Color.White);
        }

        public void Reset()
        {
            location.X = 0;
            location.Y = 90 * 3 - 2012 * 3;
        }
    }
}
