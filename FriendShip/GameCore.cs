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
	public enum RoomType
	{
		COMMANDS,
		HALL_1,
        KITCHEN,
        HALL_2,
        BRIDGE,
        //etage_2
        LADDER_1,
        HALL_3,
        LADDER_2,
        HALL_4,
        LADDER_3,
        HALL_5,
        LADDER_4,
        HALL_6,
        LADDER_5,

        //etage
        MACHINE,
        HALL_7,
        CHAMBRE,
        HALL_8,
        CALE,
	}

	enum GameEndings
	{
		EXPLODE,
		DERIVE,
		SHARE_GOLD,
		ALL_DEAD,
		WIN,
	}

	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class GameCore : Game
	{
		public GraphicsDeviceManager graphics;
		public SpriteBatch spriteBatch;
		public Texture2D OneWhitePixel;
		public Texture2D Cling;
		public SpriteFont font;
		public SoundManager SoundManager;

		public Dictionary<RoomType, Room> _rooms = new Dictionary<RoomType, Room>();
		public List<Wall> Walls = new List<Wall>();
		public List<Player> Players = new List<Player>();
		public List<EventBase> Events = new List<EventBase>();

		//ship related properties
		public float health = 1.0f;
		public float derive = 1.0f;

		public GameCore()
		{
			graphics = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferHeight = 1080,
				PreferredBackBufferWidth = 1920,
                IsFullScreen = true,
			};
            //graphics.CreateDevice();
            //graphics.ApplyChanges();'
			//IsMouseVisible = true;

			Content.RootDirectory = "Content";

			_rooms[RoomType.COMMANDS] = new Room(this, new Vector2(253,  211), new Vector2(427,290),   RoomMovementType.HORIZONTAL);
			_rooms[RoomType.HALL_1]   = new Room(this, new Vector2(601,  284), new Vector2(), 		   RoomMovementType.HORIZONTAL);
			_rooms[RoomType.KITCHEN]  = new Room(this, new Vector2(819,  211), new Vector2(), 		   RoomMovementType.HORIZONTAL);
			_rooms[RoomType.HALL_2]   = new Room(this, new Vector2(1167, 284), new Vector2(), 		   RoomMovementType.HORIZONTAL);
			_rooms[RoomType.BRIDGE]   = new Room(this, new Vector2(1385, 211), new Vector2(1600, 348), RoomMovementType.HORIZONTAL);
            //Etage 2
			_rooms[RoomType.LADDER_1] = new Room(this, new Vector2(393,  415), new Vector2(), RoomMovementType.VERTICAL);
			_rooms[RoomType.HALL_3]   = new Room(this, new Vector2(494,  443), new Vector2(), RoomMovementType.HORIZONTAL);
			_rooms[RoomType.LADDER_2] = new Room(this, new Vector2(667,  415), new Vector2(), RoomMovementType.VERTICAL);
			_rooms[RoomType.HALL_4]   = new Room(this, new Vector2(766,  443), new Vector2(), RoomMovementType.HORIZONTAL);
			_rooms[RoomType.LADDER_3] = new Room(this, new Vector2(960,  415), new Vector2(), RoomMovementType.VERTICAL);
			_rooms[RoomType.HALL_5]   = new Room(this, new Vector2(1061, 443), new Vector2(), RoomMovementType.HORIZONTAL);
            _rooms[RoomType.LADDER_4] = new Room(this, new Vector2(1267, 415), new Vector2(), RoomMovementType.VERTICAL);
			_rooms[RoomType.HALL_6]   = new Room(this, new Vector2(1368, 443), new Vector2(), RoomMovementType.HORIZONTAL);
            _rooms[RoomType.LADDER_5] = new Room(this, new Vector2(1597, 415), new Vector2(), RoomMovementType.VERTICAL);
            //Etage 3
			_rooms[RoomType.CALE]    = new Room(this, new Vector2(253,  590),      new Vector2(427,290),   RoomMovementType.HORIZONTAL);
			_rooms[RoomType.HALL_7]  = new Room(this, new Vector2(601,  590 + 74), new Vector2(), 		   RoomMovementType.HORIZONTAL);
			_rooms[RoomType.CHAMBRE] = new Room(this, new Vector2(819,  590), 	   new Vector2(), 		   RoomMovementType.HORIZONTAL);
			_rooms[RoomType.HALL_8]  = new Room(this, new Vector2(1167, 590 + 74), new Vector2(), 		   RoomMovementType.HORIZONTAL);
			_rooms[RoomType.MACHINE] = new Room(this, new Vector2(1385, 590), 	   new Vector2(1600, 348), RoomMovementType.HORIZONTAL);

			_rooms[RoomType.COMMANDS]	.Exits.Add(new RoomLink(_rooms[RoomType.HALL_1], 	new Rectangle(600,  211, 1,   200), Direction.RIGHT, new Vector2(605,  290)));
			_rooms[RoomType.HALL_1]		.Exits.Add(new RoomLink(_rooms[RoomType.COMMANDS], 	new Rectangle(603,  211, 1,   200), Direction.LEFT,  new Vector2(560,  290)));
			_rooms[RoomType.HALL_1]		.Exits.Add(new RoomLink(_rooms[RoomType.KITCHEN], 	new Rectangle(817,  211, 1,   200), Direction.RIGHT, new Vector2(825,  290)));
			_rooms[RoomType.KITCHEN]	.Exits.Add(new RoomLink(_rooms[RoomType.HALL_1], 	new Rectangle(820,  211, 1,   200), Direction.LEFT,  new Vector2(750,  290)));
			_rooms[RoomType.KITCHEN]	.Exits.Add(new RoomLink(_rooms[RoomType.HALL_2], 	new Rectangle(1165, 211, 1,   200), Direction.RIGHT, new Vector2(1172, 290)));
			_rooms[RoomType.HALL_2]		.Exits.Add(new RoomLink(_rooms[RoomType.KITCHEN], 	new Rectangle(1168, 211, 1,   200), Direction.LEFT,  new Vector2(1120, 290)));
			_rooms[RoomType.HALL_2]		.Exits.Add(new RoomLink(_rooms[RoomType.BRIDGE], 	new Rectangle(1385, 211, 1,   200), Direction.RIGHT, new Vector2(1395, 290)));
			_rooms[RoomType.BRIDGE]		.Exits.Add(new RoomLink(_rooms[RoomType.HALL_2], 	new Rectangle(1388, 211, 1,   200), Direction.LEFT,  new Vector2(1330, 290)));
			_rooms[RoomType.HALL_1]		.Exits.Add(new RoomLink(_rooms[RoomType.LADDER_2], 	new Rectangle(672,  390, 100, 1),   Direction.DOWN,  new Vector2(680,  450)));
			_rooms[RoomType.LADDER_2]	.Exits.Add(new RoomLink(_rooms[RoomType.HALL_1], 	new Rectangle(672,  410, 100, 1),   Direction.UP,    new Vector2(680,  295)));
			_rooms[RoomType.COMMANDS]	.Exits.Add(new RoomLink(_rooms[RoomType.LADDER_1], 	new Rectangle(393,  390, 100, 1),   Direction.DOWN,  new Vector2(420,  410)));
			_rooms[RoomType.LADDER_1]	.Exits.Add(new RoomLink(_rooms[RoomType.COMMANDS], 	new Rectangle(393,  393, 100, 1),   Direction.UP,    new Vector2(420,  290)));
			_rooms[RoomType.LADDER_1]	.Exits.Add(new RoomLink(_rooms[RoomType.HALL_3], 	new Rectangle(446,  443, 1,   130), Direction.RIGHT, new Vector2(500,  445)));
			_rooms[RoomType.HALL_3]		.Exits.Add(new RoomLink(_rooms[RoomType.LADDER_1], 	new Rectangle(495,  443, 1,   130), Direction.LEFT,  new Vector2(420,  443)){needBreak = true});
			_rooms[RoomType.LADDER_1]	.Exits.Add(new RoomLink(_rooms[RoomType.CALE], 		new Rectangle(393,  595, 100, 1),   Direction.DOWN,  new Vector2(393,  671)));
			_rooms[RoomType.CALE]		.Exits.Add(new RoomLink(_rooms[RoomType.LADDER_1], 	new Rectangle(393,  770, 100, 1),   Direction.UP,    new Vector2(420,  470)));
			_rooms[RoomType.HALL_3]		.Exits.Add(new RoomLink(_rooms[RoomType.LADDER_2], 	new Rectangle(663,  443, 1,   130), Direction.RIGHT, new Vector2(680,  443)){needBreak = true});
			_rooms[RoomType.LADDER_2]	.Exits.Add(new RoomLink(_rooms[RoomType.HALL_3], 	new Rectangle(682,  443, 1,   130), Direction.LEFT,  new Vector2(600,  443)));
			_rooms[RoomType.LADDER_2]	.Exits.Add(new RoomLink(_rooms[RoomType.HALL_4], 	new Rectangle(745,  443, 1,   130), Direction.RIGHT, new Vector2(782,  443)));
			_rooms[RoomType.HALL_4]		.Exits.Add(new RoomLink(_rooms[RoomType.LADDER_2], 	new Rectangle(770,  443, 1,   130), Direction.LEFT,  new Vector2(672,  443)){needBreak = true});
			_rooms[RoomType.CALE]		.Exits.Add(new RoomLink(_rooms[RoomType.HALL_7], 	new Rectangle(597,  670, 1,   130), Direction.RIGHT, new Vector2(620,  670)));
			_rooms[RoomType.HALL_7]		.Exits.Add(new RoomLink(_rooms[RoomType.CALE], 		new Rectangle(605,  670, 1,   130), Direction.LEFT,  new Vector2(530,  670)));
			_rooms[RoomType.HALL_7]		.Exits.Add(new RoomLink(_rooms[RoomType.CHAMBRE], 	new Rectangle(815,  670, 1,   130), Direction.RIGHT, new Vector2(830,  670)));
			_rooms[RoomType.CHAMBRE]	.Exits.Add(new RoomLink(_rooms[RoomType.HALL_7], 	new Rectangle(820,  670, 1,   130), Direction.LEFT,  new Vector2(750,  670)));
			_rooms[RoomType.CHAMBRE]	.Exits.Add(new RoomLink(_rooms[RoomType.HALL_8], 	new Rectangle(1163, 670, 1,   130), Direction.RIGHT, new Vector2(1178, 670)));
			_rooms[RoomType.HALL_8]		.Exits.Add(new RoomLink(_rooms[RoomType.CHAMBRE], 	new Rectangle(1168, 670, 1,   130), Direction.LEFT,  new Vector2(1080, 670)));
			_rooms[RoomType.HALL_8]		.Exits.Add(new RoomLink(_rooms[RoomType.MACHINE], 	new Rectangle(1381, 670, 1,   130), Direction.RIGHT, new Vector2(1410, 670)));
			_rooms[RoomType.MACHINE]	.Exits.Add(new RoomLink(_rooms[RoomType.HALL_8], 	new Rectangle(1386, 670, 1,   130), Direction.LEFT,  new Vector2(1290, 670)));
			_rooms[RoomType.HALL_7]		.Exits.Add(new RoomLink(_rooms[RoomType.LADDER_2], 	new Rectangle(672,  770, 100, 1),   Direction.UP,    new Vector2(672,  500)));
			_rooms[RoomType.LADDER_2]	.Exits.Add(new RoomLink(_rooms[RoomType.HALL_7], 	new Rectangle(672,  650, 100, 1),   Direction.DOWN,  new Vector2(672,  671)));
			_rooms[RoomType.HALL_4]		.Exits.Add(new RoomLink(_rooms[RoomType.LADDER_3], 	new Rectangle(960,  443, 1,   130), Direction.RIGHT, new Vector2(980,  443)) { needBreak = true });
			_rooms[RoomType.LADDER_3]	.Exits.Add(new RoomLink(_rooms[RoomType.HALL_4],	new Rectangle(980,  443, 1,   130), Direction.LEFT,  new Vector2(890,  443)) { needBreak = true });
			_rooms[RoomType.LADDER_3]	.Exits.Add(new RoomLink(_rooms[RoomType.KITCHEN], 	new Rectangle(960,  410, 100, 1),   Direction.UP,    new Vector2(980,  295)));
			_rooms[RoomType.KITCHEN]	.Exits.Add(new RoomLink(_rooms[RoomType.LADDER_3], 	new Rectangle(960,  390, 100, 1),   Direction.DOWN,  new Vector2(980,  443)) { needBreak = true });
			_rooms[RoomType.CHAMBRE]	.Exits.Add(new RoomLink(_rooms[RoomType.LADDER_3], 	new Rectangle(960,  770, 100, 1),   Direction.UP,    new Vector2(980,  443)) { needBreak = true });
			_rooms[RoomType.LADDER_3]	.Exits.Add(new RoomLink(_rooms[RoomType.CHAMBRE], 	new Rectangle(960,  580, 100, 1),   Direction.DOWN,  new Vector2(980,  671)));
			_rooms[RoomType.LADDER_3]	.Exits.Add(new RoomLink(_rooms[RoomType.HALL_5], 	new Rectangle(1020, 443, 1,   130), Direction.RIGHT, new Vector2(1087, 443)) { needBreak = true });
			_rooms[RoomType.HALL_5]		.Exits.Add(new RoomLink(_rooms[RoomType.LADDER_3], 	new Rectangle(1050, 443, 1,   130), Direction.LEFT,  new Vector2(980,  443)) { needBreak = true });
			_rooms[RoomType.HALL_5]		.Exits.Add(new RoomLink(_rooms[RoomType.LADDER_4], 	new Rectangle(1260, 443, 1,   130), Direction.RIGHT, new Vector2(1290, 443)) { needBreak = true });
			_rooms[RoomType.LADDER_4]	.Exits.Add(new RoomLink(_rooms[RoomType.HALL_5], 	new Rectangle(1295, 443, 1,   130), Direction.LEFT,  new Vector2(1200, 443)) { needBreak = true });
			_rooms[RoomType.LADDER_4]	.Exits.Add(new RoomLink(_rooms[RoomType.HALL_6], 	new Rectangle(1320, 443, 1,   130), Direction.RIGHT, new Vector2(1370, 443)) { needBreak = true });
			_rooms[RoomType.HALL_6]		.Exits.Add(new RoomLink(_rooms[RoomType.LADDER_4], 	new Rectangle(1360, 443, 1,   130), Direction.LEFT,  new Vector2(1290, 443)) { needBreak = true });
			_rooms[RoomType.HALL_2]		.Exits.Add(new RoomLink(_rooms[RoomType.LADDER_4], 	new Rectangle(1270, 390, 100, 1),   Direction.DOWN,  new Vector2(1290, 443)));
			_rooms[RoomType.LADDER_4]	.Exits.Add(new RoomLink(_rooms[RoomType.HALL_2], 	new Rectangle(1270, 410, 100, 1),   Direction.UP,    new Vector2(1290, 295)));
			_rooms[RoomType.HALL_8]		.Exits.Add(new RoomLink(_rooms[RoomType.LADDER_4], 	new Rectangle(1270, 770, 100, 1),   Direction.UP,    new Vector2(1270, 500)));
			_rooms[RoomType.LADDER_4]	.Exits.Add(new RoomLink(_rooms[RoomType.HALL_8], 	new Rectangle(1270, 650, 100, 1),   Direction.DOWN,  new Vector2(1270, 671)));
			_rooms[RoomType.BRIDGE]		.Exits.Add(new RoomLink(_rooms[RoomType.LADDER_5], 	new Rectangle(1600, 390, 100, 1),   Direction.DOWN,  new Vector2(1600, 410)));
			_rooms[RoomType.LADDER_5]	.Exits.Add(new RoomLink(_rooms[RoomType.BRIDGE], 	new Rectangle(1600, 410, 100, 1),   Direction.UP,    new Vector2(1600, 290)));
			_rooms[RoomType.LADDER_5]	.Exits.Add(new RoomLink(_rooms[RoomType.MACHINE], 	new Rectangle(1600, 580, 100, 1),   Direction.DOWN,  new Vector2(1600, 671)));
			_rooms[RoomType.MACHINE]	.Exits.Add(new RoomLink(_rooms[RoomType.LADDER_5], 	new Rectangle(1600, 770, 100, 1),   Direction.UP,    new Vector2(1600, 470)));
			_rooms[RoomType.LADDER_5]	.Exits.Add(new RoomLink(_rooms[RoomType.HALL_6], 	new Rectangle(1600, 443, 1,   130), Direction.LEFT,  new Vector2(1500, 443)));
			_rooms[RoomType.HALL_6]		.Exits.Add(new RoomLink(_rooms[RoomType.LADDER_5], 	new Rectangle(1590, 443, 1,   130), Direction.RIGHT, new Vector2(1600, 443)) { needBreak = true });

          

          




        


			Walls.Add (new Wall (new Rectangle (253/*that's the only important thing*/, 0, 1, 1080)));
			Walls.Add (new Wall (new Rectangle (1732/*that's the only important thing*/, 0, 1, 1080)));

			Events.Add (new MustDriveShip (this, "Someone must drive this ship !"));
			Events.Add (new AllToCale (this));
//			Events [0].Enabled = true;
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			SoundManager = new SoundManager (this);

			OneWhitePixel = Content.Load<Texture2D>("onewhitepixel");
			Cling = Content.Load<Texture2D>("clig");
			font = Content.Load<SpriteFont>("font");

			var machines = Content.Load<Texture2D>("Rooms/machines");
			var pilotage = Content.Load<Texture2D>("Rooms/pilotage");
			var cuisine = Content.Load<Texture2D>("Rooms/cuisine");
			var cale = Content.Load<Texture2D>("Rooms/entrepot");
			var chambre = Content.Load<Texture2D>("Rooms/chambre");
			var echelleMid = Content.Load<Texture2D>("Rooms/echelle_court_milieu");
			var echelleLeft = Content.Load<Texture2D>("Rooms/echelle_court_gauche");
			var echelleRight = Content.Load<Texture2D>("Rooms/echelle_court_droite");
			var echelleLong = Content.Load<Texture2D>("Rooms/echelle_long");

			_rooms [RoomType.COMMANDS].Texture = pilotage;
			_rooms[RoomType.MACHINE].Texture = machines;
			_rooms[RoomType.BRIDGE].Texture = cuisine;
			_rooms[RoomType.KITCHEN].Texture = cuisine;
			_rooms[RoomType.CALE].Texture = cale;
			_rooms[RoomType.CHAMBRE].Texture = chambre;

			_rooms [RoomType.HALL_1].Texture = Content.Load<Texture2D> ("Rooms/couloir_haut_gauche");
			_rooms[RoomType.HALL_2].Texture = Content.Load<Texture2D> ("Rooms/couloir_haut_droite");
			_rooms[RoomType.HALL_3].Texture = Content.Load<Texture2D> ("Rooms/couloir_m1");
			_rooms[RoomType.HALL_4].Texture = Content.Load<Texture2D> ("Rooms/couloir_m2");
			_rooms[RoomType.HALL_5].Texture = Content.Load<Texture2D> ("Rooms/couloir_m3");
			_rooms[RoomType.HALL_6].Texture = Content.Load<Texture2D> ("Rooms/couloir_m4");
			_rooms[RoomType.HALL_7].Texture = Content.Load<Texture2D> ("Rooms/couloir_bas_droite");
			_rooms[RoomType.HALL_8].Texture = Content.Load<Texture2D> ("Rooms/couloir_bas_gauche");

			_rooms [RoomType.LADDER_1].Texture = echelleLeft;
			_rooms [RoomType.LADDER_3].Texture = echelleMid;
			_rooms [RoomType.LADDER_5].Texture = echelleRight;
			_rooms [RoomType.LADDER_2].Texture = echelleLong;
			_rooms [RoomType.LADDER_4].Texture = echelleLong;

			var cire = Content.Load<Texture2D>("Players/cireman");

			var captain = Content.Load<Texture2D>("Players/capitaine");
			var captainRun = Content.Load<Texture2D>("Players/capitaine_run");
			var captainDead = Content.Load<Texture2D>("Players/capitaine_mort");
			var captainTextures = new Dictionary<PlayerState, MyTexture2D>
			{
				{PlayerState.STILL, new MyTexture2D(captain, 1)},
				{PlayerState.WALK, new MyTexture2D(captainRun, 4, new []{1000.0/12, 1000.0/12, 1000.0/12, 1000.0/12})},
				{PlayerState.HIT, new MyTexture2D(cire, 1)},
				{PlayerState.DEAD, new MyTexture2D(captainDead, 1)},
			};
			var player1Controls = new Dictionary<Direction, Keys> {
				{ Direction.LEFT, Keys.Left },
				{ Direction.RIGHT, Keys.Right },
				{ Direction.UP, Keys.Up },
				{ Direction.DOWN, Keys.Down },
				{ Direction.TRAP, Keys.End },
				{ Direction.ACTION, Keys.PageDown },
			};

			Players.Add(new Player (this, captainTextures, _rooms[RoomType.COMMANDS], player1Controls));

			var mecano = Content.Load<Texture2D>("Players/mecano");
			var mecanoRun = Content.Load<Texture2D>("Players/mecano_run");
			var mecanoDegats = Content.Load<Texture2D>("Players/mecano_degats");
			var mecanoDead = Content.Load<Texture2D>("Players/mecano_mort");
			var mecanoTextures = new Dictionary<PlayerState, MyTexture2D>
			{
				{PlayerState.STILL, new MyTexture2D(mecano, 1)},
				{PlayerState.WALK, new MyTexture2D(mecanoRun, 4, new []{1000.0/12, 1000.0/12, 1000.0/12, 1000.0/12})},
				{PlayerState.HIT, new MyTexture2D(mecanoDegats, 9, new double[]{60, 60, 60, 60, 60, 60, 1000.0/12, 1000.0/12, 1000.0/12})},
				{PlayerState.DEAD, new MyTexture2D(mecanoDead, 1)},
			};
			var player2Controls = new Dictionary<Direction, Keys> {
				{ Direction.LEFT, Keys.A },
				{ Direction.RIGHT, Keys.D },
				{ Direction.UP, Keys.W },
				{ Direction.DOWN, Keys.S },
				{ Direction.TRAP, Keys.Q },
				{ Direction.ACTION, Keys.E },
			};
			Players.Add(new Player (this, mecanoTextures, _rooms[RoomType.BRIDGE], player2Controls));

			SoundManager.Play ();
		}

		private TimeSpan _deathCounter = TimeSpan.FromMinutes(2);

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
			{
				this.Exit();
			}

			if (ended)
				return;

			_deathCounter -= gameTime.ElapsedGameTime;

			if (health < 0) //ship explodes
				EndGame(GameEndings.EXPLODE);
			if (derive < 0) //lost
				EndGame(GameEndings.DERIVE);
			if (Players.Count (p => p.Enabled) == 0) //everyone is dead
				EndGame(GameEndings.ALL_DEAD);
			if(_deathCounter < TimeSpan.Zero)
			{
				if(Players.Count (p => p.Enabled) == 1)
					EndGame(GameEndings.WIN);
				else
					EndGame(GameEndings.SHARE_GOLD);
			}

			base.Update(gameTime);
		}

		bool ended = false;
		private void EndGame(GameEndings ending)
		{
			foreach (var component in Components)
			{
				var c = component as GameComponent;
				if (c != null)
					c.Enabled = false;
			}
			ended = true;
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Gold);

			base.Draw(gameTime);

			if(spriteBatch != null)
			{
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                spriteBatch.Draw(OneWhitePixel, new Rectangle(400, 40, (int)(health * 1000), 40), Color.IndianRed); //barre de vie
				spriteBatch.Draw(OneWhitePixel, new Rectangle(400, 90, (int)(derive*1000), 40), Color.CornflowerBlue); //barre de dérive

				//timer
				spriteBatch.DrawString(font, _deathCounter.ToString("mm\\:ss"), new Vector2(40, 40), Color.White); //barre de dérive

				foreach(var @event in Events)
				{
					if (@event.Enabled)
						@event.DrawText (spriteBatch);
				}

				foreach (var wall in Walls)
					DrawHitBox (wall._boundingBox);

				spriteBatch.End();
			}
		}

		/// <summary>
		/// Spritebatch must be initialized (begin) before
		/// </summary>
		public void DrawHitBox(Rectangle r)
		{
			spriteBatch.Draw(OneWhitePixel, r, new Color(255, 0, 255));
		}
	}
}
