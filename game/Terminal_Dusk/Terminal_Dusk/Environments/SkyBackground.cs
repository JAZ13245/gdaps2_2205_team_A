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
                //timer = gameTime.ElapsedGameTime.TotalSeconds;

                location.Y += 50;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite, location, Color.White);
        }
    }
}
