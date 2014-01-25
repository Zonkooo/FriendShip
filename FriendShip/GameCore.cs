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
		LEFT,
		RIGHT,
        DOWN_LEFT,
        DOWN_RIGHT,
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

			_rooms[RoomType.LEFT] = new Room (this, new Vector2 (253, 211), new Vector2(), RoomMovementType.HORIZONTAL);
			_rooms[RoomType.RIGHT] = new Room(this, new Vector2(601, 211), new Vector2(), RoomMovementType.HORIZONTAL);
			_rooms[RoomType.DOWN_LEFT] = new Room(this, new Vector2(819, 211), new Vector2(), RoomMovementType.HORIZONTAL);
			_rooms[RoomType.DOWN_RIGHT] = new Room(this, new Vector2(1167, 211), new Vector2(), RoomMovementType.HORIZONTAL);

			_rooms[RoomType.LEFT].Exits.Add(new RoomLink(_rooms[RoomType.RIGHT], new Rectangle(600, 211, 1, 200), new Vector2(605,361)));// Premiere porte(salle1) , gauche droite
			_rooms[RoomType.RIGHT].Exits.Add(new RoomLink(_rooms[RoomType.LEFT], new Rectangle(603, 211, 1, 200), new Vector2(560,361)));// Deuxieme porte (couloir1),droite gauche
			_rooms[RoomType.RIGHT].Exits.Add(new RoomLink(_rooms[RoomType.DOWN_LEFT], new Rectangle(817, 211, 1, 200), new Vector2(825,361)));// Troisieme porte (couloir1)gauche droite
            _rooms[RoomType.DOWN_LEFT].Exits.Add(new RoomLink(_rooms[RoomType.RIGHT], new Rectangle(820, 211, 1, 200), new Vector2(750, 361)));// quatrieme porte (salle2)droite gauche
            _rooms[RoomType.DOWN_LEFT].Exits.Add(new RoomLink(_rooms[RoomType.DOWN_RIGHT], new Rectangle(1165, 211, 1, 200), new Vector2(1172, 361)));// cinquieme porte (salle2)gauche droite

			Walls.Add (new Wall (new Rectangle (10/*that's the only important thing*/, 0, 1, 1080)));
			Walls.Add (new Wall (new Rectangle (1900/*that's the only important thing*/, 0, 1, 1080)));
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

			_rooms [RoomType.LEFT].Texture = machines;
			_rooms [RoomType.RIGHT].Texture = pilotage;
            _rooms[RoomType.DOWN_LEFT].Texture = machines;

			var cireman = Content.Load<Texture2D>("Players/cireman");
			var player = new Player (this, cireman, _rooms[(int)RoomType.LEFT]);
			player.Position = new Vector2 (10, 40);

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
