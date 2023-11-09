using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Project_Spore.Managers;
using Project_Spore.State_Management;
using Project_Spore.UI;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Project_Spore.Collisions;


namespace Project_Spore.Scenes
{
    public abstract class MenuScene : IScene
    {
        public SceneManager SceneManager { get; set; }

        protected List<MenuEntry> _menuEntries = new List<MenuEntry>();
        private int _selectedIndex = 0;

        private readonly InputAction _menuUp;
        private readonly InputAction _menuDown;
        private readonly InputAction _menuSelect;
        private readonly InputAction _menuCancel;

        private Texture2D _cursorTexture; 
        private Vector2 _cursorPosition;

        private string _menuTitle;

        private ContentManager _content;

        public int VerticalOffset { get; init; }

        protected MenuScene(string menuTitle)
        {
            _menuTitle = menuTitle;

            _menuUp = new InputAction(new[] { Keys.Up }, true);
            _menuDown = new InputAction(new[] { Keys.Down }, true);
            _menuSelect = new InputAction(new[] { Keys.Enter, Keys.Space }, true);
            _menuCancel = new InputAction(new[] { Keys.Back }, true);
        }

        public virtual void LoadContent()
        {
            if (_content == null)
                _content = new ContentManager(SceneManager.Game.Services, "Content");

            _cursorTexture = _content.Load<Texture2D>("Textures/Cursor");
        }

        public virtual void UnloadContent()
        {
        }

        private void UpdateMenuEntryLocations()
        {
            var position = new Vector2(0f, 175f + VerticalOffset);

            foreach (var entry in _menuEntries)
            {
                position.X = SceneManager.VirtualResolution.X / 2 - entry.GetWidth(this) / 2;

                // set the entry's position
                entry.Position = position;

                // move down for the next entry the size of this entry
                position.Y += entry.GetHeight(this);
            }
        }

        public void HandleInput(GameTime gameTime, InputState input)
        {
            if (_menuUp.Occurred(input))
            {
                _selectedIndex--;

                if (_selectedIndex < 0)
                    _selectedIndex = _menuEntries.Count - 1;
            }

            if (_menuDown.Occurred(input))
            {
                _selectedIndex++;

                if (_selectedIndex >= _menuEntries.Count)
                    _selectedIndex = 0;
            }

            if (_menuSelect.Occurred(input))
                OnSelectEntry(_selectedIndex);
            else if (_menuCancel.Occurred(input))
                OnCancel();

            Vector2 mousePosition = input.GetAdjustedMouseLocation(SceneManager.ScaleMatrix);
            BoundingRectangle mouseRectangle = new((int)mousePosition.X, (int)mousePosition.Y, 1, 1);

            _cursorPosition = new Vector2((int)mousePosition.X, (int)mousePosition.Y);

            for (int i = 0; i < _menuEntries.Count; i++)
            {
                var entry = _menuEntries[i];
                BoundingRectangle entryRectangle = new((int)entry.Position.X, (int)(entry.Position.Y - SceneManager.Font.LineSpacing /  2), entry.GetWidth(this), entry.GetHeight(this));

                if (CollisionHelper.Collides(entryRectangle, mouseRectangle))
                {
                    _selectedIndex = i;
                    if (input.isNewMouseLeftClickPress())
                    {
                        OnSelectEntry(_selectedIndex);
                    }
                }
            }
        }

        protected virtual void OnSelectEntry(int index)
        {
            _menuEntries[index].OnSelectEntry();
        }

        protected void OnCancel()
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime)
        {
            UpdateMenuEntryLocations();

            var spriteBatch = SceneManager.SpriteBatch;
            var font = SceneManager.Font;

            spriteBatch.Begin(transformMatrix: SceneManager.ScaleMatrix, samplerState: SamplerState.PointClamp);

            for (int i = 0; i < _menuEntries.Count; i++)
            {
                var menuEntry = _menuEntries[i];
                bool isSelected = i == _selectedIndex;
                menuEntry.Draw(this, isSelected, gameTime);
            }

            var titlePosition = new Vector2(SceneManager.VirtualResolution.X / 2, 80);
            var titleOrigin = font.MeasureString(_menuTitle) / 2;
            var titleColor = new Color(192, 192, 192);
            const float titleScale = 2f;

            spriteBatch.DrawString(font, _menuTitle, titlePosition, titleColor,
                0, titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.Draw(_cursorTexture, _cursorPosition, null, Color.White,
                0, new Vector2(32 / 2, 32 / 2), 1f, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}
