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

		public MyTexture2D (Texture2D tex, int nbFrames)
		{
			Texture = tex;
			_nbFrames = nbFrames;
		}

		public Rectangle GetRectangle(int i)
		{
			return new Rectangle (
				(Texture.Width / _nbFrames) * i,
				0,
				(Texture.Width / _nbFrames),
				Texture.Height);
		}
	}
}
