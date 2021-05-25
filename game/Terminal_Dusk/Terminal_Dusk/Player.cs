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
        CrouchLeft,
    }
    enum PlayerJumpingState
    {
        Standing,
        Jumping,
        Falling
    }
    enum PlayerAttackingState
    {
        IsNotAttacking,
        IsAttacking
    }

    //to tell if the player is taking damage or not
    enum DamageState
    {
        CanTakeDamage,
        Invulnerable
    }


    //inherits from GameObject
    class Player : GameObject
    {
        private Rectangle playerLoc;  // Mc's location on the screen

        private PlayerState state;
        private PlayerJumpingState jumpingState;
        private PlayerAttackingState attackingState;
        private DamageState damageState;

        //for the character sheet
        private Texture2D spriteSheet;

        //for the animation
        private int frame;
        private double timeCounter;
        private double fps;
        private double timePerFrame;
        private int frameListIndex;

        //fields for the health
        private int health;
        private Color damageColor;
        private double timeCounterDamage;
        private double timePerFrameDamage;
        private double takeDamageCooldown;

        //constants for walk rectangles
        private const int walkFrameCount = 4;
        //Row locations
        private const int secondRow = 37*6;
        private const int thirdRow = 65*6;
        private const int fourthRow = 102 * 6;
        //Standard sprite size
        private const int mainRectHeight = 37 * 6;
        private const int mainRectWidth = 10 * 6;
        //Crouch sprite size
        private const int crouchWidth = 12*6;
        private const int crouchHeight = 28*6;
        //Attack sprite size
        private const int attackHeight = 37 * 6;
        private const int attackWidth = 28 * 6;
        //Crouch attack sprite size
        private const int attackCrouchHeight = 28 * 6;
        private const int attackCrouchWidth = 28 * 6;
        //Hitbox for attacking
        private Rectangle hitBox;
        private const int hitBoxWidth = 18 * 3;
        private const int hitBoxHeight = 18 * 3;
        //Hurtbox for player. Makes moving the rectangle around easier
        private Rectangle hurtBox;

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
            set { health = value; }
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

        public DamageState DamageState { set { damageState = value; } }
        
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

            hitBox = new Rectangle(playerLoc.X + 10*3, playerLoc.Y + 2*3, hitBoxWidth, hitBoxHeight);
            hurtBox = playerLoc;

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
            UpdateRectangleSize(gameTime);
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

        //Used to adjust the rectangle size for better hit detection
        public void UpdateRectangleSize(GameTime gameTime)
        {
            switch (state)
            {
                //All width/heights divide by 2 since it uses the full size assets and we currently run a scale 3
                case PlayerState.FaceLeft:
                case PlayerState.WalkLeft:
                    if (jumpingState == PlayerJumpingState.Jumping || jumpingState == PlayerJumpingState.Falling)
                    {
                        hurtBox = new Rectangle(playerLoc.X, playerLoc.Y + 9*3, crouchWidth / 2, crouchHeight / 2); 

                        hitBox.X = playerLoc.X - 16 * 3;
                        hitBox.Y = hurtBox.Y;
                    }
                    else if (jumpingState == PlayerJumpingState.Standing)
                    {
                        hurtBox = playerLoc;

                        hitBox.X = playerLoc.X - 16 * 3;
                        hitBox.Y = playerLoc.Y + 1 * 3;
                    }
                    break;
                case PlayerState.FaceRight:
                case PlayerState.WalkRight:
                    if(jumpingState == PlayerJumpingState.Jumping || jumpingState == PlayerJumpingState.Falling)
                    {
                        hurtBox = new Rectangle(playerLoc.X, playerLoc.Y + 9*3, crouchWidth / 2, crouchHeight / 2);

                        hitBox.X = playerLoc.X + 10 * 3;
                        hitBox.Y = hurtBox.Y;
                    }
                    else if(jumpingState == PlayerJumpingState.Standing)
                    {
                        hurtBox = playerLoc;

                        hitBox.X = playerLoc.X + 10 * 3;
                        hitBox.Y = playerLoc.Y + 1 * 3;
                    }
                    break;

                case PlayerState.CrouchLeft:
                    hurtBox = new Rectangle(playerLoc.X, playerLoc.Y + 9*3, crouchWidth / 2, crouchHeight / 2);

                    hitBox.X = playerLoc.X - 16 * 3;
                    hitBox.Y = hurtBox.Y;
                    break;
                case PlayerState.CrouchRight:
                    hurtBox = new Rectangle(playerLoc.X, playerLoc.Y + 9*3, crouchWidth / 2, crouchHeight / 2);

                    hitBox.X = playerLoc.X + 10 * 3;
                    hitBox.Y = hurtBox.Y;
                    break;
            }
        }

        //draws the player depending on their current state
        public override void Draw(SpriteBatch sb)
        {
            //Visualizes hitbox and hurtbox
            //sb.Draw(spriteSheet, hurtBox, Color.White);
            //sb.Draw(spriteSheet, hitBox, Color.White);
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
        }

        //checks to see if the player is touching an enemy
        public void CheckEnemyCollisions(Enemy check)
        {
            //if player is attacking
            if(attackingState == PlayerAttackingState.IsAttacking)
            {
                //Check if they hit it
                if (hitBox.Intersects(check.HitBox))
                {
                    check.CurrentState = EnemyState.Dying;
                }
                //or if they got hit and can take damage
                else if (damageState == DamageState.CanTakeDamage & playerLoc.Intersects(check.HitBox))
                {
                    damageState = DamageState.Invulnerable;
                    health--;
                }
            }
            //if player isn't attacking and they were hit, they take damage
            else if (damageState == DamageState.CanTakeDamage & hurtBox.Intersects(check.HitBox))
            {
                damageState = DamageState.Invulnerable;
                health--;  
            }
        }

        //checks to see if player is colliding with environment blocks
        public bool CheckCollision(Rectangle check)//some class or object inside the parantehses 
        {
            if (playerLoc.Intersects(check))// && state == PlayerState.WalkLeft)
            {
                //state = PlayerState.FaceLeft;
                return true;
            }
            return false;
            /*switch (player.State)
                    {
                        case PlayerState.FaceLeft:
                            //Changes direction
                            if (kbState.IsKeyDown(rightMove) && !kbState.IsKeyDown(leftMove))
                            {
                                player.State = PlayerState.FaceRight;
                            }
                            //Transitions to walking
                            else if (kbState.IsKeyDown(leftMove) && prevKbState.IsKeyDown(leftMove))
                            {
                                //Stops player seeing empty left screen
                                if (!wallCollide)
                                {
                                    player.State = PlayerState.WalkLeft;
                                }
                            if (!playerLoc.Intersects(check))
                            {
                                state = PlayerState.WalkLeft;
                                return true;
                            }
                            }
                            //Transitions to crouching
                            else if (kbState.IsKeyDown(crouchMove) && prevKbState.IsKeyUp(crouchMove))
                            {
                                player.State = PlayerState.CrouchLeft;
                            }

                            break;
                        case PlayerState.WalkLeft:
                            if (wallCollide)
                            {
                                player.State = PlayerState.FaceLeft;
                            }
            if (playerLoc.Intersects(check))
            {
                state = PlayerState.FaceLeft;
                return true;
            }
                            //Transitions to crouch
                            if (kbState.IsKeyDown(crouchMove) && prevKbState.IsKeyUp(crouchMove))
                            {
                                player.State = PlayerState.CrouchLeft;
                            }
                            //Allows smoother movement Ex: holding leftMove then also pressing rightMove then letting go of rightMove
                            if (kbState.IsKeyDown(rightMove) && prevKbState.IsKeyUp(rightMove))
                            {
                                player.State = PlayerState.FaceRight;
                            }
                            //Moves left
                            if (kbState.IsKeyDown(leftMove)) { }
                            //Transitions to standing
                            else
                            {
                                player.State = PlayerState.FaceLeft;
                            }
                            break;
                        case PlayerState.FaceRight:
                            //Changes direction
                            if (kbState.IsKeyDown(leftMove) && !kbState.IsKeyDown(rightMove))
                            {
                                player.State = PlayerState.FaceLeft;
                            }
                            //Transitions to walking
                            else if (kbState.IsKeyDown(rightMove))
                            {
                                player.State = PlayerState.WalkRight;
                            }
                            //Transitions to crouching
                            else if (kbState.IsKeyDown(crouchMove) && prevKbState.IsKeyUp(crouchMove))
                            {
                                player.State = PlayerState.CrouchRight;
                            }
                            break;
                        case PlayerState.WalkRight:
                            //Transitions to crouch
                            if (kbState.IsKeyDown(crouchMove) && prevKbState.IsKeyUp(crouchMove))
                            {
                                player.State = PlayerState.CrouchRight;
                            }
                            //Allows smoother movement (Example in WalkLeft)
                            if (kbState.IsKeyDown(leftMove) && prevKbState.IsKeyUp(leftMove))
                            {
                                player.State = PlayerState.FaceLeft;
                            }
                            //Moves right
                            if (kbState.IsKeyDown(rightMove)) { }
                            //Transitions to standing
                            else
                            {
                                player.State = PlayerState.FaceRight;
                            }
                            break;
                        case PlayerState.CrouchLeft:
                            //Changes direction while crouching
                            if (kbState.IsKeyDown(rightMove) && !kbState.IsKeyDown(leftMove) && kbState.IsKeyDown(crouchMove))
                            {
                                player.State = PlayerState.CrouchRight;
                            }
                            //Is crouching
                            if (kbState.IsKeyDown(crouchMove)) { }
                            //Transitions to standing
                            else
                            {
                                player.State = PlayerState.FaceLeft;
                            }
                            break;
                        case PlayerState.CrouchRight:
                            //Changes direction while crouching
                            if (kbState.IsKeyDown(leftMove) && !kbState.IsKeyDown(rightMove) && kbState.IsKeyDown(crouchMove))
                            {
                                player.State = PlayerState.CrouchLeft;
                            }
                            //Is crouching
                            if (kbState.IsKeyDown(crouchMove)) { }
                            //Transitions to standing
                            else
                            {
                                player.State = PlayerState.FaceRight;
                            }
                            break;
                    }
                    //jumping switch statement
                    switch (player.JumpingState)
                    {
                        case PlayerJumpingState.Standing:
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
                            }
                            break;
                    }
                    switch (player.AttackingState)
                    {
                        case PlayerAttackingState.IsNotAttacking:
                            if (SingleKeyPress(attack, kbState))
                            {
                                player.AttackingState = PlayerAttackingState.IsAttacking;
                            }
                            break;
                        case PlayerAttackingState.IsAttacking:

                            attackingCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                            if (attackingCurrentTime >= attackingCountDuration)
                            {
                                attackingCounter++;
                                //reset the timer to loop again
                                attackingCurrentTime -= attackingCountDuration; // "use up" the time
                            }
                            //if the counter is greater then our limit
                            //the pause has completed
                            if (attackingCounter >= attackingLimit)
                            {
                                //reset the counter
                                attackingCounter = 0;
                                player.AttackingState = PlayerAttackingState.IsNotAttacking;
                            }
                            break;
                    }*/
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
