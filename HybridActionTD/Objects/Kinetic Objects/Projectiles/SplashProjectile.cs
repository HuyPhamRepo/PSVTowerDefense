using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public class SplashProjectile : BasicProjectile
	{
		private	List<Vector2i>	affectGridList;
		private	int				affectRange;
		private	int				affectRangeSquared;
		private float			splashPercentage;
		
		public SplashProjectile (ref Texture2D texture, ref TextureInfo textureInfo)
			: base(ref texture, ref textureInfo, "Splash Projectile", ProjectileType.Slow, CommonHelper.ProjectileSplashTilePosition, CommonHelper.ProjectileSplashImpactTilePosition, CommonHelper.ProjectileSplashSpeed, CommonHelper.ProjectileSplashDisappearTime)
		{
			affectGridList = new List<Vector2i>(9);
			affectRange = CommonHelper.TowerSplashSplashDmgRange;
			splashPercentage = CommonHelper.ProjectileSplashPercentage;
			affectRangeSquared = affectRange * affectRange;
		}
		
		public void InitProjectile(ref SpriteList spriteList, ref PlayCell[,] playGrid, Vector2 position, Vector2 targetPosition, Vector2i targetGridPosition, int targetIndex, int damage)
		{
			InitProjectile(ref spriteList, position, targetPosition, targetIndex, damage);
			
			int lookRange = (int)System.Math.Round(affectRange / CommonHelper.CellSize.X, 0);
			
			for (int i = targetGridPosition.X - lookRange; i < targetGridPosition.X + lookRange + 1; i++)
			{
				if (i > -1 && i < CommonHelper.GridSize.X)
				{
					for (int j = targetGridPosition.Y - lookRange; j < targetGridPosition.Y + lookRange + 1; j++)
					{
						if (j > -1 && j < CommonHelper.GridSize.Y && playGrid[i,j].GetCellType() == CellType.Ocean)
						{
							affectGridList.Add(new Vector2i(i,j));
						}
					}
				}
			}
			
		}
		
		public override void ResetParameters (ref SpriteList spriteList)
		{
			base.ResetParameters (ref spriteList);
			
			affectGridList.Clear();
		}
		
		public void Update(float dt, ref SpriteList spriteList, List<BasicEnemy> enemyList, ref PlayCell[,] playGrid)
		{
			if (isActive)
			{
				if (isHit)
				{
					if (currentDisappearTime >= disappearTime)
					{
						ResetParameters(ref spriteList);
					}
					else
					{
						currentDisappearTime += dt;
					}
				}
				else
				{
					if (CommonHelper.IsInside(centerPosition, enemyList[targetIndex].GetHitBox()))
					{
						enemyList[targetIndex].GetHit((int)(impactDamage * (1 - splashPercentage)));
						for (int i = 0; i < affectGridList.Count; i++)
						{
							for (int j = 0; j < playGrid[affectGridList[i].X, affectGridList[i].Y].GetEnemyList().Count; j++)
							{
								if (CommonHelper.GetDistanceSquared(enemyList[playGrid[affectGridList[i].X, affectGridList[i].Y].GetEnemyList()[j]].GetCenterPosition(), centerPosition) <= affectRangeSquared)
								{
									enemyList[playGrid[affectGridList[i].X, affectGridList[i].Y].GetEnemyList()[j]].GetHit((int)(impactDamage * splashPercentage));
								}
							}
						}
						spriteList.RemoveChild(spriteTile, false);
						impactSpriteTile.Position = position;
						spriteList.AddChild(impactSpriteTile, CommonHelper.DrawOrderProjectile);
						isHit = true;
					}
					else if (CheckOutOfScreen())
					{
						ResetParameters(ref spriteList);
					}
					else
					{
						SetPosition(position + directionVector);
					}
				}
			}
		}
	}
}

