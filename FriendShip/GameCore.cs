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
	enum RoomType
	{
		ROOM_1,
		HALL_1,
        ROOM_2,
        HALL_2,
        ROOM_3,
	}

	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class GameCore : Game
	{
		public GraphicsDeviceManager graphics;
		public SpriteBatch spriteBatch;
		public Texture2D OneWhitePixel;

		private Dictionary<RoomType, Room> _rooms = new Dictionary<RoomType, Room>();
		public List<Wall> Walls = new List<Wall>();

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

			_rooms[RoomType.ROOM_1] = new Room (this, new Vector2 (253, 211), new Vector2(427,348), RoomMovementType.HORIZONTAL);
			_rooms[RoomType.HALL_1] = new Room(this, new Vector2(601, 211 + 73), new Vector2(), RoomMovementType.HORIZONTAL);
			_rooms[RoomType.ROOM_2] = new Room(this, new Vector2(819, 211), new Vector2(), RoomMovementType.HORIZONTAL);
			_rooms[RoomType.HALL_2] = new Room(this, new Vector2(1167, 211 + 73), new Vector2(), RoomMovementType.HORIZONTAL);
            _rooms[RoomType.ROOM_3] = new Room(this, new Vector2(1385, 211), new Vector2(), RoomMovementType.HORIZONTAL);

			_rooms[RoomType.ROOM_1].Exits.Add(
				new RoomLink(_rooms[RoomType.HALL_1], new Rectangle(600, 211, 1, 200), Direction.RIGHT, new Vector2(605,348)));
			_rooms[RoomType.HALL_1].Exits.Add(
				new RoomLink(_rooms[RoomType.ROOM_1], new Rectangle(603, 211, 1, 200), Direction.LEFT, new Vector2(560,348)));
			_rooms[RoomType.HALL_1].Exits.Add(
				new RoomLink(_rooms[RoomType.ROOM_2], new Rectangle(817, 211, 1, 200), Direction.RIGHT, new Vector2(825,348)));
            _rooms[RoomType.ROOM_2].Exits.Add(
				new RoomLink(_rooms[RoomType.HALL_1], new Rectangle(820, 211, 1, 200), Direction.LEFT, new Vector2(750, 348)));
            _rooms[RoomType.ROOM_2].Exits.Add(
				new RoomLink(_rooms[RoomType.HALL_2], new Rectangle(1165, 211, 1, 200), Direction.RIGHT, new Vector2(1172, 348)));
            _rooms[RoomType.HALL_2].Exits.Add(
				new RoomLink(_rooms[RoomType.ROOM_2], new Rectangle(1168, 211, 1, 200), Direction.LEFT, new Vector2(1120, 348)));
            _rooms[RoomType.HALL_2].Exits.Add(
				new RoomLink(_rooms[RoomType.ROOM_3], new Rectangle(1385, 211, 1, 200), Direction.RIGHT, new Vector2(1395, 348)));
            _rooms[RoomType.ROOM_3].Exits.Add(
				new RoomLink(_rooms[RoomType.HALL_2], new Rectangle(1388, 211, 1, 200), Direction.LEFT, new Vector2(1330, 348)));

			Walls.Add (new Wall (new Rectangle (253/*that's the only important thing*/, 0, 1, 1080)));
			Walls.Add (new Wall (new Rectangle (1904/*that's the only important thing*/, 0, 1, 1080)));
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

			var machines = Content.Load<Texture2D>("Rooms/machines");
			var pilotage = Content.Load<Texture2D>("Rooms/pilotage");
			var couloir1 = Content.Load<Texture2D>("Rooms/couloir1");
			var cuisine = Content.Load<Texture2D>("Rooms/cuisine");

			_rooms [RoomType.ROOM_1].Texture = pilotage;
			_rooms [RoomType.HALL_1].Texture = couloir1;
            _rooms[RoomType.ROOM_2].Texture = machines;
			_rooms[RoomType.HALL_2].Texture = couloir1;
			_rooms[RoomType.ROOM_3].Texture = cuisine;

			var cireman = Content.Load<Texture2D>("Players/cireman");
			var player1Controls = new Dictionary<Direction, Keys> {
				{ Direction.LEFT, Keys.Left },
				{ Direction.RIGHT, Keys.Right },
				{ Direction.UP, Keys.Up },
				{ Direction.DOWN, Keys.Down },
				{ Direction.TRAP, Keys.End },
			};
			var player = new Player (this, cireman, _rooms[RoomType.ROOM_1], player1Controls);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
		}

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

			base.Update(gameTime);
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
