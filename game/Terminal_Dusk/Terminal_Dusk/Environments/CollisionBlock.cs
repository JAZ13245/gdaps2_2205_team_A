using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Terminal_Dusk.Environments
{
    class CollisionBlock : Environment
    {
        //works as a collision block
        private Texture2D spriteSheet;
        private Rectangle location;
        GameState state;
        PlayerState playerState;
        int speed;

        private Rectangle originalLoc;
        private Rectangle hitBox;

        //Should all be base 10X10

        public int LocationX
        {
            get { return location.X; }
            set { location.X = value; }
        }
        public override GameState State
        {
            set { state = value; }
        }

        public override PlayerState PlayerState
        {
            set { playerState = value; }
        }

        public Rectangle HitBox
        {
            get { return hitBox; }
        }

        //inherits from environment
        public CollisionBlock(Texture2D sprite, Rectangle location, GameState state, PlayerState playerState, int speed) : base(sprite, location)
        {
            this.spriteSheet = sprite;
            this.location = location;
            originalLoc = location;
            this.state = state;
            this.playerState = playerState;
            this.speed = speed;
            hitBox = new Rectangle(location.X - 2, location.Y - 1, location.Width + 4, location.Height + 2);
        }

        //updates based on player 'location'
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
                    hitBox.X = location.X;
                }
                else if (playerState == PlayerState.WalkLeft)
                {
                    location.X += speed;
                    hitBox.X = location.X;
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(spriteSheet, location, Color.White);
        }

        public void Reset()
        {
            location = originalLoc;
        }
    }
}

