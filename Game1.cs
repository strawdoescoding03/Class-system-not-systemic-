using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Class_system__not_systemic_
{

    enum Screen
    {
        StartMenu,
        Instructions,
        LevelSelector,
        Level1,
        Level2,
        Level3,
        DeathScreen,
        EndScreen
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Screen screen;
        Texture2D characterSpriteSheet, rectangleTexture, backgroundSpriteSheet, currentBackgroundTexture, background1, 
            background2, background3, background4, background5, background6, background7, background8, trainTexture, startmenuTexture, 
            instructionsMenuTexture, endGameTexture, levelSelectionMenuTexture, enemyBall, yellowKeyCardTexture,
            blueKeyCardTexture, redKeyCardTexture, yellowLockDoor, redLockDoor, blueLockDoor, exitDoorTexture, zeroHeartsTexture, oneHeartsTexture, ladderTexture,
            twoHeartsTexture, fullHeartsTexture, escapeTexture, deathScreenTexture;

        List<Texture2D> cyborgSprites;
        List<Rectangle> barriers;
        List<Rectangle> verticalBarriers;
        List<Rectangle> platforms;
        List<Rectangle> ladders;
        KeyboardState keyboardState;
        MouseState mouseState, prevMouseState;

        SpriteEffects daveFlipHorizontally;
        int[] daveFrames;
        int[] cyborgFrames;
        int rows, columns, bgRows, bgColumns;
        int cyborgColumns;
        int frame;
        int frames;
        int backgroundFrame;
        int backgroundFrames;
        int enemyBallSpeedX = 2;

        int directionRow, bgDraw;
        int mouseX, mouseY;
        int leftRow, rightRow, climbUp, idle, playerjump;
        int width, backgroundWidth;
        int height, backgroundHeight;
        int hitCount = 0;


        float speed;
        float time, bgtime;
        float frameSpeed, backgroundFrameSpeed;
        float gravity = 0.3f; // This is how fast player accelerated downwards
        float gravitySpeed = 0f;
        float ballGravitySpeed = 2f;
        float jumpSpeed = 7f; // This will determine the strength of the jump
        bool onGround = false, ballOnGround = false, yellowCardCaptured = false, redCardCaptured = false, blueCardCaptured = false, gameEnd= false,
            canExitYellow = false, canExitRed = false, canExitBlue = false, canEscape = false, playerDeathCheck = false, 
            oneHeartLost = false, twoHeartLost = false, noHeart= false, fullHeart = true;

        float cyborgSpeed;
        float cyborgFrameSpeed;
        float cyborgTime;
        float enemyBallRotation;

        SoundEffect backgroundAudio, damageSound, doorUnlockSound, keycardPickUpSound;
        SoundEffectInstance backgroundAudioInstance, damageSoundInstance, doorUnlockSoundInstance, keycardPickUpSoundInstance;


        string debug = "";

        Vector2 playerLocation = new Vector2(10, 10);
        Vector2 enemyBallLocation = new Vector2(700, 150);
        Vector2 enemyBallDirection = Vector2.Zero;
        Vector2 playerDirection;
        Vector2 playerSpeed = Vector2.Zero;
        Rectangle playerCollisionRect, enemyBallCollisionRect, playerDrawRect, window, gameWindow, movingWindow,
            playGameButton, instructionsButton, instructionsMenuPlayBtn, returnToMenuBtn, enemyBallDrawRect, 
            yellowCardBtn, redCardBtn, blueCardBtn, yellowKeyCardCollectable, redKeyCardCollectable, blueKeyCardCollectable, enemyBallRect, 
            exitDoorRect, heartDisplayRect, escapeRect;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


            screen = Screen.StartMenu;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 500;
            _graphics.ApplyChanges();
            window = new Rectangle(0, 0, 800, 500);
            movingWindow = new Rectangle(-800, 0, 800, 500);
            gameWindow = new Rectangle(0, 0, 800, 500);
            instructionsMenuPlayBtn = new Rectangle(566, 60, 110, 50);
            returnToMenuBtn = new Rectangle(680, 60, 110, 50);
            enemyBallCollisionRect = new Rectangle(700, 100, 50, 50);
            enemyBallDrawRect = new Rectangle(700, 100, 50, 50);

            escapeRect = new Rectangle(310, 425, 180, 50);
            barriers = new List<Rectangle>();

            
            barriers.Add(new Rectangle(0, -10, 800, 10));
            barriers.Add(new Rectangle(-10, 0, 10, 500));
            barriers.Add(new Rectangle(800, 0, 10, 500));
            barriers.Add(new Rectangle(0, 500, 800, 10));

            barriers.Add(new Rectangle(0, 400, 800, 20)); // Ground

            barriers.Add(new Rectangle(600, 225, 150, 10));
            barriers.Add(new Rectangle(0, 65, 150, 10));

            barriers.Add(new Rectangle(200, 165, 175, 10));


            verticalBarriers = new List<Rectangle>();
            verticalBarriers.Add(new Rectangle(530, 300, 10, 170));

            ladders = new List<Rectangle>();
            ladders.Add(new Rectangle(754, 180, 35, 235)); 
            ladders.Add(new Rectangle(155, 60, 35, 215));


            daveFrames = new int[]{
            6,
            8,
            8,
            6,
            6,
            6,
            2,
            4,
            4,
            6,
            6,
            6
            };

            cyborgFrames = new int[]{
                4,
                6,
                6,
                8,
                8,
                6,
                6 };

           
            speed = 1.5f;

            columns = 8;
            rows = 12;
            leftRow = 10;
            rightRow = 10;
            climbUp = 3;
            idle = 7;
            playerjump = 5;
            time = 0.0f;
            bgtime = 0.0f;
            frameSpeed = 0.08f;
            backgroundFrameSpeed = 0.18f;
            frames = 12;
            frame = 0;

            bgColumns = 5;
            bgRows = 4;
            bgDraw = 3;
            backgroundFrames = 5;
            backgroundFrame = 0;

            enemyBallRotation = 0f;

            directionRow = leftRow;

            playerLocation = new Vector2(10, 30);
            playerCollisionRect = new Rectangle(10, 30, 35, 50);
            playerDrawRect = new Rectangle(10, 20, 55, 65);


            playGameButton = new Rectangle(215, 290, 380, 35);
            instructionsButton = new Rectangle(215, 330, 380, 35);
            exitDoorRect = new Rectangle (500, 225, 400, 176);


            yellowKeyCardCollectable = new Rectangle(215, 350, 50, 50);
            redKeyCardCollectable = new Rectangle(215, 350, 50, 50);
            blueKeyCardCollectable = new Rectangle(215, 350, 50, 50);



            UpdateRects();

            base.Initialize();
            width = characterSpriteSheet.Width / columns;
            height = characterSpriteSheet.Height / rows;
            //backgroundWidth = backgroundSpriteSheet.Width / bgColumns;
            //backgroundHeight = backgroundSpriteSheet.Height / bgRows;

            yellowCardBtn = new Rectangle(165, 232, 125, 110);
            redCardBtn = new Rectangle(339, 232, 125, 110);
            blueCardBtn = new Rectangle(512, 232, 125, 110);


        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            rectangleTexture = Content.Load<Texture2D>("rectangle");
            characterSpriteSheet = Content.Load<Texture2D>("daveSpriteSheet");
            exitDoorTexture = Content.Load<Texture2D>("ExitDoorUnlocked");
            yellowLockDoor = Content.Load<Texture2D>("yellowLevelExitDoorLocked");
            redLockDoor = Content.Load<Texture2D>("redLevelExitDoorLocked");
            blueLockDoor = Content.Load<Texture2D>("blueLevelExitDoorLocked");
            //Hearts
            zeroHeartsTexture = Content.Load<Texture2D>("no_health"); //goes w/no hearts
            oneHeartsTexture = Content.Load<Texture2D>("1_heart"); //goes w/2 hearts lost
            twoHeartsTexture = Content.Load<Texture2D>("2_hearts"); // goes w/1 heart lost
            fullHeartsTexture = Content.Load<Texture2D>("full_health"); //goes w/ full hearts

            trainTexture = Content.Load<Texture2D>("trainTexture");
            enemyBall = Content.Load<Texture2D>("enemyBall");

            background1 = Content.Load<Texture2D>("background1override");
            background2 = Content.Load<Texture2D>("background2");
            background3 = Content.Load<Texture2D>("background3");
            background4 = Content.Load<Texture2D>("background4");
            background5 = Content.Load<Texture2D>("background5");
            background6 = Content.Load<Texture2D>("background6");
            background7 = Content.Load<Texture2D>("background7");
            background8 = Content.Load<Texture2D>("background8");
            startmenuTexture = Content.Load<Texture2D>("smokebox");
            instructionsMenuTexture = Content.Load<Texture2D>("instructionsMenuUpdated");
            levelSelectionMenuTexture = Content.Load<Texture2D>("noLevelComplete");
            escapeTexture = Content.Load<Texture2D>("escapeMenu");
            endGameTexture = Content.Load<Texture2D>("endMenu");
            deathScreenTexture = Content.Load<Texture2D>("deathScreen");

            //ladder
            ladderTexture = Content.Load<Texture2D>("ladderSmall");
            

            yellowKeyCardTexture = Content.Load<Texture2D>("yellowCard");
            redKeyCardTexture = Content.Load<Texture2D>("redCard");
            blueKeyCardTexture = Content.Load<Texture2D>("blueCard");

            backgroundAudio = Content.Load<SoundEffect>("LindseyStirlingCrystallize");
            backgroundAudioInstance = backgroundAudio.CreateInstance();

            damageSound = Content.Load<SoundEffect>("damageSound");
            damageSoundInstance = damageSound.CreateInstance();

            keycardPickUpSound = Content.Load<SoundEffect>("keyCardPickUp");
            keycardPickUpSoundInstance = keycardPickUpSound.CreateInstance();

            doorUnlockSound = Content.Load<SoundEffect>("doorUnlock");
            doorUnlockSoundInstance = doorUnlockSound.CreateInstance();



            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            keyboardState = Keyboard.GetState();
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
            backgroundAudioInstance.Play();

            mouseX = mouseState.X;
            mouseY = mouseState.Y;
            this.Window.Title = mouseX + " ," + mouseY + " ";


            if (screen == Screen.StartMenu)
            {

                if (mouseState.LeftButton == ButtonState.Pressed && playGameButton.Contains(mouseState.Position) && prevMouseState.LeftButton == ButtonState.Released)
                
                {
                    screen = Screen.LevelSelector;
                }


                else if (mouseState.LeftButton == ButtonState.Pressed && instructionsButton.Contains(mouseState.Position) && prevMouseState.LeftButton == ButtonState.Released)

                {
                    screen = Screen.Instructions;
                }

                window.X += 2;

                if (window.X > window.Width * 7 - 1)
                {
                    window.X = -800;
                }

                movingWindow.X += 2;

                if (movingWindow.X > movingWindow.Width * 7 - 1) 
                {
                    movingWindow.X = -800;
                }

            }


            else if (screen == Screen.Instructions)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && instructionsMenuPlayBtn.Contains(mouseState.Position) && prevMouseState.LeftButton == ButtonState.Released)
                {
                    screen = Screen.LevelSelector;
                }

                if (mouseState.LeftButton == ButtonState.Pressed && returnToMenuBtn.Contains(mouseState.Position) && prevMouseState.LeftButton == ButtonState.Released)
                {
                    screen = Screen.StartMenu;
                }

                
            }

            else if (screen == Screen.LevelSelector)
            {

                if(mouseState.LeftButton == ButtonState.Pressed && yellowCardBtn.Contains(mouseState.Position) 
                    && prevMouseState.LeftButton == ButtonState.Released && !yellowCardCaptured)
                {

                    playerLocation = new Vector2(10, 30);
                    enemyBallLocation = new Vector2(700, 150);
                    enemyBallSpeedX = 2;
                    hitCount = 0;
                    fullHeart = true;
                    oneHeartLost = false;
                    twoHeartLost = false;
                    noHeart = false;
                    UpdateRects();
                    screen = Screen.Level1;
                }

                if (mouseState.LeftButton == ButtonState.Pressed && redCardBtn.Contains(mouseState.Position) 
                    && prevMouseState.LeftButton == ButtonState.Released && !redCardCaptured)
                {
                    playerLocation = new Vector2(10, 30);
                    enemyBallLocation = new Vector2(700, 150);
                    enemyBallSpeedX = 2;
                    hitCount = 0;
                    fullHeart = true;
                    oneHeartLost = false;
                    twoHeartLost = false;
                    noHeart = false;
                    UpdateRects();
                    screen = Screen.Level2;


                }

                if (mouseState.LeftButton == ButtonState.Pressed && blueCardBtn.Contains(mouseState.Position)
                    && prevMouseState.LeftButton == ButtonState.Released && !blueCardCaptured)
                {
                    playerLocation = new Vector2(10, 30);
                    enemyBallLocation = new Vector2(700, 150);
                    enemyBallSpeedX = 2;
                    hitCount = 0;
                    fullHeart = true;
                    oneHeartLost = false;
                    twoHeartLost = false;
                    noHeart = false;
                    screen = Screen.Level3;
                }


                if (yellowCardCaptured == true && redCardCaptured == true && blueCardCaptured == true)
                {
                    canEscape = true;
                    if (escapeRect.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        gameEnd = true;
                        screen = Screen.EndScreen;
                    }

                }
                      
            }


            else if (screen == Screen.Level1)
            {

                mouseState = Mouse.GetState();
                keyboardState = Keyboard.GetState();

                SetPlayerDirection();
                playerLocation += playerDirection * speed;
                UpdateRects();
                playerSpeed.X = 0f;
                enemyBallDirection.X -= 0.01f;
                enemyBallRotation += 0.15f;
                enemyBallLocation.X -= enemyBallSpeedX;

              

                time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (time > frameSpeed)
                {
                    time = 0f;
                    frame = (frame + 1) % daveFrames[directionRow];
                }
                //debug += frame.ToString();
               
                bgtime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (bgtime > backgroundFrameSpeed)
                {
                    backgroundFrame = (1 + backgroundFrame) % backgroundFrames;
                    bgtime = 0f;
                }


                //backgroundCODE!!!

                currentBackgroundTexture = background1;

                window.X += 2;

                if (window.X > window.Width * 7 - 1)
                {
                    window.X = -800;
                }

                movingWindow.X += 2;

                if (movingWindow.X > movingWindow.Width * 7 - 1)
                {
                    movingWindow.X = -800;
                }

                if (!onGround)
                {
                    gravitySpeed += gravity;
                }
                else if (keyboardState.IsKeyDown(Keys.Space) && onGround)
                {
                    gravitySpeed = -jumpSpeed;
                    onGround = false;
                }
                else if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.W))
                {
                    onGround = true;
                    gravitySpeed = 0f;
                }

                else gravitySpeed += gravity;

                foreach (Rectangle ladder in ladders)
                {
                    if (playerCollisionRect.Intersects(ladder))
                    {
                        gravitySpeed = 0;
                    }

                }

                playerLocation.Y += gravitySpeed;
                enemyBallLocation.Y += ballGravitySpeed;

                UpdateRects();


                foreach (Rectangle barrier in barriers)
                    if (playerCollisionRect.Intersects(barrier))
                    {
                        playerLocation -= playerDirection * speed;
                        UpdateRects();
                    }

                foreach (Rectangle verticalBarrier in verticalBarriers)
                {
                    if (playerCollisionRect.Intersects(verticalBarrier))
                    {
                         if (gravitySpeed > 0)
                        {
                            playerLocation.Y = verticalBarrier.Top - playerCollisionRect.Height;
                            onGround = true;
                            gravitySpeed = 0;
                        }
                        UpdateRects();

                        playerLocation -= playerDirection * speed;
                        UpdateRects();
                    }
                }

                //Ball hitting barriers
                foreach (Rectangle barrier in barriers)
                {
                    if (enemyBallCollisionRect.Intersects(barrier))
                    {
                        enemyBallLocation.Y = barrier.Top - enemyBallCollisionRect.Height;
                        ballGravitySpeed = 0;
                    }
                    else
                    {
                        ballGravitySpeed = 2;
                    }
                }

                foreach (Rectangle verticalBarrier in verticalBarriers)
                {
                    if (enemyBallCollisionRect.Intersects(verticalBarrier))
                    {
                        enemyBallSpeedX *= -1;
                        UpdateRects();
                    }
                }

                //Hitting a platform while moving vertically
                foreach (Rectangle barrier in barriers)
                    if (playerCollisionRect.Intersects(barrier))
                    {
                        //Moving Up
                        if (gravitySpeed < 0)
                        {
                            playerLocation.Y = barrier.Bottom;
                            gravitySpeed = 0;
                        }
                        //Moving Down
                        else if (gravitySpeed > 0)
                        {
                            playerLocation.Y = barrier.Top - playerCollisionRect.Height;
                            onGround = true;
                            gravitySpeed = 0;
                        }
                        UpdateRects();
                    }


                if (enemyBallCollisionRect.X <= 0 || enemyBallCollisionRect.X >= 800 - enemyBallCollisionRect.Width)
                {
                    enemyBallSpeedX *= -1;
                    UpdateRects();
                }



                //Ball harm player yay!

                if (enemyBallCollisionRect.Intersects(playerCollisionRect))
                {
                    damageSoundInstance.Play();
                    hitCount++;
                    enemyBallLocation.X = 400;
                    enemyBallLocation.Y = 100;
                    UpdateRects();

                    if (hitCount == 1)
                    {
                        oneHeartLost = true;
                        fullHeart = false;
                    }

                    if (hitCount == 2)
                    {
                        oneHeartLost = false;
                        twoHeartLost = true;
                    }

                    if (hitCount == 3)
                    {
                        noHeart = true;
                        twoHeartLost = false;
                        playerDeathCheck = true;
                        yellowCardCaptured = false;
                        screen = Screen.DeathScreen;
                        

                    }

                    
                }



                //Keycard Grabbers: 
                if (playerCollisionRect.Intersects(yellowKeyCardCollectable) && keyboardState.IsKeyDown(Keys.E))
                {
                    keycardPickUpSoundInstance.Play();
                    yellowCardCaptured = true;    
                    
                    
                }

                //Exit Door

                if (playerCollisionRect.Intersects(exitDoorRect) && yellowCardCaptured && keyboardState.IsKeyDown(Keys.E))
                {
                    doorUnlockSoundInstance.Play();
                    canExitYellow = true;
                }

                else if (canExitYellow)
                {
                    screen = Screen.LevelSelector;
                }

            }

            else if (screen == Screen.Level2)
            {

                mouseState = Mouse.GetState();
                keyboardState = Keyboard.GetState();

                SetPlayerDirection();
                playerLocation += playerDirection * speed;
                UpdateRects();
                playerSpeed.X = 0f;
                enemyBallDirection.X -= 0.01f;
                enemyBallRotation += 0.15f;
                enemyBallLocation.X -= enemyBallSpeedX;



                time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (time > frameSpeed)
                {
                    time = 0f;
                    frame = (frame + 1) % daveFrames[directionRow];
                }
                //debug += frame.ToString();

                bgtime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (bgtime > backgroundFrameSpeed)
                {
                    backgroundFrame = (1 + backgroundFrame) % backgroundFrames;
                    bgtime = 0f;
                }


                //backgroundCODE!!!

                currentBackgroundTexture = background1;

                window.X += 2;

                if (window.X > window.Width * 7 - 1)
                {
                    window.X = -800;
                }

                movingWindow.X += 2;

                if (movingWindow.X > movingWindow.Width * 7 - 1)
                {
                    movingWindow.X = -800;
                }

                if (!onGround)
                {
                    gravitySpeed += gravity;
                }
                else if (keyboardState.IsKeyDown(Keys.Space) && onGround)
                {
                    gravitySpeed = -jumpSpeed;
                    onGround = false;
                }
                else if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.W))
                {
                    onGround = true;
                    gravitySpeed = 0f;
                }

                else gravitySpeed += gravity;

                foreach (Rectangle ladder in ladders)
                {
                    if (playerCollisionRect.Intersects(ladder))
                    {
                        gravitySpeed = 0;
                    }

                }

                playerLocation.Y += gravitySpeed;
                enemyBallLocation.Y += ballGravitySpeed;

                UpdateRects();


                foreach (Rectangle barrier in barriers)
                    if (playerCollisionRect.Intersects(barrier))
                    {
                        playerLocation -= playerDirection * speed;
                        UpdateRects();
                    }

                foreach (Rectangle verticalBarrier in verticalBarriers)
                {
                    if (playerCollisionRect.Intersects(verticalBarrier))
                    {
                        if (gravitySpeed > 0)
                        {
                            playerLocation.Y = verticalBarrier.Top - playerCollisionRect.Height;
                            onGround = true;
                            gravitySpeed = 0;
                        }
                        UpdateRects();

                        playerLocation -= playerDirection * speed;
                        UpdateRects();
                    }
                }

                //Ball hitting barriers
                foreach (Rectangle barrier in barriers)
                {
                    if (enemyBallCollisionRect.Intersects(barrier))
                    {
                        enemyBallLocation.Y = barrier.Top - enemyBallCollisionRect.Height;
                        ballGravitySpeed = 0;
                    }
                    else
                    {
                        ballGravitySpeed = 2;
                    }
                }

                foreach (Rectangle verticalBarrier in verticalBarriers)
                {
                    if (enemyBallCollisionRect.Intersects(verticalBarrier))
                    {
                        enemyBallSpeedX *= -1;
                        UpdateRects();
                    }
                }

                //Hitting a platform while moving vertically
                foreach (Rectangle barrier in barriers)
                    if (playerCollisionRect.Intersects(barrier))
                    {
                        //Moving Up
                        if (gravitySpeed < 0)
                        {
                            playerLocation.Y = barrier.Bottom;
                            gravitySpeed = 0;
                        }
                        //Moving Down
                        else if (gravitySpeed > 0)
                        {
                            playerLocation.Y = barrier.Top - playerCollisionRect.Height;
                            onGround = true;
                            gravitySpeed = 0;
                        }
                        UpdateRects();
                    }


                if (enemyBallCollisionRect.X <= 0 || enemyBallCollisionRect.X >= 800 - enemyBallCollisionRect.Width)
                {
                    enemyBallSpeedX *= -1;
                    UpdateRects();
                }



                //Ball harm player yay!

                if (enemyBallCollisionRect.Intersects(playerCollisionRect))
                {
                    damageSoundInstance.Play();
                    hitCount++;
                    enemyBallLocation.X = 400;
                    enemyBallLocation.Y = 100;
                    UpdateRects();

                    if (hitCount == 1)
                    {
                        oneHeartLost = true;
                        fullHeart = false;
                    }

                    if (hitCount == 2)
                    {
                        oneHeartLost = false;
                        twoHeartLost = true;
                    }

                    if (hitCount == 3)
                    {
                        noHeart = true;
                        twoHeartLost = false;
                        playerDeathCheck = true;

                        screen = Screen.DeathScreen;
                        redCardCaptured = false;
                    }


                }


                //Keycard Grabbers: 
                if (playerCollisionRect.Intersects(redKeyCardCollectable) && keyboardState.IsKeyDown(Keys.E))
                {
                    keycardPickUpSoundInstance.Play();
                    redCardCaptured = true;
                }

                //Exit Door

                if (playerCollisionRect.Intersects(exitDoorRect) && redCardCaptured && keyboardState.IsKeyDown(Keys.E))
                {
                    doorUnlockSoundInstance.Play();
                    canExitRed = true;
                }

                else if (canExitRed)
                {
                    screen = Screen.LevelSelector;
                }




            }


            else if (screen == Screen.Level3)
            {
                mouseState = Mouse.GetState();
                keyboardState = Keyboard.GetState();

                SetPlayerDirection();
                playerLocation += playerDirection * speed;
                UpdateRects();
                playerSpeed.X = 0f;
                enemyBallDirection.X -= 0.01f;
                enemyBallRotation += 0.15f;
                enemyBallLocation.X -= enemyBallSpeedX;



                time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (time > frameSpeed)
                {
                    time = 0f;
                    frame = (frame + 1) % daveFrames[directionRow];
                }
                //debug += frame.ToString();

                bgtime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (bgtime > backgroundFrameSpeed)
                {
                    backgroundFrame = (1 + backgroundFrame) % backgroundFrames;
                    bgtime = 0f;
                }


                //backgroundCODE!!!

                currentBackgroundTexture = background1;

                window.X += 2;

                if (window.X > window.Width * 7 - 1)
                {
                    window.X = -800;
                }

                movingWindow.X += 2;

                if (movingWindow.X > movingWindow.Width * 7 - 1)
                {
                    movingWindow.X = -800;
                }

                if (!onGround)
                {
                    gravitySpeed += gravity;
                }
                else if (keyboardState.IsKeyDown(Keys.Space) && onGround)
                {
                    gravitySpeed = -jumpSpeed;
                    onGround = false;
                }
                else if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.W))
                {
                    onGround = true;
                    gravitySpeed = 0f;
                }

                else gravitySpeed += gravity;

                foreach (Rectangle ladder in ladders)
                {
                    if (playerCollisionRect.Intersects(ladder))
                    {
                        gravitySpeed = 0;
                    }

                }

                playerLocation.Y += gravitySpeed;
                enemyBallLocation.Y += ballGravitySpeed;

                UpdateRects();


                foreach (Rectangle barrier in barriers)
                    if (playerCollisionRect.Intersects(barrier))
                    {
                        playerLocation -= playerDirection * speed;
                        UpdateRects();
                    }

                foreach (Rectangle verticalBarrier in verticalBarriers)
                {
                    if (playerCollisionRect.Intersects(verticalBarrier))
                    {
                        if (gravitySpeed > 0)
                        {
                            playerLocation.Y = verticalBarrier.Top - playerCollisionRect.Height;
                            onGround = true;
                            gravitySpeed = 0;
                        }
                        UpdateRects();

                        playerLocation -= playerDirection * speed;
                        UpdateRects();
                    }
                }

                //Ball hitting barriers
                foreach (Rectangle barrier in barriers)
                {
                    if (enemyBallCollisionRect.Intersects(barrier))
                    {
                        enemyBallLocation.Y = barrier.Top - enemyBallCollisionRect.Height;
                        ballGravitySpeed = 0;
                    }
                    else
                    {
                        ballGravitySpeed = 2;
                    }
                }

                foreach (Rectangle verticalBarrier in verticalBarriers)
                {
                    if (enemyBallCollisionRect.Intersects(verticalBarrier))
                    {
                        enemyBallSpeedX *= -1;
                        UpdateRects();
                    }
                }

                //Hitting a platform while moving vertically
                foreach (Rectangle barrier in barriers)
                    if (playerCollisionRect.Intersects(barrier))
                    {
                        //Moving Up
                        if (gravitySpeed < 0)
                        {
                            playerLocation.Y = barrier.Bottom;
                            gravitySpeed = 0;
                        }
                        //Moving Down
                        else if (gravitySpeed > 0)
                        {
                            playerLocation.Y = barrier.Top - playerCollisionRect.Height;
                            onGround = true;
                            gravitySpeed = 0;
                        }
                        UpdateRects();
                    }


                if (enemyBallCollisionRect.X <= 0 || enemyBallCollisionRect.X >= 800 - enemyBallCollisionRect.Width)
                {
                    enemyBallSpeedX *= -1;
                    UpdateRects();
                }



                //Ball harm player yay!

                if (enemyBallCollisionRect.Intersects(playerCollisionRect))
                {
                    
                    damageSoundInstance.Play();
                    hitCount++;
                    enemyBallLocation.X = 400;
                    enemyBallLocation.Y = 100;
                    UpdateRects();

                    if (hitCount == 1)
                    {
                        oneHeartLost = true;
                        fullHeart = false;
                    }

                    if (hitCount == 2)
                    {
                        oneHeartLost = false;
                        twoHeartLost = true;
                    }

                    if (hitCount == 3)
                    {
                        noHeart = true;
                        twoHeartLost = false;
                        playerDeathCheck = true;
                        screen = Screen.DeathScreen;
                        blueCardCaptured = false;

                    }
                }

                //Keycard Grabbers: 
                if (playerCollisionRect.Intersects(blueKeyCardCollectable) && keyboardState.IsKeyDown(Keys.E))
                {
                    keycardPickUpSoundInstance.Play();
                    blueCardCaptured = true;
                }

                //Exit Door

                if (playerCollisionRect.Intersects(exitDoorRect) && blueCardCaptured == true && keyboardState.IsKeyDown(Keys.E))
                {
                    doorUnlockSoundInstance.Play();
                    canExitBlue = true;
                }

                else if (canExitBlue)
                {
                    screen = Screen.LevelSelector;
                }

            }

            else if (screen == Screen.EndScreen)
            {
                if (escapeRect.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed 
                    && prevMouseState.LeftButton == ButtonState.Released)
                {
                    Exit();
                }
            }

            base.Update(gameTime);
        }
        
        

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (screen == Screen.StartMenu)
            {


                _spriteBatch.Draw(background2, movingWindow, Color.Magenta);
                _spriteBatch.Draw(background3, new Rectangle(movingWindow.X - movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);
                _spriteBatch.Draw(background4, new Rectangle(movingWindow.X - 2 * movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);
                _spriteBatch.Draw(background5, new Rectangle(movingWindow.X - 3 * movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);
                _spriteBatch.Draw(background6, new Rectangle(movingWindow.X - 4 * movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);
                _spriteBatch.Draw(background7, new Rectangle(movingWindow.X - 6 * movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);
                _spriteBatch.Draw(background8, new Rectangle(movingWindow.X - 5 * movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);

                _spriteBatch.Draw(background1, window, Color.Magenta);
                //_spriteBatch.Draw(trainTexture, new Rectangle(0, 400, trainTexture.Width, trainTexture.Height), Color.White);

                _spriteBatch.Draw(startmenuTexture, new Rectangle(0,0, 800, 500), Color.White);
                //_spriteBatch.Draw(rectangleTexture, new Rectangle(215, 290, 380, 35), Color.Black * 0.5f);

                //_spriteBatch.Draw(rectangleTexture, new Rectangle(215, 330, 380, 35), Color.Black * 0.5f);


            }


            if (screen == Screen.Instructions)
            {

                _spriteBatch.Draw(instructionsMenuTexture, new Rectangle(0, 0, 800, 500), Color.White);
                //_spriteBatch.Draw(rectangleTexture, new Rectangle(566, 60, 110, 50), Color.Black * 0.5f);
                //_spriteBatch.Draw(rectangleTexture, new Rectangle(680, 60, 110, 50), Color.Black * 0.5f);

            }

            if (screen == Screen.LevelSelector)
            {
                
                if (canEscape == false)
                    _spriteBatch.Draw(levelSelectionMenuTexture, new Rectangle(0, 0, 800, 500), Color.White);

                if (canEscape == true)
                    _spriteBatch.Draw(escapeTexture, new Rectangle(0, 0, 800, 500), Color.White);


                _spriteBatch.Draw(rectangleTexture, escapeRect, Color.White * 0.15f);



                if (yellowCardCaptured == true)
                    _spriteBatch.Draw(yellowKeyCardTexture, new Rectangle(185, 251, 80, 80), Color.White);


                if (redCardCaptured == true)
                    _spriteBatch.Draw(redKeyCardTexture, new Rectangle(359, 251, 80, 80), Color.White);

                if (blueCardCaptured == true)
                    _spriteBatch.Draw(blueKeyCardTexture, new Rectangle(532, 251, 80, 80), Color.White);

            }


            if (screen == Screen.Level1)
            {
                _spriteBatch.Draw(background2, movingWindow, Color.Magenta);
                _spriteBatch.Draw(background3, new Rectangle(movingWindow.X - movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);
                _spriteBatch.Draw(background4, new Rectangle(movingWindow.X - 2 * movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);
                _spriteBatch.Draw(background5, new Rectangle(movingWindow.X - 3 * movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);
                _spriteBatch.Draw(background6, new Rectangle(movingWindow.X - 4 * movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);
                _spriteBatch.Draw(background7, new Rectangle(movingWindow.X - 6 * movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);
                _spriteBatch.Draw(background8, new Rectangle(movingWindow.X - 5 * movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);

                _spriteBatch.Draw(background1, window, Color.Magenta);

                _spriteBatch.Draw(trainTexture, new Rectangle(0, 400, trainTexture.Width, trainTexture.Height), Color.Magenta);

                if (yellowCardCaptured == false)
                {
                    _spriteBatch.Draw(yellowKeyCardTexture, yellowKeyCardCollectable, Color.White);
                }

                if (canExitYellow == false)
                {
                    _spriteBatch.Draw(yellowLockDoor, new Rectangle(500, 225, exitDoorTexture.Width, exitDoorTexture.Height), Color.Magenta);
                }

                else
                {
                    _spriteBatch.Draw(exitDoorTexture, new Rectangle(500, 225, exitDoorTexture.Width, exitDoorTexture.Height), Color.Magenta);
                }

                foreach (Rectangle barrier in barriers)
                {
                    _spriteBatch.Draw(rectangleTexture, barrier, Color.Blue * 0.3f);
                }

                foreach (Rectangle verticalBarrier in verticalBarriers)
                {
                    _spriteBatch.Draw(rectangleTexture, verticalBarrier, Color.Blue * 0.3f);
                }
                foreach (Rectangle ladder in ladders)
                {
                    _spriteBatch.Draw(ladderTexture, ladder, Color.Brown);
                }

                _spriteBatch.Draw(rectangleTexture, playerCollisionRect, Color.Black * 0.3f);
                _spriteBatch.Draw(characterSpriteSheet, playerDrawRect,
                    new Rectangle(frame * width, directionRow * height, width, height),
                    Color.White,
                    0,
                    new((int)playerDrawRect.Width / 2, 0),
                    daveFlipHorizontally,
                    0
                    );

                _spriteBatch.Draw(enemyBall, new Rectangle(enemyBallCollisionRect.X +25, enemyBallCollisionRect.Y +25, enemyBallCollisionRect.Width, enemyBallCollisionRect.Height),
                   null,
                   Color.White,
                   enemyBallRotation,
                   new Vector2(enemyBall.Width / 2, enemyBall.Height / 2),
                   SpriteEffects.None,
                   0f);

                if (fullHeart == true)
                {
                    _spriteBatch.Draw(fullHeartsTexture, new Rectangle(50, 450, fullHeartsTexture.Width, fullHeartsTexture.Height), Color.White);
                }

                if (oneHeartLost == true)
                    _spriteBatch.Draw(twoHeartsTexture, new Rectangle(50, 450, fullHeartsTexture.Width, fullHeartsTexture.Height), Color.White);

                if (twoHeartLost == true)
                    _spriteBatch.Draw(oneHeartsTexture, new Rectangle(50, 450, fullHeartsTexture.Width, fullHeartsTexture.Height), Color.White);

                if (noHeart == true)
                    _spriteBatch.Draw(zeroHeartsTexture, new Rectangle(50, 450, fullHeartsTexture.Width, fullHeartsTexture.Height), Color.White);

            }

            if (screen == Screen.Level2)
            {
                _spriteBatch.Draw(background2, movingWindow, Color.Magenta);
                _spriteBatch.Draw(background3, new Rectangle(movingWindow.X - movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);
                _spriteBatch.Draw(background4, new Rectangle(movingWindow.X - 2 * movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);
                _spriteBatch.Draw(background5, new Rectangle(movingWindow.X - 3 * movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);
                _spriteBatch.Draw(background6, new Rectangle(movingWindow.X - 4 * movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);
                _spriteBatch.Draw(background7, new Rectangle(movingWindow.X - 6 * movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);
                _spriteBatch.Draw(background8, new Rectangle(movingWindow.X - 5 * movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);

                _spriteBatch.Draw(background1, window, Color.Magenta);

                _spriteBatch.Draw(trainTexture, new Rectangle(0, 400, trainTexture.Width, trainTexture.Height), Color.Magenta);

                if (redCardCaptured == false)
                {
                    _spriteBatch.Draw(redKeyCardTexture, redKeyCardCollectable, Color.White);
                }

                if (canExitRed == false)
                {
                    _spriteBatch.Draw(redLockDoor, new Rectangle(500, 225, exitDoorTexture.Width, exitDoorTexture.Height), Color.Magenta);
                }

                else
                {
                    _spriteBatch.Draw(exitDoorTexture, new Rectangle(500, 225, exitDoorTexture.Width, exitDoorTexture.Height), Color.Magenta);
                }

                foreach (Rectangle barrier in barriers)
                {
                    _spriteBatch.Draw(rectangleTexture, barrier, Color.Blue * 0.3f);
                }

                foreach (Rectangle ladder in ladders)
                {
                    _spriteBatch.Draw(ladderTexture, ladder, Color.Brown);
                }

                _spriteBatch.Draw(rectangleTexture, playerCollisionRect, Color.Black * 0.3f);
                _spriteBatch.Draw(characterSpriteSheet, playerDrawRect,
                    new Rectangle(frame * width, directionRow * height, width, height),
                    Color.White,
                    0,
                    new((int)playerDrawRect.Width / 2, 0),
                    daveFlipHorizontally,
                    0
                    );

                _spriteBatch.Draw(enemyBall, new Rectangle(enemyBallCollisionRect.X + 25, enemyBallCollisionRect.Y + 25, enemyBallCollisionRect.Width, enemyBallCollisionRect.Height),
                   null,
                   Color.White,
                   enemyBallRotation,
                   new Vector2(enemyBall.Width / 2, enemyBall.Height / 2),
                   SpriteEffects.None,
                   0f);


                if (fullHeart == true)
                {
                    _spriteBatch.Draw(fullHeartsTexture, new Rectangle(50, 450, fullHeartsTexture.Width, fullHeartsTexture.Height), Color.White);
                }

                if (oneHeartLost == true)
                    _spriteBatch.Draw(twoHeartsTexture, new Rectangle(50, 450, fullHeartsTexture.Width, fullHeartsTexture.Height), Color.White);

                if (twoHeartLost == true)
                    _spriteBatch.Draw(oneHeartsTexture, new Rectangle(50, 450, fullHeartsTexture.Width, fullHeartsTexture.Height), Color.White);

                if (noHeart == true)
                    _spriteBatch.Draw(zeroHeartsTexture, new Rectangle(50, 450, fullHeartsTexture.Width, fullHeartsTexture.Height), Color.White);
            }


            if (screen == Screen.Level3)
            {
                _spriteBatch.Draw(background2, movingWindow, Color.Magenta);
                _spriteBatch.Draw(background3, new Rectangle(movingWindow.X - movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);
                _spriteBatch.Draw(background4, new Rectangle(movingWindow.X - 2 * movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);
                _spriteBatch.Draw(background5, new Rectangle(movingWindow.X - 3 * movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);
                _spriteBatch.Draw(background6, new Rectangle(movingWindow.X - 4 * movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);
                _spriteBatch.Draw(background7, new Rectangle(movingWindow.X - 6 * movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);
                _spriteBatch.Draw(background8, new Rectangle(movingWindow.X - 5 * movingWindow.Width, movingWindow.Y, movingWindow.Width, movingWindow.Height), Color.Magenta);

                _spriteBatch.Draw(background1, window, Color.Magenta);

                _spriteBatch.Draw(trainTexture, new Rectangle(0, 400, trainTexture.Width, trainTexture.Height), Color.Magenta);

                if (blueCardCaptured == false)
                {
                    _spriteBatch.Draw(blueKeyCardTexture, blueKeyCardCollectable, Color.White);
                }

                if (canExitBlue == false)
                {
                    _spriteBatch.Draw(blueLockDoor, new Rectangle(500, 225, exitDoorTexture.Width, exitDoorTexture.Height), Color.Magenta);
                }

                else
                {
                    _spriteBatch.Draw(exitDoorTexture, new Rectangle(500, 225, exitDoorTexture.Width, exitDoorTexture.Height), Color.Magenta);
                }

                foreach (Rectangle barrier in barriers)
                {
                    _spriteBatch.Draw(rectangleTexture, barrier, Color.Blue * 0.3f);
                }

                foreach (Rectangle ladder in ladders)
                {
                    _spriteBatch.Draw(ladderTexture, ladder, Color.Brown);
                }

                _spriteBatch.Draw(rectangleTexture, playerCollisionRect, Color.Black * 0.3f);
                _spriteBatch.Draw(characterSpriteSheet, playerDrawRect,
                    new Rectangle(frame * width, directionRow * height, width, height),
                    Color.White,
                    0,
                    new((int)playerDrawRect.Width / 2, 0),
                    daveFlipHorizontally,
                    0
                    );

                _spriteBatch.Draw(enemyBall, new Rectangle(enemyBallCollisionRect.X + 25, enemyBallCollisionRect.Y + 25, enemyBallCollisionRect.Width, enemyBallCollisionRect.Height),
                   null,
                   Color.White,
                   enemyBallRotation,
                   new Vector2(enemyBall.Width / 2, enemyBall.Height / 2),
                   SpriteEffects.None,
                   0f);


                if (fullHeart == true)
                {
                    _spriteBatch.Draw(fullHeartsTexture, new Rectangle(50, 450, fullHeartsTexture.Width, fullHeartsTexture.Height), Color.White);
                }

                if (oneHeartLost == true)
                    _spriteBatch.Draw(twoHeartsTexture, new Rectangle(50, 450, fullHeartsTexture.Width, fullHeartsTexture.Height), Color.White);

                if (twoHeartLost == true)
                    _spriteBatch.Draw(oneHeartsTexture, new Rectangle(50, 450, fullHeartsTexture.Width, fullHeartsTexture.Height), Color.White);

                if (noHeart == true)
                    _spriteBatch.Draw(zeroHeartsTexture, new Rectangle(50, 450, fullHeartsTexture.Width, fullHeartsTexture.Height), Color.White);
            }


            if (screen == Screen.DeathScreen)
            {
                _spriteBatch.Draw(deathScreenTexture, new Rectangle(0, 0, 800, 500), Color.White);
            }

            if (screen == Screen.EndScreen)
            {
                _spriteBatch.Draw(endGameTexture, new Rectangle(0, 0, 800, 500), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void UpdateRects() 
        {
            if (screen == Screen.Level1)
            {
                enemyBallCollisionRect.Location = enemyBallLocation.ToPoint();
                playerCollisionRect.Location = playerLocation.ToPoint();
                playerDrawRect.X = playerCollisionRect.X;

                if (directionRow == climbUp)
                {

                    playerDrawRect.X = playerCollisionRect.X - 5;
                    playerDrawRect.Y = playerCollisionRect.Y - 10;

                }

                else
                    playerDrawRect.Y = playerCollisionRect.Y;

                if (daveFlipHorizontally == SpriteEffects.FlipHorizontally)
                {
                    playerDrawRect.X = playerCollisionRect.X;
                }
            }

            if (screen == Screen.Level2)
            {
                enemyBallCollisionRect.Location = enemyBallLocation.ToPoint();
                playerCollisionRect.Location = playerLocation.ToPoint();
                playerDrawRect.X = playerCollisionRect.X;

                if (directionRow == climbUp)
                {

                    playerDrawRect.X = playerCollisionRect.X - 5;
                    playerDrawRect.Y = playerCollisionRect.Y - 10;

                }

                else
                    playerDrawRect.Y = playerCollisionRect.Y;

                if (daveFlipHorizontally == SpriteEffects.FlipHorizontally)
                {
                    playerDrawRect.X = playerCollisionRect.X;
                }
            }

            if (screen == Screen.Level3)
            {
                enemyBallCollisionRect.Location = enemyBallLocation.ToPoint();
                playerCollisionRect.Location = playerLocation.ToPoint();
                playerDrawRect.X = playerCollisionRect.X;

                if (directionRow == climbUp)
                {

                    playerDrawRect.X = playerCollisionRect.X - 5;
                    playerDrawRect.Y = playerCollisionRect.Y - 10;

                }

                else
                    playerDrawRect.Y = playerCollisionRect.Y;

                if (daveFlipHorizontally == SpriteEffects.FlipHorizontally)
                {
                    playerDrawRect.X = playerCollisionRect.X;
                }
            }

        }

        private void SetPlayerDirection()
        {       
            playerDirection = Vector2.Zero;

            //move to other method
            if (keyboardState.IsKeyDown(Keys.A))
            {
                daveFlipHorizontally = SpriteEffects.FlipHorizontally;

            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                daveFlipHorizontally = SpriteEffects.None;
            }


            if (keyboardState.IsKeyDown(Keys.A))
            {
                if (keyboardState.IsKeyDown(Keys.A) && keyboardState.IsKeyDown(Keys.LeftControl))
                {
                    playerDirection.X -= 1;
                    speed = 4f;
                }
                else
                {
                    if (keyboardState.IsKeyDown(Keys.A))
                    {
                        playerDirection.X -= 1;
                        speed = 1.5f;
                    }
                }
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                if (keyboardState.IsKeyDown(Keys.D) && keyboardState.IsKeyDown(Keys.LeftControl))
                {
                    playerDirection.X += 1;
                    speed = 4f;
                }
                else
                {
                    if (keyboardState.IsKeyDown(Keys.D))
                    {
                        playerDirection.X += 1;
                        speed = 1.5f;
                    }
                }
            }
                       

            foreach (Rectangle ladder in ladders)
            {
                if (playerCollisionRect.Intersects(ladder))
                {
                    if (keyboardState.IsKeyDown(Keys.W))
                        playerDirection.Y -= 1;
                    if (keyboardState.IsKeyDown(Keys.S))
                        playerDirection.Y += 1;

                }
                    
            }

            foreach(Rectangle ladder in ladders)
            {
                if (playerCollisionRect.Intersects(ladder))

                    directionRow = climbUp;

                else 
                    directionRow = idle;
            }
                

            //if (!onGround && keyboardState.IsKeyDown(Keys.Space))
            //        directionRow = playerjump;
           

            if (playerDirection != Vector2.Zero)
            {
                if (playerDirection.X < 0) // Moving left
                    directionRow = leftRow;
                else if (playerDirection.X > 0) // Moving right
                    directionRow = rightRow;
                else if (playerDirection.Y < 0) // Moving up
                    directionRow = climbUp;
                else if (playerDirection.Y > 0) // Moving down
                    directionRow = climbUp;
               



                playerDirection.Normalize();
                
                foreach (Rectangle ladder in ladders)
                {
                    if (playerCollisionRect.Intersects(ladder))
                    {
                       if (playerDirection.Y < 0) // Moving up
                            directionRow = climbUp;
                       else if (playerDirection.Y > 0) // Moving down
                            directionRow = climbUp;
                    }
                }
                
                if (playerDirection.X < 0) // Moving left
                    directionRow = leftRow;
                else if (playerDirection.X > 0) // Moving right
                    directionRow = rightRow;




            }
            



        }
        


    }
}
