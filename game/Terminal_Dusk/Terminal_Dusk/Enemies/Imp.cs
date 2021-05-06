using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal_Dusk
{
    class Imp : Enemy //(will add when I start working on this enemy)
    {
        //Copy/Pasted for Slime, just want to have it drawn - James
        private int frame;
        private double timeCounter;
        private double timePerFrame;
        private SpriteEffects flip;
        private int currentSprite;

        private int swoopCounter = 0;
        private int pauseLimit = 100;
        private int swoopLimit = 15;
        private float swoopDurration = 0.03f; //every  .03s.
        private float swoopTime = 0f;
        private int swoopSpeed = 18;

        private int randomDirection;
        private int attackSpeed = 3;

        public Imp(Texture2D sprite, Rectangle location, GameState state, PlayerState playerState, int speed) : base(sprite, location, state, playerState, speed)
        {
            image = sprite;
            position = location;
            timePerFrame = 0.15;
            currentState = EnemyState.Attacking;
            frame = 0;
            currentSprite = 0;
            enemyRNG = new Random();
            this.state = state;
            this.playerState = playerState;
            this.speed = speed;
        }


        //the slime will just stand still and jump for its attack and movement, very basic stuff
        //if theres more time I could implement the bone attack idea, but most likely not

        public override void Draw(SpriteBatch sb)
        {
            //138 x 102
            //DrawJump(sb, flip);
            sb.Draw(image, position, Color.White); //Draws no animation, delete once animation added
        }

        public override void Load(string filename)
        {
            base.Load(filename);
        }

        public override void Save(string filename)
        {
            base.Save(filename);
        }

        public override string ToString()
        {
            return ($"Enemy:Imp({position}) - HP:{health} - State:{currentState}");
        }

        public override void Update(GameTime gameTime)
        {
            ScrollWithPlayer();
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            /*case PlayerJumpingState.Standing:
                            if (SingleKeyPress(upMove, kbState))
            {
                player.JumpingState = PlayerJumpingState.Jumping;

                jumpSpeed = -16;//Give it upward thrust
                jumpingCurrentTime = 0f;
            }
            break;

                        case PlayerJumpingState.Jumping:
                            

                            //Would need to be edited to work with collision
                            //Will be jank with falling of edges. Not perfect fit but a good start to having better jumping
                            if (player.Y != GraphicsDevice.Viewport.Height - (92 * scale))
            {
                player.Y += jumpSpeed;
                jumpSpeed++; //Acts as the physics accelerating/deccelerating
            }

            //Keeps player a peak for a small amount of time
            else
            {
                jumpingCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (jumpingCurrentTime >= jumpingCountDuration)
                {
                    jumpingCounter++;
                    //reset the timer to loop again
                    jumpingCurrentTime -= jumpingCountDuration; // "use up" the time
                }
                //if the counter is greater then our limit
                //the pause has completed
                if (jumpingCounter >= jumpingLimit)
                {
                    //reset the counter
                    jumpingCounter = 0;
                    //continues movement
                    player.Y += jumpSpeed;
                    jumpSpeed++;
                }
            }


            if (player.Y >= GraphicsDevice.Viewport.Height - (47 * scale))
            //If it's farther than ground
            {
                player.Y = GraphicsDevice.Viewport.Height - (47 * scale);//Then set it on
                player.JumpingState = PlayerJumpingState.Standing;
            }*/
            

            switch (currentState)
            {
                case EnemyState.Idle:
                    randomDirection = enemyRNG.Next(0, 2);

                    swoopTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (swoopTime >= swoopDurration)
                    {
                        swoopCounter++;
                        //reset the timer to loop again
                        swoopTime -= swoopDurration; // "use up" the time
                    }
                    //if the counter is greater then our limit
                    //the pause has completed
                    if (swoopCounter >= pauseLimit)
                    {
                        //reset the counter
                        swoopCounter = 0;
                        currentState = EnemyState.Attacking;
                    }
                    break;

                case EnemyState.IdleBehaviour:
                    break;

                case EnemyState.Attacking:
                    if (randomDirection == 0)
                    {
                        position.X += attackSpeed;
                        
                    }
                    else if (randomDirection == 1)
                    {
                        position.X -= attackSpeed;
                    }

                    if (swoopSpeed < 0)
                    {
                        if (randomDirection == 0)
                        {
                            position.X += attackSpeed;

                        }
                        else if (randomDirection == 1)
                        {
                            position.X -= attackSpeed;
                        }
                    }

                    else if (swoopSpeed > 0)
                    {
                        if (randomDirection == 0)
                        {
                            position.X += attackSpeed;

                        }
                        else if (randomDirection == 1)
                        {
                            position.X -= attackSpeed;
                        }
                    }

                    else if (swoopSpeed == 0)
                    {
                        swoopTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                        if (swoopTime >= swoopDurration)
                        {
                            swoopCounter++;
                            //reset the timer to loop again
                            swoopTime -= swoopDurration; // "use up" the time
                        }
                        //if the counter is greater then our limit
                        //the pause has completed
                        if (swoopCounter >= swoopLimit)
                        {
                            //reset the counter
                            swoopCounter = 0;
                            position.Y += swoopSpeed;
                            swoopSpeed--;
                        }
                    }
                    else if (swoopSpeed == -19)
                    {
                        swoopSpeed = 18;
                        currentState = EnemyState.Idle;
                    }
                    else
                    {
                        position.Y += swoopSpeed;
                        swoopSpeed--;
                    }
                    break;

                case EnemyState.Dying:
                    break;

                default:
                    break;
            }



            base.Update(gameTime);
        }


        private void DrawJump(SpriteBatch sb, SpriteEffects flip)
        {
            sb.Draw(
                image,                    // - The texture to draw
                new Vector2(position.X, position.Y),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    138 * currentSprite,                          //   - This rectangle specifies
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

        //Moved to Enemy
        /*public GameState State
        {
            set { state = value; }
        }

        public PlayerState PlayerState
        {
            set { playerState = value; }
        }*/




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
