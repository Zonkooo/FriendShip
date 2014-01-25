using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FriendShip
{

	public static class InitHelper
	{
		public static void InitRooms(GameCore game, Dictionary<RoomType, Room> rooms)
		{
			rooms[RoomType.COMMANDS] = new Room(game, new Vector2(253,  211), new Vector2(427,290),   RoomMovementType.HORIZONTAL);
			rooms[RoomType.HALL_1]   = new Room(game, new Vector2(601,  284), new Vector2(), 		   RoomMovementType.HORIZONTAL);
			rooms[RoomType.KITCHEN]  = new Room(game, new Vector2(819,  211), new Vector2(), 		   RoomMovementType.HORIZONTAL);
			rooms[RoomType.HALL_2]   = new Room(game, new Vector2(1167, 284), new Vector2(), 		   RoomMovementType.HORIZONTAL);
			rooms[RoomType.BRIDGE]   = new Room(game, new Vector2(1385, 211), new Vector2(1600, 348), RoomMovementType.HORIZONTAL);
			//Etage 2
			rooms[RoomType.LADDER_1] = new Room(game, new Vector2(393,  415), new Vector2(), RoomMovementType.VERTICAL);
			rooms[RoomType.HALL_3]   = new Room(game, new Vector2(494,  443), new Vector2(), RoomMovementType.HORIZONTAL);
			rooms[RoomType.LADDER_2] = new Room(game, new Vector2(667,  415), new Vector2(), RoomMovementType.VERTICAL);
			rooms[RoomType.HALL_4]   = new Room(game, new Vector2(766,  443), new Vector2(), RoomMovementType.HORIZONTAL);
			rooms[RoomType.LADDER_3] = new Room(game, new Vector2(960,  415), new Vector2(), RoomMovementType.VERTICAL);
			rooms[RoomType.HALL_5]   = new Room(game, new Vector2(1061, 443), new Vector2(), RoomMovementType.HORIZONTAL);
			rooms[RoomType.LADDER_4] = new Room(game, new Vector2(1267, 415), new Vector2(), RoomMovementType.VERTICAL);
			rooms[RoomType.HALL_6]   = new Room(game, new Vector2(1368, 443), new Vector2(), RoomMovementType.HORIZONTAL);
			rooms[RoomType.LADDER_5] = new Room(game, new Vector2(1597, 415), new Vector2(), RoomMovementType.VERTICAL);
			//Etage 3
			rooms[RoomType.CALE]    = new Room(game, new Vector2(253,  590),      new Vector2(427,290),   RoomMovementType.HORIZONTAL);
			rooms[RoomType.HALL_7]  = new Room(game, new Vector2(601,  590 + 74), new Vector2(), 		   RoomMovementType.HORIZONTAL);
			rooms[RoomType.CHAMBRE] = new Room(game, new Vector2(819,  590), 	   new Vector2(), 		   RoomMovementType.HORIZONTAL);
			rooms[RoomType.HALL_8]  = new Room(game, new Vector2(1167, 590 + 74), new Vector2(), 		   RoomMovementType.HORIZONTAL);
			rooms[RoomType.MACHINE] = new Room(game, new Vector2(1385, 590), 	   new Vector2(1600, 348), RoomMovementType.HORIZONTAL);

			rooms[RoomType.COMMANDS]	.Exits.Add(new RoomLink(rooms[RoomType.HALL_1], 	new Rectangle(600,  211, 1,   200), Direction.RIGHT, new Vector2(605,  290)));
			rooms[RoomType.HALL_1]		.Exits.Add(new RoomLink(rooms[RoomType.COMMANDS], 	new Rectangle(603,  211, 1,   200), Direction.LEFT,  new Vector2(560,  290)));
			rooms[RoomType.HALL_1]		.Exits.Add(new RoomLink(rooms[RoomType.KITCHEN], 	new Rectangle(817,  211, 1,   200), Direction.RIGHT, new Vector2(825,  290)));
			rooms[RoomType.KITCHEN]		.Exits.Add(new RoomLink(rooms[RoomType.HALL_1], 	new Rectangle(820,  211, 1,   200), Direction.LEFT,  new Vector2(750,  290)));
			rooms[RoomType.KITCHEN]		.Exits.Add(new RoomLink(rooms[RoomType.HALL_2], 	new Rectangle(1165, 211, 1,   200), Direction.RIGHT, new Vector2(1172, 290)));
			rooms[RoomType.HALL_2]		.Exits.Add(new RoomLink(rooms[RoomType.KITCHEN], 	new Rectangle(1168, 211, 1,   200), Direction.LEFT,  new Vector2(1120, 290)));
			rooms[RoomType.HALL_2]		.Exits.Add(new RoomLink(rooms[RoomType.BRIDGE], 	new Rectangle(1385, 211, 1,   200), Direction.RIGHT, new Vector2(1395, 290)));
			rooms[RoomType.BRIDGE]		.Exits.Add(new RoomLink(rooms[RoomType.HALL_2], 	new Rectangle(1388, 211, 1,   200), Direction.LEFT,  new Vector2(1330, 290)));
			rooms[RoomType.HALL_1]		.Exits.Add(new RoomLink(rooms[RoomType.LADDER_2], 	new Rectangle(672,  390, 100, 1),   Direction.DOWN,  new Vector2(680,  450)));
			rooms[RoomType.LADDER_2]	.Exits.Add(new RoomLink(rooms[RoomType.HALL_1], 	new Rectangle(672,  410, 100, 1),   Direction.UP,    new Vector2(680,  295)));
			rooms[RoomType.COMMANDS]	.Exits.Add(new RoomLink(rooms[RoomType.LADDER_1], 	new Rectangle(393,  390, 100, 1),   Direction.DOWN,  new Vector2(420,  410)));
			rooms[RoomType.LADDER_1]	.Exits.Add(new RoomLink(rooms[RoomType.COMMANDS], 	new Rectangle(393,  393, 100, 1),   Direction.UP,    new Vector2(420,  290)));
			rooms[RoomType.LADDER_1]	.Exits.Add(new RoomLink(rooms[RoomType.HALL_3], 	new Rectangle(446,  443, 1,   130), Direction.RIGHT, new Vector2(500,  445)));
			rooms[RoomType.HALL_3]		.Exits.Add(new RoomLink(rooms[RoomType.LADDER_1], 	new Rectangle(495,  443, 1,   130), Direction.LEFT,  new Vector2(420,  443)){needBreak = true});
			rooms[RoomType.LADDER_1]	.Exits.Add(new RoomLink(rooms[RoomType.CALE], 		new Rectangle(393,  595, 100, 1),   Direction.DOWN,  new Vector2(393,  671)));
			rooms[RoomType.CALE]		.Exits.Add(new RoomLink(rooms[RoomType.LADDER_1], 	new Rectangle(393,  770, 100, 1),   Direction.UP,    new Vector2(420,  470)));
			rooms[RoomType.HALL_3]		.Exits.Add(new RoomLink(rooms[RoomType.LADDER_2], 	new Rectangle(663,  443, 1,   130), Direction.RIGHT, new Vector2(680,  443)){needBreak = true});
			rooms[RoomType.LADDER_2]	.Exits.Add(new RoomLink(rooms[RoomType.HALL_3], 	new Rectangle(682,  443, 1,   130), Direction.LEFT,  new Vector2(600,  443)));
			rooms[RoomType.LADDER_2]	.Exits.Add(new RoomLink(rooms[RoomType.HALL_4], 	new Rectangle(745,  443, 1,   130), Direction.RIGHT, new Vector2(782,  443)));
			rooms[RoomType.HALL_4]		.Exits.Add(new RoomLink(rooms[RoomType.LADDER_2], 	new Rectangle(770,  443, 1,   130), Direction.LEFT,  new Vector2(680,  443)){needBreak = true});
			rooms[RoomType.CALE]		.Exits.Add(new RoomLink(rooms[RoomType.HALL_7], 	new Rectangle(597,  670, 1,   130), Direction.RIGHT, new Vector2(620,  670)));
			rooms[RoomType.HALL_7]		.Exits.Add(new RoomLink(rooms[RoomType.CALE], 		new Rectangle(605,  670, 1,   130), Direction.LEFT,  new Vector2(530,  670)));
			rooms[RoomType.HALL_7]		.Exits.Add(new RoomLink(rooms[RoomType.CHAMBRE], 	new Rectangle(815,  670, 1,   130), Direction.RIGHT, new Vector2(830,  670)));
			rooms[RoomType.CHAMBRE]		.Exits.Add(new RoomLink(rooms[RoomType.HALL_7], 	new Rectangle(820,  670, 1,   130), Direction.LEFT,  new Vector2(750,  670)));
			rooms[RoomType.CHAMBRE]		.Exits.Add(new RoomLink(rooms[RoomType.HALL_8], 	new Rectangle(1163, 670, 1,   130), Direction.RIGHT, new Vector2(1178, 670)));
			rooms[RoomType.HALL_8]		.Exits.Add(new RoomLink(rooms[RoomType.CHAMBRE], 	new Rectangle(1168, 670, 1,   130), Direction.LEFT,  new Vector2(1080, 670)));
			rooms[RoomType.HALL_8]		.Exits.Add(new RoomLink(rooms[RoomType.MACHINE], 	new Rectangle(1381, 670, 1,   130), Direction.RIGHT, new Vector2(1410, 670)));
			rooms[RoomType.MACHINE]		.Exits.Add(new RoomLink(rooms[RoomType.HALL_8], 	new Rectangle(1386, 670, 1,   130), Direction.LEFT,  new Vector2(1290, 670)));
			rooms[RoomType.HALL_7]		.Exits.Add(new RoomLink(rooms[RoomType.LADDER_2], 	new Rectangle(672,  770, 100, 1),   Direction.UP,    new Vector2(672,  500)));
			rooms[RoomType.LADDER_2]	.Exits.Add(new RoomLink(rooms[RoomType.HALL_7], 	new Rectangle(672,  650, 100, 1),   Direction.DOWN,  new Vector2(672,  671)));
			rooms[RoomType.HALL_4]		.Exits.Add(new RoomLink(rooms[RoomType.LADDER_3], 	new Rectangle(960,  443, 1,   130), Direction.RIGHT, new Vector2(980,  443)) { needBreak = true });
			rooms[RoomType.LADDER_3]	.Exits.Add(new RoomLink(rooms[RoomType.HALL_4], 	new Rectangle(980,  443, 1,   130), Direction.LEFT,  new Vector2(890,  443)) { needBreak = true });
			rooms[RoomType.LADDER_3]	.Exits.Add(new RoomLink(rooms[RoomType.KITCHEN], 	new Rectangle(960,  410, 100, 1),   Direction.UP,    new Vector2(980,  295)));
			rooms[RoomType.KITCHEN]		.Exits.Add(new RoomLink(rooms[RoomType.LADDER_3], 	new Rectangle(960,  390, 100, 1),   Direction.DOWN,  new Vector2(980,  443)) { needBreak = true });
			rooms[RoomType.CHAMBRE]		.Exits.Add(new RoomLink(rooms[RoomType.LADDER_3], 	new Rectangle(960,  770, 100, 1),   Direction.UP,    new Vector2(980,  443)) { needBreak = true });
			rooms[RoomType.LADDER_3]	.Exits.Add(new RoomLink(rooms[RoomType.CHAMBRE], 	new Rectangle(960,  580, 100, 1),   Direction.DOWN,  new Vector2(980,  671)));
			rooms[RoomType.LADDER_3]	.Exits.Add(new RoomLink(rooms[RoomType.HALL_5], 	new Rectangle(1020, 443, 1,   130), Direction.RIGHT, new Vector2(1087, 443)) { needBreak = true });
			rooms[RoomType.HALL_5]		.Exits.Add(new RoomLink(rooms[RoomType.LADDER_3], 	new Rectangle(1050, 443, 1,   130), Direction.LEFT,  new Vector2(980,  443)) { needBreak = true });
			rooms[RoomType.HALL_5]		.Exits.Add(new RoomLink(rooms[RoomType.LADDER_4], 	new Rectangle(1260, 443, 1,   130), Direction.RIGHT, new Vector2(1290, 443)) { needBreak = true });
			rooms[RoomType.LADDER_4]	.Exits.Add(new RoomLink(rooms[RoomType.HALL_5], 	new Rectangle(1295, 443, 1,   130), Direction.LEFT,  new Vector2(1200, 443)) { needBreak = true });
			rooms[RoomType.LADDER_4]	.Exits.Add(new RoomLink(rooms[RoomType.HALL_6], 	new Rectangle(1320, 443, 1,   130), Direction.RIGHT, new Vector2(1370, 443)) { needBreak = true });
			rooms[RoomType.HALL_6]		.Exits.Add(new RoomLink(rooms[RoomType.LADDER_4], 	new Rectangle(1360, 443, 1,   130), Direction.LEFT,  new Vector2(1290, 443)) { needBreak = true });
			rooms[RoomType.HALL_2]		.Exits.Add(new RoomLink(rooms[RoomType.LADDER_4], 	new Rectangle(1270, 390, 100, 1),   Direction.DOWN,  new Vector2(1290, 443)));
			rooms[RoomType.LADDER_4]	.Exits.Add(new RoomLink(rooms[RoomType.HALL_2], 	new Rectangle(1270, 410, 100, 1),   Direction.UP,    new Vector2(1290, 295)));
			rooms[RoomType.HALL_8]		.Exits.Add(new RoomLink(rooms[RoomType.LADDER_4], 	new Rectangle(1270, 770, 100, 1),   Direction.UP,    new Vector2(1270, 500)));
			rooms[RoomType.LADDER_4]	.Exits.Add(new RoomLink(rooms[RoomType.HALL_8], 	new Rectangle(1270, 650, 100, 1),   Direction.DOWN,  new Vector2(1270, 671)));
			rooms[RoomType.BRIDGE]		.Exits.Add(new RoomLink(rooms[RoomType.LADDER_5], 	new Rectangle(1600, 390, 100, 1),   Direction.DOWN,  new Vector2(1600, 410)));
			rooms[RoomType.LADDER_5]	.Exits.Add(new RoomLink(rooms[RoomType.BRIDGE], 	new Rectangle(1600, 410, 100, 1),   Direction.UP,    new Vector2(1600, 290)));
			rooms[RoomType.LADDER_5]	.Exits.Add(new RoomLink(rooms[RoomType.MACHINE], 	new Rectangle(1600, 580, 100, 1),   Direction.DOWN,  new Vector2(1600, 671)));
			rooms[RoomType.MACHINE]		.Exits.Add(new RoomLink(rooms[RoomType.LADDER_5], 	new Rectangle(1600, 770, 100, 1),   Direction.UP,    new Vector2(1600, 470)));
			rooms[RoomType.LADDER_5]	.Exits.Add(new RoomLink(rooms[RoomType.HALL_6], 	new Rectangle(1600, 443, 1,   130), Direction.LEFT,  new Vector2(1500, 443)));
			rooms[RoomType.HALL_6]		.Exits.Add(new RoomLink(rooms[RoomType.LADDER_5], 	new Rectangle(1590, 443, 1,   130), Direction.RIGHT, new Vector2(1600, 443)) { needBreak = true });

		}

		public static void LoadAndSetRoomTextures(Dictionary<RoomType, Room> rooms, ContentManager content)
		{
			var machines = content.Load<Texture2D>("Rooms/machines");
			var pilotage = content.Load<Texture2D>("Rooms/pilotage");
			var cuisine = content.Load<Texture2D>("Rooms/cuisine");
			var cale = content.Load<Texture2D>("Rooms/entrepot");
			var chambre = content.Load<Texture2D>("Rooms/chambre");
			var echelleMid = content.Load<Texture2D>("Rooms/echelle_court_milieu");
			var echelleLeft = content.Load<Texture2D>("Rooms/echelle_court_gauche");
			var echelleRight = content.Load<Texture2D>("Rooms/echelle_court_droite");
			var echelleLong = content.Load<Texture2D>("Rooms/echelle_long");

			rooms [RoomType.COMMANDS].Texture = pilotage;
			rooms[RoomType.MACHINE].Texture = machines;
			rooms[RoomType.BRIDGE].Texture = cuisine;
			rooms[RoomType.KITCHEN].Texture = cuisine;
			rooms[RoomType.CALE].Texture = cale;
			rooms[RoomType.CHAMBRE].Texture = chambre;

			rooms [RoomType.HALL_1].Texture = content.Load<Texture2D> ("Rooms/couloir_haut_gauche");
			rooms[RoomType.HALL_2].Texture = content.Load<Texture2D> ("Rooms/couloir_haut_droite");
			rooms[RoomType.HALL_3].Texture = content.Load<Texture2D> ("Rooms/couloir_m1");
			rooms[RoomType.HALL_4].Texture = content.Load<Texture2D> ("Rooms/couloir_m2");
			rooms[RoomType.HALL_5].Texture = content.Load<Texture2D> ("Rooms/couloir_m3");
			rooms[RoomType.HALL_6].Texture = content.Load<Texture2D> ("Rooms/couloir_m4");
			rooms[RoomType.HALL_7].Texture = content.Load<Texture2D> ("Rooms/couloir_bas_droite");
			rooms[RoomType.HALL_8].Texture = content.Load<Texture2D> ("Rooms/couloir_bas_gauche");

			rooms [RoomType.LADDER_1].Texture = echelleLeft;
			rooms [RoomType.LADDER_3].Texture = echelleMid;
			rooms [RoomType.LADDER_5].Texture = echelleRight;
			rooms [RoomType.LADDER_2].Texture = echelleLong;
			rooms [RoomType.LADDER_4].Texture = echelleLong;
		}
	}
}
