using System;
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

	public class AllToCale : EventBase
	{
		private readonly Room _target;

		public AllToCale (GameCore game)
			:base(game, "Everyone in the storage room !")
		{
			_target = _game._rooms [RoomType.CALE];
		}

		public override void Update (GameTime gameTime)
		{
			bool missingPlayer = false;
			foreach(var player in _game.Players.Values)
			{
				if (player.Enabled && player.currentRoom != _target)
				{
					missingPlayer = true;
					break;
				}
			}

			if (missingPlayer)
				_game.health -= 0.0001f;
			else
				this.Enabled = false;
		}

		public override void DrawText (SpriteBatch sb)
		{
			sb.DrawString (_game.font, _text, new Vector2 (410, 50), Color.White);
		}
	}

}
