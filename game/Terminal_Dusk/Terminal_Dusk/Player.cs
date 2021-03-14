using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Terminal_Dusk
{
    // Mario State enum
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
        Vector2 playerLoc;       // Mc's location on the screen
        PlayerState state;
    }
}
