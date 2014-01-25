using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace FriendShip
{
	public enum Direction
	{
		UP,
		DOWN,
		RIGHT,
		LEFT,
	}

	public class Player : DrawableGameComponent
	{
		public const int moveSpeed = 5;

		public Vector2 Position;
		private Room currentRoom;

		private Dictionary<Direction, Keys> controls;
		private Texture2D _texture;
		private GameCore _game;

		public Player (GameCore game, Texture2D texture, Room startRoom, Dictionary<Direction, Keys> controls)
			: base(game)
		{
			this.controls = controls;
			_game = game;
			_texture = texture;

			Position = startRoom.SpawnPosition;
			currentRoom = startRoom;
			startRoom.PlayerEnters ();

			Visible = true;
			Enabled = true;
			game.Components.Add (this);
		}

        public override void Update(GameTime gameTime)
        {
            KeyboardState currentKeyState = Keyboard.GetState();

			var delta = new Vector2();
			var directions = new List<Direction>();

			if (currentKeyState.IsKeyDown (controls [Direction.LEFT]))
			{
				if (currentRoom.MoveType == RoomMovementType.HORIZONTAL)
					delta.X = -moveSpeed;
				directions.Add (Direction.LEFT);
			}
			if (currentKeyState.IsKeyDown (controls [Direction.RIGHT]))
			{
				if (currentRoom.MoveType == RoomMovementType.HORIZONTAL)
					delta.X = moveSpeed;
				directions.Add (Direction.RIGHT);
			}
			if (currentKeyState.IsKeyDown (controls [Direction.UP]))
			{
				if(currentRoom.MoveType == RoomMovementType.VERTICAL)
					delta.Y = -moveSpeed;
				directions.Add (Direction.UP);
			}
			if (currentKeyState.IsKeyDown (controls [Direction.DOWN]))
			{
				if(currentRoom.MoveType == RoomMovementType.VERTICAL)
					delta.Y = moveSpeed;
				directions.Add (Direction.DOWN);
			}

            Position = Position + delta;

			//check collision with room exits
			var boundingBox = GetBoundingBox();
			foreach(var wall in _game.Walls)
			{
				if(wall.Collides(boundingBox))
				{
					//cancel move
					Position = Position - delta;
					break;
				}
			}
			foreach(var exit in currentRoom.Exits)
			{
				if (exit.Collides (boundingBox, directions))
				{
					this.currentRoom.PlayerLeaves ();
					this.currentRoom = exit.NextRoom;
					this.currentRoom.PlayerEnters ();
					this.Position = exit.SpawPoint;
				}
			}

            base.Update(gameTime);
        }

		private Rectangle GetBoundingBox()
		{
			return new Rectangle ((int)Position.X, (int)Position.Y, 31, 56);
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
