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
		WIN_CAP,
		WIN_COOK,
		WIN_MECA,
		WIN_FISH,
	}

	public enum PlayerType
	{
		CAP,
		COOK,
		MECA,
		FISH
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

		private Dictionary<GameEndings, Texture2D> _gameOverTex = new Dictionary<GameEndings, Texture2D> ();
		public Texture2D _backGnd;

		public Dictionary<RoomType, Room> _rooms = new Dictionary<RoomType, Room>();
		public List<Wall> Walls = new List<Wall>();
		public Dictionary<PlayerType, Player> Players = new Dictionary<PlayerType, Player>();
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

			InitHelper.InitRooms (this, _rooms);

			new Gui (this);

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
			_backGnd = Content.Load<Texture2D>("Interface/fond");
			font = Content.Load<SpriteFont>("font");

			var bg = new TemporaryEffect (this, new Vector2 (), new MyTexture2D (_backGnd, 1), 1E20 /*beaucoup*/);
			bg.DrawOrder = -10;

			_gameOverTex[GameEndings.WIN_CAP] = Content.Load<Texture2D>("win_cap");
			_gameOverTex[GameEndings.WIN_COOK] = Content.Load<Texture2D>("win_cuistot");
			_gameOverTex[GameEndings.WIN_FISH] = Content.Load<Texture2D>("win_cireman");
			_gameOverTex[GameEndings.WIN_MECA] = Content.Load<Texture2D>("win_mecano");
			_gameOverTex[GameEndings.ALL_DEAD] = Content.Load<Texture2D>("game_over");
			_gameOverTex[GameEndings.EXPLODE] = Content.Load<Texture2D>("game_over");
			_gameOverTex[GameEndings.DERIVE] = Content.Load<Texture2D>("game_over");
			_gameOverTex[GameEndings.SHARE_GOLD] = Content.Load<Texture2D>("lose_screen");

			InitHelper.LoadAndSetRoomTextures (_rooms, Content);

			var captain = Content.Load<Texture2D>("Players/capitaine");
			var captainRun = Content.Load<Texture2D>("Players/capitaine_run");
			var captainDead = Content.Load<Texture2D>("Players/capitaine_mort");
			var captainDmg = Content.Load<Texture2D>("Players/capitaine_degats");
			var captainTextures = new Dictionary<PlayerState, MyTexture2D>
			{
				{PlayerState.STILL, new MyTexture2D(captain, 1)},
				{PlayerState.WALK, new MyTexture2D(captainRun, 4, new []{1000.0/12, 1000.0/12, 1000.0/12, 1000.0/12})},
				{PlayerState.HIT, new MyTexture2D(captainDmg, 9, new []{1000.0/12, 1000.0/12, 1000.0/12, 1000.0/12, 1000.0/12, 1000.0/12, 1000.0/12, 1000.0/12, 1000.0/12})},
				{PlayerState.DEAD, new MyTexture2D(captainDead, 1)},
			};
			var player1Controls = new Dictionary<Direction, Keys> {
				{ Direction.LEFT, Keys.Left },
				{ Direction.RIGHT, Keys.Right },
				{ Direction.UP, Keys.Up },
				{ Direction.DOWN, Keys.Down },
				{ Direction.TRAP, Keys.NumPad0 },
				{ Direction.ACTION, Keys.PageDown },
			};

			Players[PlayerType.CAP] = new Player (this, captainTextures, _rooms[RoomType.COMMANDS], player1Controls);

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
				{ Direction.LEFT, Keys.Q },
				{ Direction.RIGHT, Keys.D },
				{ Direction.UP, Keys.Z },
				{ Direction.DOWN, Keys.S },
				{ Direction.TRAP, Keys.E },
				{ Direction.ACTION, Keys.A },
			};
			Players[PlayerType.MECA] = new Player (this, mecanoTextures, _rooms[RoomType.MACHINE], player2Controls);

            var cuisto = Content.Load<Texture2D>("Players/cuisto");
            var cuistoRun = Content.Load<Texture2D>("Players/cuisto_run");
            var cuistoDead = Content.Load<Texture2D>("Players/cuisto_mort");
            var cuistoTextures = new Dictionary<PlayerState, MyTexture2D>
			{
				{PlayerState.STILL, new MyTexture2D(cuisto, 1)},
				{PlayerState.WALK, new MyTexture2D(cuistoRun, 4, new []{1000.0/12, 1000.0/12, 1000.0/12, 1000.0/12})},
                {PlayerState.HIT, new MyTexture2D(mecanoDegats, 9, new double[]{60, 60, 60, 60, 60, 60, 1000.0/12, 1000.0/12, 1000.0/12})},
				{PlayerState.DEAD, new MyTexture2D(cuistoDead, 1)},
			};
            var player3Controls = new Dictionary<Direction, Keys> {
				{ Direction.LEFT, Keys.NumPad4 },
				{ Direction.RIGHT, Keys.NumPad6},
				{ Direction.UP, Keys.NumPad8 },
				{ Direction.DOWN, Keys.NumPad5 },
				{ Direction.TRAP, Keys.NumPad9 },
				{ Direction.ACTION, Keys.NumPad7},
			};

			Players[PlayerType.COOK] = new Player(this, cuistoTextures, _rooms[RoomType.KITCHEN], player3Controls);

            var cireman = Content.Load<Texture2D>("Players/cireman");
            var ciremanRun = Content.Load<Texture2D>("Players/cireman_run");
            var ciremanDead = Content.Load<Texture2D>("Players/cireman_mort");
            var ciremanTextures = new Dictionary<PlayerState, MyTexture2D>
			{
				{PlayerState.STILL, new MyTexture2D(cireman, 1)},
				{PlayerState.WALK, new MyTexture2D(ciremanRun, 4, new []{1000.0/12, 1000.0/12, 1000.0/12, 1000.0/12})},
                {PlayerState.HIT, new MyTexture2D(mecanoDegats, 9, new double[]{60, 60, 60, 60, 60, 60, 1000.0/12, 1000.0/12, 1000.0/12})},
				{PlayerState.DEAD, new MyTexture2D(ciremanDead, 1)},
			};
            var player4Controls = new Dictionary<Direction, Keys> {
				{ Direction.LEFT, Keys.K },
				{ Direction.RIGHT, Keys.M},
				{ Direction.UP, Keys.O },
				{ Direction.DOWN, Keys.L },
				{ Direction.TRAP, Keys.P },
				{ Direction.ACTION, Keys.I},
			};

			Players[PlayerType.FISH] = new Player(this, ciremanTextures, _rooms[RoomType.BRIDGE], player4Controls);

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
			if (Players.Count (p => p.Value.Enabled) == 0) //everyone is dead
				EndGame(GameEndings.ALL_DEAD);
			if(_deathCounter < TimeSpan.Zero)
			{
				if (Players.Count (p => p.Value.Enabled) == 1)
				{
					switch (Players.First (p => p.Value.Enabled).Key)
					{
					case PlayerType.CAP:
						EndGame (GameEndings.WIN_CAP);
						break;
					case PlayerType.COOK:
						EndGame (GameEndings.WIN_COOK);
						break;
					case PlayerType.FISH:
						EndGame (GameEndings.WIN_FISH);
						break;
					case PlayerType.MECA:
						EndGame (GameEndings.WIN_MECA);
						break;
					}
				}
				else
					EndGame(GameEndings.SHARE_GOLD);
			}

			base.Update(gameTime);
		}

		bool ended = false;
		GameEndings? _ending = null;
		private void EndGame(GameEndings ending)
		{
			foreach (var component in Components)
			{
				var c = component as GameComponent;
				if (c != null)
					c.Enabled = false;
			}
			ended = true;
			_ending = ending;
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
				if (ended)
					spriteBatch.Draw (_gameOverTex [_ending.Value], new Vector2 (), Color.White);
				else
				{
					spriteBatch.Draw (OneWhitePixel, new Rectangle (400, 900, (int)(health * 450), 100), Color.IndianRed); //barre de vie
					spriteBatch.Draw (OneWhitePixel, new Rectangle (1070, 900, (int)(derive * 450), 100), Color.CornflowerBlue); //barre de dérive

					//timer
					spriteBatch.DrawString (font, _deathCounter.ToString ("mm\\:ss"), new Vector2 (900, 950), Color.White);

					foreach (var @event in Events)
					{
						if (@event.Enabled)
							@event.DrawText (spriteBatch);
					}

					foreach (var wall in Walls)
						DrawHitBox (wall._boundingBox);
				}

				spriteBatch.End();
			}
		}

		/// <summary>
		/// Spritebatch must be initialized (begin) before
		/// </summary>
		public void DrawHitBox(Rectangle r)
		{
			//spriteBatch.Draw(OneWhitePixel, r, new Color(255, 0, 255));
		}
	}

}
