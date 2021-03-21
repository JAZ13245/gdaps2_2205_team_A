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
    }
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

        //constants for walk rectangles
        const int WalkFrameCount = 4;
        const int MainRectHeight = 37;
        const int MainRectWidth = 10;

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
            fps = 10.0;                     // Will cycle through 10 walk frames per second
            timePerFrame = 1.0 / fps;       // Time per frame = amount of time in a single walk image
        }

        //updating the player's animation
        public void UpdateAnimation(GameTime gameTime)
        {

        }
    }
}
