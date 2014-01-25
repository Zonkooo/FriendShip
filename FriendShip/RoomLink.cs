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
	public class RoomLink
	{
		/// <summary>
		/// true if we need to stop the player after taking this link
		/// because he would otherwise go through the room in one frame
		/// </summary>
		public bool needBreak;

		public Room NextRoom { get; private set; }
		private Rectangle _triggerZone;
		public Vector2 SpawPoint { get; private set;}
		private Direction _direction;

		public RoomLink(Room nextRoom, Rectangle trigger, Direction direction, Vector2 reappearancePoint)
		{
			NextRoom = nextRoom;
			_triggerZone = trigger;
			SpawPoint = reappearancePoint;
			_direction = direction;
		}

		public bool Collides(Rectangle player, List<Direction> directionsPressed)
		{
			if(player.Intersects(_triggerZone) && directionsPressed.Contains(_direction))
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
