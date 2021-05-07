﻿using System;
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
        CrouchLeft,
    }
    enum PlayerJumpingState
    {
        Standing,
        Jumping
    }
    enum PlayerAttackingState
    {
        IsNotAttacking,
        IsAttacking
    }
    /* //Not all of these need to be created
     * //However, if this works the way I think it will in my head, combinations will work better than the 16 needed states in PlayerState
     * //If only two states don't use enum use bool
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
     * } */

    //to tell if the player is taking damage or not
    enum DamageState
    {
        CanTakeDamage,
        Invulnerable
    }


    //inherits from GameObject
    class Player : GameObject
    {
        Rectangle playerLoc;  // Mc's location on the screen
        PlayerState state;
        PlayerJumpingState jumpingState;
        PlayerAttackingState attackingState;
        DamageState damageState;

        //for the character sheet
        Texture2D spriteSheet;

        //for the animation
        int frame;
        double timeCounter;
        double fps;
        double timePerFrame;
        int frameListIndex;

        //fields for the health
        private int health;
        private Color damageColor;
        private double timeCounterDamage;
        private double timePerFrameDamage;
        private double takeDamageCooldown;

        //constants for walk rectangles
        const int walkFrameCount = 4;
        const int mainRectHeight = 37*6;
        const int mainRectWidth = 10*6;
        const int secondRow = 37*6;
        const int thirdRow = 65*6;
        const int fourthRow = 102 * 6;
        const int crouchWidth = 12*6;
        const int crouchHeight = 28*6;
        const int attackHeight = 37 * 6;
        const int attackWidth = 28 * 6;
        const int attackCrouchHeight = 28 * 6;
        const int attackCrouchWidth = 28 * 6;

        // Properties to get & set player's current location and animation state
        public float X
        {
            get
            {
                return this.playerLoc.X;
            }
            set
            {
                this.playerLoc.X = (int)value;
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
                this.playerLoc.Y = (int)value;
            }
        }

        //public properties for other fields
        public PlayerState State
        {
            get { return state; }
            set { state = value; }
        }
        
        public int Health
        {
            get { return health; }
        }
        
        public PlayerJumpingState JumpingState
        {
            get { return jumpingState; }
            set { jumpingState = value; }
        }

        public PlayerAttackingState AttackingState
        {
            get { return attackingState; }
            set { attackingState = value; }
        }
        //the constructor
        public Player(Texture2D spriteSheet, Rectangle playerLoc, PlayerState startingState, int health) : base(spriteSheet, playerLoc)
        {
            this.spriteSheet = spriteSheet;
            this.playerLoc = playerLoc;
            this.state = startingState;
            this.health = health;
            this.jumpingState = PlayerJumpingState.Standing;
            damageState = DamageState.CanTakeDamage;
            damageColor = Color.White;
            timePerFrameDamage = 1 / fps;
            takeDamageCooldown = 0;

            // Initialize
            fps = 5.0;                     // Will cycle through 5 walk frames per second
            timePerFrame = 1.0 / fps;       // Time per frame = amount of time in a single walk image
            frameListIndex = 0;
        }

        //Handles any updates and keeps game clean
        public override void Update(GameTime gameTime)
        {
            UpdateAnimation(gameTime);
            UpdateDamageState(gameTime);
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

        //updates to tell whether or not the player has been hit
        public void UpdateDamageState(GameTime gameTime)
        {
            if (damageState == DamageState.Invulnerable)
            {
                if (timeCounterDamage >= timePerFrame)
                {
                    takeDamageCooldown++;
                    if (takeDamageCooldown > 8)
                    {
                        damageColor = Color.White;
                        takeDamageCooldown = 0;
                        damageState = DamageState.CanTakeDamage;
                    }
                    else
                    {
                        if (damageColor == Color.White)
                        {
                            damageColor = Color.Red;
                        }
                        else
                        {
                            damageColor = Color.White;
                        }
                    }
                    timeCounterDamage -= timePerFrame;
                }
                timeCounterDamage += gameTime.ElapsedGameTime.TotalSeconds;
            }
            
            
            
        }

        //draws the player depending on their current state
        public void Draw(SpriteBatch sb)
        {
            switch (jumpingState)
            {
                case PlayerJumpingState.Jumping:
                    switch (state)
                    {
                        case PlayerState.FaceLeft:
                        case PlayerState.WalkLeft:
                        case PlayerState.CrouchLeft:
                            DrawCrouching(SpriteEffects.FlipHorizontally, sb);
                            break;
                        case PlayerState.FaceRight:
                        case PlayerState.WalkRight:
                        case PlayerState.CrouchRight:
                            DrawCrouching(SpriteEffects.None, sb);
                            break;
                    }
                    break;
                
                case PlayerJumpingState.Standing:
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
                    break;
            }
        }

        //draws the regular standing sprites
        private void DrawStanding(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            switch (attackingState)
            {
                case PlayerAttackingState.IsNotAttacking:
                    spriteBatch.Draw(
                        spriteSheet,                    // - The texture to draw
                        new Vector2(playerLoc.X, playerLoc.Y),                       // - The location to draw on the screen
                        new Rectangle(                  // - The "source" rectangle
                            0,                          //   - This rectangle specifies
                            0,                          //	   where "inside" the texture
                            mainRectWidth,             //     to get pixels (We don't want to
                            mainRectHeight),           //     draw the whole thing)
                        damageColor,                    // - The color
                        0,                              // - Rotation (none currently)
                        Vector2.Zero,                   // - Origin inside the image (top left)
                        0.5f,                           // - Scale (100% - no change)  //Should eventually take screenSize to keep main clean
                        flipSprite,                     // - Can be used to flip the image
                        0);                             // - Layer depth (unused)
                    break;
                case PlayerAttackingState.IsAttacking:
                    if (flipSprite == SpriteEffects.FlipHorizontally)
                    {
                        spriteBatch.Draw(
                        spriteSheet,                    // - The texture to draw
                        new Vector2(playerLoc.X - 19 * 3, playerLoc.Y),                       // - The location to draw on the screen
                        new Rectangle(                  // - The "source" rectangle
                            0,                          //   - This rectangle specifies
                            thirdRow,                          //	   where "inside" the texture
                            attackWidth,             //     to get pixels (We don't want to
                            attackHeight),           //     draw the whole thing)
                        damageColor,                    // - The color
                        0,                              // - Rotation (none currently)
                        Vector2.Zero,                   // - Origin inside the image (top left)
                        0.5f,                           // - Scale (100% - no change)  //Should eventually take screenSize to keep main clean
                        flipSprite,                     // - Can be used to flip the image
                        0);
                    }
                    else
                    {
                        spriteBatch.Draw(
                        spriteSheet,                    // - The texture to draw
                        new Vector2(playerLoc.X, playerLoc.Y),                       // - The location to draw on the screen
                        new Rectangle(                  // - The "source" rectangle
                            0,                          //   - This rectangle specifies
                            thirdRow,                          //	   where "inside" the texture
                            attackWidth,             //     to get pixels (We don't want to
                            attackHeight),           //     draw the whole thing)
                        damageColor,                    // - The color
                        0,                              // - Rotation (none currently)
                        Vector2.Zero,                   // - Origin inside the image (top left)
                        0.5f,                           // - Scale (100% - no change)  //Should eventually take screenSize to keep main clean
                        flipSprite,                     // - Can be used to flip the image
                        0);
                    }
                    break;
            }
            /*
            spriteBatch.Draw(
                spriteSheet,                    // - The texture to draw
                new Vector2(playerLoc.X,playerLoc.Y),                       // - The location to draw on the screen
                new Rectangle(                  // - The "source" rectangle
                    0,                          //   - This rectangle specifies
                    0,                          //	   where "inside" the texture
                    mainRectWidth,             //     to get pixels (We don't want to
                    mainRectHeight),           //     draw the whole thing)
                damageColor,                    // - The color
                0,                              // - Rotation (none currently)
                Vector2.Zero,                   // - Origin inside the image (top left)
                0.5f,                           // - Scale (100% - no change)  //Should eventually take screenSize to keep main clean
                flipSprite,                     // - Can be used to flip the image
                0);                             // - Layer depth (unused)
            */
        }

        //draws the walking sprites
        private void DrawWalking(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            switch (attackingState)
            {
                case PlayerAttackingState.IsNotAttacking:
                    spriteBatch.Draw(
                        spriteSheet,
                        new Vector2(playerLoc.X, playerLoc.Y),
                        new Rectangle(
                            frame * mainRectWidth,
                            0,
                            mainRectWidth,
                            mainRectHeight),
                        damageColor,
                        0,
                        Vector2.Zero,
                        0.5f,
                        flipSprite,
                        0);
                    break;
                case PlayerAttackingState.IsAttacking:
                    if (flipSprite == SpriteEffects.FlipHorizontally)
                    {
                        spriteBatch.Draw(
                        spriteSheet,                    // - The texture to draw
                        new Vector2(playerLoc.X - 19 * 3, playerLoc.Y),                       // - The location to draw on the screen
                        new Rectangle(                  // - The "source" rectangle
                            0,                          //   - This rectangle specifies
                            thirdRow,                          //	   where "inside" the texture
                            attackWidth,             //     to get pixels (We don't want to
                            attackHeight),           //     draw the whole thing)
                        damageColor,                    // - The color
                        0,                              // - Rotation (none currently)
                        Vector2.Zero,                   // - Origin inside the image (top left)
                        0.5f,                           // - Scale (100% - no change)  //Should eventually take screenSize to keep main clean
                        flipSprite,                     // - Can be used to flip the image
                        0);
                    }
                    else
                    {
                        spriteBatch.Draw(
                        spriteSheet,                    // - The texture to draw
                        new Vector2(playerLoc.X, playerLoc.Y),                       // - The location to draw on the screen
                        new Rectangle(                  // - The "source" rectangle
                            0,                          //   - This rectangle specifies
                            thirdRow,                          //	   where "inside" the texture
                            attackWidth,             //     to get pixels (We don't want to
                            attackHeight),           //     draw the whole thing)
                        damageColor,                    // - The color
                        0,                              // - Rotation (none currently)
                        Vector2.Zero,                   // - Origin inside the image (top left)
                        0.5f,                           // - Scale (100% - no change)  //Should eventually take screenSize to keep main clean
                        flipSprite,                     // - Can be used to flip the image
                        0);
                    }
                    break;
            }

            /*
            //Adjust rectangle to not get the odd clipping - done
            spriteBatch.Draw(
                spriteSheet,
                new Vector2(playerLoc.X, playerLoc.Y),              
                new Rectangle(                 
                    frame * mainRectWidth,   
                    0,         
                    mainRectWidth,             
                    mainRectHeight),        
                damageColor,               
                0,                            
                Vector2.Zero,
                0.5f,                    
                flipSprite,                 
                0);  
            */
        }

        //draws the crouching sprites
        public void DrawCrouching(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            switch (attackingState)
            {
                case PlayerAttackingState.IsNotAttacking:
                    spriteBatch.Draw(
                        spriteSheet,
                        new Vector2(X, Y + 9 * 3),
                        new Rectangle(
                            0,
                            secondRow,
                            crouchWidth,
                            crouchHeight),
                        damageColor,
                        0,
                        Vector2.Zero,
                        0.5f,
                        flipSprite,
                        0);
                    break;
                case PlayerAttackingState.IsAttacking:
                    if (flipSprite == SpriteEffects.FlipHorizontally)
                    {
                        spriteBatch.Draw(
                        spriteSheet,                    // - The texture to draw
                        new Vector2(X - 18 * 3, Y + 9 * 3),                       // - The location to draw on the screen
                        new Rectangle(                  // - The "source" rectangle
                            0,                          //   - This rectangle specifies
                            fourthRow,                          //	   where "inside" the texture
                            attackCrouchWidth,             //     to get pixels (We don't want to
                            attackCrouchHeight),           //     draw the whole thing)
                        damageColor,                    // - The color
                        0,                              // - Rotation (none currently)
                        Vector2.Zero,                   // - Origin inside the image (top left)
                        0.5f,                           // - Scale (100% - no change)  //Should eventually take screenSize to keep main clean
                        flipSprite,                     // - Can be used to flip the image
                        0);
                    }
                    else
                    {
                        spriteBatch.Draw(
                            spriteSheet,                    // - The texture to draw
                            new Vector2(X + 3 * 3, Y + 9 * 3),                       // - The location to draw on the screen
                            new Rectangle(                  // - The "source" rectangle
                                0,                          //   - This rectangle specifies
                                fourthRow,                          //	   where "inside" the texture
                                attackCrouchWidth,             //     to get pixels (We don't want to
                                attackCrouchHeight),           //     draw the whole thing)
                            damageColor,                    // - The color
                            0,                              // - Rotation (none currently)
                            Vector2.Zero,                   // - Origin inside the image (top left)
                            0.5f,                           // - Scale (100% - no change)  //Should eventually take screenSize to keep main clean
                            flipSprite,                     // - Can be used to flip the image
                            0);
                    }
                    break;
            }
            /*
            spriteBatch.Draw(
                spriteSheet,
                new Vector2(X, Y + 9*3),
                new Rectangle(
                    0,    
                    secondRow,
                    crouchWidth,
                    crouchHeight),
                damageColor,
                0,
                Vector2.Zero,
                0.5f,
                flipSprite,
                0);
            */
        }

        //checks to see if the player is touching an enemy
        public void CheckEnemyCollisions(Enemy check)
        {
            //if player is attacking, enemy dies
            if(playerLoc.Intersects(check.Position) && attackingState == PlayerAttackingState.IsAttacking)
            {
                check.CurrentState = EnemyState.Dying;
            }
            //if player isn't attacking, they were hit, they take damage
            else if (damageState == DamageState.CanTakeDamage && playerLoc.Intersects(check.Position) && attackingState==PlayerAttackingState.IsNotAttacking)
            {
                damageState = DamageState.Invulnerable;
                health--;  
            }
        }

        //checks to see if player is colliding with environment blocks
        public bool CheckCollision(Environments.CollisionBlock check)//some class or object inside the parantehses 
        {
            if (this.Position.Intersects(check.Position))
            {
                return true;
            }
            return false;
        }

        //not currently implemented
        public override void Save(string fileName) 
        { 
        }
        public override void Load(string fileName)
        {
        }
    }
}
