using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal_Dusk.Managers;

namespace Terminal_Dusk.PlayerComponents
{
    class PlayerControls
    {
        private KeyboardState kbState;
        private KeyboardState prevKbState;
        private Player player;
        private PlayerVariableManager playerManager;

        private Keys rightMove;
        private Keys leftMove;
        private Keys crouchMove;
        private Keys upMove;
        private Keys attack;

        private bool topCollision;
        private bool leftCollision;
        private bool rightCollision;
        private bool bottomCollision;

        //Creates the physics of the jump
        int jumpSpeed = -16;

        private PlayerState playerState;
        public PlayerState PlayerState { get { return playerState; } set { playerState = value; } }

        private PlayerJumpingState jumpState;
        public PlayerJumpingState JumpState { get { return jumpState; } set { jumpState = value; } }

        private PlayerAttackingState attackState;
        public PlayerAttackingState AttackState { get { return attackState; } set { attackState = value; } }

        public PlayerControls(Player player, PlayerVariableManager playerManager)
        {
            this.player = player;
            this.playerManager = playerManager;

            rightMove = playerManager.KeysArray[0];
            leftMove = playerManager.KeysArray[1];
            crouchMove = playerManager.KeysArray[2];
            upMove = playerManager.KeysArray[3];
            attack = playerManager.KeysArray[4];

            topCollision = playerManager.BlockCollisionArray[0];
            leftCollision = playerManager.BlockCollisionArray[1];
            rightCollision = playerManager.BlockCollisionArray[2];
            bottomCollision = playerManager.BlockCollisionArray[3];
        }

        public void KeysUpdate()
        {
            rightMove = playerManager.KeysArray[0];
            leftMove = playerManager.KeysArray[1];
            crouchMove = playerManager.KeysArray[2];
            upMove = playerManager.KeysArray[3];
            attack = playerManager.KeysArray[4];
        }

        private void CollisionUpdate()
        {
            topCollision = playerManager.BlockCollisionArray[0];
            leftCollision = playerManager.BlockCollisionArray[1];
            rightCollision = playerManager.BlockCollisionArray[2];
            bottomCollision = playerManager.BlockCollisionArray[3];
        }

        public void StateUpdate(GameTime gameTime)
        {
            CollisionUpdate();

            kbState = Keyboard.GetState();

            switch (player.State)
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
                        if (!rightCollision)
                        {
                            player.State = PlayerState.WalkLeft;
                        }
                    }
                    //Transitions to crouching
                    else if (kbState.IsKeyDown(crouchMove) && prevKbState.IsKeyUp(crouchMove))
                    {
                        player.State = PlayerState.CrouchLeft;
                    }

                    playerState = player.State;
                    break;

                case PlayerState.WalkLeft:
                    if (rightCollision)
                    {
                        player.State = PlayerState.FaceLeft;
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

                    playerState = player.State;
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
                        if (!leftCollision)
                        {
                            player.State = PlayerState.WalkRight;
                        }
                    }
                    //Transitions to crouching
                    else if (kbState.IsKeyDown(crouchMove) && prevKbState.IsKeyUp(crouchMove))
                    {
                        player.State = PlayerState.CrouchRight;
                    }

                    playerState = player.State;
                    break;

                case PlayerState.WalkRight:
                    if (leftCollision)
                    {
                        player.State = PlayerState.FaceRight;
                    }
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

                    playerState = player.State;
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

                    playerState = player.State;
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

                    playerState = player.State;
                    break;
            }

