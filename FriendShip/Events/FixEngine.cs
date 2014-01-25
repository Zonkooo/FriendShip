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
		bool isFixed = false;

		public FixEngine (GameCore game, string text)
			:base(game, text)
		{
			_target = _game._rooms [RoomType.MACHINE];
			Enabled = false;
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
				isFixed = true;
				Enabled = false;
			}
		}

		public override void DrawText (SpriteBatch sb)
		{
//				sb.DrawString (_game.font, _text, new Vector2 (410, 100), Color.Crimson);
		}
	}
}
