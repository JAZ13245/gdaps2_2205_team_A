using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal_Dusk.Managers
{
    //Holds the variables that only the player class needs to know and allows less culstered constructors
    class PlayerVariableManager
    {
        //Fields
        //Keys array
        private Keys[] keysArray;
        public Keys[] KeysArray { get { return keysArray; } set { keysArray = value; } }

        //Block collision array
        private bool[] blockCollisionArray;
        public bool[] BlockCollisionArray { get { return blockCollisionArray; } set { blockCollisionArray = value; } }

        //Variables for jumping
        private int jumpingCounter = 0;
        public int JumpingCounter { get { return jumpingCounter; } set { jumpingCounter = value; } }

        private float jumpingCountDuration = 0.06f; //every  .06s.
        public float JumpingCountDuration { get { return jumpingCountDuration; } }

        private float jumpingCurrentTime = 0f;
        public float JumpingCurrentTime { get { return jumpingCurrentTime; } set { jumpingCurrentTime = value; } }

        //Creates the physics of the jump
        private int jumpSpeed;
        //Tracks player begining height
        private int startHeight;
        //Adjust where you land so that you don't get stuck in the ground.
        private int landingHeight;

        //Fields for attacking
        private int attackingCounter = 0;
        private int attackingLimit = 2;
        private float attackingCountDuration = .13f; //every  .13s.
        private float attackingCurrentTime = 0f;

        //Constructor
        public PlayerVariableManager(Keys[] keysArray, bool[] blockCollisionArray)
        {
            this.keysArray = keysArray;
            this.blockCollisionArray = blockCollisionArray;
        }
    }
}
