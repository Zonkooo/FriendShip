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
	public class MyTexture2D
	{
		public readonly Texture2D Texture;
		private int _nbFrames;
		private double[] _timings;
		private int index = 0;
		private double currentTime;

		public MyTexture2D (Texture2D tex, int nbFrames, double[] timings = null)
		{
			_timings = timings;
			Texture = tex;
			_nbFrames = nbFrames;
		}

		public void Update(double time)
		{
			if (_timings == null)
				return;

			currentTime -= time;
			if(currentTime < 0)
			{
				index = (index + 1)%_nbFrames;
				currentTime = _timings [index] - currentTime;
			}
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
