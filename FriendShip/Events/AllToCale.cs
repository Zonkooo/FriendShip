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
			_game.leak.Update (gameTime.ElapsedGameTime.TotalMilliseconds);
			if (missingPlayer)
				_game.health -= 0.0003f;
			else
			{
				this.Enabled = false;
				this.Visible = false;
			}
		}

		public override void Draw (GameTime gameTime)
		{
			var sb = _game.spriteBatch;
			if (sb != null)
			{
				sb.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend);
				sb.DrawString (_game.font, _text, new Vector2 (410, 50), Color.White);
				sb.Draw (_game.leak.Texture, new Vector2 (330, 670), _game.leak.GetRectangle (), Color.White);
				sb.End ();
			}
		}
	}
}
