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
        private bool[] blockCollisionArray; //Should be Top Left Right Bottom
        public bool[] BlockCollisionArray { get { return blockCollisionArray; } set { blockCollisionArray = value; } }

        //Variables for jumping
        private int jumpingCounter = 0;
        public int JumpingCounter { get { return jumpingCounter; } set { jumpingCounter = value; } }

        private float jumpingCountDuration = 0.06f; //every  .06s.
        public float JumpingCountDuration { get { return jumpingCountDuration; } }

        private float jumpingCurrentTime = 0f;
        public float JumpingCurrentTime { get { return jumpingCurrentTime; } set { jumpingCurrentTime = value; } }

        //Tracks player begining height
        private int startHeight;
        public int StartHeight { get { return startHeight; } set { startHeight = value; } }
        //Adjust where you land so that you don't get stuck in the ground.
        private int landingHeight;
        public int LandingHeight { get { return landingHeight; } set { landingHeight = value; } }

        //Fields for attacking
        private int attackingCounter = 0;
        public int AttackingCounter { get { return attackingCounter; } set { attackingCounter = value; } }

        private float attackingCountDuration = .13f; //every  .13s.
        public float AttackingCountDuration { get { return attackingCountDuration; } }

        private float attackingCurrentTime = 0f;
        public float AttackingCurrentTime { get { return attackingCurrentTime; } set { attackingCurrentTime = value; } }


        //Constructor
        public PlayerVariableManager(Keys[] keysArray, bool[] blockCollisionArray)
        {
            this.keysArray = keysArray;
            this.blockCollisionArray = blockCollisionArray;
        }

        public void UpdateControls(Keys[] newKeys)
        {
            keysArray = newKeys;
        }

        public void UpdateCollision(bool[] newColl)
        {
            blockCollisionArray = newColl;
        }
    }
}
