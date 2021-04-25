using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Terminal_Dusk.Environments;
using System.IO;

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

        //for the player hp
        private Health health;
        private Texture2D healthSprite;

        //for the slime
        private Slime slime1;
        private Texture2D slimeSpriteSheet;
        private List<Slime> slimeEnemies = new List<Slime>();

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

        //field for jumping
        private int jumpingCounter = 0;
        private int jumpingLimit = 21;
        private float jumpingCountDuration = 0.03f; //every  .03s.
        private float jumpingCurrentTime = 0f;

        //fields for attacking
        private int attackingCounter = 0;
        private int attackingLimit = 2;
        private float attackingCountDuration = 1f; //every  1s.
        private float attackingCurrentTime = 0f;

        //Environment list
        private List<Environment> environments = new List<Environment>();
        private List<Texture2D> envirImgs = new List<Texture2D>();
        Environment envirConverter;

        //SkyBackground object
        private SkyBackground skyBackground;
        //GameBackground
        private EnvironmentBackground gameBackground;
        //Forground objects
        private Forground foreground;
        //Ground
        private CollisionBlock ground;

        //A scale for changing the size of the screen
        private int scale = 3;

        //options for keyboard controls
        private Keys rightMove = Keys.D;
        private Keys leftMove = Keys.A;
        private Keys crouchMove = Keys.S;
        private Keys upMove = Keys.W;
        private Keys attack = Keys.Space;
        private bool usingWASD = true;
        private bool usingArrow = false;

        //FileIO
        private string levelFile = "..\\..\\levelFile.txt";


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
            backImgs.Add(Content.Load<Texture2D>("newTitleScale"));
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
                    new Rectangle((10/3) * scale, (40/3) * scale, (100/3) * scale, (50/3) * scale),    // where to put the button
                    "Start",                        // button label
                    buttonFont,                               // label font
                    Color.Purple));
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
                    Color.Purple));
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
                    Color.Purple));
            buttons.Add(new Button(
                    _graphics.GraphicsDevice,
                    new Rectangle((10 / 3) * scale, (260 / 3) * scale, (200 / 3) * scale, (50 / 3) * scale),
                    "Change Arrow Keys Control",
                    buttonFont,
                    Color.Purple));
            //options menu scale change controls
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
            //buttons that change scale
            buttons[7].OnLeftButtonClick += ScaleTo3;
            buttons[8].OnLeftButtonClick += ScaleTo4;
            buttons[9].OnLeftButtonClick += ScaleTo6;


            //Game State Loads

            // Sets up the player location      
            playerSpreadSheet = Content.Load<Texture2D>("pixel_charTestScale");
            Rectangle playerLoc = new Rectangle(GraphicsDevice.Viewport.Width / 4, GraphicsDevice.Viewport.Height - 47 * scale, 10*scale, 37 * scale);//3 is scale
            player = new Player(playerSpreadSheet, playerLoc, PlayerState.FaceRight,5);

            //player HP
            healthSprite = Content.Load<Texture2D>("hpScale");
            health = new Health(healthSprite,new Rectangle(GraphicsDevice.Viewport.Width-healthSprite.Width -(5*scale), (5 * scale), healthSprite.Width,healthSprite.Height));

            //slime enemy
            slimeSpriteSheet = Content.Load<Texture2D>("slimeEnemyScale");
            
            //Loading to Environment Texture List
            envirImgs.Add(Content.Load<Texture2D>("SkyBackgroundScale"));
            envirImgs.Add(Content.Load<Texture2D>("Scroll background(update 2)"));
            envirImgs.Add(Content.Load<Texture2D>("DirtWithGrassScale"));
            envirImgs.Add(Content.Load<Texture2D>("treeScale"));
            envirImgs.Add(Content.Load<Texture2D>("ShrubsScale"));

            //Sky Background
            skyBackground = new SkyBackground(envirImgs[0], new Rectangle(0, 90*scale - 2012*scale, 320*scale, 2012*scale), currentState);//3 is scale
            envirConverter = (Environment)skyBackground;
            environments.Add(envirConverter);

            //Background
            gameBackground = new EnvironmentBackground(envirImgs[1], new Rectangle(0, 0, 437*scale, 180*scale), currentState, player.State, 1); //1 is speed
            envirConverter = (Environment)gameBackground;
            environments.Add(envirConverter);

            
            //TempGround
            /*for (int i = 0; i < 1000; i++)
            {
                ground = new CollisionBlock(envirImgs[2], new Rectangle(i*scale, GraphicsDevice.Viewport.Height - 10*scale, 10 * scale, 10 * scale), currentState, player.State, 2);
                envirConverter = (Environment)ground;
                environments.Add(envirConverter);
                i += 9;
            }*/
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
                        buttons[7].Update();
                        buttons[8].Update();
                        buttons[9].Update();
                    }
                    else
                    {
                        buttons[4].Update();
                        buttons[5].Update();
                        buttons[6].Update();
                        buttons[7].Update();
                        buttons[8].Update();
                        buttons[9].Update();
                    }

                    //add a way for the player to change the scale of the screen
                    break;

                case GameState.SaveFileMenu:
                    ProcessSaveFileMenu(kbState, mouseState);
                    break;

                case GameState.GamePlayState:
                    ProcessGamePlayState(kbState);
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
                        currentState = GameState.MainMenu;
                    }

                    //Background
                    for (int i = 0; i < environments.Count; i++)
                    {
                        environments[i].Update(gameTime);
                    }
                    // TODO: Should upcast slime to Enemy so that Imps can be included in this update
                    foreach (Slime slime in slimeEnemies)
                    {
                        slime.Update(gameTime);
                        player.CheckEnemyCollisions(slime);
                    }
                    /*
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
                    // TODO: Should upcast slime to Enemy so that Imps can be included in this update
                    /*foreach (Slime slime in slimeEnemies)
                    {
                        //Update
                        slime.Update(gameTime);
                        player.CheckEnemyCollisions(slime);
                        if(slime.CurrentState == EnemyState.Dying)
                        {
                            slimeEnemies.RemoveAt(slime);
                        }
                        //Updates PlayerState
                        slime.State = currentState;
                        slime.PlayerState = player.State;
                      }*/


            for (int i = 0; i < slimeEnemies.Count; i++)
                    {
                        //Update
                        slimeEnemies[i].Update(gameTime);
                        player.CheckEnemyCollisions(slimeEnemies[i]);
                        //Updates PlayerState
                        slimeEnemies[i].State = currentState;
                        slimeEnemies[i].PlayerState = player.State;
                        if (slimeEnemies[i].CurrentState == EnemyState.Dying)
                        {
                            slimeEnemies.RemoveAt(i);
                        }
                    }
                    player.Update(gameTime);
                    // TODO: Logic should be moved and handled in Player class, just copy/pasted for ease
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
                    //jumping switch statement
                    // TODO: Needs to be completely rewritten. No physics, no animation, improper user input.
                    switch (player.JumpingState)
                    {
                        case PlayerJumpingState.Standing:
                            if (SingleKeyPress(upMove, kbState))
                            {
                                player.JumpingState = PlayerJumpingState.Jumping;
                            }
                            break;
                        case PlayerJumpingState.Jumping:
                            jumpingCurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 
                            //jumping timer to create a jumping animation
                            //whenever the elapsed time gets lerger then jumpingCountDuration it runs through the loop
                            if (jumpingCurrentTime >= jumpingCountDuration)
                            {
                                //if the counter is less then or equal to ten, the player goes up by 10 pixels
                                if (jumpingCounter <= 10)
                                {
                                    player.Y -= 15;
                                }
                                jumpingCounter++;
                                //if the counter is greater then 10, the player goes down by 10 pixels
                                if (jumpingCounter > 10)
                                {
                                    player.Y += 15;
                                }
                                //reset the timer to loop again
                                jumpingCurrentTime -= jumpingCountDuration; // "use up" the time
                                                                            //any actions to perform
                            }
                            //if the counter is greater then our limit
                            //the jump has completed
                            if (jumpingCounter >= jumpingLimit)
                            {
                                //reset the counter
                                jumpingCounter = 0;
                                //set the player to the standing state as they are no longer jumping
                                player.JumpingState = PlayerJumpingState.Standing;
                            }
                            break;
                    }
                    switch (player.AttackingState)
                    {
                        case PlayerAttackingState.IsNotAttacking:
                            if (SingleKeyPress(attack, kbState))
                            {
                                player.AttackingState = PlayerAttackingState.IsAttacking;
                            }
                            break;
                        case PlayerAttackingState.IsAttacking:
                            if(prevKbState.IsKeyUp(attack))
                            {
                                player.AttackingState = PlayerAttackingState.IsNotAttacking;
                            }
                            break;
                    }

                    health.Update(gameTime, player.Health);
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
            
            foreach (Slime slime in slimeEnemies)
            {
                slime.State = currentState;
                slime.PlayerState = player.State;
            }
            


            prevKbState = kbState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Purple);

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
                    foreach(Slime slime in slimeEnemies)
                    {
                        slime.Draw(_spriteBatch);
                    }
                    

                    _spriteBatch.DrawString(labelFont, "" + counter, new Vector2(5, 5), Color.Black);
                    _spriteBatch.DrawString(labelFont, "Press P to pause", new Vector2(5, 25), Color.Black);
                    health.Draw(_spriteBatch);
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
                        buttons[7].Draw(_spriteBatch);
                        buttons[8].Draw(_spriteBatch);
                        buttons[9].Draw(_spriteBatch);
                    }
                    else
                    {
                        buttons[4].Draw(_spriteBatch);
                        buttons[5].Draw(_spriteBatch);
                        buttons[6].Draw(_spriteBatch);
                        buttons[7].Draw(_spriteBatch);
                        buttons[8].Draw(_spriteBatch);
                        buttons[9].Draw(_spriteBatch);
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
            if(player.Health == 0)
            {
                currentState = GameState.MainMenu;
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
                        // TODO: slime load at 150 - 2
                        case 'X':
                            xPlacement += 10 * scale;
                            break;
                        //Ground
                        case 'O':
                            ground = new CollisionBlock(envirImgs[2], new Rectangle(xPlacement, yPlacement, 10 * scale, 10 * scale), currentState, player.State, 2);
                            envirConverter = (Environment)ground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        //Bushes
                        case '0':
                            foreground = new Forground(envirImgs[4], new Rectangle(xPlacement, yPlacement, 10 * scale, 10 * scale),
                                currentState, player.State, 2, 50, 30,0);
                            foreground.Flipped = false;
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        case '1':
                            foreground = new Forground(envirImgs[4], new Rectangle(xPlacement, yPlacement, 10 * scale, 10 * scale),
                                currentState, player.State, 2, 50, 30, 1);
                            foreground.Flipped = false;
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        case '2':
                            foreground = new Forground(envirImgs[4], new Rectangle(xPlacement, yPlacement, 10 * scale, 10 * scale),
                                currentState, player.State, 2, 50, 30, 2);
                            foreground.Flipped = false;
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        case '3':
                            foreground = new Forground(envirImgs[4], new Rectangle(xPlacement, yPlacement, 10 * scale, 10 * scale),
                               currentState, player.State, 2, 50, 30, 0);
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        case '4':
                            foreground = new Forground(envirImgs[4], new Rectangle(xPlacement, yPlacement, 10 * scale, 10 * scale),
                                currentState, player.State, 2, 50, 30, 1);
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        case '5':
                            foreground = new Forground(envirImgs[4], new Rectangle(xPlacement, yPlacement, 10 * scale, 10 * scale),
                                currentState, player.State, 2, 50, 30, 2);
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        //Trees
                        case '6':
                            foreground = new Forground(envirImgs[3], new Rectangle(xPlacement, yPlacement, 110 * scale, 120 * scale),
                                currentState, player.State, 2, 110, 120, 0);
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        case '7':
                            foreground = new Forground(envirImgs[3], new Rectangle(xPlacement, yPlacement, 110 * scale, 120 * scale),
                                currentState, player.State, 2, 110, 120, 1);
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        case '8':
                            foreground = new Forground(envirImgs[3], new Rectangle(xPlacement, yPlacement, 110 * scale, 120 * scale),
                                currentState, player.State, 2, 110, 120, 2);
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        case '9':
                            foreground = new Forground(envirImgs[3], new Rectangle(xPlacement, yPlacement, 110*scale, 120*scale),
                                currentState, player.State, 2, 110, 120, 3);
                            envirConverter = (Environment)foreground;
                            environments.Add(envirConverter);
                            xPlacement += 10 * scale;
                            break;
                        //Enemies
                        //Slime
                        case '*':
                            slimeEnemies.Add(new Slime(slimeSpriteSheet, new Rectangle(xPlacement, yPlacement + (3 * scale), 23 * scale, 17 * scale), currentState, player.State, 1));
                            xPlacement += 10 * scale;
                            break;
                        //Imp
                        case '$':
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