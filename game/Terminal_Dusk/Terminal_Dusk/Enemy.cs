﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Terminal_Dusk
{
    class Enemy : GameObject
    {
        protected int health;
        protected int strength;
        protected int speed;
        protected int[] drops;
        protected EnemyState currentState;
        protected enum EnemyState
        {
            Idle,
            IdleBehaviour,
            Hostile,
            Attacking,
            Dying
        }


        public Enemy(Texture2D sprite, Rectangle location) : base(sprite, location)
        {
            image = sprite;
            position = location;
        }
        
        //a method to check if the enemy is touching another game object
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
            switch(currentState)
            {
                case EnemyState.Idle:
                    break;

                case EnemyState.IdleBehaviour:
                    break;

                case EnemyState.Hostile:
                    break;
                
                case EnemyState.Attacking:
                    break;
                
                case EnemyState.Dying:
                    break;
                
                default:
                    break;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    break;

                case EnemyState.IdleBehaviour:
                    break;

                case EnemyState.Hostile:
                    break;

                case EnemyState.Attacking:
                    break;

                case EnemyState.Dying:
                    break;

                default:
                    break;
            }
            base.Draw(sb);
        }


        public override void Save(string filename){}
        
        public override void Load(string filename){}
        
    }
}