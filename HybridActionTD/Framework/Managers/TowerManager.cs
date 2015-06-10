using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public class TowerManager
	{
		private		List<BasicTower>	towerList;
		
		private		Messenger			messenger;
		
		public TowerManager (ref Messenger messenger)
		{
			towerList = new List<BasicTower>(56);
			
			this.messenger = messenger;
		}
		
		public void BuildTower(TowerType towerType, ref Texture2D texture, ref TextureInfo textureInfo, Vector2i gridPosition, ref PlayCell[,] playGrid, ref SpriteList spriteList)
		{
			switch (towerType)
			{
				case TowerType.Basic:
					towerList.Add(new BasicTower(ref texture, ref textureInfo, gridPosition, ref playGrid, ref spriteList));
					break;
				case TowerType.Slow:
					towerList.Add(new SlowTower(ref texture, ref textureInfo, gridPosition, ref playGrid, ref spriteList));
					break;
				case TowerType.Splash:
					towerList.Add(new SplashTower(ref texture, ref textureInfo, gridPosition, ref playGrid, ref spriteList));
					break;
			} 
		}
		
		public void DestroyTower(int towerIndex, ref SpriteList spriteList)
		{
			towerList[towerIndex].DestroyTower(ref spriteList);
			towerList.RemoveAt(towerIndex);
		}
		
		public void Update(float dt, ref PlayCell[,] playGrid, List<BasicEnemy> enemyList, ref ProjectileManager projectileManager, ref SpriteList spriteList)
		{
			for (int i = 0; i < towerList.Count; i++)
			{
				towerList[i].Update(dt, ref playGrid, enemyList, ref projectileManager, ref spriteList);
//				towerList[i].SetMessenger(ref messenger);
			}
		}
		
		public void SetMessenger(ref Messenger messenger)
		{
			this.messenger = messenger;
		}
		
		public List<BasicTower> GetTowerList()
		{
			return towerList;
		}
		
		public void LoadResumeData(List<int> towerIntList, ref Texture2D texture, ref TextureInfo textureInfo, ref PlayCell[,] playGrid, ref SpriteList spriteList)
		{
			for (int i = 0; i < towerIntList.Count; i+=3)
			{
				BuildTower((TowerType)towerIntList[i], ref texture, ref textureInfo, new Vector2i(towerIntList[i + 1], towerIntList[i + 2]), ref playGrid, ref spriteList);
				playGrid[towerIntList[i + 1], towerIntList[i + 2]].ChangeCellType(CellType.Occupied);
			}
		}
	}
}

