using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project_Spore.Camera;
using Project_Spore.Heightmap;
using Project_Spore.Managers;
using Project_Spore.State_Management;
using MonoGame.Extended;

namespace Project_Spore.Scenes
{
    public class GameScene : IScene
    {
        private ContentManager _content;

        public SceneManager SceneManager { get; set; }

        private FPSCamera _camera;

        private Terrain _terrain;

        private Model _model;

        private Matrix _modelWorldMatrix;

        public FPSCamera Camera => _camera;

        public Terrain Terrain => _terrain;

        public GameScene()
        {

        }

        public void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(SceneManager.Game.Services, "Content");

            // Create the camera
            _camera = new FPSCamera(SceneManager.Game, new Vector3(0, 5, 0));

            // TODO: use this.Content to load your game content here

            Texture2D heightmap = _content.Load<Texture2D>("Heightmaps/heightmap");
            _terrain = new Terrain(SceneManager.Game, heightmap, 10f, Matrix.Identity);

            _model = _content.Load<Model>("Models/shaylushay/source/Shaylushay");
            float modelDistance = 10;
            Vector3 modelPosition = _camera.Position + (_camera.Direction * modelDistance);
            _modelWorldMatrix = Matrix.CreateTranslation(modelPosition);

            _camera.HeightMap = _terrain;
        }

        public void UnloadContent()
        {
            if (_content != null)
                _content.Unload();
        }

        public void HandleInput(GameTime gameTime, InputState input)
        {

        }

        public void Update(GameTime gameTime)
        {
            _camera.Update(gameTime);

            float modelDistance = 10; // or whatever distance you want the model to be
            Vector3 modelPosition = _camera.Position + (_camera.Direction * modelDistance);
            _modelWorldMatrix = Matrix.CreateTranslation(modelPosition);
        }

        public void Draw(GameTime gameTime)
        {
            _terrain.Draw(_camera);

            foreach (var mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = _modelWorldMatrix;
                    effect.View = _camera.View;
                    effect.Projection = _camera.Projection;

                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
                mesh.Draw();
            }
        }
    }
}
