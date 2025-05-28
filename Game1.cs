using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Class_system__not_systemic_
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D characterSpriteSheet, rectangleTexture;

        List<Rectangle> barriers;
        KeyboardState keyboardState;
        
        int rows, columns;
        int frame;
        int frames;
        int directionRow;
        int leftRow, rightRow, upRow, downRow;
        int width;
        int height;

        float speed;
        float time;
        float frameSpeed;

        Vector2 playerLocation;
        Vector2 playerDirection;

        Rectangle playerCollisionRect, playerDrawRect, window;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 500;
            _graphics.ApplyChanges();
            window = new Rectangle(0, 0, 800, 500);


            barriers = new List<Rectangle>();
            barriers.Add(new Rectangle(100, 100, 30, 150));
            barriers.Add(new Rectangle(10, 400, 300, 30));
            barriers.Add(new Rectangle(400, 200, 200, 30));
            barriers.Add(new Rectangle(700, 450, 30, 100));

            speed = 1.5f;

            columns = 9;
            rows = 4;
            upRow = 0;
            leftRow = 1;
            downRow = 2;
            rightRow = 3;

            time = 0.0f;
            frameSpeed = 0.08f;
            frames = 9;
            frame = 0;


            directionRow = downRow;

            playerLocation = new Vector2(20, 20);
            playerCollisionRect = new Rectangle(20, 20, 20, 48);
            playerDrawRect = new Rectangle(20, 20, 50, 65);

            UpdateRects();

            base.Initialize();
            width = characterSpriteSheet.Width / columns;
            height = characterSpriteSheet.Height / rows;

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            rectangleTexture = Content.Load<Texture2D>("rectangle");
            characterSpriteSheet = Content.Load<Texture2D>("skeleton_spritesheet");


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keyboardState = Keyboard.GetState();

            SetPlayerDirection();
            playerLocation += playerDirection * speed;
            UpdateRects();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            foreach (Rectangle barrier in barriers)
            {
                _spriteBatch.Draw(rectangleTexture, barrier, Color.Black);
            }

            _spriteBatch.Draw(rectangleTexture, playerCollisionRect, Color.Black * 0.3f);
            _spriteBatch.Draw(characterSpriteSheet, playerDrawRect,
            new Rectangle(0, directionRow * height, width, height), Color.White);


            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void UpdateRects() 
        {
            playerCollisionRect.Location = playerLocation.ToPoint();
            playerDrawRect.X = playerCollisionRect.X - 15;
            playerDrawRect.Y = playerCollisionRect.Y - 15;
        }

        private void SetPlayerDirection()
        {
            playerDirection = Vector2.Zero;
            if (keyboardState.IsKeyDown(Keys.A))
                playerDirection.X -= 1;
            if (keyboardState.IsKeyDown(Keys.D))
                playerDirection.X += 1;
            if (keyboardState.IsKeyDown(Keys.W))
                playerDirection.Y -= 1;
            if (keyboardState.IsKeyDown(Keys.S))
                playerDirection.Y += 1;

            if (playerDirection != Vector2.Zero)
            {
                playerDirection.Normalize();
                if (playerDirection.X < 0) // Moving left
                    directionRow = leftRow;
                else if (playerDirection.X > 0) // Moving right
                    directionRow = rightRow;
                else if (playerDirection.Y < 0) // Moving up
                    directionRow = upRow;
                else
                    directionRow = downRow;

            }


        }
        


    }
}
