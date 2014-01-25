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
	public class Room : DrawableGameComponent
	{
		public Vector2 Position { get; private set; }
		readonly GameCore _game;

		public Texture2D Texture;
		public List<RoomLink> Exits { get; private set;}

		/// <summary> the position where the player should spawn at the begining of the game </summary>
		public Vector2 SpawnPosition {get; private set;}

		public Room (GameCore game, Vector2 position, Vector2 spawnPoint)
			: base(game)
		{
			_game = game;
			Position = position;
			SpawnPosition = spawnPoint;
			Exits = new List<RoomLink> ();

			_game.Components.Add (this);
			this.Enabled = true;
			this.Visible = true;
		}

		public override void Draw (GameTime gameTime)
		{
			if (Texture != null && _game.spriteBatch != null)
			{
				_game.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

				_game.spriteBatch.Draw(Texture, Position, Color.White);

				foreach (var exit in Exits)
					exit.DrawHitBox (_game.spriteBatch, _game.OneWhitePixel);

				_game.spriteBatch.End();
			}
			base.Draw (gameTime);
		}
	}
}
