using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public class SlowProjectile : BasicProjectile
	{
		private float	slowPercentage;
		private	float	slowDuration;
		
		public SlowProjectile (ref Texture2D texture, ref TextureInfo textureInfo) 
			: base(ref texture, ref textureInfo, "Slow Projectile", ProjectileType.Slow, CommonHelper.ProjectileSlowTilePosition, CommonHelper.ProjectileSlowImpactTilePosition, CommonHelper.ProjectileSlowSpeed, CommonHelper.ProjectileSlowDisappearTime)
		{
			slowPercentage = CommonHelper.ProjectileSlowPercentage;
			slowDuration = CommonHelper.ProjectileSlowDuration;
		}
		
		public override void Update(float dt, ref SpriteList spriteList, List<BasicEnemy> enemyList)
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
					if (CommonHelper.IsInside(centerPosition, enemyList[targetIndex].GetHitBox()))//CommonHelper.IsInside(centerPosition, enemyList[targetIndex].GetBoundingBox()))
					{
						enemyList[targetIndex].GetHit(impactDamage);
						enemyList[targetIndex].GetSlowed(slowPercentage, slowDuration);
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

