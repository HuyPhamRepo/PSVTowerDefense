using System;
using System.IO;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;

namespace HybridActionTD
{
	public class GameScreen : Scene
	{
		private Texture2D 			gameTexture;
		private TextureInfo			gameTextureInfo;
		private SpriteList			gameSpriteList;
		
		private Texture2D			menuButtonTexture;
		private	TextureInfo			menuButtonTextureInfo;
		private	SpriteList			menuButtonSpriteList;
		
		private Texture2D			backgroundTexture;
		private TextureInfo			backgroundTextureInfo;
		private SpriteUV			backgroundSprite;
		
		private	CircleButton		basicTowerButton;
		private	CircleButton		slowTowerButton;
		private	CircleButton		splashTowerButton;
		
		private	Vector4				buttonLabelColor;
		private	Vector4				blackColor;
		
		private	PauseScreen			pauseMenu;
		private	WinScreen			winScreen;
		private	LoseScreen			loseScreen;
		
		private int 				currentMap;
		private int					lifeCount;
		
		private TowerType			currentType;
		
		private PlayCell[,]			gameGrid;
		private List<EnemyWave>		enemyWaveList;
		
		private Vector2i			spawnGridPosition;
		
		private EnemyManager		enemyManager;
		
		private	ProjectileManager	projectileManager;
		
		private TowerManager		towerManager;
		
		private MountainGod			mountainGod;
		
		private PlayState			currentState;
		
		private Label				villagerCount;
		private Label				villagerShadow;
		
		private Messenger			messenger;
		
		private FPS					fps;
		
