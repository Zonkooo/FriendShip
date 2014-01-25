using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;

namespace FriendShip
{
	public enum RoomMovementType
	{
		VERTICAL,
		HORIZONTAL,
	}

	public class Room : DrawableGameComponent
	{
		public Vector2 Position { get; private set; }
		public RoomMovementType MoveType { get; private set; }
		readonly GameCore _game;

		public Texture2D Texture;
		public List<RoomLink> Exits { get; private set;}
		private int nbPlayersInRoom = 0;
		private bool _lightOn { get { return nbPlayersInRoom > 0; } }
		private List<Trap> _traps = new List<Trap> ();

		/// <summary> the position where the player should spawn at the begining of the game </summary>
		public Vector2 SpawnPosition {get; private set;}

		public Room (GameCore game, Vector2 position, Vector2 spawnPoint, RoomMovementType moveType)
			: base(game)
		{
			_game = game;
			Position = position;
			SpawnPosition = spawnPoint;
			MoveType = moveType;

			Exits = new List<RoomLink> ();

			_game.Components.Add (this);
			this.Enabled = true;
			this.Visible = true;
		}

		/// <returns>true if a trap was triggered</returns>
		public void PlayerEnters()
		{
			nbPlayersInRoom++;
		}

		public void PlayerLeaves()
		{
			nbPlayersInRoom--;
			if (nbPlayersInRoom == 0 && _traps.Count > 0)
				foreach (var trap in _traps)
					trap.Enable ();
		}

		public void AddTrap(Trap trap)
		{
			_traps.Add (trap);
		}

		public bool CheckTraps(Vector2 playerPos)
		{
			foreach (var trap in _traps)
			{
				if (trap.Enabled)
				{
					//check distance
					if (Math.Abs (trap.Position.X - playerPos.X) < 5 && Math.Abs (trap.Position.Y - playerPos.Y) < 5)
					{
						_traps.Remove (trap);
						return true;
					}
				}
			}
			return false;
		}

		public override void Draw (GameTime gameTime)
		{
			if (Texture != null && _game.spriteBatch != null)
			{
				_game.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

				if(_lightOn)
					_game.spriteBatch.Draw(Texture, Position, Color.White);
				else
					_game.spriteBatch.Draw(_game.OneWhitePixel, new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height), Color.Black);

				foreach (var exit in Exits)
					exit.DrawHitBox (_game.spriteBatch, _game.OneWhitePixel);

				_game.spriteBatch.End();
			}
			base.Draw (gameTime);
		}
	}
}