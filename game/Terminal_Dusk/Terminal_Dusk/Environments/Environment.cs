using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Terminal_Dusk
{
    class Environment : GameObject
    {
        private Texture2D sprite;
        private Rectangle location;
        GameState state;
        PlayerState playerState;

        public virtual GameState State
        {
            set { state = value; }
        }

        public virtual PlayerState PlayerState
        {
            set { playerState = value; }
        }

        //uncomment code and change as necessary
        public Environment(Texture2D sprite, Rectangle location) : base(sprite, location)
        {
            this.sprite = sprite;
            this.location = location;
        }

        //a method to check if the player and the enviornment are touching       
        public bool CheckCollision(GameObject check)//some class or object inside the parantehses 
        {
            if (this.Position.Intersects(check.Position))
            {
                return true;
            }
            return false;
        }

        public override void Update(GameTime gameTime)
        {

        }
        
        public override void Save(string filename)
        {

        }
        
        public override void Load(string filename)
        {

        }
        
    }
}
