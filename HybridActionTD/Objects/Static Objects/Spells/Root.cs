using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public class Root : BasicSpell
	{
		protected	float	slowDuration;
		protected	float	slowPercentage;
//		protected	float	amplifyPercentage;
		protected	int		effectLength;
		protected	float	affectTime;
		protected	float	currentAffectTime;
		
		public Root (ref Texture2D texture, ref TextureInfo textureInfo) : base (ref texture, ref textureInfo, CommonHelper.SpellRootName)
		{
			coolTime = CommonHelper.SpellRootCoolTime;
			currentCoolTime = 0;
			castRange = CommonHelper.SpellRootCastRange;
			powerCost = CommonHelper.SpellRootPowerCost;
			impactDamage = CommonHelper.SpellRootDamage;
			
			slowDuration = CommonHelper.SpellRootSlowDuration;
			slowPercentage = CommonHelper.SpellRootSlowPercentage;
//			amplifyPercentage = CommonHelper.SpellRootAplifyPercentage;
			effectLength = CommonHelper.SpellRootLength;
			affectTime = CommonHelper.SpellRootAffectTime;
			
			spellEffect = SpellEffect.Slowed;
			
			spellType = SpellType.Root;
			
			affectGridList = new List<Vector2i>(8);
			
			isActive = false;
		}
		
		public void Init(ref SpriteList spriteList, ref PlayCell[,] playGrid, Vector2 gridPosition)
		{
			currentCoolTime = 0;
			
			currentAffectTime = 0;
			
			Init(gridPosition);
			
			GetAffectCell(ref playGrid, gridPosition, 0, effectLength);
			
			spriteTile.TileIndex2D = CommonHelper.SpellRootTileIndex;
			spriteList.AddChild(spriteTile, 900);
			
			isActive = true;
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
						if (currentAffectTime < affectTime)
							currentAffectTime += dt;
						else
						{
							for (int i = 0; i < affectGridList.Count; i++)
							{
								for (int j = 0; j < playGrid[affectGridList[i].X, affectGridList[i].Y].GetEnemyList().Count; j++)
								{
									enemyManager.GetEnemyList()[playGrid[affectGridList[i].X, affectGridList[i].Y].GetEnemyList()[j]].GetHit(impactDamage);
									enemyManager.GetEnemyList()[playGrid[affectGridList[i].X, affectGridList[i].Y].GetEnemyList()[j]].GetSlowed(slowPercentage, slowDuration);
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
		
		public void CastSpell(ref SpriteList spriteList, ref PlayCell[,] playGrid, Vector2 targetPosition)
		{
			spellState = SpellState.Casting;
			Init(ref spriteList, ref playGrid, targetPosition);
		}
	}
}

