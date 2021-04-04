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


        //for drawing the player
        private Player player;
        private Texture2D playerSpreadSheet;

        //for the slime
        private Slime slime1;
        private Texture2D slimeSpriteSheet;

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

        //options for keyboard controls
        private Keys rightMove = Keys.D;
        private Keys leftMove = Keys.A;
        private Keys crouchMove = Keys.S;
        private Keys upMove = Keys.W;
        private bool usingWASD = true;
        private bool usingArrow = false;


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
            backImgs.Add(Content.Load<Texture2D>("TitleScreen1Scale"));
            // TODO: use this.Content to load your game content here
            labelFont = this.Content.Load<SpriteFont>("LabelFont");

            //font for the button
            buttonFont = Content.Load<SpriteFont>("LabelFont");

            //adding background(s)
            backgrounds.Add(new Environment(backImgs[0],
                    new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)));
            //adding buttons
            //main menu start button
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,           // device to create a custom texture
                    new Rectangle(10, 40, 100, 50),    // where to put the button
                    "Start",                        // button label
                    buttonFont,                               // label font
                    Color.Purple));
            //main menu exit button
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(10, 260, 100, 50),
                    "Exit",
                    buttonFont,
                    Color.Purple));
            //main menu options button
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(10, 150, 100, 50),
                    "Options",
                    buttonFont,
                    Color.Purple));
            //options menu go to main button
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(10, 40, 200, 50),
                    "Return to Main Menu",
                    buttonFont,
                    Color.Purple));
            //options menu go to pause'
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(10, 40, 200, 50),
                    "Return to Pause Menu",
                    buttonFont,
                    Color.Purple));
            //options menu change controls
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(10, 150, 200, 50),
                    "Change WASD Control",
                    buttonFont,
                    Color.Purple));
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle(10, 260, 200, 50),
                    "Change Arrow Keys Control",
                    buttonFont,
                    Color.Purple));

            //main menu buttons
            buttons[0].OnLeftButtonClick += GoToSaveMenu;
            buttons[1].OnLeftButtonClick += ExitGame;
            buttons[2].OnLeftButtonClick += MainOptions;
            //options menu buttons
            buttons[3].OnLeftButtonClick += GoToMainMenu;
            buttons[4].OnLeftButtonClick += GoToPauseMenu;
            //buttons that change controls
            buttons[5].OnLeftButtonClick += ChangeToWASD;
            buttons[6].OnLeftButtonClick += ChangeToArrows;


            //Game State Loads

            // Sets up the player location
            Vector2 playerLoc = new Vector2(GraphicsDevice.Viewport.Width / 4, GraphicsDevice.Viewport.Height - 50 * scale);//3 is scale
            playerSpreadSheet = Content.Load<Texture2D>("pixel_charTestScale");
            player = new Player(playerSpreadSheet, playerLoc, PlayerState.FaceRight);

            //slime enemy
            slimeSpriteSheet = Content.Load<Texture2D>("slimeEnemyScaled");
            slime1 = new Slime(slimeSpriteSheet, new Rectangle(GraphicsDevice.Viewport.Width / 2, 0, 240, 240));

            //Sky Background
            skyTexture = Content.Load<Texture2D>("SkyBackgroundScale");
            skyBackground = new SkyBackground(skyTexture, new Rectangle(0, 90 * scale - 2012 * scale, 320 * scale, 2012 * scale), currentState);//3 is scale

            //Background
            backgroundTexture = Content.Load<Texture2D>("TestScrollScale");
            gameBackground = new EnvironmentBackground(backgroundTexture, new Rectangle(0, 0, 437 * scale, 180 * scale), currentState, player.State);//3 is scale
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            kbState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            switch (currentState)
            {
                case GameState.MainMenu:
                    for (int i = 0; i < 3; i++)
                    {
                        buttons[i].Update();
                    }
                    break;

                case GameState.PauseMenu:
                    ProcessPauseMenu(kbState);
                    break;

                case GameState.OptionsMenu:
                    if (wasMainOrPause)
                    {
                        buttons[3].Update();
                        buttons[5].Update();
                        buttons[6].Update();
                    }
                    else
                    {
                        buttons[4].Update();
                        buttons[5].Update();
                        buttons[6].Update();
                    }

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

                    slime1.Update(gameTime);

                    player.UpdateAnimation(gameTime);//animation update
                    //Logic should be moved and handled in Player class, just copy/pasted for ease
                    //Should be able to go from walking to crouching more easily
                    switch (player.State)
                    {
                        case PlayerState.FaceLeft:
                            //Changes direction
                            if (kbState.IsKeyDown(rightMove) && prevKbState.IsKeyUp(rightMove))
                            {
                                player.State = PlayerState.FaceRight;
                            }
                            //Transitions to walking
                            else if (kbState.IsKeyDown(leftMove) && prevKbState.IsKeyDown(leftMove))
                            {
                                player.State = PlayerState.WalkLeft;
                            }
                            //Transitions to crouching
                            else if (kbState.IsKeyDown(crouchMove) && prevKbState.IsKeyUp(crouchMove))
                            {
                                player.State = PlayerState.CrouchLeft;
                            }

                            break;
                        case PlayerState.WalkLeft:
                            if (kbState.IsKeyDown(crouchMove) && prevKbState.IsKeyUp(crouchMove))
                            {
                                player.State = PlayerState.CrouchLeft;
                            }
                            //Moves Mario left
                            if (kbState.IsKeyDown(leftMove))
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
                            if (kbState.IsKeyDown(leftMove) && prevKbState.IsKeyUp(leftMove))
                            {
                                player.State = PlayerState.FaceLeft;
                            }
                            //Transitions to walking
                            else if (kbState.IsKeyDown(rightMove))
                            {
                                player.State = PlayerState.WalkRight;
                            }
                            //Transitions to crouching
                            else if (kbState.IsKeyDown(crouchMove) && prevKbState.IsKeyUp(crouchMove))
                            {
                                player.State = PlayerState.CrouchRight;
                            }
                            break;
                        case PlayerState.WalkRight:
                            if (kbState.IsKeyDown(crouchMove) && prevKbState.IsKeyUp(crouchMove))
                            {
                                player.State = PlayerState.CrouchRight;
                            }
                            //Moves Mario right
                            if (kbState.IsKeyDown(rightMove))
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
                            if (kbState.IsKeyDown(crouchMove))
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
                            if (kbState.IsKeyDown(crouchMove))
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
                    backgrounds[0].Draw(_spriteBatch);
                    for (int i = 0; i < 3; i++)
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
                    slime1.Draw(_spriteBatch);

                    _spriteBatch.DrawString(labelFont, "This is the Game Play State", new Vector2(5, 5), Color.White);
                    _spriteBatch.DrawString(labelFont, "Press P to pause", new Vector2(5, 25), Color.White);
                    break;
                case GameState.PauseMenu:
                    _spriteBatch.DrawString(labelFont, "This is the pause Menu", new Vector2(5, 5), Color.White);
                    _spriteBatch.DrawString(labelFont, "Press M for the Main Menu, O for options, or Escape to Exit", new Vector2(5, 25), Color.White);
                    break;
                case GameState.OptionsMenu:
                    _spriteBatch.DrawString(labelFont, "This is the Options Menu", new Vector2(5, 5), Color.White);
                    if (wasMainOrPause)
                    {
                        buttons[3].Draw(_spriteBatch);
                        buttons[5].Draw(_spriteBatch);
                        buttons[6].Draw(_spriteBatch);
                    }
                    else
                    {
                        buttons[4].Draw(_spriteBatch);
                        buttons[5].Draw(_spriteBatch);
                        buttons[6].Draw(_spriteBatch);
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
            //Added so it actually quits
            Exit();
        }
        //method to go to the main menu
        private void GoToMainMenu()
        {
            currentState = GameState.MainMenu;
        }
        //method to go to the pause menu
        private void GoToPauseMenu()
        {
            currentState = GameState.PauseMenu;
        }

        //method to go to options from main
        private void MainOptions()
        {
            wasMainOrPause = true;
            currentState = GameState.OptionsMenu;
        }

        //method for changing controls
        private void ChangeToWASD()
        {
            rightMove = Keys.D;
            leftMove = Keys.A;
            crouchMove = Keys.S;
            upMove = Keys.W;
            usingWASD = true;
            usingArrow = false;
        }

        private void ChangeToArrows()
        {
            rightMove = Keys.Right;
            leftMove = Keys.Left;
            crouchMove = Keys.Down;
            upMove = Keys.Up;
            usingWASD = false;
            usingArrow = true;
        }
    }
}
