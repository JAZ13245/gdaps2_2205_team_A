using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Terminal_Dusk
{
    //enumertaion for all the different game states
    enum GameState
    {
        MainMenu, SaveFileMenu, OptionsMenu, ExitGame, GamePlayState, PauseMenu
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameState currentState;
        private SpriteFont LabelFont;
        private bool wasMainOrPause = false;
        //for drawing the MC
        private Player mc;
        // User input fields
        private KeyboardState kbState;
        private KeyboardState prevKbState;
        private MouseState mouseState;
        private MouseState prevMouseState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            currentState = GameState.MainMenu;
            _graphics.PreferredBackBufferWidth = 500;
            _graphics.PreferredBackBufferHeight = 500;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            // TODO: use this.Content to load your game content here
            LabelFont = this.Content.Load<SpriteFont>("LabelFont");
            // Sets up the mario location
            Vector2 mcLoc = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);



        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            KeyboardState kbState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            switch (currentState)
            {
                case GameState.MainMenu:
                    ProcessMainMenu(kbState);
                    break;
                case GameState.PauseMenu:
                    ProcessPauseMenu(kbState);
                    break;
                case GameState.OptionsMenu:
                    ProcessOptionsMenu(kbState);
                    break;
                case GameState.SaveFileMenu:
                    ProcessSaveFileMenu(kbState, mouseState);
                    break;
                case GameState.GamePlayState:
                    ProcessGamePlayState(kbState);
                    break;
                case GameState.ExitGame:
                    break;
                default:
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            switch (currentState)
            {
                case GameState.MainMenu:
                    _spriteBatch.DrawString(LabelFont, "This is the Main Menu", new Vector2(5, 5), Color.White);
                    _spriteBatch.DrawString(LabelFont, "Press O for the Options Menu, S for Save Files, or Escape to Exit", new Vector2(5, 25), Color.White);
                    break;
                case GameState.SaveFileMenu:
                    _spriteBatch.DrawString(LabelFont, "This is the Save Files Menu", new Vector2(5, 5), Color.White);
                    _spriteBatch.DrawString(LabelFont, "Press M for the Main Menu or LMB to go to GamePlay", new Vector2(5, 25), Color.White);
                    break;
                case GameState.GamePlayState:
                    _spriteBatch.DrawString(LabelFont, "This is the Game Play State", new Vector2(5, 5), Color.White);
                    _spriteBatch.DrawString(LabelFont, "Press P to pause", new Vector2(5, 25), Color.White);
                    break;
                case GameState.PauseMenu:
                    _spriteBatch.DrawString(LabelFont, "This is the pause Menu", new Vector2(5, 5), Color.White);
                    _spriteBatch.DrawString(LabelFont, "Press M for the Main Menu, O for options, or Escape to Exit", new Vector2(5, 25), Color.White);
                    break;
                case GameState.OptionsMenu:
                    _spriteBatch.DrawString(LabelFont, "This is the Options Menu", new Vector2(5, 5), Color.White);
                    if(wasMainOrPause)
                    {
                        _spriteBatch.DrawString(LabelFont, "Press M for the Main Menu", new Vector2(5, 25), Color.White);
                    }
                    else
                    {
                        _spriteBatch.DrawString(LabelFont, "Press P for the Pause Menu", new Vector2(5, 25), Color.White);
                    }
                    break;
                case GameState.ExitGame:
                    break;
            }
            _spriteBatch.End();

            base.Draw(gameTime);
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

        //helper method for Main Menu
        private void ProcessMainMenu(KeyboardState kbState)
        {
            kbState = Keyboard.GetState();
            if(SingleKeyPress(Keys.S, kbState))
            {
                currentState = GameState.SaveFileMenu;
            }
            if(SingleKeyPress(Keys.O, kbState))
            {
                currentState = GameState.OptionsMenu;
                wasMainOrPause = true;
            }
            
        }
        //helper method for PauseMenu
        private void ProcessPauseMenu(KeyboardState kbState)
        {
            kbState = Keyboard.GetState();
            if (SingleKeyPress(Keys.M, kbState))
            {
                currentState = GameState.MainMenu;
            }
            if (SingleKeyPress(Keys.O, kbState))
            {
                currentState = GameState.OptionsMenu;
                wasMainOrPause = false;
            }
            if (SingleKeyPress(Keys.Escape, kbState))
            {
                currentState = GameState.ExitGame;
            }
        }
        //helper method for OptionsMenu
        private void ProcessOptionsMenu(KeyboardState kbState)
        {
            kbState = Keyboard.GetState();
            if (SingleKeyPress(Keys.M, kbState) && wasMainOrPause == true)
            {
                currentState = GameState.MainMenu;
            }
            if (SingleKeyPress(Keys.P, kbState) && wasMainOrPause == false)
            {
                currentState = GameState.PauseMenu;
            }
        }
        //helper method for SaveFileMenu
        private void ProcessSaveFileMenu(KeyboardState kbState, MouseState mouseState)
        {
            kbState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            if (SingleKeyPress(Keys.M, kbState))
            {
                currentState = GameState.MainMenu;
            }
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                currentState = GameState.GamePlayState;
            }
        }
        //helper method for GamePlayState
        private void ProcessGamePlayState(KeyboardState kbState)
        {
            kbState = Keyboard.GetState();
            if (SingleKeyPress(Keys.P, kbState))
            {
                currentState = GameState.PauseMenu;
            }
        }


    }
}
