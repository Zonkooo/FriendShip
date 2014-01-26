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

	public class Gui : DrawableGameComponent
	{
		GameCore _game;

		Texture2D _cookHud;
		Texture2D _capHud;
		Texture2D _fishHud;
		Texture2D _mecaHud;

		Vector2 _position;

		public Gui (GameCore game)
			:base(game)
		{
			_game = game;
			_game.Components.Add (this);
		}

		protected override void LoadContent ()
		{
			base.LoadContent ();
			_cookHud = _game.Content.Load<Texture2D> ("Interface/cuisto");
			_capHud = _game.Content.Load<Texture2D> ("Interface/cap");
			_fishHud = _game.Content.Load<Texture2D> ("Interface/pecheur");
			_mecaHud = _game.Content.Load<Texture2D> ("Interface/mecano");
		}

		public override void Update (GameTime gameTime)
		{
		}

		void Suicide ()
		{
			_game.Components.Remove (this);
		}

		public override void Draw (GameTime gameTime)
		{
			if (_game.spriteBatch != null)
			{
				_game.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

				_game.spriteBatch.Draw(_capHud,  new Vector2(60, 4), Color.White);
				_game.spriteBatch.Draw(_mecaHud, new Vector2(1550, 4), Color.White);
				_game.spriteBatch.Draw(_fishHud, new Vector2(1550, 850), Color.White);
				_game.spriteBatch.Draw(_cookHud, new Vector2(60, 850), Color.White);

				_game.spriteBatch.End();
			}
		}
	}
}