using System;

using Sce.PlayStation.HighLevel.GameEngine2D;

namespace HybridActionTD
{
	public class Messenger
	{
		private ProjectileManager	projectileManager;
		private EnemyManager		enemyManager;
		private TowerManager		towerManager;
		private SpriteList			gameSpriteList;
		private PlayCell[,]			playGrid;
		private GameScreen			gameScreen;
		private SpriteList			buttonSpriteList;
		
		public Messenger ()
		{
		}
		
		public void SetReferences(GameScreen gameScreen, ref ProjectileManager projectileManager, ref EnemyManager enemyManager, ref TowerManager towerManager, ref SpriteList spriteList, ref SpriteList buttonSpriteList, ref PlayCell[,] playGrid)
		{
			this.projectileManager = projectileManager;
			this.enemyManager = enemyManager;
			this.towerManager = towerManager;
			this.gameSpriteList = spriteList;
			this.playGrid = playGrid;
			this.buttonSpriteList = buttonSpriteList;
			this.gameScreen = gameScreen;
		}
		
		public ProjectileManager GetProjectileManager()
		{
			return this.projectileManager;
		}
		
		public SpriteList GetTextureSpriteList()
		{
			return this.gameSpriteList;
		}
		
		public EnemyManager GetEnemyManager()
		{
			return this.enemyManager;
		}
		
		public PlayCell[,] GetGameGrid()
		{
			return this.playGrid;
		}
		
		public TowerManager GetTowerManager()
		{
			return this.towerManager;
		}
		
		public SpriteList GetButtonSpriteList()
		{
			return buttonSpriteList;
		}
		
		public GameScreen GetGameScreen()
		{
			return gameScreen;
		}
	}
}

