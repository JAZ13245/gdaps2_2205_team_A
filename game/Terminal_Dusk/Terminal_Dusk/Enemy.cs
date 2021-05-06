using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Terminal_Dusk
{
    //declares the different states the enemy can be in
    enum EnemyState
    {
        Idle, //standing still, picks random direction to move in (slime)
        IdleBehaviour, //slime - jumps in chosen direction
        Attacking, //imp - swoops down towards player
        Dying //self explanatory, enemy dies after taking a hit
    }
    class Enemy : GameObject
    {
        //all fields used for enemy
        protected int health;
        protected int strength; 
        protected int speed;
        protected EnemyState currentState;
        protected Random enemyRNG;
        protected GameState state;
        protected PlayerState playerState; //enemy must use playerstate to determine if it's being attacked
        
        public EnemyState CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        public GameState State
        {
            set { state = value; }
        }

        public PlayerState PlayerState
        {
            set { playerState = value; }
        }

        //standard constructor for all enemies
        public Enemy(Texture2D sprite, Rectangle location, GameState state, PlayerState playerState, int speed) : base(sprite, location)
        {
            image = sprite;
            position = location;
            this.state = state;
            this.playerState = playerState;
            this.speed = speed;
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




        public virtual void ScrollWithPlayer() { }
    }
}