		public GameScreen (int mapNum = 0)
		{
			//setup camera
			this.Camera.SetViewFromViewport();
			
			fps = new FPS();
			//this.AddChild(fps, 500);
			
			//Init map value
			currentMap = mapNum;
			Player.CurrentMission = currentMap;
			
			//Init life count
			lifeCount = 10;
			
			//setup Texure
			gameTexture = new Texture2D(CommonHelper.ArtDirectory + "GameArt3.png", false);
			gameTextureInfo = new TextureInfo(gameTexture, CommonHelper.TextureDivision);
			Console.WriteLine("Game texture loaded");
			
			menuButtonTexture = new Texture2D(CommonHelper.ArtDirectory + "Button.png", false);
			menuButtonTextureInfo = new TextureInfo(menuButtonTexture, CommonHelper.ButtonTileDivision);
			Console.WriteLine("Game button texture loaded");
			
			backgroundTexture = new Texture2D(CommonHelper.ArtDirectory + "Background.png", false);
			backgroundTextureInfo = new TextureInfo(backgroundTexture);
			Console.WriteLine("Game background texture loaded");
			backgroundSprite = new SpriteUV(backgroundTextureInfo);
			backgroundSprite.Quad.S = backgroundTextureInfo.TextureSizef;
			backgroundSprite.Position = new Vector2(0,0);
			this.AddChild(backgroundSprite);
			
			//setup SpriteList
			gameSpriteList = new SpriteList(gameTextureInfo);
			this.AddChild(gameSpriteList);
			
			menuButtonSpriteList = new SpriteList(menuButtonTextureInfo);
			this.AddChild(menuButtonSpriteList);
			
			//Init labels
			villagerCount = new Label("Villagers: " + villagerCount, new FontMap((Sce.PlayStation.Core.Imaging.Font)CommonHelper.GameFont.ShallowClone()));
			villagerShadow = new Label("Villagers: " + villagerCount, new FontMap((Sce.PlayStation.Core.Imaging.Font)CommonHelper.GameFont.ShallowClone()));
			villagerCount.Position = new Vector2(10, CommonHelper.ScreenSize.Y - villagerCount.CharWorldHeight - 5);
			villagerShadow.Position = new Vector2(10 + 2, CommonHelper.ScreenSize.Y - villagerCount.CharWorldHeight - 7);
			villagerShadow.Color = new Vector4(0,0,0, .75f);
			this.AddChild(villagerShadow);
			this.AddChild(villagerCount);
			
			//Init messenger class
			messenger = new Messenger();
			
			//setup grid
			gameGrid = new PlayCell[CommonHelper.GridSize.X, CommonHelper.GridSize.Y];
			InitPlayGrid();
			
			//setup Projectile manager
			projectileManager = new ProjectileManager(ref gameTexture, ref gameTextureInfo, ref messenger);
			Console.WriteLine("Projectile manager initiated");
			
			//setup Tower manager
			towerManager = new TowerManager(ref messenger);
			Console.WriteLine("Tower manager initiated");
			
			//setup Mountain God
			mountainGod = new MountainGod(ref gameTexture, ref gameTextureInfo, ref messenger);
			mountainGod.Init(new Vector2(0, 160), ref gameSpriteList);
			Console.WriteLine("Mountain God initiated");
			
			pauseMenu = new PauseScreen(ref menuButtonTextureInfo, ref gameTextureInfo);
			Console.WriteLine("Pause menu initiated");
			loseScreen = new LoseScreen(ref menuButtonTextureInfo, ref gameTextureInfo);
			Console.WriteLine("Pause menu initiated");
			winScreen = new WinScreen(ref menuButtonTextureInfo, ref gameTextureInfo);
			Console.WriteLine("Pause menu initiated");
			
			currentState = PlayState.Normal;
			
			//Load Map
			LoadMap(currentMap);
			
			basicTowerButton = new CircleButton(this, ref gameSpriteList, ref gameTextureInfo, "10", CommonHelper.SmallFont, CommonHelper.TowerBasicBaseTileIndex, CommonHelper.TowerBasicBaseTileIndex);
			basicTowerButton.SetPosition(CommonHelper.ScreenSize.X - 10 - basicTowerButton.GetWidth(), CommonHelper.ScreenSize.Y - 5 - basicTowerButton.GetHeight());
			
			slowTowerButton = new CircleButton(this, ref gameSpriteList, ref gameTextureInfo, "25", CommonHelper.SmallFont, CommonHelper.TowerSlowBaseTileIndex, CommonHelper.TowerSlowBaseTileIndex);
			slowTowerButton.SetPosition(basicTowerButton.GetPosition().X - 15 - slowTowerButton.GetWidth(), basicTowerButton.GetPosition().Y);
			
			splashTowerButton = new CircleButton(this, ref gameSpriteList, ref gameTextureInfo, "50", CommonHelper.SmallFont, CommonHelper.TowerSplashBaseTileIndex, CommonHelper.TowerSplashBaseTileIndex);
			splashTowerButton.SetPosition(slowTowerButton.GetPosition().X - 15 - splashTowerButton.GetWidth(), slowTowerButton.GetPosition().Y);
			
			Console.WriteLine("Tower buttons initiated");
			
			currentType = TowerType.Basic;
			
			blackColor = new Vector4(0,0,0,.5f);
			buttonLabelColor = basicTowerButton.GetButtonLabel().Color;
			
			messenger.SetReferences(this, ref projectileManager, ref enemyManager, ref towerManager, ref gameSpriteList, ref menuButtonSpriteList, ref gameGrid);
			
			if (Player.IsResumed)
			{
				LoadResumeData();
				Player.IsResumed = false;
			}
			
			//schedule update
            Scheduler.Instance.ScheduleUpdateForTarget(this,0,false);

            // Clear any queued clicks so we dont immediately exit if coming in from the menu
            Touch.GetData(0).Clear();
			
		}
		
