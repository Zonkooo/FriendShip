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

	public class FixEngine : EventBase
	{
		private readonly Room _target;

		public FixEngine (GameCore game)
			:base(game, game.txtSdm)
		{
			_target = _game._rooms [RoomType.MACHINE];
		}

		public override void Enable ()
		{
			base.Enable ();
			_target.EnableAction ();
		}

		public override void Update (GameTime gameTime)
		{
			if (_target.Actionned)
			{
				Enabled = false;
				Visible = false;
			}
			else
			{
				_game.health -= 0.0005f;
			}
		}

		public override void Draw (GameTime gameTime)
		{
			var sb = _game.spriteBatch;
			if (sb != null)
			{
				sb.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend);
				sb.Draw (_text, basePos, Color.White);
				sb.End ();
			}
		}
	}
}
