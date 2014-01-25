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
	public class Trap
	{
		public readonly Vector2 Position ;
		public bool Enabled{ get; private set; }

		public Trap (Vector2 position)
		{
			Position = position;
		}

		public void Enable()
		{
			Enabled = true;
		}
	}
}