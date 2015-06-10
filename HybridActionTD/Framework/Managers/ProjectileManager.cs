using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public class ProjectileManager
	{
		protected	List<BasicProjectile>	projectileList;
		protected	List<SlowProjectile>	slowProjectileList;
		protected	List<SplashProjectile>	splashProjectileList;
		
		protected	int						lastProjectileIndex;
		protected	int						lastSlowProjectileIndex;
		protected	int						lastSplashProjectileIndex;
		
		private		Messenger				messenger;
		
		public ProjectileManager (ref Texture2D texture, ref TextureInfo textureInfo, ref Messenger messenger)
		{
			projectileList = new List<BasicProjectile>(CommonHelper.MAX_PROJECTILE_COUNT);
			lastProjectileIndex = -1;
			
			slowProjectileList = new List<SlowProjectile>(CommonHelper.MAX_PROJECTILE_COUNT);
			lastSlowProjectileIndex = -1;
			
			splashProjectileList = new List<SplashProjectile>(CommonHelper.MAX_PROJECTILE_COUNT);
			lastSplashProjectileIndex = -1;
			
			InitProjectilePool(ref texture, ref textureInfo);
			
			this.messenger = messenger;
		}
		
		public void InitProjectilePool(ref Texture2D texture, ref TextureInfo textureInfo)
		{
			for (int i = 0; i < CommonHelper.MAX_PROJECTILE_COUNT; i++)
			{
				projectileList.Add(new BasicProjectile(ref texture, ref textureInfo));
				slowProjectileList.Add(new SlowProjectile(ref texture, ref textureInfo));
				splashProjectileList.Add(new SplashProjectile(ref texture, ref textureInfo));
			}
		}
		
		public void ShootProjectile(TowerType type, ref SpriteList spriteList, ref PlayCell[,] playGrid, Vector2 shootPosition, List<BasicEnemy> enemyList, int targetIndex, int damage)
		{
			switch(type)
			{
			case TowerType.Basic:
				for (int i = 0; i < CommonHelper.MAX_PROJECTILE_COUNT; i++)
				{
					if (!projectileList[i].isActive)
					{
						projectileList[i].InitProjectile(ref spriteList, shootPosition, enemyList[targetIndex].GetPosition(), targetIndex, CommonHelper.TowerBasicAP);
						if (i > lastProjectileIndex)
							lastProjectileIndex = i;
						
						return;
					}
				}
				break;
			case TowerType.Slow:
				for (int i = 0; i < CommonHelper.MAX_PROJECTILE_COUNT; i++)
				{
					if (!slowProjectileList[i].isActive)
					{
						slowProjectileList[i].InitProjectile(ref spriteList, shootPosition, enemyList[targetIndex].GetPosition(), targetIndex, CommonHelper.TowerSlowAP);
						if (i > lastSlowProjectileIndex)
							lastSlowProjectileIndex = i;
						
						return;
					}
				}
				break;
			case TowerType.Splash:
				for (int i = 0; i < CommonHelper.MAX_PROJECTILE_COUNT; i++)
				{
					if (!splashProjectileList[i].isActive)
					{
						splashProjectileList[i].InitProjectile(ref spriteList, ref playGrid, shootPosition, enemyList[targetIndex].GetPosition(),enemyList[targetIndex].GetCurrentGrid() , targetIndex, CommonHelper.TowerSplashAP);
						if (i > lastSplashProjectileIndex)
							lastSplashProjectileIndex = i;
						
						return;
					}
				}
				break;
			}
		}
		
		public void Update(float dt, ref SpriteList spriteList, List<BasicEnemy> enemyList, ref PlayCell[,] playGrid)
		{
			for (int i = lastProjectileIndex; i > -1; i--)
				projectileList[i].Update(dt, ref spriteList, enemyList);
			for (int i = lastSlowProjectileIndex; i > -1; i--)
				slowProjectileList[i].Update(dt, ref spriteList, enemyList);
			for (int i = lastSplashProjectileIndex; i > -1; i--)
				splashProjectileList[i].Update(dt, ref spriteList, enemyList, ref playGrid);
		}		
		
		public void SetMessenger(ref Messenger messenger)
		{
			this.messenger = messenger;
		}
		
//		public
	}
}

