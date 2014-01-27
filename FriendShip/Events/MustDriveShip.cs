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

		public MustDriveShip (GameCore game)
			:base(game, game.txtPilot)
		{
			_target = _game._rooms [RoomType.COMMANDS];
			Visible = true;
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

		public override void Draw (GameTime gameTime)
		{
			var sb = _game.spriteBatch;
			if (sb != null && missingPlayer)
			{
				sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(_game.Scale));
				sb.Draw (_text, basePos - new Vector2(0, 110), Color.White);
				sb.End ();
			}
		}
	}
}
