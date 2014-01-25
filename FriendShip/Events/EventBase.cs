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
	public abstract class EventBase : GameComponent
	{
		public string _text;
		protected GameCore _game;

		public EventBase (GameCore game, string text)
			: base(game)
		{
			_game = game;
			_text = text;
			_game.Components.Add (this);
			Enabled = false;
		}

		public abstract void DrawText (SpriteBatch sb);
	}

	public class AllToCale : EventBase
	{
		private readonly Room _target;

		public AllToCale (GameCore game)
			:base(game, "Everyone in the storage room !")
		{
			_target = _game._rooms [RoomType.MACHINE];
		}

		public override void Update (GameTime gameTime)
		{
			bool missingPlayer = false;
			foreach(var player in _game.Players)
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
			foreach(var player in _game.Players)
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
