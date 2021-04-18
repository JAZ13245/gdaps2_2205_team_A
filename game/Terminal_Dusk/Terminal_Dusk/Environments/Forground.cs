using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Terminal_Dusk.Environments
{
    class Forground : Environment
    {
        private Texture2D sprite;
        private Rectangle location;
        GameState state;
        PlayerState playerState;
        int speed;

        public override GameState State
        {
            set { state = value; }
        }

        public override PlayerState PlayerState
        {
            set { playerState = value; }
        }

        public Forground(Texture2D sprite, Rectangle location, GameState state, PlayerState playerState, int speed) : base(sprite, location)
        {
            this.sprite = sprite;
            this.location = location;
            this.state = state;
            this.playerState = playerState;
            this.speed = speed;
        }

        public override void Update(GameTime gameTime)
        {
            if (state == GameState.MainMenu)
            {
                Reset();
            }
            if (state == GameState.GamePlayState)
            {
                if (playerState == PlayerState.WalkRight)
                {
                    location.X -= speed;
                }
                else if (playerState == PlayerState.WalkLeft)
                {
                    location.X += speed;
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
            location.Y = 0;
        }
    }
    
}

