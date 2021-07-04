using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Terminal_Dusk.Environments;
using Terminal_Dusk.PlayerComponents;
using Terminal_Dusk.Managers;
using System.IO;

namespace Terminal_Dusk
{
    //enumertaion for all the different game states
    enum GameState
    {
        MainMenu, SaveFileMenu, OptionsMenu, ExitGame, GamePlayState, PauseMenu, GameOverMenu, Winner
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

        //for the player hp
        private Health health;
        private Texture2D healthSprite;

        //for the slime
        private Slime slime1;
        private Texture2D slimeSpriteSheet;
        //For imps
        private Imp imp1;
        private Texture2D impSpriteSheet;
        //Enemies List
        private List<Enemy> enemies = new List<Enemy>();

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
        private int counter = 1;
        private int limit = 300;
        private float countDuration = 1f; //every  1s.
        private float currentTime = 0f;

        
        //Environment list
        private List<Environment> environments = new List<Environment>();
        private List<Texture2D> envirImgs = new List<Texture2D>();
        Environment envirConverter;

        //SkyBackground object
        private SkyBackground skyBackground;
        //Sun object
        private Sun sun;
        //GameBackground
        private EnvironmentBackground gameBackground;
        //Foreground objects
        private Foreground foreground;
        //Ground
        private CollisionBlock ground;
        //Invisible wall at the begining of the game 
        private CollisionBlock startWallBlock;
        private List<CollisionBlock> startWall;

        //bools for collision
        private bool topCollision;
        private bool leftCollision;
        private bool rightCollision;
        private bool bottomCollision;
        private bool[] blockCollisionArray;

        //A scale for changing the size of the screen
        private int scale = 3;
        //Property so that constructors don't need to hold the scale
        public int Scale { get { return scale; } }

        //options for keyboard controls
        private Keys rightMove = Keys.D;
        private Keys leftMove = Keys.A;
        private Keys crouchMove = Keys.S;
        private Keys upMove = Keys.W;
        private Keys attack = Keys.Space;
        private Keys[] keysArray; //TODO: Array needs to be updated to be used properly.
        private bool usingWASD = true;

        //Manager
        private PlayerVariableManager playerVariableManager;

        //PlayerControls class
        private PlayerControls playerControls;

        //FileIO
        private string levelFile = "..\\..\\..\\FileIO\\levelFile.txt";


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //sets the size of the game window
            currentState = GameState.MainMenu;
            _graphics.PreferredBackBufferWidth = 320 * scale;
            _graphics.PreferredBackBufferHeight = 180 * scale;
            _graphics.ApplyChanges();

            keysArray = new Keys[5] { rightMove, leftMove, crouchMove, upMove, attack };
            blockCollisionArray = new bool[4];

            playerVariableManager = new PlayerVariableManager(keysArray, blockCollisionArray);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //background images
            backImgs.Add(Content.Load<Texture2D>("newTitleScale"));
            backImgs.Add(Content.Load<Texture2D>("gameoverScale"));
            //font used for the menu text
            labelFont = this.Content.Load<SpriteFont>("LabelFont");

            //font for the button
            buttonFont = Content.Load<SpriteFont>("LabelFont");

