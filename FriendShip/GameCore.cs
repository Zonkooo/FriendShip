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
	    private readonly int _players;
	    public float Scale = 1f;

		//events stuff
		public Texture2D bonusTrap;
		public MyTexture2D leak;
		public Texture2D kohl;
		public Texture2D txtCharb;
		public Texture2D txtCook;
		public Texture2D txtDodo;
		public Texture2D txtFuite;
		public Texture2D txtPilot;
		public Texture2D txtSdm;

		public MyTexture2D chrono;
		public Texture2D support;
		public MyTexture2D warning;
		public Texture2D action;
		public Texture2D credits;
        private Song _song;

		public GraphicsDeviceManager graphics;
		public SpriteBatch spriteBatch;
		public Texture2D OneWhitePixel;
		public Texture2D Cling;
		public SpriteFont font;

		private Dictionary<GameEndings, Texture2D> _gameOverTex = new Dictionary<GameEndings, Texture2D> ();
		public Texture2D _backGnd;

		public Dictionary<RoomType, Room> _rooms = new Dictionary<RoomType, Room>();
		public List<Wall> Walls = new List<Wall>();
		public Dictionary<PlayerType, Player> Players = new Dictionary<PlayerType, Player>();
		public List<EventBase> Events = new List<EventBase>();
		private TimeSpan[] _eventTriggers;

		//ship related properties
		public float health = 1.0f;
		public float derive = 1.0f;

		public GameCore(int resolution, int players)
		{
		    _players = Math.Min(Math.Max(players, 1), 4);
		    Scale = resolution/1080f;

			graphics = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferHeight = resolution,
				PreferredBackBufferWidth = (resolution*16)/9,
			};

		    if (resolution == 1080)
		        graphics.IsFullScreen = true;

			Content.RootDirectory = "Content";

			InitHelper.InitRooms (this, _rooms);

			new Gui (this);

			Walls.Add (new Wall (new Rectangle (253/*that's the only important thing*/, 0, 1, 1080)));
			Walls.Add (new Wall (new Rectangle (1732/*that's the only important thing*/, 0, 1, 1080)));
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			OneWhitePixel = Content.Load<Texture2D>("onewhitepixel");
			Cling = Content.Load<Texture2D>("clig");
			_backGnd = Content.Load<Texture2D>("Interface/fond");
			//font = Content.Load<SpriteFont>("font");

			leak = new MyTexture2D(Content.Load<Texture2D>("jet_eau"), 4, new double[]{1000.0/12, 1000.0/12, 1000.0/12, 1000.0/12});
			kohl = Content.Load<Texture2D>("charbon");
			warning = new MyTexture2D(Content.Load<Texture2D>("warning"), 2, new double[]{150, 150});
			action = Content.Load<Texture2D>("action");
			credits = Content.Load<Texture2D>("credits");

			txtCharb = Content.Load<Texture2D>("Text/texte_charbon");
			txtCook = Content.Load<Texture2D>("Text/texte_cuisine");
			txtDodo = Content.Load<Texture2D>("Text/texte_dortoir");
			txtFuite = Content.Load<Texture2D>("Text/texte_fuite");
			txtPilot = Content.Load<Texture2D>("Text/texte_pilotage");
			txtSdm = Content.Load<Texture2D>("Text/texte_sdm");

			//must be after txt textures init
			Events.Add (new MustDriveShip (this));
			Events.Add (new AllToCale (this));
			Events.Add (new GetFood (this));
			Events.Add (new FixEngine (this));
			Events.Add (new GetTrap (this));
			Events.Add (new CarryKohl (this));
			Events.Add (new GetTrap (this));
			Events.Add (new AllToCale (this));
			Events.Add (new GetTrap (this));
			Events.Add (new FixEngine (this));
			Events.Add (new GetFood (this));
			Events.Add (new FixEngine (this));
			Events.Add (new AllToCale (this));

			_eventTriggers = new []{
				TimeSpan.FromSeconds(120 - 8),  //goto cale
				TimeSpan.FromSeconds(120 - 15), //+life
				TimeSpan.FromSeconds(120 - 25), //fix engine
				TimeSpan.FromSeconds(120 - 33), //+trap
				TimeSpan.FromSeconds(120 - 40), //coal
				TimeSpan.FromSeconds(120 - 47), //+trap
				TimeSpan.FromSeconds(120 - 60),  //goto cale
				TimeSpan.FromSeconds(120 - 70), //+trap
				TimeSpan.FromSeconds(120 - 80), //fix engine
				TimeSpan.FromSeconds(120 - 90), //+life
				TimeSpan.FromSeconds(120 - 94), //fix engine
				TimeSpan.FromSeconds(120 - 110), //goto cale
			};

			chrono = new MyTexture2D(Content.Load<Texture2D>("Interface/chrnometre_anime"), 21, new double[]{1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000});
			support = Content.Load<Texture2D>("Interface/interface_barre");
			splashScreen = Content.Load<Texture2D>("comment_jouer");

			var bg = new TemporaryEffect (this, new Vector2 (), new MyTexture2D (_backGnd, 1), 1E20 /*beaucoup*/);
			bg.DrawOrder = -10;
			var fg = new TemporaryEffect (this, new Vector2 (), new MyTexture2D (Content.Load<Texture2D>("lumieres"), 1), 1E20 /*			beaucoup*/);
			fg.DrawOrder = 1000;

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
				{ Direction.ACTION, Keys.RightControl },
			};

			Players[PlayerType.CAP] = new Player (this, captainTextures, _rooms[RoomType.COMMANDS], player1Controls, PlayerIndex.Two);

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

			Players[PlayerType.MECA] = new Player (this, mecanoTextures, _rooms[RoomType.MACHINE], player2Controls, PlayerIndex.Three);
            
            if (!(_players > 1))
            {
                Players[PlayerType.MECA].Death();
                Players[PlayerType.MECA].Visible = false;
            }

            var cuisto = Content.Load<Texture2D>("Players/cuisto");
            var cuistoRun = Content.Load<Texture2D>("Players/cuisto_run");
			var cuistoDegat = Content.Load<Texture2D>("Players/cuistot_degats");
            var cuistoDead = Content.Load<Texture2D>("Players/cuisto_mort");
            var cuistoTextures = new Dictionary<PlayerState, MyTexture2D>
			{
				{PlayerState.STILL, new MyTexture2D(cuisto, 1)},
				{PlayerState.WALK, new MyTexture2D(cuistoRun, 4, new []{1000.0/12, 1000.0/12, 1000.0/12, 1000.0/12})},
				{PlayerState.HIT, new MyTexture2D(cuistoDegat, 9, new double[]{60, 60, 60, 60, 60, 60, 1000.0/12, 1000.0/12, 1000.0/12})},
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

            Players[PlayerType.COOK] = new Player(this, cuistoTextures, _rooms[RoomType.KITCHEN], player3Controls, PlayerIndex.One);

            if (!(_players > 2))
            {
                Players[PlayerType.COOK].Death();
                Players[PlayerType.COOK].Visible = false;
            }

            var cireman = Content.Load<Texture2D>("Players/cireman");
            var ciremanRun = Content.Load<Texture2D>("Players/cireman_run");
			var ciremanDegat = Content.Load<Texture2D>("Players/cireman_degats");
            var ciremanDead = Content.Load<Texture2D>("Players/cireman_mort");
            var ciremanTextures = new Dictionary<PlayerState, MyTexture2D>
			{
				{PlayerState.STILL, new MyTexture2D(cireman, 1)},
				{PlayerState.WALK, new MyTexture2D(ciremanRun, 4, new []{1000.0/12, 1000.0/12, 1000.0/12, 1000.0/12})},
				{PlayerState.HIT, new MyTexture2D(ciremanDegat, 9, new double[]{60, 60, 60, 60, 60, 60, 1000.0/12, 1000.0/12, 1000.0/12})},
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

            Players[PlayerType.FISH] = new Player(this, ciremanTextures, _rooms[RoomType.BRIDGE], player4Controls, PlayerIndex.Four);

            if (!(_players > 3))
            {
                Players[PlayerType.FISH].Death();
                Players[PlayerType.FISH].Visible = false;
            }

		    _song = Content.Load<Song>("musique_off");
		}

		private TimeSpan _deathCounter = TimeSpan.FromMinutes(2);

		double endGameTime = 5000;

		bool splash = true;
		Texture2D splashScreen;

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if(splash)
			{
				//anything stops the splash screen
			    if (Keyboard.GetState().GetPressedKeys().Length > 0
			        || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.Two).Buttons.Back == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.Two).Buttons.Start == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.Two).Buttons.A == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.Two).Buttons.B == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.Two).Buttons.X == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.Two).Buttons.Y == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.Three).Buttons.Back == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.Three).Buttons.Back == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.Three).Buttons.Start == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.Three).Buttons.A == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.Three).Buttons.B == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.Three).Buttons.X == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.Three).Buttons.Y == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.Four).Buttons.Back == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.Four).Buttons.Back == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.Four).Buttons.Start == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.Four).Buttons.A == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.Four).Buttons.B == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.Four).Buttons.X == ButtonState.Pressed
			        || GamePad.GetState(PlayerIndex.Four).Buttons.Y == ButtonState.Pressed)
			    {
                    MediaPlayer.Play(_song);
                    splash = false;
			    }

			    return;
			}

			if (Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
			{
				this.Exit();
			}

			if (ended)
			{
				endGameTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
				if (endGameTime < 0)
					_gameOverTex [_ending.Value] = credits; //foohaa
				return;
			}

			var prevTime = _deathCounter;
			_deathCounter -= gameTime.ElapsedGameTime;
			chrono.Update (gameTime.ElapsedGameTime.TotalMilliseconds);

			for (int i = 0; i < _eventTriggers.Length; i++)
			{
				if(prevTime >= _eventTriggers[i] && _eventTriggers[i] > _deathCounter) //time goes backwards
				{
					Events [i + 1].Enable (); //miam les index out of bounds
				}
			}

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

			if(splash)
			{
				if (spriteBatch != null)
				{
					spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(Scale));
					spriteBatch.Draw (splashScreen, new Vector2(), Color.White);
					spriteBatch.End();
				}
				return;
			}

			base.Draw(gameTime);

			if(spriteBatch != null)
			{
				warning.Update (gameTime.ElapsedGameTime.TotalMilliseconds);

				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(Scale));
				if (ended)
					spriteBatch.Draw (_gameOverTex [_ending.Value], new Vector2 (), Color.White);
				else
				{
					spriteBatch.Draw (support, new Vector2(385, 893), Color.White);

					var barLength = 495;
					spriteBatch.Draw (OneWhitePixel, new Rectangle (400 + (int)((1-health) * barLength), 930, (int)(health * barLength), 41), Color.IndianRed); //barre de vie
					spriteBatch.Draw (OneWhitePixel, new Rectangle (1020, 930, (int)(derive * barLength), 41), Color.CornflowerBlue); //barre de dérive

					if(health < 0.3)
						spriteBatch.Draw (warning.Texture, new Vector2(600, 870), warning.GetRectangle(), Color.White);
					if(derive < 0.3)
						spriteBatch.Draw (warning.Texture, new Vector2(1250, 870), warning.GetRectangle(), Color.White);

					spriteBatch.Draw (chrono.Texture, new Vector2(890, 820), chrono.GetRectangle(), Color.White);

					//timer
					//spriteBatch.DrawString (font, _deathCounter.ToString ("mm\\:ss"), new Vector2 (900, 950), Color.White);

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
