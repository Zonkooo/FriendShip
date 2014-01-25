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
		TRAP,
	}

	public enum PlayerState
	{
		STILL,
		WALK,
		HIT,
		DEAD,
	}

	public class Player : DrawableGameComponent
	{
		public const int moveSpeed = 5;

		public Vector2 Position;
		public Room currentRoom;
		private PlayerState currentState = PlayerState.STILL;
		private double hitTime;
		private bool flipHorizontally = true;
		public int life = 3;

		private Dictionary<Direction, Keys> controls;
		private Dictionary<PlayerState, MyTexture2D> _textures;
		private GameCore _game;

		public Player (GameCore game, Dictionary<PlayerState, MyTexture2D> textures, Room startRoom, Dictionary<Direction, Keys> controls)
			: base(game)
		{
			this.controls = controls;
			_game = game;
			_textures = textures;

			Position = startRoom.SpawnPosition;
			currentRoom = startRoom;
			startRoom.PlayerEnters ();

			Visible = true;
			Enabled = true;
			game.Components.Add (this);
		}

		bool trapKeyWasDown = false;
        public override void Update(GameTime gameTime)
        {
			if(hitTime > 0)
			{
				hitTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
				_textures [currentState].Update (gameTime.ElapsedGameTime.TotalMilliseconds);

				return;
			}

            KeyboardState currentKeyState = Keyboard.GetState();
			var prevPos = Position;

			var delta = new Vector2();
			var directions = new List<Direction>();

			if (currentKeyState.IsKeyDown (controls [Direction.LEFT]))
			{
				if (currentRoom.MoveType == RoomMovementType.HORIZONTAL)
					delta.X = -moveSpeed;
				directions.Add (Direction.LEFT);
				flipHorizontally = false;
			}
			if (currentKeyState.IsKeyDown (controls [Direction.RIGHT]))
			{
				if (currentRoom.MoveType == RoomMovementType.HORIZONTAL)
					delta.X = moveSpeed;
				directions.Add (Direction.RIGHT);
				flipHorizontally = true;
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

			if (currentKeyState.IsKeyDown (controls [Direction.TRAP]))
			{
				if (!trapKeyWasDown)
					LayTrap ();
				trapKeyWasDown = true;
			}
			else
				trapKeyWasDown = false;

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


			if (Position != prevPos)
				currentState = PlayerState.WALK;
			else
			currentState = PlayerState.STILL;

			//check traps
			if (currentRoom.CheckTraps (Position))
			{
				life--;
				if (life == 0)
					Death ();
				else
				{
					currentState = PlayerState.HIT;
					_textures [currentState].Reset ();
					hitTime = 800;
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

			_textures [currentState].Update (gameTime.ElapsedGameTime.TotalMilliseconds);

            base.Update(gameTime);
        }

		void LayTrap ()
		{
			currentRoom.AddTrap (new Trap(Position));
		}

		void Death ()
		{
			//TODO play death anim
			currentState = PlayerState.DEAD;
			Enabled = false;
			currentRoom.PlayerLeaves ();
		}

		private Rectangle GetBoundingBox()
		{
			return new Rectangle ((int)Position.X, (int)Position.Y, 66, 112);
		}

		public override void Draw (GameTime gameTime)
		{
			if (_game.spriteBatch != null)
			{
				_game.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

				_game.spriteBatch.Draw(_textures[currentState].Texture, Position, _textures[currentState].GetRectangle(), Color.White, 0f, new Vector2(), new Vector2(1), flipHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

				_game.spriteBatch.End();
			}
			base.Draw (gameTime);
		}
	}
}
