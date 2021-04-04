using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal_Dusk
{
    class Slime : Enemy
    {
        private int frame;
        private double timeCounter;
        private double timePerFrame;
        private SpriteEffects flip;
        private int currentSprite;

        public Slime(Texture2D sprite, Rectangle location) :base (sprite, location)
        {
            image = sprite;
            position = location;
            enemyLocation = new Vector2(location.X, location.Y);
            timePerFrame = 0.08;
            currentState = EnemyState.Idle;
            frame = 0;
            currentSprite = 0;
            enemyRNG = new Random();
        }


        //the slime will just stand still and jump for its attack and movement, very basic stuff
        //if theres more time I could implement the bone attack idea, but most likely not

        public override void Draw(SpriteBatch sb)
        {
            DrawJump(sb,flip);
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
            return ($"Enemy:Slime({position}) - HP:{health} - State:{currentState}");
        }

        public override void Update(GameTime gameTime)
        {
            
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
                                    enemyLocation.X -= 10;
                                    
                                    break;
                                case SpriteEffects.None:
                                    enemyLocation.X += 10;
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (frame > 5 & frame < 9)
                        {
                            enemyLocation.Y -= 5;
                        }
                        if (frame > 8 & frame < 12)
                        {
                            enemyLocation.Y += 5;
                        }
                        
                        
                        currentSprite++;
                        if (frame == 15)
                        { 
                            frame = 0;
                            currentSprite = 0;
                            currentState = EnemyState.Idle;
                        }
                        break;

                    case EnemyState.Hostile:
                        frame = 0;
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


        private void DrawJump(SpriteBatch sb,SpriteEffects flip)
        {
            sb.Draw(
                image,                    // - The texture to draw
                enemyLocation,                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    240*currentSprite,                          //   - This rectangle specifies
                    0,                          //	   where "inside" the texture
                    240,             //     to get pixels (We don't want to
                    240),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                0.5f,                           // - Scale (100% - no change)  //Should eventually take screenSize to keep main clean
                flip,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

    }
}