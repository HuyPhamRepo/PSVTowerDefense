using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public class FireBall : BasicSpell
	{
//		private SpriteTile		shadowSpriteTile;
		
		public FireBall (ref Texture2D texture, ref TextureInfo textureInfo) : base (ref texture, ref textureInfo, "Fire Ball")
		{
			coolTime = CommonHelper.SpellFireBallCoolTime;
			powerCost = CommonHelper.SpellFireBallPowerCost;
			castDuration = CommonHelper.SpellFireBallCastDuration;
			baseMovementSpeed = CommonHelper.SpellFireBallMS;
			castRange = CommonHelper.SpellFireBallCastRange;
			
			impactDamage = CommonHelper.SpellFireBallDamage;
			impactRadius = CommonHelper.SpellFireBallRoE;
			impactRadiusSquared = impactRadius * impactRadius;
			
			spellEffect = SpellEffect.Normal;
			
			spellType = SpellType.FireBall;
			
			moveDirection = new Vector2();
			
			affectGridList = new List<Vector2i>(8);
			
			isActive = false;
		}
		
		public void Init(ref SpriteList spriteList, Vector2 position, ref PlayCell[,] playGrid, Vector2i gridPosition, Vector2 targetPosition)
		{
			currentCoolTime = 0;
			
			Init(position);
			
			targetGrid = gridPosition;
			
			moveDirection = targetPosition - position;
			moveDirection.Normalize();
			moveDirection *= baseMovementSpeed;
			
			GetAffectCell(ref playGrid, gridPosition);
			
			spriteTile.RotationNormalize = GetRotation(playGrid[gridPosition.X, gridPosition.Y].GetPosition());
			spriteTile.TileIndex2D = CommonHelper.SpellFireBallTileIndex;
			spriteList.AddChild(spriteTile);
			
			isActive = true;
		}
		
		public void Init(ref SpriteList spriteList, Vector2 position, ref PlayCell[,] playGrid, Vector2 gridPosition)
		{
			currentCoolTime = 0;
			
			Init(position);
			
			GetAffectCell(ref playGrid, gridPosition);
						moveDirection = targetPosition - position;
			moveDirection.Normalize();
			moveDirection *= baseMovementSpeed;
			
			spriteTile.RotationNormalize = GetRotation(gridPosition);
			spriteTile.TileIndex2D = CommonHelper.SpellFireBallTileIndex;
			spriteList.AddChild(spriteTile, 900);
			
			isActive = true;
		}
		
		public Vector2 GetRotation(Vector2 targetPosition)
		{
			float distance = CommonHelper.GetDistance(position, targetPosition);
			return new Vector2((targetPosition.X - centerPosition.X) / distance, (targetPosition.Y - centerPosition.Y) / distance);
		}
		
		public void Reset(ref SpriteList spriteList)
		{
			affectGridList.Clear();
			
			spriteList.RemoveChild(spriteTile, true);
			
			moveDirection = new Vector2(0, 0);
			currentCoolTime = 0;
			
			targetGrid = new Vector2i(0,0);
			
			isActive = false;
		}
		
		public void Update(float dt, ref PlayCell[,] playGrid, ref SpriteList spriteList, ref EnemyManager enemyManager)
		{
			if (isActive)
			{
				switch (spellState)
				{
					case SpellState.Casting:
						if (CommonHelper.GetDistanceSquared(position, targetPosition) > 100)
							SetPosition(position + moveDirection);
						else
						{
							for (int i = 0; i < affectGridList.Count; i++)
							{
								for (int j = 0; j < playGrid[affectGridList[i].X, affectGridList[i].Y].GetEnemyList().Count; j++)
								{
									if (CommonHelper.GetDistanceSquared(enemyManager.GetEnemyList()[playGrid[affectGridList[i].X, affectGridList[i].Y].GetEnemyList()[j]].GetCenterPosition(), centerPosition) <= impactRadiusSquared)
										enemyManager.GetEnemyList()[playGrid[affectGridList[i].X, affectGridList[i].Y].GetEnemyList()[j]].GetHit(impactDamage);
								}
							}
							
							spellState = SpellState.Cooling;
						}
						break;
					case SpellState.Cooling:
						if (currentCoolTime < coolTime)
							currentCoolTime += dt;
						else
						{
							currentCoolTime = 0;
							spellState = SpellState.Ready;
							Reset(ref spriteList);
						}
						break;
					case SpellState.Ready:
						break;
				}
			}
		}
		
		public void CastSpell(ref SpriteList spriteList, Vector2 position, ref PlayCell[,] playGrid, Vector2 targetPosition)
		{
			spellState = SpellState.Casting;
			Init(ref spriteList, position, ref playGrid, targetPosition);
		}
	}
}

