using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project_Spore.Camera;
using Project_Spore.Heightmap;
using Project_Spore.Scenes;
using Project_Spore.State_Management;

namespace Project_Spore.Managers
{
    public class SceneManager : DrawableGameComponent
    {
        private IScene _currentScene;

        private readonly ContentManager _content;
        private GraphicsDeviceManager _graphics;
        private Matrix _scaleMatrix = Matrix.Identity;

        private readonly InputState _input = new InputState();

        public SpriteBatch SpriteBatch { get; private set; }

        public SpriteFont Font { get; private set; }

        public readonly Point VirtualResolution = new Point(1280, 720);
        public Matrix ScaleMatrix => _scaleMatrix;

        private FPSCamera _camera;
        private Terrain _terrain;

        public SceneManager(Game game, GraphicsDeviceManager graphics) : base(game)
        {
            _content = new ContentManager(game.Services, "Content");
            _graphics = graphics;
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Font = _content.Load<SpriteFont>("Fonts/Silkscreen");

            CalculateMatrix();
        }

        public void SetScene(IScene newScene)
        {
            
            if (_currentScene != null) { _currentScene.UnloadContent(); }

            _currentScene = newScene;
            newScene.SceneManager = this;

            _currentScene.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _input.Update();

            _currentScene?.HandleInput(gameTime, _input);
            _currentScene.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _currentScene.Draw(gameTime);
        }

        public void CalculateMatrix()
        {
            float scaleX = (float)Game.GraphicsDevice.Viewport.Width / VirtualResolution.X;
            float scaleY = (float)Game.GraphicsDevice.Viewport.Height / VirtualResolution.Y;

            _scaleMatrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);
        }

        public void SetResolution(Point resolution)
        {
            _graphics.PreferredBackBufferWidth = resolution.X;
            _graphics.PreferredBackBufferHeight = resolution.Y;
            _graphics.ApplyChanges();

            CalculateMatrix();
        }

        public int ReturnMaxWidth()
        {
            return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        }

        public bool SetFullScreen()
        {
            if (!_graphics.IsFullScreen)
            {
                // When going to fullscreen, set to the default monitor resolution
                _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }

            _graphics.ToggleFullScreen();

            _graphics.ApplyChanges();
            CalculateMatrix();

            return _graphics.IsFullScreen;
        }
    }
}