		public override void Update(float dt)
		{
			List<TouchData> touches = Touch.GetData(0);
			
			if (touches.Count > 0)
				Console.WriteLine("Touch position: " + CommonHelper.TouchToScreenX(touches[0].X) + " : " + CommonHelper.TouchToScreenY(touches[0].Y));
			
			switch(currentState)
			{
				case PlayState.Normal:
					mountainGod.Update(dt, ref gameGrid, ref gameSpriteList, ref enemyManager);
					
					if (Input2.GamePad0.Start.Release)
					{
						currentState = PlayState.Paused;
						pauseMenu.Display(this, ref menuButtonSpriteList, ref gameSpriteList);
						HideLabels();
						return;
					}
					
					if (touches.Count > 0 && touches[0].Status == TouchStatus.Up)
					{
						for (int i = 0; i < CommonHelper.GridSize.X; i++)
						{
							for (int j = 0; j < CommonHelper.GridSize.Y; j++)
							{
								if (CommonHelper.IsInside(CommonHelper.TouchToScreenX(touches[0].X), CommonHelper.TouchToScreenY(touches[0].Y), gameGrid[i,j].GetBoundingBox()))
								{
									switch (gameGrid[i,j].GetCellType())
									{
									case CellType.Sea:
										gameGrid[i,j].ChangeCellType(CellType.Land);
										break;
									case CellType.Land:
										gameGrid[i,j].ChangeCellType(CellType.Occupied);
										towerManager.BuildTower(currentType, ref gameTexture, ref gameTextureInfo, new Vector2i(i, j), ref gameGrid, ref gameSpriteList);
										break;
									}
								}
							}
						}
					}
					
					enemyManager.Update(dt, ref gameGrid, ref gameSpriteList, ref lifeCount, ref currentState);
				
					if (lifeCount <= 0)
					{
						currentState = PlayState.Lose;
						loseScreen.Display(this, ref menuButtonSpriteList, ref gameSpriteList);
						HideLabels();
					}
					else if (enemyManager.isFinished)
					{
						currentState = PlayState.Win;
						winScreen.SetWinScore(lifeCount);
						winScreen.Display(this, ref menuButtonSpriteList, ref gameSpriteList);
						HideLabels();
					}
					
					towerManager.Update(dt, ref gameGrid, enemyManager.GetEnemyList(), ref projectileManager, ref gameSpriteList);
					
					projectileManager.Update(dt, ref gameSpriteList, enemyManager.GetEnemyList(), ref gameGrid);
					
					basicTowerButton.Update(dt, touches);
					if (basicTowerButton.isSelected)
						currentType = TowerType.Basic;
					
					slowTowerButton.Update(dt, touches);
					if (slowTowerButton.isSelected)
						currentType = TowerType.Slow;
					
					splashTowerButton.Update(dt, touches);
					if (splashTowerButton.isSelected)
						currentType = TowerType.Splash;
				
					break;
				
				case PlayState.Paused:
					pauseMenu.Update(dt, touches);
					if (pauseMenu.GetResumeButton().isSelected)
					{
						currentState = PlayState.Normal;
						pauseMenu.Remove(this, ref menuButtonSpriteList, ref gameSpriteList);
						DisplayLabels();
						return;
					}
					else if (pauseMenu.GetQuitButton().isSelected)
					{
						enemyManager.SavePlayerData();
						Director.Instance.ReplaceScene(new MenuScreen());
					}
					break;
				
				case PlayState.Lose:
					loseScreen.Update(dt, touches);
					if (loseScreen.GetReplayButton().isSelected)
					{
						currentState = PlayState.Reset;
						loseScreen.Remove(this, ref menuButtonSpriteList, ref gameSpriteList);
					}
					else if (loseScreen.GetQuitButton().isSelected)
					{
						enemyManager.SavePlayerData();
						Director.Instance.ReplaceScene(new MenuScreen());
					}
					break;
				case PlayState.Win:
					winScreen.Update(dt, touches);
					if (winScreen.GetQuitButton().isSelected)
					{
						Director.Instance.ReplaceScene(new MenuScreen());
					}
					else if (winScreen.GetReplayButton().isSelected)
					{
						currentState = PlayState.Reset;
						winScreen.Remove(this, ref menuButtonSpriteList, ref gameSpriteList);
					}
					else if (winScreen.GetNextButton().isSelected)
					{
						if (currentMap > Player.FinishedMission)
							Player.FinishedMission ++;
						Player.CurrentMission++;
						Player.CurrentWave = 0;
						Player.CurrentVillagers = 10;
						Player.IsResumed = false;
						Player.Save();
						Director.Instance.ReplaceScene(new LoadScreen(Player.FinishedMission));
					}
					break;
				case PlayState.Reset:
					Reset();
					break;
			}
			Player.CurrentVillagers = lifeCount;
			villagerCount.Text = "Villagers: " + lifeCount;
			villagerShadow.Text = "Villagers: " + lifeCount;
			base.Update(dt);
		}
		
