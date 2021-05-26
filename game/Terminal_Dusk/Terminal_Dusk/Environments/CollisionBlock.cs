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

        private Rectangle top;
        private Rectangle left; 
        private Rectangle right;
        private Rectangle bottom;

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

        public Rectangle[] EdgeArray { get; }

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
            //edges for directional collision. Scale should eventually be added
            top = new Rectangle(hitBox.X + 3, hitBox.Y, hitBox.Width - 6, 3);
            left = new Rectangle(hitBox.X, hitBox.Y + 3, 3, hitBox.Height - 6);
            right = new Rectangle(hitBox.X + hitBox.Width - 3, hitBox.Y + 3, 3, hitBox.Height - 6);
            bottom = new Rectangle(hitBox.X + 3, hitBox.Y + hitBox.Height- 3, hitBox.Width - 6, 3);

            EdgeArray = new Rectangle[4] { top, left, right, bottom };
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
                UpdateEdges();
            }
        }
        private void UpdateEdges()
        {
            top.X = hitBox.X + 3;
            EdgeArray[0] = top;
            left.X = hitBox.X;
            EdgeArray[1] = left;
            right.X = hitBox.X + hitBox.Width - 3;
            EdgeArray[2] = right;
            bottom.X = hitBox.X + 3;
            EdgeArray[3] = bottom;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(spriteSheet, location, Color.White);
            //Edges
            //sb.Draw(spriteSheet, top, Color.Red);
            //sb.Draw(spriteSheet, left, Color.Red);
            //sb.Draw(spriteSheet, right, Color.Red);
            //sb.Draw(spriteSheet, bottom, Color.Red);
        }

        public void Reset()
        {
            location = originalLoc;
        }
    }
}

