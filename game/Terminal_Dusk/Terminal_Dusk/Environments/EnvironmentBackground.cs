using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Terminal_Dusk.Environments
{
    class EnvironmentBackground : Environment
    {
        private Texture2D sprite;
        private Rectangle location;
        GameState state;
        PlayerState playerState;

        public GameState State
        {
            set { state = value; }
        }

        public PlayerState PlayerState
        {
            set { playerState = value; }
        }

        public EnvironmentBackground(Texture2D sprite, Rectangle location, GameState state, PlayerState playerState) : base(sprite, location)
        {
            this.sprite = sprite;
            this.location = location;
            this.state = state;
            this.playerState = playerState;
        }

        public override void Update(GameTime gameTime)
        {
            //Should be tied to player state
            if (state == GameState.MainMenu)
            {
                Reset();
            }
            if (state == GameState.GamePlayState)
            {
                if (playerState == PlayerState.WalkRight)
                {
                    location.X -= 1;
                }
                else if (playerState == PlayerState.WalkLeft)
                {
                    location.X += 1;
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
