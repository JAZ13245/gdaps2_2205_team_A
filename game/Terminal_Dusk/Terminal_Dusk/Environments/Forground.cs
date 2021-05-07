using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Terminal_Dusk.Environments
{
    class Foreground : Environment
    {
        private Texture2D spriteSheet;
        private Rectangle location;
        GameState state;
        PlayerState playerState;
        private int speed;
        //Size of rectangle
        private int xSize;
        private int ySize;
        //number in sprite sheet
        private int objectNumber;

        //allows access so it can be flipped in LoadEnvironment
        public bool Flipped { get; set; }

        public override GameState State
        {
            set { state = value; }
        }

        public override PlayerState PlayerState
        {
            set { playerState = value; }
        }

        public Foreground(Texture2D sprite, Rectangle location, GameState state, PlayerState playerState, int speed, int xSize, int ySize, int objectNumber) : base(sprite, location)
        {
            this.spriteSheet = sprite;
            this.location = location;
            this.state = state;
            this.playerState = playerState;
            this.speed = speed;
            this.xSize = xSize;
            this.ySize = ySize;
            this.objectNumber = objectNumber;
            Flipped = true;
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
            if (Flipped)
            {
                FlipDraw(SpriteEffects.FlipHorizontally, sb);
            }
            else
            {
                FlipDraw(SpriteEffects.None, sb);
            }
        }

        private void FlipDraw(SpriteEffects flipSprite, SpriteBatch sb)
        {
            sb.Draw(spriteSheet,
            new Vector2(location.X, location.Y),
            new Rectangle(objectNumber * xSize*6, 0, xSize*6, ySize*6),
            Color.White,
            0,
            Vector2.Zero,
            0.5f, //scale should be changed with screen size
            flipSprite,
            0);
        }

        public void Reset()
        {
            location.X = 0;
            location.Y = 0;
        }
    }
    
}

