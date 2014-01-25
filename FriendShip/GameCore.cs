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

			InitHelper.InitRooms (_rooms);

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

			InitHelper.LoadAndSetRoomTextures (_rooms, Content);

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
