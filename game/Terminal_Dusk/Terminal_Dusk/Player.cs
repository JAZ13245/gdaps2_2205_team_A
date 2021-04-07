using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Terminal_Dusk
{
    // Player State enum
    enum PlayerState
    {
        FaceLeft,
        WalkLeft,
        FaceRight,
        WalkRight,
        CrouchRight,
        CrouchLeft // Add state(s) to support crouching
        //This is ridiculous
        //I think a total overhaul of the Player's State Machine is necessary
        //Especially because of the Jump/Attack states 
        /*JumpLeft
         * JumpRight
         * StandAttackLeft
         * StandAttackRight
         * WalkAttackLeft
         * WalkAttackRight
         * CrouchAttackLeft
         * CrouchAttackRight
         * JumpAttackLeft
         * JumpAttackRight
         */
    }
    /* //Not all of these need to be created
     * //However, if this works the way I think it will in my head, combinations will work better than the 16 needed states in PlayerState
     * enum PlayerDirectionState
     * {
     *      Left,
     *      Right
     * }
     * 
     * enum PlayerMovementState
     * {
     *      Still,
     *      Crouching, //Could be seperate enum or put into the jump enum(jump may actually be better)
     *      Moving
     * }
     * 
     * //I would argue that these last two are close to necessary. Attack alone would get rid of the 8 states 
     * enum PlayerAttackState
     * {
     *      NotAttacking,
     *      Attacking
     * }
     * 
     * enum PlayerJumpingState
     * {
     *      Standing,
     *      Jumping
     * }
     */
    class Player 
    {
        Vector2 playerLoc;  // Mc's location on the screen
        PlayerState state;

        //for the character sheet
        Texture2D spriteSheet;

        //for the animation
        int frame;
        double timeCounter;
        double fps;
        double timePerFrame;
        int frameListIndex;

        //constants for walk rectangles
        const int walkFrameCount = 4;
        const int mainRectHeight = 37*6;
        const int mainRectWidth = 10*6;
        const int secondRow = 37*6;
        const int crouchWidth = 12*6;
        const int crouchHeight = 28*6;

        // Properties to get & set player's current location and animation state
        public float X
        {
            get
            {
                return this.playerLoc.X;
            }
            set
            {
                this.playerLoc.X = value;
            }
        }

        //y value for crouching in particular
        public float Y
        {
            get
            {
                return this.playerLoc.Y;
            }
            set
            {
                this.playerLoc.Y = value;
            }
        }

        public PlayerState State
        {
            get { return state; }
            set { state = value; }
        }

        //the constructor
        public Player(Texture2D spriteSheet, Vector2 playerLoc, PlayerState startingState) 
        {
            this.spriteSheet = spriteSheet;
            this.playerLoc = playerLoc;
            this.state = startingState;

            // Initialize
            fps = 5.0;                     // Will cycle through 5 walk frames per second
            timePerFrame = 1.0 / fps;       // Time per frame = amount of time in a single walk image
            frameListIndex = 0;
        }

        //updating the player's animation
        public void UpdateAnimation(GameTime gameTime)
        {
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // If enough time has passed:
            if (timeCounter >= timePerFrame)
            {
                frameListIndex += 1;

                if (frameListIndex == walkFrameCount) //End of cycle //Changed to == 4 since I'm comparing the index not the frame number
                { frameListIndex = 0; }
              
                List<int> frameList = new List<int> { 1, 0, 2, 0 };
                //frame += 1;   //Different logic needed. I assume you wanted to go 1, 2, 1, 3 for the cycle, this won't do that. -James                
                frame = frameList[frameListIndex];

                                  

                timeCounter -= timePerFrame;
            }
        }

        //Added by James, feel free to delete/change
        public void Draw(SpriteBatch sb)
        {
            switch (state)
            {
                case PlayerState.FaceLeft:
                    DrawStanding(SpriteEffects.FlipHorizontally, sb);
                    break;
                case PlayerState.WalkLeft:
                    DrawWalking(SpriteEffects.FlipHorizontally, sb);
                    break;
                case PlayerState.FaceRight:
                    DrawStanding(SpriteEffects.None, sb);
                    break;
                case PlayerState.WalkRight:
                    DrawWalking(SpriteEffects.None, sb);
                    break;
                case PlayerState.CrouchLeft:
                    DrawCrouching(SpriteEffects.FlipHorizontally, sb);
                    break;
                case PlayerState.CrouchRight:
                    DrawCrouching(SpriteEffects.None, sb);
                    break;
            }
        }

        private void DrawStanding(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                spriteSheet,                    // - The texture to draw
                playerLoc,                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    0,                          //   - This rectangle specifies
                    0,                          //	   where "inside" the texture
                    mainRectWidth,             //     to get pixels (We don't want to
                    mainRectHeight),           //     draw the whole thing)
                Color.White,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                0.5f,                           // - Scale (100% - no change)  //Should eventually take screenSize to keep main clean
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
        }

        private void DrawWalking(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            //Adjust rectangle to not get the odd clipping - done
            spriteBatch.Draw(
                spriteSheet,                  
                playerLoc,              
                new Rectangle(                 
                    frame * mainRectWidth,   
                    0,         
                    mainRectWidth,             
                    mainRectHeight),        
                Color.White,               
                0,                            
                Vector2.Zero,
                0.5f,                    
                flipSprite,                 
                0);                     
        }

        private void DrawCrouching(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                spriteSheet,
                new Vector2(X, Y + 9*3),
                new Rectangle(
                    0,    
                    secondRow,
                    crouchWidth,
                    crouchHeight),
                Color.White,
                0,
                Vector2.Zero,
                0.5f,
                flipSprite,
                0);
        }

        public void Save() 
        { 
        }
    }
}
