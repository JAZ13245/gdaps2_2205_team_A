using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Prototype_Character_Movement
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D player;
        private Vector2 playerPosition;
        private float speed = 1.2f;
        private KeyboardState kbState;
        private KeyboardState prevKBState;

        private Vector2 startingPosition = Vector2.Zero;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            playerPosition = new Vector2(8, 120);
            Player player = new Player();

            _graphics.PreferredBackBufferWidth = 320;
            _graphics.PreferredBackBufferHeight = 180;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            player = this.Content.Load<Texture2D>("pixel_charTest");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            ProcessInput();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(player, playerPosition, Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }


        private void ProcessInput()
        {

            

            if (kbState.IsKeyDown(Keys.Space) && prevKBState.IsKeyUp(Keys.Space))
            {

            }
            //prevKBState = kbState;

            if (kbState.IsKeyDown(Keys.A))
            {
                playerPosition.X -= speed;
            }

            if (kbState.IsKeyDown(Keys.D))
            {
                playerPosition.X += speed;
            }

        }

        
    }
}
