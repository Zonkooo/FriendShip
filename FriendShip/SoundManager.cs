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

	public class SoundManager : GameComponent
	{
		Song music;

		public SoundManager (Game game)
			: base(game)
		{
			music = game.Content.Load<Song> ("theme");
		}

		public void Play()
		{
			MediaPlayer.Play (music);
		}
	}
}