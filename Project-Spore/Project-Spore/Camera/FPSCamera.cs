using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Project_Spore.Heightmap;

namespace Project_Spore.Camera
{
    public class FPSCamera : ICamera
    {
        float horizontalAngle;

        float verticalAngle;

        Vector3 position;

        MouseState oldMouseState;

        Game game;

        public float HeightOffset { get; set; } = 5;

        public float Sensitivity { get; set; } = 0.0018f;

        public float Speed { get; set; } = 0.5f;

        public Matrix View { get; protected set; }

        public Matrix Projection { get; protected set; }

        public IHeightMap HeightMap { get; set; }

        public Vector3 Position => position;

        public Vector3 Direction { get; private set; }

        public void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            var newMouseState = Mouse.GetState();

            var facing = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(horizontalAngle));

            if (keyboard.IsKeyDown(Keys.W)) position += facing * Speed;
            if (keyboard.IsKeyDown(Keys.S)) position -= facing * Speed;

            if (keyboard.IsKeyDown(Keys.A)) position += Vector3.Cross(Vector3.Up, facing) * Speed;
            if (keyboard.IsKeyDown(Keys.D)) position -= Vector3.Cross(Vector3.Up, facing) * Speed;

            if (HeightMap != null)
            {
                position.Y = HeightMap.GetHeightAt(position.X, position.Z) + HeightOffset;
            }

            horizontalAngle += Sensitivity * (oldMouseState.X - newMouseState.X);

            verticalAngle += Sensitivity * (oldMouseState.Y - newMouseState.Y);

            var direction = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationX(verticalAngle) * Matrix.CreateRotationY(horizontalAngle));

            Direction = direction;

            View = Matrix.CreateLookAt(position, position + direction, Vector3.Up);

            Mouse.SetPosition(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
            oldMouseState = Mouse.GetState();
        }

        public FPSCamera(Game game, Vector3 position)
        {
            this.game = game;
            this.position = position;

            this.horizontalAngle = 0;
            this.verticalAngle = 0;

            this.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, game.GraphicsDevice.Viewport.AspectRatio, 1, 1000);
            Mouse.SetPosition(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
            oldMouseState = Mouse.GetState();
        }
    }
}
