using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Terminal_Dusk
{
    class Enemy : GameObject
    {
        private int health;
        private int strength;
        private int speed;
        private Texture2D spriteSheet;
        private int[] drops;
        private EnemyState currentState;
        private enum EnemyState
        {
            Idle,
            IdleBehaviour,
            Hostile,
            Attacking,
            Dying
        }


        public Enemy(Texture2D sprite, Rectangle location) : base(sprite, location)
        {
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
