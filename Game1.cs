using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using Microsoft.Xna.Framework;
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
        EndScreen
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Screen screen;
        Texture2D characterSpriteSheet, rectangleTexture, backgroundSpriteSheet, currentBackgroundTexture, background1, background2, background3,
            background4, background5, background6, background7, background8, trainTexture, startmenuTexture, instructionsMenuTexture, cyborgAttack1, cyborgAttack2, cyborgAttack3,
            cyborgDeath, cyborgRun, cyborgIdle, cyborgPunch, currentCyborgTexture, levelSelectionMenuTexture;

        List<Texture2D> cyborgSprites;
        List<Rectangle> barriers;
        List<Rectangle> platforms;
        List<Rectangle> ladders;
        KeyboardState keyboardState;
        MouseState mouseState;

        SpriteEffects daveFlipHorizontally;
        int[] daveFrames;
        int[] cyborgFrames;
        int rows, columns, bgRows, bgColumns;
        int cyborgColumns;
        int frame;
        int frames;
        int backgroundFrame;
        int backgroundFrames;

        int directionRow, bgDraw;
        int mouseX, mouseY;
        int leftRow, rightRow, climbUp, idle, playerjump;
        int width, backgroundWidth;
        int height, backgroundHeight;

        float speed;
        float time, bgtime;
        float frameSpeed, backgroundFrameSpeed;
        float gravity = 0.3f; // This is how fast player accelerated downwards
        float gravitySpeed = 0f;
        float jumpSpeed = 7f; // This will determine the strength of the jump
        bool onGround = false, yellowCardCaptured = false, redCardCaptured = false, blueCardCaptured = false, gameEnd= false;

        string debug = "";

        Vector2 playerLocation = new Vector2(10, 10);
        Vector2 playerDirection;
        Vector2 playerSpeed = Vector2.Zero;
        Rectangle playerCollisionRect, playerDrawRect, window, gameWindow, movingWindow, playGameButton, instructionsButton, instructionsMenuPlayBtn, returnToMenuBtn;


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


            barriers = new List<Rectangle>();

            
            barriers.Add(new Rectangle(0, -10, 800, 10));
            barriers.Add(new Rectangle(-10, 0, 10, 500));
            barriers.Add(new Rectangle(800, 0, 10, 500));
            barriers.Add(new Rectangle(0, 500, 800, 10));

            barriers.Add(new Rectangle(0, 400, 800, 20)); // Ground

            barriers.Add(new Rectangle(100, 250, 800, 20));
            barriers.Add(new Rectangle(80, 80, 800, 20));


            ladders = new List<Rectangle>();
            ladders.Add(new Rectangle(50, 50, 20, 350));


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


            directionRow = leftRow;

            playerLocation = new Vector2(10, 30);
            playerCollisionRect = new Rectangle(10, 30, 35, 50);
            playerDrawRect = new Rectangle(10, 20, 55, 65);


            playGameButton = new Rectangle(215, 290, 380, 35);
            instructionsButton = new Rectangle(215, 330, 380, 35);

            UpdateRects();

            base.Initialize();
            width = characterSpriteSheet.Width / columns;
            height = characterSpriteSheet.Height / rows;
            //backgroundWidth = backgroundSpriteSheet.Width / bgColumns;
            //backgroundHeight = backgroundSpriteSheet.Height / bgRows;




        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            rectangleTexture = Content.Load<Texture2D>("rectangle");
            characterSpriteSheet = Content.Load<Texture2D>("daveSpriteSheet");
            trainTexture = Content.Load<Texture2D>("trainTexture");

            background1 = Content.Load<Texture2D>("background1override");
            background2 = Content.Load<Texture2D>("background2");
            background3 = Content.Load<Texture2D>("background3");
            background4 = Content.Load<Texture2D>("background4");
            background5 = Content.Load<Texture2D>("background5");
            background6 = Content.Load<Texture2D>("background6");
            background7 = Content.Load<Texture2D>("background7");
            background8 = Content.Load<Texture2D>("background8");
            startmenuTexture = Content.Load<Texture2D>("smokebox");
            instructionsMenuTexture = Content.Load<Texture2D>("instructionsMenu");
            levelSelectionMenuTexture = Content.Load<Texture2D>("noLevelComplete");



            cyborgIdle = Content.Load<Texture2D>("Cyborg_idle");       //0
            cyborgRun = Content.Load<Texture2D>("Cyborg_run");         //1
            cyborgAttack1 = Content.Load<Texture2D>("Cyborg_attack1"); //2
            cyborgAttack2 = Content.Load<Texture2D>("Cyborg_attack2"); //3
            cyborgAttack3 = Content.Load<Texture2D>("Cyborg_attack3"); //4
            cyborgPunch = Content.Load<Texture2D>("Cyborg_punch");     //5
            cyborgDeath = Content.Load<Texture2D>("Cyborg_death");     //6


            cyborgSprites = new List<Texture2D>();
            cyborgSprites.Add(cyborgIdle);    //0
            cyborgSprites.Add(cyborgRun);     //1
            cyborgSprites.Add(cyborgAttack1); //2
            cyborgSprites.Add(cyborgAttack2); //3
            cyborgSprites.Add(cyborgAttack3); //4
            cyborgSprites.Add(cyborgPunch);   //5
            cyborgSprites.Add(cyborgDeath);   //6




            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouseState = Mouse.GetState();
            mouseX = mouseState.X;
            mouseY = mouseState.Y;
            this.Window.Title = mouseX + " ," + mouseY + " ";


            if (screen == Screen.StartMenu)
            {
                mouseState = Mouse.GetState();

                if (mouseState.LeftButton == ButtonState.Pressed && playGameButton.Contains(mouseState.Position))
                
                {
                    screen = Screen.LevelSelector;
                }


                if (mouseState.LeftButton == ButtonState.Pressed && instructionsButton.Contains(mouseState.Position))

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


            if (screen == Screen.Instructions)
            {
                mouseState = Mouse.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed && instructionsMenuPlayBtn.Contains(mouseState.Position))
                {
                    screen = Screen.LevelSelector;
                }

                if (mouseState.LeftButton == ButtonState.Pressed && returnToMenuBtn.Contains(mouseState.Position))
                {
                    screen = Screen.StartMenu;
                }

                
            }

            if (screen == Screen.LevelSelector)
            {

                if (yellowCardCaptured == true && redCardCaptured == true == blueCardCaptured == true);
                {
                    gameEnd = true;
                }




            }




            if (screen == Screen.Level1)
            {
                mouseState = Mouse.GetState();
                keyboardState = Keyboard.GetState();

                SetPlayerDirection();
                playerLocation += playerDirection * speed;
                UpdateRects();
                playerSpeed.X = 0f;

                foreach (Rectangle barrier in barriers)
                    if (playerCollisionRect.Intersects(barrier))
                    {
                        playerLocation -= playerDirection * speed;
                        UpdateRects();


                    }

              

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
                UpdateRects();
                // Hitting a platform while moving vertically
                foreach (Rectangle barrier in barriers)
                    if (playerCollisionRect.Intersects(barrier))
                    {
                        // Moving Up
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
                        //playerLocation.Y -= playerDirection * speed;
                        UpdateRects();

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
            }

            if (screen == Screen.EndScreen)
            {

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
                _spriteBatch.Draw(levelSelectionMenuTexture, new Rectangle(0, 0, 800, 500), Color.White);


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

                //_spriteBatch.Draw(backgroundSpriteSheet, window,
                //    new Rectangle(backgroundFrame * backgroundWidth, bgDraw * backgroundHeight, backgroundWidth, backgroundHeight),
                //    Color.White);



                foreach (Rectangle barrier in barriers)
                {
                    _spriteBatch.Draw(rectangleTexture, barrier, Color.Blue * 0.3f);
                }

                foreach (Rectangle ladder in ladders)
                {
                    _spriteBatch.Draw(rectangleTexture, ladder, Color.Blue * 0.3f);
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
            }
            
            if (screen == Screen.EndScreen)
            {

            }




           

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void UpdateRects() 
        {
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
