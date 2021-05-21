using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal_Dusk
{
    class Slime : Enemy
    {
        //basic properties
        private int frame;
        private double timeCounter;
        private double timePerFrame;
        private SpriteEffects flip;
        private int currentSprite;

        //Hitbox size
        private const int hitBoxWidth = 13;
        private const int hitBoxHeight = 13;

        //constructor
        public Slime(Texture2D sprite, Rectangle location, GameState state, PlayerState playerState, int speed) :base (sprite, location, state,playerState,speed)
        {
            image = sprite;
            position = location;
            timePerFrame = 0.15;
            currentState = EnemyState.Idle;
            frame = 0;
            currentSprite = 0;
            enemyRNG = new Random();
            this.state = state;
            this.playerState = playerState;
            this.speed = speed;

            hitBox = new Rectangle(position.X + 5 * 3, position.Y + 4 * 3, hitBoxWidth * 3, hitBoxHeight * 3);
        }


        //the slime will just stand still and jump for its attack and movement, very basic stuff
        //if theres more time I could implement the bone attack idea, but most likely not

        public override void Draw(SpriteBatch sb)
        {
            //138 x 102
            DrawJump(sb,flip);
        }

        //loads in the slimes via file io
        public override void Load(string filename)
        {
            base.Load(filename);
        }

        //doesn't do anything at the moment- hypothetically saves
        public override void Save(string filename)
        {
            base.Save(filename);
        }

        //for saving the position, health, and state of the slime
        public override string ToString()
        {
            return ($"Enemy:Slime({position}) - HP:{health} - State:{currentState}");
        }

        public override void Update(GameTime gameTime)
        {
            UpdateRectangleSize(gameTime);
            ScrollWithPlayer();
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeCounter >= timePerFrame)
            {
                
                switch (currentState)
                {
                    case EnemyState.Idle:
                        
                        currentSprite = 0;
                        frame++;
                        if (frame == 5)
                        {
                            frame = 0;
                            int randomDirection = enemyRNG.Next(0,2);
                            if (randomDirection == 0)
                            {
                                flip = SpriteEffects.FlipHorizontally;
                            }
                            else if (randomDirection == 1)
                            {
                                flip = SpriteEffects.None;
                            }
                            currentState = EnemyState.IdleBehaviour;
                        }
                        
                        break;

                    case EnemyState.IdleBehaviour:

                        
                        frame++;
                        
                        if (frame > 5 & frame < 15)
                        {
                            switch (flip)
                            {
                                case SpriteEffects.FlipHorizontally:
                                    position.X -= 10;
                                    
                                    break;
                                case SpriteEffects.None:
                                    position.X += 10;
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (frame > 5 & frame < 9)
                        {
                            position.Y -= 5;
                        }
                        if (frame > 8 & frame < 12)
                        {
                            position.Y += 5;
                        }
                        
                        
                        currentSprite++;
                        if (frame == 15)
                        { 
                            frame = 0;
                            currentSprite = 0;
                            currentState = EnemyState.Idle;
                        }
                        
                        break;

                    case EnemyState.Attacking:
                        frame++;
                        if (frame == 15)
                        { frame = 0; }
                        break;

                    case EnemyState.Dying:

                        break;

                    default:
                        break;

                }
                timeCounter -= timePerFrame;
            }
            


            base.Update(gameTime);
        }

        //for animating the slime jumping
        private void DrawJump(SpriteBatch sb,SpriteEffects flip)
        {
            sb.Draw(
                image,                    // - The texture to draw
                new Vector2(position.X,position.Y),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    138*currentSprite,                          //   - This rectangle specifies
                    0,                          //	   where "inside" the texture
                    138,             //     to get pixels (We don't want to
                    102),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                0.5f,                           // - Scale (100% - no change)  //Should eventually take screenSize to keep main clean
                flip,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        //Updates Hitbox
        public void UpdateRectangleSize(GameTime gameTime)
        {
            switch (frame)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    hitBox.X = position.X + 5 * 3;
                    hitBox.Y = position.Y + 4 * 3;
                    break;
                case 6:
                case 7:
                case 8:
                    hitBox.X = position.X;
                    hitBox.Y = position.Y + 5 * 3;
                    break;
                case 9:
                case 10:
                    hitBox.X = position.X + 5 * 3;
                    hitBox.Y = position.Y + 4 * 3;
                    break;
                case 11:
                    hitBox.X = position.X + 8 * 3;
                    hitBox.Y = position.Y + 4 * 3;
                    break;
                case 12:
                    hitBox.X = position.X + 9 * 3;
                    hitBox.Y = position.Y + 4 * 3;
                    break;
                case 13:
                    hitBox.X = position.X + 7 * 3;
                    hitBox.Y = position.Y + 4 * 3;
                    break;
                case 14:
                    hitBox.X = position.X + 5 * 3;
                    hitBox.Y = position.Y + 4 * 3;
                    break;
            }
        }

        //for scrolling as the player moves
        public override void ScrollWithPlayer()
        {
            if (state == GameState.MainMenu)
            {
                //Reset();
            }
            if (state == GameState.GamePlayState)
            {
                if (playerState == PlayerState.WalkRight)
                {
                    position.X -= speed;
                }
                else if (playerState == PlayerState.WalkLeft)
                {
                    position.X += speed;
                }
            }
        }

    }
}