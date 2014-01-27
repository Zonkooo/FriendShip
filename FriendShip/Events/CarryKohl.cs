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

	public class CarryKohl : EventBase
	{
		private readonly Room _source;
		private readonly Room _cible;
		private int nbCarried = 0;

		public CarryKohl (GameCore game)
			:base(game, game.txtCharb)
		{
			_source = _game._rooms [RoomType.BRIDGE];
			_cible = _game._rooms [RoomType.MACHINE];
		}

		public override void Enable ()
		{
			base.Enable ();
			_source.EnableAction ();
			_cible.EnableAction ();
		}

		public override void Update (GameTime gameTime)
		{
			if (_source.Actionned)
			{
				_source.ActionnedBy.hasKohl = true;
				_source.EnableAction ();
			}

			if (_cible.Actionned && _cible.ActionnedBy.hasKohl)
			{
				_cible.ActionnedBy.hasKohl = false;
				_cible.EnableAction ();
				nbCarried++;
			}

			if(nbCarried < 9)
				_game.health -= 0.0005f;
			else
			{
				Enabled = false;
				Visible = false;
				foreach(var player in _game.Players.Values)
				{
					player.hasKohl = false;
				}
			}
		}

		public override void Draw (GameTime gameTime)
		{
			var sb = _game.spriteBatch;
			if (sb != null)
			{
				sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(_game.Scale));
				sb.Draw (_text, basePos, Color.White);
				sb.End ();
			}
		}
	}
}
