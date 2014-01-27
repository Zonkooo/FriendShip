using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;

namespace FriendShip
{

	public class TemporaryEffect : DrawableGameComponent
	{
		GameCore _game;
		MyTexture2D _tex;
		double _lifeTime;
		Vector2 _position;

		public TemporaryEffect (GameCore game, Vector2 position, MyTexture2D tex, double lifeTime)
			:base(game)
		{
			this._position = position;
			this._lifeTime = lifeTime;
			this._tex = tex;
			_game = game;
			_game.Components.Add (this);
			DrawOrder = 1000;
		}

		public override void Update (GameTime gameTime)
		{
			_lifeTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
			if (_lifeTime < 0)
				Suicide ();
			_tex.Update (gameTime.ElapsedGameTime.TotalMilliseconds);
		}

		void Suicide ()
		{
			_game.Components.Remove (this);
		}

		public override void Draw (GameTime gameTime)
		{
			if (_game.spriteBatch != null)
			{
				_game.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(_game.Scale));
				_game.spriteBatch.Draw(_tex.Texture, _position, _tex.GetRectangle(), Color.White);
				_game.spriteBatch.End();
			}
		}
	}
}
