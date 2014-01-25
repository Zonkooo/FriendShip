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
	public class MyTexture2D
	{
		public readonly Texture2D Texture;
		private int _nbFrames;
		private double[] _timings;
		private int index;
		private double currentTime;

		public MyTexture2D (Texture2D tex, int nbFrames, double[] timings = null)
		{
			_timings = timings;
			Texture = tex;
			_nbFrames = nbFrames;
			Reset ();
		}

		public void Update(double time)
		{
			if (_timings == null)
				return;

			currentTime -= time;
			if(currentTime < 0)
			{
				index = Math.Abs((index - 1)%_nbFrames); //sprites are reversed
				currentTime = _timings [index] - currentTime;
			}
		}

		public void Reset()
		{
			index = _nbFrames - 1;
			currentTime = _timings != null ? _timings [_nbFrames - 1] : 0;
		}

		public Rectangle GetRectangle()
		{
			return new Rectangle (
				(Texture.Width / _nbFrames) * index,
				0,
				(Texture.Width / _nbFrames),
				Texture.Height);
		}
	}
}
