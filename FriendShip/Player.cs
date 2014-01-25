using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FriendShip
{

	public class Player : DrawableGameComponent
	{
		public const int moveSpeed = 5;

		public Vector2 Position;
		private Texture2D _texture;
		private GameCore _game;
		private Room currentRoom;

		public Player (GameCore game, Texture2D texture, Room startRoom)
			: base(game)
		{
			_game = game;
			_texture = texture;

			Position = startRoom.SpawnPosition;
			currentRoom = startRoom;

			Visible = true;
			Enabled = true;
			game.Components.Add (this);
		}

        public override void Update(GameTime gameTime)
        {
            KeyboardState currentKeyState = Keyboard.GetState();
            var Delta = new Vector2();
            if (currentKeyState.IsKeyDown(Keys.Left))
                Delta.X = -moveSpeed;

            if (currentKeyState.IsKeyDown(Keys.Right))
                Delta.X = moveSpeed;

            if (currentKeyState.IsKeyDown(Keys.Up))
                Delta.Y = -moveSpeed;

            if (currentKeyState.IsKeyDown(Keys.Down))
                Delta.Y = moveSpeed;
            Position = Position + Delta;

			//check collision with room exits
			var boundingBox = new Rectangle ((int)Position.X, (int)Position.Y, 31, 56);
			foreach(var exit in currentRoom.Exits)
			{
				if (exit.Collides (boundingBox))
				{
					this.currentRoom = exit.NextRoom;
					this.Position = exit.SpawPoint;
				}
			}

            base.Update(gameTime);
        }

		public override void Draw (GameTime gameTime)
		{
			if (_game.spriteBatch != null)
			{
				_game.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

				_game.spriteBatch.Draw(_texture, Position, Color.White);

				_game.spriteBatch.End();
			}
			base.Draw (gameTime);
		}
	}
}
