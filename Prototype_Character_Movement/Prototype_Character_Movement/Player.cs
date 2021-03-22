using System;
using System.Collections.Generic;
using System.Text;

namespace Prototype_Character_Movement
{
    enum State
    {
        Walking,
        Jumping
    }

    class Player
    {
        private State currentState = State.Walking;


        private void Jump()
        {
            if (currentState != State.Jumping)
            {
                currentState = State.Jumping;
            }
        }


    }
}
