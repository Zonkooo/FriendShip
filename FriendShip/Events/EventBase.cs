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
	public abstract class EventBase : DrawableGameComponent
	{
		public Texture2D _text;
		protected GameCore _game;
		protected Vector2 basePos = new Vector2 (485, 60+55);

		public EventBase (GameCore game, Texture2D text)
			: base(game)
		{
			_game = game;
			_text = text;
			_game.Components.Add (this);
			Enabled = false;
			Visible = false;
			DrawOrder = 150;
		}

		public virtual void Enable()
		{
			Enabled = true;
			Visible = true;
		}
	}
}
