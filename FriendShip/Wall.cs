using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FriendShip
{

	public class Wall
	{
		public readonly Rectangle _boundingBox; //TODO : private

		public Wall (Rectangle boundingBox)
		{
			_boundingBox = boundingBox;
		}

		public bool Collides(Rectangle player)
		{
			return player.Intersects (_boundingBox);
		}
	}
}
