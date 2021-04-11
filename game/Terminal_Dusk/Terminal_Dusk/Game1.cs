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

        //Environment list
        private List<Environment> environments = new List<Environment>();
        private List<Texture2D> envirImgs = new List<Texture2D>();
        Environment envirConverter;

        //SkyBackground object
        private SkyBackground skyBackground;
        //GameBackground
        private EnvironmentBackground gameBackground;
        //Shrubs
        private EnvironmentBackground shrubs;
        //Ground
        private CollisionBlock ground;

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
            Vector2 playerLoc = new Vector2(GraphicsDevice.Viewport.Width / 4, GraphicsDevice.Viewport.Height - 47 * scale);//3 is scale
            playerSpreadSheet = Content.Load<Texture2D>("pixel_charTestScale");
            player = new Player(playerSpreadSheet, playerLoc, PlayerState.FaceRight);

            //slime enemy
            slimeSpriteSheet = Content.Load<Texture2D>("slimeEnemyScaled");
            slime1 = new Slime(slimeSpriteSheet, new Rectangle(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height -150, 240, 240),currentState,player.State,1);

            //Loading to Environment Texture List
            envirImgs.Add(Content.Load<Texture2D>("SkyBackgroundScale"));
            envirImgs.Add(Content.Load<Texture2D>("Scroll background(update 2)"));
            envirImgs.Add(Content.Load<Texture2D>("DirtWithGrassScale"));
            envirImgs.Add(Content.Load<Texture2D>("ShrubsScale"));

            //Sky Background
            skyBackground = new SkyBackground(envirImgs[0], new Rectangle(0, 90*scale - 2012*scale, 320*scale, 2012*scale), currentState);//3 is scale
            envirConverter = (Environment)skyBackground;
            environments.Add(envirConverter);

            //Background
            gameBackground = new EnvironmentBackground(envirImgs[1], new Rectangle(0, 0, 440*scale, 180*scale), currentState, player.State, 1); //1 is speed
            envirConverter = (Environment)gameBackground;
            environments.Add(envirConverter);

            //Ground
            for (int i = 0; i < 1000; i++)
            {
                ground = new CollisionBlock(envirImgs[2], new Rectangle(i*scale, GraphicsDevice.Viewport.Height - 10*scale, 10 * scale, 10 * scale), currentState, player.State, 1);
                envirConverter = (Environment)ground;
                environments.Add(envirConverter);
                i += 9;
            }

            //Shrubs //Needs more suitable class
            /*shrubs = new EnvironmentBackground(envirImgs[3], new Rectangle(0, 0, 50*scale, 90*scale), currentState, player.State, 2); //2 is speed
            envirConverter = (Environment)shrubs;
            environments.Add(envirConverter);*/
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
                    for (int i = 0; i < environments.Count; i++)
                    {
                        environments[i].Update(gameTime);
                    }

                    slime1.Update(gameTime);

                    player.UpdateAnimation(gameTime);//animation update
                    //Logic should be moved and handled in Player class, just copy/pasted for ease
                    //Should be able to go from walking to crouching more easily
                    switch (player.State)
                    {
                        case PlayerState.FaceLeft:
                            //Changes direction
                            if (kbState.IsKeyDown(rightMove) && !kbState.IsKeyDown(leftMove))
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
                            //Transitions to crouch
                            if (kbState.IsKeyDown(crouchMove) && prevKbState.IsKeyUp(crouchMove))
                            {
                                player.State = PlayerState.CrouchLeft;
                            }
                            //Allows smoother movement Ex: holding leftMove then also pressing rightMove then letting go of rightMove
                            if (kbState.IsKeyDown(rightMove) && prevKbState.IsKeyUp(rightMove))
                            {
                                player.State = PlayerState.FaceRight;
                            }
                            //Moves left
                            if (kbState.IsKeyDown(leftMove)) { }
                            //Transitions to standing
                            else
                            {
                                player.State = PlayerState.FaceLeft;
                            }
                            break;
                        case PlayerState.FaceRight:
                            //Changes direction
                            if (kbState.IsKeyDown(leftMove) && !kbState.IsKeyDown(rightMove))
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
                            //Transitions to crouch
                            if (kbState.IsKeyDown(crouchMove) && prevKbState.IsKeyUp(crouchMove))
                            {
                                player.State = PlayerState.CrouchRight;
                            }
                            //Allows smoother movement (Example in WalkLeft)
                            if (kbState.IsKeyDown(leftMove) && prevKbState.IsKeyUp(leftMove))
                            {
                                player.State = PlayerState.FaceLeft;
                            }
                            //Moves right
                            if (kbState.IsKeyDown(rightMove)) { }
                            //Transitions to standing
                            else
                            {
                                player.State = PlayerState.FaceRight;
                            }
                            break;
                        case PlayerState.CrouchLeft:
                            //Changes direction while crouching
                            if (kbState.IsKeyDown(rightMove) && !kbState.IsKeyDown(leftMove) && kbState.IsKeyDown(crouchMove))
                            {
                                player.State = PlayerState.CrouchRight;
                            }
                            //Is crouching
                            if (kbState.IsKeyDown(crouchMove)) { }
                            //Transitions to standing
                            else
                            {
                                player.State = PlayerState.FaceLeft;
                            }
                            break;
                        case PlayerState.CrouchRight:
                            //Changes direction while crouching
                            if (kbState.IsKeyDown(leftMove) && !kbState.IsKeyDown(rightMove) && kbState.IsKeyDown(crouchMove))
                            {
                                player.State = PlayerState.CrouchLeft;
                            }
                            //Is crouching
                            if (kbState.IsKeyDown(crouchMove)) { }
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

            //Updates the game state for environments
            for (int i = 0; i < environments.Count; i++)
            {
                environments[i].State = currentState;
            }
            //Updates the player state for environments
            for (int i = 0; i < environments.Count; i++)
            {
                environments[i].PlayerState = player.State;
            }

            slime1.State = currentState;
            slime1.PlayerState = player.State;


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
                    for(int i=0; i < environments.Count; i++)
                    {
                        environments[i].Draw(_spriteBatch);
                    }
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
