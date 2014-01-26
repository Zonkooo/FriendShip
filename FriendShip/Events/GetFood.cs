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
	public class GetFood : EventBase
	{
		private readonly Room _target;

		public GetFood (GameCore game)
			:base(game, game.txtCook)
		{
			_target = _game._rooms [RoomType.KITCHEN];
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
				Visible = false;
				Enabled = false;
				_target.ActionnedBy.life = Math.Min (_target.ActionnedBy.life + 1, 3);
			}
		}

		public override void Draw (GameTime gameTime)
		{
			var sb = _game.spriteBatch;
			if (sb != null)
			{
				sb.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend);
				sb.Draw (_text, basePos - new Vector2(0, 55), Color.White);
				sb.End ();
			}
		}
	}

	public class GetTrap : EventBase
	{
		private readonly Room _target;

		public GetTrap (GameCore game)
			:base(game, game.txtDodo)
		{
			_target = _game._rooms [RoomType.CHAMBRE];
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
				Visible = false;
				Enabled = false;
				_target.ActionnedBy.nbTraps = Math.Min (_target.ActionnedBy.nbTraps + 1, 4);
			}
		}

		public override void Draw (GameTime gameTime)
		{
			var sb = _game.spriteBatch;
			if (sb != null)
			{
				sb.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend);
				sb.Draw (_text, basePos - new Vector2(0, 55), Color.White);
				sb.End ();
			}
		}
	}
}
