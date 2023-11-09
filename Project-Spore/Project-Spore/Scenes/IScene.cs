using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project_Spore.Managers;
using Project_Spore.State_Management;

namespace Project_Spore.Scenes
{
    public interface IScene
    {
        SceneManager SceneManager { get; set; }

        void LoadContent();

        void UnloadContent();

        void HandleInput(GameTime gameTime, InputState input);

        void Update(GameTime gameTime);

        void Draw(GameTime gameTime);
    }
}
