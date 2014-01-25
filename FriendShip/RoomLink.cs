using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FriendShip
{
	public class RoomLink
	{
		public Room NextRoom { get; private set; }
		private Rectangle _triggerZone;
		private Vector2 _spawPoint;
		//TODO condition on key

		public RoomLink(Room nextRoom, Rectangle trigger)
		{
			NextRoom = nextRoom;
			_triggerZone = trigger;
		}

		public bool Collides(Rectangle player)
		{
			if(player.Intersects(_triggerZone)) //&& right key pressed
			{
				return true;
			}
			return false;
		}

		public void DrawHitBox(SpriteBatch sb, Texture2D pixel)
		{
			sb.Draw(pixel, _triggerZone, new Color(255, 0, 255));
		}
	}
}