		~GameScreen()
		{
			gameTexture.Dispose();
			gameTextureInfo.Dispose();
		}
		
		void InitPlayGrid()
		{
			Console.WriteLine("Play grid initiation started");
			for (int i = 0; i < CommonHelper.GridSize.X; i++)
			{
				for (int j = 0; j < CommonHelper.GridSize.Y; j++)
				{
					gameGrid[i,j] = new PlayCell(ref gameTexture, ref gameTextureInfo, CellType.Ocean, i, j);
				}
			}
			Console.WriteLine("Play grid initiation finished");
		}
		
		void HideLabels()
		{
			basicTowerButton.GetButtonLabel().Color = blackColor;
			splashTowerButton.GetButtonLabel().Color = blackColor;
			slowTowerButton.GetButtonLabel().Color = blackColor;
			villagerCount.Color = blackColor;
		}
		
		void DisplayLabels()
		{
			basicTowerButton.GetButtonLabel().Color = buttonLabelColor;
			splashTowerButton.GetButtonLabel().Color = buttonLabelColor;
			slowTowerButton.GetButtonLabel().Color = buttonLabelColor;
			villagerCount.Color = buttonLabelColor;
		}
		
		void Reset()
		{
			Director.Instance.ReplaceScene(new LoadScreen(Player.CurrentMission));
		}
		
		void LoadMap(int map)
		{
			Console.WriteLine("Loading map " + map);
			BinaryReader reader = new BinaryReader(File.OpenRead(CommonHelper.MapDirectory + "Map" + map + ".gmf"));
			
			for (int i = 0; i < CommonHelper.GridSize.X; i++)
			{
				for (int j = 0; j < CommonHelper.GridSize.Y; j++)
				{
					gameGrid[i,j].ChangeCellType((CellType)reader.ReadInt32());
					
					if (gameGrid[i,j].GetCellType() == CellType.Start)
						spawnGridPosition = new Vector2i(i, j);
					
					gameGrid[i,j].SetEnumMoveDirection((MoveDirection)reader.ReadInt32());
					
					gameGrid[i,j].SetDirection(new Vector2i(reader.ReadInt32(), reader.ReadInt32()));
					gameGrid[i,j].SetNextGrid(new Vector2i(reader.ReadInt32(), reader.ReadInt32()));
					gameSpriteList.AddChild(gameGrid[i, j].GetSpriteTile());
				}
			}
			
			Console.WriteLine("Map loaded");
			
			enemyWaveList = new List<EnemyWave>(reader.ReadInt32());
			Console.WriteLine("Loading enemy wave data");
			
			for (int i = 0; i < enemyWaveList.Capacity; i++)
			{
				enemyWaveList.Add(new EnemyWave((EnemyType)reader.ReadInt32(), reader.ReadInt32()));
			}
			Console.WriteLine("Enemy wave data loaded");
			enemyManager = new EnemyManager(ref gameSpriteList, ref gameTexture, ref gameTextureInfo, enemyWaveList, spawnGridPosition, ref messenger);
			Console.WriteLine("Enemy manager initiated");
			reader.Close();
		}
		
		void LoadResumeData()
		{
			enemyManager.LoadResumeData();
			List<int> towerIntList = new List<int>();
			Player.LoadTowerData(towerIntList);
			towerManager.LoadResumeData(towerIntList, ref gameTexture, ref gameTextureInfo, ref gameGrid, ref gameSpriteList);
		}
	}
}

