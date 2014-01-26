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

	public class MustDriveShip : EventBase
	{
		private readonly Room _target;
		bool missingPlayer;

		public MustDriveShip (GameCore game, string text)
			:base(game, text)
		{
			_target = _game._rooms [RoomType.COMMANDS];
			Enabled = true; //always enabled
		}

		public override void Update (GameTime gameTime)
		{
			missingPlayer = true;
			foreach(var player in _game.Players.Values)
			{
				if (player.Enabled && player.currentRoom == _target)
				{
					missingPlayer = false;
					break;
				}
			}

			if (missingPlayer)
				_game.derive -= 0.001f;
			else
				_game.derive = Math.Min(_game.derive + 0.0005f, 1f);
		}

		public override void DrawText (SpriteBatch sb)
		{
			if(missingPlayer)
				sb.DrawString (_game.font, _text, new Vector2 (410, 100), Color.Crimson);
		}
	}
}
