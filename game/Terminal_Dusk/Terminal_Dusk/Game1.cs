using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Terminal_Dusk.Environments;

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

        //Adjust screen size, should be added to options.
        private double screenSize;
        
        //for drawing the player
        private Player player;
        private Texture2D playerSpreadSheet;
        
        // User input fields
        private KeyboardState kbState;
        private KeyboardState prevKbState;
        private MouseState mouseState;
        private MouseState prevMouseState;

        //fields for button class
        private SpriteFont buttonFont;
        private List<Button> buttons = new List<Button>();

        //fields for background class
        private List<Environment> backgrounds = new List<Environment>();
        private List<Texture2D> backImgs = new List<Texture2D>();

        //field for timer
        private double timer;

        //SkyBackground object
        private SkyBackground skyBackground;
        private Texture2D skyTexture;
        //GameBackground
        private EnvironmentBackground gameBackground;
        private Texture2D backgroundTexture;
        //A scale for changing the size of the screen
        private int scale = 3;

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
            _graphics.PreferredBackBufferWidth = 320 * scale;
            _graphics.PreferredBackBufferHeight = 180 * scale;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //background images
            backImgs.Add(Content.Load<Texture2D>("TitleScreen1"));
            // TODO: use this.Content to load your game content here
            labelFont = this.Content.Load<SpriteFont>("LabelFont");
            
            //font for the button
            buttonFont = Content.Load<SpriteFont>("LabelFont");

            //adding background(s)
            backgrounds.Add(new Environment(backImgs[0],
                    new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)));
            //adding buttons
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,           // device to create a custom texture
                    new Rectangle(10, 40, 100, 50),    // where to put the button
                    "Start",                        // button label
                    buttonFont,                               // label font
                    Color.Purple));
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(10, 150, 100, 50),
                    "Exit",
                    buttonFont,
                    Color.Purple));

            //main menu buttons
            buttons[0].OnLeftButtonClick += GoToSaveMenu;
            buttons[1].OnLeftButtonClick += ExitGame;

            //Game State Loads

            // Sets up the player location
            Vector2 playerLoc = new Vector2(GraphicsDevice.Viewport.Width / 4, GraphicsDevice.Viewport.Height - 50*scale);//3 is scale
            playerSpreadSheet = Content.Load<Texture2D>("pixel_charTest");
            player = new Player(playerSpreadSheet, playerLoc, PlayerState.FaceRight);

            //Sky Background
            skyTexture = Content.Load<Texture2D>("SkyBackground");
            skyBackground = new SkyBackground(skyTexture, new Rectangle(0, 90*scale - 2012*scale, 320*scale, 2012*scale), currentState);//3 is scale

            //Background
            backgroundTexture = Content.Load<Texture2D>("TestScroll");
            gameBackground = new EnvironmentBackground(backgroundTexture, new Rectangle(0, 0, 437*scale, 180*scale), currentState, player.State);//3 is scale
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
                    for (int i = 0; i < 2; i++)
                    {
                        buttons[i].Update();
                    }
                    break;

                case GameState.PauseMenu:
                    ProcessPauseMenu(kbState);
                    break;

                case GameState.OptionsMenu:
                    ProcessOptionsMenu(kbState);

                    //add a way for the player to change the scale of the screen
                    break;

                case GameState.SaveFileMenu:
                    ProcessSaveFileMenu(kbState, mouseState);
                    break;

                case GameState.GamePlayState:
                    ProcessGamePlayState(kbState);
                    double temptimer = timer;
                    timer = temptimer - gameTime.ElapsedGameTime.TotalSeconds;
                    //Background
                    skyBackground.Update(gameTime);
                    gameBackground.Update(gameTime);

                    player.UpdateAnimation(gameTime);//animation update
                    //Logic should be moved and handled in Player class, just copy/pasted for ease
                    //Should be able to go from walking to crouching more easily
                    switch (player.State)
                    {
                        case PlayerState.FaceLeft:
                            //Changes direction
                            if (kbState.IsKeyDown(Keys.D) && prevKbState.IsKeyUp(Keys.D))
                            {
                                player.State = PlayerState.FaceRight;
                            }
                            //Transitions to walking
                            else if (kbState.IsKeyDown(Keys.A) && prevKbState.IsKeyDown(Keys.A))
                            {
                                player.State = PlayerState.WalkLeft;
                            }
                            //Transitions to crouching
                            else if (kbState.IsKeyDown(Keys.S) && prevKbState.IsKeyUp(Keys.S))
                            {
                                player.State = PlayerState.CrouchLeft;
                            }

                            break;
                        case PlayerState.WalkLeft:
                            //Moves Mario left
                            if (kbState.IsKeyDown(Keys.A))
                            {
                                //mario.X -= 3;
                            }
                            
                            //Transitions to standing
                            else
                            {
                                player.State = PlayerState.FaceLeft;
                            }
                            break;
                        case PlayerState.FaceRight:
                            //Changes direction
                            if (kbState.IsKeyDown(Keys.A) && prevKbState.IsKeyUp(Keys.A))
                            {
                                player.State = PlayerState.FaceLeft;
                            }
                            //Transitions to walking
                            else if (kbState.IsKeyDown(Keys.D))
                            {
                                player.State = PlayerState.WalkRight;
                            }
                            //Transitions to crouching
                            else if (kbState.IsKeyDown(Keys.S) && prevKbState.IsKeyUp(Keys.S))
                            {
                                player.State = PlayerState.CrouchRight;
                            }
                            break;
                        case PlayerState.WalkRight:
                            //Moves Mario right
                            if (kbState.IsKeyDown(Keys.D))
                            {
                                //player.X += 3;
                            }
                            //Transitions to standing
                            else
                            {
                                player.State = PlayerState.FaceRight;
                            }
                            break;
                        case PlayerState.CrouchLeft:
                            if (kbState.IsKeyDown(Keys.S))
                            {
                                //Should change location player is drawn to be lower
                            }
                            //Transitions to standing
                            else
                            {
                                player.State = PlayerState.FaceLeft;
                            }
                            break;
                        case PlayerState.CrouchRight:
                            if (kbState.IsKeyDown(Keys.S))
                            {
                                //Should change location player is drawn to be lower
                            }
                            //Transitions to standing
                            else
                            {
                                player.State = PlayerState.FaceRight;
                            }
                            break;
                    }
                    break;

                case GameState.ExitGame:
                    break;
                default:
                    break;
            }

            //Updates the game state in SkyBackground
            skyBackground.State = currentState;
            gameBackground.State = currentState;
            gameBackground.PlayerState = player.State;

            prevKbState = kbState;
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
                    backgrounds[0].Draw(_spriteBatch);
                    for (int i = 0; i < 2; i++)
                    {
                        buttons[i].Draw(_spriteBatch);
                    }
                    break;
                case GameState.SaveFileMenu:
                    _spriteBatch.DrawString(labelFont, "This is the Save Files Menu", new Vector2(5, 5), Color.White);
                    _spriteBatch.DrawString(labelFont, "Press M for the Main Menu or G to go to GamePlay", new Vector2(5, 25), Color.White);
                    break;
                case GameState.GamePlayState:
                    //Sky background
                    skyBackground.Draw(_spriteBatch);
                    //Background
                    gameBackground.Draw(_spriteBatch);
                    //Player
                    player.Draw(_spriteBatch);

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
            if (SingleKeyPress(Keys.P, kbState))
            {
                currentState = GameState.GamePlayState;
            }
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
        //also add a way for the player to change the scale and the keys for movement
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
            if (SingleKeyPress(Keys.G, kbState))
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