            //adding background(s)
            backgrounds.Add(new Environment(backImgs[0],
                    new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)));
            backgrounds.Add(new Environment(backImgs[1],
                    new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)));
            //adding buttons
            //main menu start button
            buttons.Add(new Button(
                    _graphics.GraphicsDevice, //device to create a custom texture
                    new Rectangle((10/3) * scale, (40/3) * scale, (100/3) * scale, (50/3) * scale), //where to put the button
                    "Start", //button label
                    buttonFont, //label font
                    Color.Purple)); //button color
            //main menu exit button
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle((10/3) * scale, (260/3) * scale, (100/3) * scale, (50/3) * scale),
                    "Exit",
                    buttonFont,
                    Color.Purple));
            //main menu options button
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle((10/3) * scale, (150/3) * scale, (100 / 3) * scale, (50 / 3) * scale),
                    "Options",
                    buttonFont,
                    Color.Purple));
            //options menu go to main button
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle((10 / 3) * scale, (40 / 3) * scale, (200 / 3) * scale, (50 / 3) * scale),
                    "Return to Main Menu",
                    buttonFont,
                    Color.Black));
            //options menu go to pause'
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle((10 / 3) * scale, (40 / 3) * scale, (200 / 3) * scale, (50 / 3) * scale),
                    "Return to Pause Menu",
                    buttonFont,
                    Color.Purple));
            //options menu change controls
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle((10 / 3) * scale, (150 / 3) * scale, (200 / 3) * scale, (50 / 3) * scale),
                    "Change WASD Control",
                    buttonFont,
                    Color.Black));
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle((10 / 3) * scale, (260 / 3) * scale, (200 / 3) * scale, (50 / 3) * scale),
                    "Change Arrow Keys Control",
                    buttonFont,
                    Color.Black));
            //returns to main menu from game over
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle((380 / 3) * scale, (450 / 3) * scale, (200 / 3) * scale, (50 / 3) * scale),
                    "Return to Main Menu",
                    buttonFont,
                    Color.Black));
            //options menu scale change controls - not implemented
            /*
            //scale equals 3
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle((700 / 3) * scale, (40 / 3) * scale, (200 / 3) * scale, (50 / 3) * scale),
                    "Set to 960 * 540",
                    buttonFont,
                    Color.Purple));
            //scale equals 4
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle((700 / 3) * scale, (150 / 3) * scale, (200 / 3) * scale, (50 / 3) * scale),
                    "Set to 1280 * 720",
                    buttonFont,
                    Color.Purple));
            //scale equals 6
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle((700 / 3) * scale, (260 / 3) * scale, (200 / 3) * scale, (50 / 3) * scale),
                    "Set to 1920 * 1080",
                    buttonFont,
                    Color.Purple)); */

            //main menu buttons
            buttons[0].OnLeftButtonClick += StartGame;
            buttons[1].OnLeftButtonClick += ExitGame;
            buttons[2].OnLeftButtonClick += MainOptions;
            //options menu buttons
            buttons[3].OnLeftButtonClick += GoToMainMenu;
            buttons[4].OnLeftButtonClick += GoToPauseMenu;
            //buttons that change controls
            buttons[5].OnLeftButtonClick += ChangeToWASD;
            buttons[6].OnLeftButtonClick += ChangeToArrows;
            buttons[7].OnLeftButtonClick += ReturnToMenu;
            //buttons that change scale - not implemented
            //buttons[7].OnLeftButtonClick += ScaleTo3;
            //buttons[8].OnLeftButtonClick += ScaleTo4;
            //buttons[9].OnLeftButtonClick += ScaleTo6;


            //Game State Loads

            // Sets up the player location      
            playerSpreadSheet = Content.Load<Texture2D>("pixel_charTestScale");
            Rectangle playerLoc = new Rectangle(GraphicsDevice.Viewport.Width / 4, GraphicsDevice.Viewport.Height - 47 * scale, 10*scale, 37 * scale);//3 is scale
            player = new Player(playerSpreadSheet, playerLoc, PlayerState.FaceRight,5);

            //Player Controls
            playerControls = new PlayerControls(player, playerVariableManager);

            //player HP
            healthSprite = Content.Load<Texture2D>("hpScale");
            health = new Health(healthSprite,new Rectangle(GraphicsDevice.Viewport.Width-healthSprite.Width -(5*scale), (5 * scale), healthSprite.Width,healthSprite.Height));

            //slime enemy
            slimeSpriteSheet = Content.Load<Texture2D>("slimeEnemyScale");
            //imp enemy
            impSpriteSheet = Content.Load<Texture2D>("ImpScale");

            //Loading to Environment Texture List
            envirImgs.Add(Content.Load<Texture2D>("SkyBackgroundScale"));
            envirImgs.Add(Content.Load<Texture2D>("Scroll background(update 2)"));
            envirImgs.Add(Content.Load<Texture2D>("DirtWithGrassScale"));
            envirImgs.Add(Content.Load<Texture2D>("treeScale"));
            envirImgs.Add(Content.Load<Texture2D>("ShrubsScale"));
            envirImgs.Add(Content.Load<Texture2D>("LogCabinScale"));
            envirImgs.Add(Content.Load<Texture2D>("Transparent10X10"));
            envirImgs.Add(Content.Load<Texture2D>("SunScale"));

            //Sky Background
            skyBackground = new SkyBackground(envirImgs[0], new Rectangle(0, 90*scale - 2012*scale, 320*scale, 2012*scale), currentState);//3 is scale
            envirConverter = (Environment)skyBackground;
            environments.Add(envirConverter);

            //Sun
            sun = new Sun(envirImgs[7], new Rectangle(90 * scale, -20 * scale, 50 * scale, 50 * scale), currentState);
            envirConverter = (Environment)sun;
            environments.Add(envirConverter);

            //Background
            gameBackground = new EnvironmentBackground(envirImgs[1], new Rectangle(0, 0, 437*scale, 180*scale), currentState, player.State, 1); //1 is speed
            envirConverter = (Environment)gameBackground;
            environments.Add(envirConverter);

            //Creates wall list
            startWall = new List<CollisionBlock>();

            LoadEnvironment(levelFile);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            kbState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            switch (currentState)
            {
                case GameState.MainMenu:
                    IsMouseVisible = true;

                    for (int i = 0; i < 3; i++)
                    {
                        buttons[i].Update();
                    }
                    break;

                case GameState.PauseMenu:
                    IsMouseVisible = true;
                    ProcessPauseMenu(kbState);
                    break;

                case GameState.OptionsMenu:
                    IsMouseVisible = true;

                    if (wasMainOrPause)
                    {
                        buttons[3].Update();
                        buttons[5].Update();
                        buttons[6].Update();
                        //buttons[7].Update(); - currently not implemented
                        //buttons[8].Update();
                        //buttons[9].Update();
                    }
                    else
                    {
                        buttons[4].Update();
                        buttons[5].Update();
                        buttons[6].Update();
                        //buttons[7].Update();
                        //buttons[8].Update();
                        //buttons[9].Update();
                    }

                    //add a way for the player to change the scale of the screen
                    break;

                case GameState.SaveFileMenu:
                    //ProcessSaveFileMenu(kbState, mouseState);
                    //IsMouseVisible = true;
                    break;

                case GameState.GamePlayState:
                    ProcessGamePlayState(kbState);
                    IsMouseVisible = false;
                    //code for timer update
                    
                    currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 

                    if (currentTime >= countDuration)
                    {
                        counter++;
                        currentTime -= countDuration; // "use up" the time
                                                      //any actions to perform
                    }
                    if (counter >= limit)
                    {
                        counter = 0;//Reset the counter;
                        //any actions to perform
                        currentState = GameState.GameOverMenu;
                    }

                    //Background
                    for (int i = 0; i < environments.Count; i++)
                    {
                        //Updates
                        environments[i].Update(gameTime);
                        //Updates the game state for environments
                        environments[i].State = currentState;
                        //Updates the player state for environments
                        environments[i].PlayerState = player.State;
                    }

                    topCollision = false;
                    leftCollision = false;
                    rightCollision = false;
                    bottomCollision = false;

                    playerVariableManager.LandingHeight = GraphicsDevice.Viewport.Height - (47 * scale);

                    //Test collision check
                    foreach (CollisionBlock c in startWall)
                    {
                        if (player.CheckCollision(c)[0])
                        {
                            playerVariableManager.LandingHeight = c.Position.Y - (37 * scale);
                            playerVariableManager.StartHeight = playerVariableManager.LandingHeight;
                            topCollision = true;
                        }
                        if (player.CheckCollision(c)[1])
                        {
                            leftCollision = true;
                        }
                        if (player.CheckCollision(c)[2])
                        {
                            rightCollision = true;
                        }
                        if (player.CheckCollision(c)[3])
                        {
                            bottomCollision = true;
                        }
                        //TODO: Try again once each type has its own loop
                        //If one or more is true break out of the loop
                        //if (topCollision || leftCollision || rightCollision || bottomCollision)
                        //{
                        //    break;
                        //}
                    }

                    blockCollisionArray[0] = topCollision;
                    blockCollisionArray[1] = leftCollision;
                    blockCollisionArray[2] = rightCollision;
                    blockCollisionArray[3] = bottomCollision;
                    playerVariableManager.BlockCollisionArray = blockCollisionArray;


                    for (int i = 0; i < enemies.Count; i++)
                    {
                        //Update
                        //TODO: Fix enemy speed this was a double update causing it to look right
                        enemies[i].Update(gameTime);
                        enemies[i].Update(gameTime);
                        player.CheckEnemyCollisions(enemies[i]);

                        //Updates PlayerState
                        enemies[i].State = currentState;
                        enemies[i].PlayerState = player.State;

                        if (enemies[i].CurrentState == EnemyState.Dying)
                        {
                            enemies.RemoveAt(i);
                        }
                    }


                    player.Update(gameTime);
                   
                    //Collision check
                    if (topCollision)
                    {
                        System.Diagnostics.Debug.WriteLine("Top: True");
                    }
                    if (leftCollision)
                    {
                        System.Diagnostics.Debug.WriteLine("Left: True");
                    }
                    if (rightCollision)
                    {
                        System.Diagnostics.Debug.WriteLine("Right: True");
                    }
                    if (bottomCollision)
                    {
                        System.Diagnostics.Debug.WriteLine("Bottom: True");
                    }
                    if (!(topCollision || leftCollision || rightCollision || bottomCollision))
                    {
                        System.Diagnostics.Debug.WriteLine("False");
                    }
                    playerControls.StateUpdate(gameTime);

                    health.Update(gameTime, player.Health);

                    //TODO: Move to appropiate section of code if there is a better place.
                    //Win condition
                    if(enemies.Count == 0)
                    {
                        currentState = GameState.Winner;
                    }
                    break;

                case GameState.GameOverMenu:
                    IsMouseVisible = true;
                    buttons[7].Update();
                    break;

                case GameState.Winner:
                    IsMouseVisible = true;
                    buttons[7].Update();
                    break;

                case GameState.ExitGame:
                    break;
                default:
                    break;
            }

            for (int i = 0; i < environments.Count; i++)
            {
                //Updates the game state for environments
                environments[i].State = currentState;
                //Updates the player state for environments
                environments[i].PlayerState = player.State;
            }
            
            foreach (Enemy enemy in enemies)
            {
                enemy.State = currentState;
                enemy.PlayerState = player.State;
            }
            


            prevKbState = kbState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Purple);

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
                    //Enemies
                    foreach(Enemy enemy in enemies)
                    {
                        enemy.Draw(_spriteBatch);
                    }


                    _spriteBatch.DrawString(labelFont, "" + counter, new Vector2(5, 5), Color.Black);
                    _spriteBatch.DrawString(labelFont, "Press P to Pause.", new Vector2(5, 25), Color.Black);
                    health.Draw(_spriteBatch);
                    break;
                case GameState.PauseMenu:
                    _spriteBatch.DrawString(labelFont, "This is the Pause Menu.", new Vector2(5, 5), Color.White);
                    _spriteBatch.DrawString(labelFont, "Press M for the Main Menu, O for Options, P to go back, or Escape to Exit.", new Vector2(5, 25), Color.White);
                    break;
                case GameState.OptionsMenu:
                    _spriteBatch.DrawString(labelFont, "This is the Options Menu.", new Vector2(5, 5), Color.White);
                    if (wasMainOrPause)
                    {
                        buttons[3].Draw(_spriteBatch);
                        buttons[5].Draw(_spriteBatch);
                        buttons[6].Draw(_spriteBatch);
                        //buttons[7].Draw(_spriteBatch); - not implemented
                        //buttons[8].Draw(_spriteBatch);
                        //buttons[9].Draw(_spriteBatch);
                    }
                    else
                    {
                        buttons[4].Draw(_spriteBatch);
                        buttons[5].Draw(_spriteBatch);
                        buttons[6].Draw(_spriteBatch);
                        //buttons[7].Draw(_spriteBatch);
                        //buttons[8].Draw(_spriteBatch);
                        //buttons[9].Draw(_spriteBatch);
                    }
                    break;
                case GameState.GameOverMenu:
                    backgrounds[1].Draw(_spriteBatch);
                    buttons[7].Draw(_spriteBatch);
                    break;
                case GameState.Winner:
                    buttons[7].Draw(_spriteBatch);
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

        //return to main menu for button
        private void ReturnToMenu()
        {
            enemies.Clear();
            environments.Clear();
            //environment
            //Adds objects back to environments
            //skybackground
            skyBackground.Reset();
            envirConverter = (Environment)skyBackground;
            environments.Add(envirConverter);

            //sun
            sun.LocationX = 40 * Scale;
            sun.LocationY = -20 * Scale;
            envirConverter = (Environment)sun;
            environments.Add(envirConverter);

            //normal background
            gameBackground.Reset();
            envirConverter = (Environment)gameBackground;
            environments.Add(envirConverter);
            LoadEnvironment(levelFile);

            //TODO: Make a player.reset method
            //Player values
            player.Health = 5;
            //Stops red blinking
            player.DamageState = DamageState.CanTakeDamage;
            //Faces right
            player.State = PlayerState.FaceRight;
            playerVariableManager.StartHeight = GraphicsDevice.Viewport.Height - (47 * scale);

            currentState = GameState.MainMenu;
        }
        //helper method for GamePlayState
        private void ProcessGamePlayState(KeyboardState kbState)
        {
            kbState = Keyboard.GetState();
            if (SingleKeyPress(Keys.P, kbState))
            {
                currentState = GameState.PauseMenu;
            }
            if(player.Health == 0)
            {
                currentState = GameState.GameOverMenu;
            }
        }

        //method to go to save menu with the button
        private void GoToSaveMenu()
        {
            currentState = GameState.SaveFileMenu;
        }
        //method to start the game
        private void StartGame()
        {
            currentState = GameState.GamePlayState;
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
        }

        private void ChangeToArrows()
        {
            rightMove = Keys.Right;
            leftMove = Keys.Left;
            crouchMove = Keys.Down;
            upMove = Keys.Up;
            usingWASD = false;
        }

        //methods for changing scale
        private void ScaleTo3()
        {
            scale = 3;
        }

        private void ScaleTo4()
        {
            scale = 4;
        }

        private void ScaleTo6()
        {
            scale = 6;
        }

        //Save the screen to be loaded up when restarted
        public void SaveEnvironment(string filename)
        {

        }


        //Loads the environment based off of the text file
        public void LoadEnvironment(string filename)
        {
            int xPlacement = 0;
            int yPlacement = -10 * scale;
            int lastX = xPlacement;

            StreamReader reader = new StreamReader(filename);

            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                yPlacement += 10 * scale;
                xPlacement = lastX;
                foreach (char ch in line)
{
                    switch (ch)
                    {
                       
                        case 'X':
                            xPlacement += 10 * scale;
                            break;
                        //Ground
                        case 'O':
                            ground = new CollisionBlock(envirImgs[2], new Rectangle(xPlacement, yPlacement, 10 * scale, 10 * scale), currentState, player.State, 2);
                            //TODO: Remove this is for temp testing only
                            startWall.Add(ground);
                            envirConverter = (Environment)ground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        case '|':
                            startWallBlock = new CollisionBlock(envirImgs[6], new Rectangle(xPlacement, yPlacement, 10 * scale, 10 * scale), currentState, player.State, 2);
                            startWall.Add(startWallBlock);
                            envirConverter = (Environment)startWallBlock;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        //Start Cabin
                        case 'L':
                            foreground = new Foreground(envirImgs[5], new Rectangle(xPlacement, yPlacement, 10 * scale, 10 * scale),
                                currentState, player.State, 2, 70, 80, 0);
                            foreground.Flipped = false;
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        //Bushes
                        case '0':
                            foreground = new Foreground(envirImgs[4], new Rectangle(xPlacement, yPlacement, 10 * scale, 10 * scale),
                                currentState, player.State, 2, 50, 30,0);
                            foreground.Flipped = false;
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        case '1':
                            foreground = new Foreground(envirImgs[4], new Rectangle(xPlacement, yPlacement, 10 * scale, 10 * scale),
                                currentState, player.State, 2, 50, 30, 1);
                            foreground.Flipped = false;
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        case '2':
                            foreground = new Foreground(envirImgs[4], new Rectangle(xPlacement, yPlacement, 10 * scale, 10 * scale),
                                currentState, player.State, 2, 50, 30, 2);
                            foreground.Flipped = false;
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        case '3':
                            foreground = new Foreground(envirImgs[4], new Rectangle(xPlacement, yPlacement, 10 * scale, 10 * scale),
                               currentState, player.State, 2, 50, 30, 0);
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        case '4':
                            foreground = new Foreground(envirImgs[4], new Rectangle(xPlacement, yPlacement, 10 * scale, 10 * scale),
                                currentState, player.State, 2, 50, 30, 1);
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        case '5':
                            foreground = new Foreground(envirImgs[4], new Rectangle(xPlacement, yPlacement, 10 * scale, 10 * scale),
                                currentState, player.State, 2, 50, 30, 2);
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        //Trees
                        case '6':
                            foreground = new Foreground(envirImgs[3], new Rectangle(xPlacement, yPlacement, 110 * scale, 120 * scale),
                                currentState, player.State, 2, 110, 120, 0);
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        case '7':
                            foreground = new Foreground(envirImgs[3], new Rectangle(xPlacement, yPlacement, 110 * scale, 120 * scale),
                                currentState, player.State, 2, 110, 120, 1);
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        case '8':
                            foreground = new Foreground(envirImgs[3], new Rectangle(xPlacement, yPlacement, 110 * scale, 120 * scale),
                                currentState, player.State, 2, 110, 120, 2);
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        case '9':
                            foreground = new Foreground(envirImgs[3], new Rectangle(xPlacement, yPlacement, 110*scale, 120*scale),
                                currentState, player.State, 2, 110, 120, 3);
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        //Enemies
                        //Slime
                        case '*':
                            //slime load at 150 + 3
                            slime1 = new Slime(slimeSpriteSheet, new Rectangle(xPlacement, yPlacement + (3 * scale), 23 * scale, 17 * scale), currentState, player.State, 1);
                            enemies.Add((Enemy)slime1);
                            xPlacement += 10 * scale;
                            break;
                        //Imp
                        case '$':
                            imp1 = new Imp(impSpriteSheet, new Rectangle(xPlacement, yPlacement, 20 * scale, 30 * scale), currentState, player.State, 1);
                            enemies.Add((Enemy)imp1);
                            xPlacement += 10 * scale;
                            break;
                        //Next screen load
                        case 'N':
                            yPlacement = -10 * scale;
                            lastX = xPlacement + (320 * scale);
                            break;
                    }
                }
            }
            reader.Close();
            /*
             * N - next
             * X - none
            O - ground
            | - wall (walls may be able to work as the same class as ground)
            ^ - spikes

            Platforms
            \ - left platform side
            / - right platform side
            = - platform top (block)
            _ - platform bottom (can be a thin platform)


            Foliage
            0-5 - bush 3 tall 5 wide
            6-9 - tree 12 tall 11 wide

            L - log cabin

            + - health
            $ - enemy
            */
        }
    }
}