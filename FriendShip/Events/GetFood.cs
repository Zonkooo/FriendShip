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
			:base(game, "Premier a la cantine gagne 1 pv")
		{
			_target = _game._rooms [RoomType.KITCHEN];
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
				sb.DrawString (_game.font, _text, new Vector2 (600, 20), Color.Crimson);
				sb.End ();
			}
		}
	}

	public class GetTrap : EventBase
	{
		private readonly Room _target;

		public GetTrap (GameCore game)
			:base(game, "Premier a la chambre gagne 1 bombe")
		{
			_target = _game._rooms [RoomType.CHAMBRE];
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
				Enabled = false;
				_target.ActionnedBy.nbTraps = Math.Min (_target.ActionnedBy.nbTraps + 1, 3);
			}
		}

		public override void Draw (GameTime gameTime)
		{
			var sb = _game.spriteBatch;
			if (sb != null)
			{
				sb.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend);
				sb.DrawString (_game.font, _text, new Vector2 (600, 20), Color.Crimson);
				sb.End ();
			}
		}
	}
}