            //jumping switch statement
            switch (player.JumpingState)
            {
                case PlayerJumpingState.Standing:
                    playerManager.JumpingCurrentTime = 0f;

                    if (SingleKeyPress(upMove, kbState))
                    {
                        player.JumpingState = PlayerJumpingState.Jumping;
                        jumpSpeed = -16;//Give it upward thrust
                    }
                    if (!topCollision)
                    {
                        jumpSpeed = 0;
                        player.JumpingState = PlayerJumpingState.Falling;
                    }

                    jumpState = player.JumpingState;
                    break;

                case PlayerJumpingState.Jumping:
                    if (bottomCollision & jumpSpeed <= 0)
                    {
                        jumpSpeed = 0;
                        player.JumpingState = PlayerJumpingState.Falling;
                    }

                    if (player.Y != playerManager.StartHeight - (45 * 3)) //Need to use proper scale variable
                    {
                        if (jumpSpeed < 15)
                        {
                            player.Y += jumpSpeed;
                            jumpSpeed++; //Acts as the physics accelerating/deccelerating
                            if (jumpSpeed == -1)
                            {
                                jumpSpeed++;
                            }
                        }
                        else if (jumpSpeed == 15)
                        {
                            player.Y += jumpSpeed;
                        }
                    }

                    //Keeps player a peak for a small amount of time
                    else
                    {
                        playerManager.JumpingCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                        if (playerManager.JumpingCurrentTime >= playerManager.JumpingCountDuration)
                        {
                            playerManager.JumpingCounter++;
                            //reset the timer to loop again
                            playerManager.JumpingCurrentTime -= playerManager.JumpingCountDuration; // "use up" the time
                        }
                        //if the counter is greater then our limit
                        //the pause has completed
                        if (playerManager.JumpingCounter >= 1) //Previously jumpingLimit
                        {
                            //reset the counter
                            playerManager.JumpingCounter = 0;
                            //continues movement
                            jumpSpeed = 2;
                            player.Y += jumpSpeed;
                            jumpSpeed++;
                        }
                    }
                    //Allows player to leave ground
                    if (jumpSpeed < 0)
                    {
                        topCollision = false;
                    }

                    if (topCollision)
                    //If it's farther than ground
                    {
                        //TODO: Find a proper way to stop. W/out player.Y statement sinks into ground
                        player.Y = playerManager.LandingHeight;//Then set it on
                        player.JumpingState = PlayerJumpingState.Standing;
                    }
                    break;

                case PlayerJumpingState.Falling:
                    if (!topCollision)
                    {
                        if (jumpSpeed < 15)
                        {
                            player.Y += jumpSpeed;
                            jumpSpeed++;
                        }
                        else if (jumpSpeed == 15)
                        {
                            player.Y += jumpSpeed;
                        }
                    }
                    else
                    {
                        //TODO: Find a proper way to stop. W/out player.Y statement sinks into ground
                        player.Y = playerManager.LandingHeight;//Then set it on
                        player.JumpingState = PlayerJumpingState.Standing;
                    }

                    jumpState = player.JumpingState;
                    break;
            }

            switch (player.AttackingState)
            {
                case PlayerAttackingState.IsNotAttacking:
                    if (SingleKeyPress(attack, kbState))
                    {
                        player.AttackingState = PlayerAttackingState.IsAttacking;
                    }

                    attackState = player.AttackingState;
                    break;
                case PlayerAttackingState.IsAttacking:

                    playerManager.AttackingCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (playerManager.AttackingCurrentTime >= playerManager.AttackingCountDuration)
                    {
                        playerManager.AttackingCounter++;
                        //reset the timer to loop again
                        playerManager.AttackingCurrentTime -= playerManager.AttackingCountDuration; // "use up" the time
                    }
                    //if the counter is greater then our limit
                    //the pause has completed
                    if (playerManager.AttackingCounter >= 1) //Previously attackingLimit
                    {
                        //reset the counter
                        playerManager.AttackingCounter = 0;
                        player.AttackingState = PlayerAttackingState.IsNotAttacking;
                    }

                    attackState = player.AttackingState;
                    break;
            }

            prevKbState = kbState;
        }

        //A helper method to check single key press
        private bool SingleKeyPress(Keys key, KeyboardState currentKbState)
        {
            if (currentKbState.IsKeyDown(key) && prevKbState.IsKeyUp(key))
            {
                return true;
            }
            return false;
        }
    }
}
