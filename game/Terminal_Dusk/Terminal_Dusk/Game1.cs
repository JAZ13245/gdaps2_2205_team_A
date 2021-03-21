using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

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
        private SpriteFont labelFont;
        private bool wasMainOrPause = false;
        
        //for drawing the player
        private Player player;
        
        // User input fields
        private KeyboardState kbState;
        private KeyboardState prevKbState;
        private MouseState mouseState;
        private MouseState prevMouseState;

        //fields for button class
        private SpriteFont buttonFont;
        private List<Button> buttons = new List<Button>();
        private Color bgColor = Color.White;
        private Random rng = new Random();

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
            labelFont = this.Content.Load<SpriteFont>("LabelFont");
            // Sets up the mario location
            Vector2 playerLoc = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            //font for the button
            buttonFont = Content.Load<SpriteFont>("LabelFont");

            buttons.Add(new Button(
                    _graphics.GraphicsDevice,           // device to create a custom texture
                    new Rectangle(10, 40, 200, 100),    // where to put the button
                    "Start",                        // button label
                    buttonFont,                               // label font
                    Color.Purple));
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(10, 150, 200, 100),
                    "Exit",
                    buttonFont,
                    Color.Purple));

            buttons[0].OnLeftButtonClick += GoToSaveMenu;
            buttons[1].OnLeftButtonClick += ExitGame;

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
                    _spriteBatch.DrawString(labelFont, "This is the Main Menu", new Vector2(5, 5), Color.White);
                    _spriteBatch.DrawString(labelFont, "Press O for the Options Menu, S for Save Files, or Escape to Exit", new Vector2(5, 25), Color.White);
                    break;
                case GameState.SaveFileMenu:
                    _spriteBatch.DrawString(labelFont, "This is the Save Files Menu", new Vector2(5, 5), Color.White);
                    _spriteBatch.DrawString(labelFont, "Press M for the Main Menu or LMB to go to GamePlay", new Vector2(5, 25), Color.White);
                    break;
                case GameState.GamePlayState:
                    _spriteBatch.DrawString(labelFont, "This is the Game Play State", new Vector2(5, 5), Color.White);
                    _spriteBatch.DrawString(labelFont, "Press P to pause", new Vector2(5, 25), Color.White);
                    break;
                case GameState.PauseMenu:
                    _spriteBatch.DrawString(labelFont, "This is the pause Menu", new Vector2(5, 5), Color.White);
                    _spriteBatch.DrawString(labelFont, "Press M for the Main Menu, O for options, or Escape to Exit", new Vector2(5, 25), Color.White);
                    break;
                case GameState.OptionsMenu:
                    _spriteBatch.DrawString(labelFont, "This is the Options Menu", new Vector2(5, 5), Color.White);
                    if(wasMainOrPause)
                    {
                        _spriteBatch.DrawString(labelFont, "Press M for the Main Menu", new Vector2(5, 25), Color.White);
                    }
                    else
                    {
                        _spriteBatch.DrawString(labelFont, "Press P for the Pause Menu", new Vector2(5, 25), Color.White);
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

        //method to go to save menu with the button
        private void GoToSaveMenu()
        {
            currentState = GameState.SaveFileMenu;
        }
        //method to exit the game with the button
        private void ExitGame()
        {
            currentState = GameState.ExitGame;
        }


    }
}
