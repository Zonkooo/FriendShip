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

		Texture2D _cookHudDead;
		Texture2D  _capHudDead;
		Texture2D _fishHudDead;
		Texture2D _mecaHudDead;

		Texture2D _heart;
		Texture2D _bomb;

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

			_cookHudDead = _game.Content.Load<Texture2D> ( "Interface/cuisto_dead");
			_capHudDead = _game.Content.Load<Texture2D> (     "Interface/cap_dead");
			_fishHudDead = _game.Content.Load<Texture2D> ("Interface/pecheur_dead");
			_mecaHudDead = _game.Content.Load<Texture2D> ( "Interface/mecano_dead");

			_heart = _game.Content.Load<Texture2D> ("Interface/coeur");
			_bomb = _game.Content.Load<Texture2D> ("Interface/bombe");
		}

		public override void Update (GameTime gameTime)
		{
			if (!_game.Players [PlayerType.CAP].Enabled)
				_capHud = _capHudDead;
			if (!_game.Players [PlayerType.COOK].Enabled)
				_cookHud = _cookHudDead;
			if (!_game.Players [PlayerType.MECA].Enabled)
				_mecaHud = _mecaHudDead;
			if (!_game.Players [PlayerType.FISH].Enabled)
				_fishHud = _fishHudDead;
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

				var posCap = new Vector2 (60, 4);
				var posMeca = new Vector2 (1550, 4);
				var posFish = new Vector2 (1550, 850);
				var posCook = new Vector2 (60, 850);

				_game.spriteBatch.Draw (_capHud, posCap, Color.White);
				_game.spriteBatch.Draw (_mecaHud, posMeca, Color.White);
				_game.spriteBatch.Draw (_fishHud, posFish, Color.White);
				_game.spriteBatch.Draw(_cookHud, posCook, Color.White);

				var yHeartOffset = 110;
				var xHeartSpacing = 45;
				var xHBombSpacing = 30;
				var xOffsetBig = 150;
				var xOffsetSmall = 26;
				var yOffsetBomb = 26;

				//player1
				var cap = _game.Players [PlayerType.CAP];
				for (int i = 0; i < cap.life; i++)
					_game.spriteBatch.Draw(_heart, posCap + new Vector2(xOffsetBig + xHeartSpacing*i, yHeartOffset), Color.White);
				for (int i = 0; i < cap.nbTraps; i++)
					_game.spriteBatch.Draw(_bomb, posCap + new Vector2(xOffsetBig + xHBombSpacing*i - 10, yHeartOffset + yOffsetBomb), Color.White);

				//player2
				var meca = _game.Players [PlayerType.MECA];
				for (int i = 0; i < meca.life; i++)
					_game.spriteBatch.Draw(_heart, posMeca + new Vector2(xOffsetSmall + xHeartSpacing*(2-i), yHeartOffset), Color.White);
				for (int i = 0; i < meca.nbTraps; i++)
					_game.spriteBatch.Draw(_bomb, posMeca + new Vector2(30 + xOffsetSmall + xHBombSpacing*(2-i) + 30, yHeartOffset + yOffsetBomb), Color.White);

				//player3
				var cook = _game.Players [PlayerType.COOK];
				for (int i = 0; i < cook.life; i++)
					_game.spriteBatch.Draw(_heart, posCook + new Vector2(xOffsetBig + xHeartSpacing*i, yHeartOffset), Color.White);
				for (int i = 0; i < cook.nbTraps; i++)
					_game.spriteBatch.Draw(_bomb, posCook + new Vector2(xOffsetBig + xHBombSpacing*i - 10, yHeartOffset + yOffsetBomb), Color.White);

				//player4
				var fish = _game.Players [PlayerType.FISH];
				for (int i = 0; i < fish.life; i++)
					_game.spriteBatch.Draw(_heart, posFish + new Vector2(xOffsetSmall + xHeartSpacing*(2-i), yHeartOffset), Color.White);
				for (int i = 0; i < fish.nbTraps; i++)
					_game.spriteBatch.Draw(_bomb, posFish + new Vector2(30 + xOffsetSmall + xHBombSpacing*(2-i) + 30, yHeartOffset + yOffsetBomb), Color.White);

				_game.spriteBatch.End();
			}
		}
	}
}