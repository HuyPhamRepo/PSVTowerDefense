using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public class BasicTower : GameObject
	{
		protected TowerType			towerType;
		
		protected TowerState		currentState;
		
		protected Vector2i			gridPosition;
		
		protected float				lastUsedDistance;
		protected float				currentDistance;
		
		protected int				targetIndex;
		protected int				attackRange;
		protected int				attackRangeSquared;
		protected int				attackDamage;
		
		protected float				coolDownTime;
		protected float				currentCoolTime;
		
		protected List<Vector2i>	targetCellList;
		
		public BasicTower (ref Texture2D inputTexture, ref TextureInfo textureInfo, Vector2i gridPosition, ref PlayCell[,] playGrid, ref SpriteList spriteList) : base(ref inputTexture, ref textureInfo, CommonHelper.TowerBasicName)
		{
			this.towerType = TowerType.Basic;
			this.currentState = TowerState.Idle;
			this.targetIndex = -1;
			this.attackRange = CommonHelper.TowerBasicAttackRange;
			this.attackRangeSquared = attackRange * attackRange;
			this.gridPosition = gridPosition;
			
			attackDamage = CommonHelper.TowerBasicAP;
			
			coolDownTime = CommonHelper.TowerBasicAS;
			currentCoolTime = 0;
			
			targetCellList = new List<Vector2i>(6);
			
			Init(new Vector2(playGrid[gridPosition.X, gridPosition.Y].GetPosition().X + 10, playGrid[gridPosition.X, gridPosition.Y].GetPosition().Y));
			
			GetTargetCells(ref playGrid);
			
			InitTowerSprite(ref spriteList);
		}
		
		public BasicTower (TowerType type, ref Texture2D inputTexture, ref TextureInfo textureInfo, Vector2i gridPosition, ref PlayCell[,] playGrid, ref SpriteList spriteList) : base(ref inputTexture, ref textureInfo, CommonHelper.TowerBasicName)
		{
			this.towerType = type;
			this.currentState = TowerState.Idle;
			this.targetIndex = -1;
			this.attackRange = CommonHelper.TowerBasicAttackRange;
			this.attackRangeSquared = attackRange * attackRange;
			this.gridPosition = gridPosition;
			
			attackDamage = CommonHelper.TowerBasicAP;
			
			coolDownTime = CommonHelper.TowerBasicAS;
			currentCoolTime = 0;
			
			targetCellList = new List<Vector2i>(6);
			
			Init(new Vector2(playGrid[gridPosition.X, gridPosition.Y].GetPosition().X, playGrid[gridPosition.X, gridPosition.Y].GetPosition().Y));
			
			GetTargetCells(ref playGrid);
			
			InitTowerSprite(ref spriteList);
		}
		
		public virtual void InitTowerSprite(ref SpriteList spriteList)
		{
			spriteTile.TileIndex2D = CommonHelper.TowerBasicBaseTileIndex;
			spriteTile.Pivot = CommonHelper.TowerBasicPivot;
			
			spriteList.AddChild(spriteTile, CommonHelper.DrawOrderTower);
		}
		
		public virtual void GetTargetCells(ref PlayCell[,] playGrid)
		{
			int lookRange = (int)System.Math.Round(attackRange / CommonHelper.CellSize.X, 0);
			
			for (int i = gridPosition.X - lookRange; i < gridPosition.X + lookRange + 1; i++)
			{
				if (i > -1 && i < CommonHelper.GridSize.X)
				{
					for (int j = gridPosition.Y - lookRange; j < gridPosition.Y + lookRange + 1; j++)
					{
						if (j > -1 && j < CommonHelper.GridSize.Y && playGrid[i,j].GetCellType() == CellType.Ocean)
						{
							float a = CommonHelper.GetDistanceSquared(centerPosition, playGrid[i,j].GetCenterPosition());
							if (a <= attackRangeSquared)
							{
								targetCellList.Add(new Vector2i(i,j));
							}
						}
					}
				}
			}
		}
		
		public virtual void Update(float dt, ref PlayCell[,] playGrid, List<BasicEnemy> enemyList, ref ProjectileManager projectileManager, ref SpriteList spriteList)
		{
			switch (currentState)
			{
				case TowerState.Idle:
					//currentDistance = 0;
					for (int i = 0; i < targetCellList.Count; i++)
					{
						for (int j = 0; j < playGrid[targetCellList[i].X, targetCellList[i].Y].GetEnemyList().Count; j++)
						{
							if (CommonHelper.GetDistance(enemyList[playGrid[targetCellList[i].X, targetCellList[i].Y].GetEnemyList()[j]].GetCenterPosition(), centerPosition) <  attackRange)
							//if (enemyList[playGrid[targetCellList[i].X, targetCellList[i].Y].GetEnemyList()[j]].travelledDistance > highestDistance)
							{
								targetIndex = playGrid[targetCellList[i].X, targetCellList[i].Y].GetEnemyList()[j];
								//highestDistance = enemyList[playGrid[targetCellList[i].X, targetCellList[i].Y].GetEnemyList()[j]].travelledDistance;
							}
						}
					}
					if (targetIndex > -1)
					{
						if (currentCoolTime <= 0)
							currentState = TowerState.Attacking;
						else
							currentState = TowerState.Cooling;
					}
					break;
				
				case TowerState.Attacking:
					float distance = CommonHelper.GetDistance(centerPosition, enemyList[targetIndex].GetCenterPosition());
					if (CheckTargetDistance(distance))
				   	{
						RotateSprite(distance, enemyList[targetIndex].GetCenterPosition());
						Shoot(ref projectileManager, ref spriteList, ref playGrid, enemyList);
						currentState = TowerState.Cooling;
						currentCoolTime = coolDownTime;
					}
					else
					{
						targetIndex = -1;
						currentState = TowerState.Idle;
					}
					break;
				
				case TowerState.Cooling:
					float distance2 = CommonHelper.GetDistance(centerPosition, enemyList[targetIndex].GetCenterPosition());
					if (CheckTargetDistance(distance2))
				   	{
						RotateSprite(distance2, enemyList[targetIndex].GetCenterPosition());
						if (currentCoolTime <= 0)
						{
							currentCoolTime = coolDownTime;
							currentState = TowerState.Attacking;
						}
						else
							currentCoolTime -= dt;
					}
					else
					{
						targetIndex = -1; 
						currentState = TowerState.Idle;
					}
					break;
			}
		}
		
		public void DestroyTower(ref SpriteList spriteList)
		{
			spriteList.RemoveChild(spriteTile, true);
		}
		
		public void Shoot(ref ProjectileManager projectileManager, ref SpriteList spriteList, ref PlayCell[,] playGrid, List<BasicEnemy> enemyList)
		{
			projectileManager.ShootProjectile(this.towerType, ref spriteList, ref playGrid, position, enemyList, targetIndex, attackDamage);
		}
		
		void RotateSprite(float distance, Vector2 targetPosition)
		{
			spriteTile.RotationNormalize = new Vector2((targetPosition.X - centerPosition.X) / distance, (targetPosition.Y - centerPosition.Y) / distance);
		}
		
		bool CheckTargetDistance(float distance)
		{
			if (distance > attackRange)
				return false;
			return true;
		}
		
		public Vector2i GetGridPosition()
		{
			return gridPosition;
		}
		
		public TowerType GetTowerType()
		{
			return towerType;
		}
	}
}

