using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public class BasicProjectile : GameObject
	{
		protected	ProjectileType	projectileType;
		
		protected	float			projectileSpeed;
		protected	float			disappearTime;
		protected	float			currentDisappearTime;
		
		protected	int				targetIndex;
		protected	int				impactDamage;
		
		protected	SpriteTile		impactSpriteTile;
		
		public		bool			isActive;
		public		bool			isHit;
		
		protected	Vector2			directionVector;
		
		public BasicProjectile (ref Texture2D texture, ref TextureInfo textureInfo) : base(ref texture, ref textureInfo, "Basic Projectile")
		{
			projectileType = ProjectileType.Basic;
			
			impactSpriteTile = new SpriteTile(textureInfo);
			impactSpriteTile.Quad.S = CommonHelper.CellSize;
			impactSpriteTile.Pivot = CommonHelper.CellSize/2;
			
			spriteTile.TileIndex2D = CommonHelper.ProjectileBasicTilePosition;
			
			impactSpriteTile.TileIndex2D = CommonHelper.ProjectileBasicImpactTilePosition;
			
			projectileSpeed = CommonHelper.ProjectileBasicSpeed;
			
			currentDisappearTime = 0;
			disappearTime = CommonHelper.ProjectileBasicDisappearTime;
			
			directionVector = new Vector2();
			
			isActive = false;
			isHit = false;
		}
		
		public BasicProjectile (ref Texture2D texture, ref TextureInfo textureInfo, string name, ProjectileType type, Vector2i projectileDefaultTile, Vector2i projectileImpactTile, float baseSpeed, float disappearTime) : base(ref texture, ref textureInfo, name)
		{
			projectileType = type;
			
			impactSpriteTile = new SpriteTile(textureInfo);
			impactSpriteTile.Quad.S = CommonHelper.CellSize;
			impactSpriteTile.Pivot = CommonHelper.CellSize/2;
			
			spriteTile.TileIndex2D = projectileDefaultTile;
			
			impactSpriteTile.TileIndex2D = projectileImpactTile;
			
			projectileSpeed = baseSpeed;
			
			currentDisappearTime = 0;
			this.disappearTime = disappearTime;
			
			directionVector = new Vector2();
			
			isActive = false;
			isHit = false;
		}
		
		public virtual void InitProjectile(ref SpriteList spriteList, Vector2 position, Vector2 targetPosition, int targetIndex, int damage)
		{
			Init(position);
			spriteList.AddChild(spriteTile, CommonHelper.DrawOrderProjectile);
			
			directionVector = (targetPosition - this.position);
			directionVector.Normalize();
			directionVector *= projectileSpeed;
			
			impactDamage = damage;
			
			this.targetIndex = targetIndex;
			
			isActive = true;
		}
		
		public virtual void ResetParameters(ref SpriteList spriteList)
		{
			targetIndex = -1;
			
			directionVector = new Vector2();
			
			currentDisappearTime = 0;
			
			spriteList.RemoveChild(impactSpriteTile, true);
			
			isActive = false;
			isHit = false;
		}
		
		public virtual void Update(float dt, ref SpriteList spriteList, List<BasicEnemy> enemyList)
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
		
		public bool CheckOutOfScreen()
		{
			return (position.X > CommonHelper.ScreenSize.X || position.X < 0 || position.Y < 0 || position.Y > CommonHelper.ScreenSize.Y);
		}
	}
}

