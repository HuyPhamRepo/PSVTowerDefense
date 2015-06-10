using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public class BasicSpell : GameObject
	{
		protected	SpellType			spellType;
		protected	SpellState			spellState;
		
		protected	Vector2				moveDirection;
		protected	Vector2				targetPosition;
		
		protected	int					powerCost;
		protected	int					impactDamage;
		protected	int					impactRadius;
		protected	int					impactRadiusSquared;
		
		protected	SpellEffect			spellEffect;
		
		protected	float				coolTime;
		protected	float				currentCoolTime;
		protected	float				castDuration;
		protected	float				baseMovementSpeed;
		protected	float				castRange;
		
		protected	List<Vector2i>		affectGridList;
		
		protected	Vector2i			targetGrid;
		
		public		bool				isActive;
		
		public BasicSpell (ref Texture2D texture, ref TextureInfo textureInfo) : base(ref texture, ref textureInfo, "Basic Spell")
		{
			currentCoolTime = 0;
			
			spellState = SpellState.Ready;
		}
		
		public BasicSpell (ref Texture2D texture, ref TextureInfo textureInfo, string name) : base(ref texture, ref textureInfo, name)
		{
			currentCoolTime = 0;
			
			spellState = SpellState.Ready;
		}
		
		protected void GetAffectCell(ref PlayCell[,] playGrid, Vector2i gridPosition)
		{
			int lookRange = (int)System.Math.Round(impactRadius / CommonHelper.CellSize.X, 0);
			
			for (int i = gridPosition.X - lookRange; i <= gridPosition.X + lookRange; i++)
			{
				if (i > -1 && i < CommonHelper.GridSize.X)
				{
					for (int j = gridPosition.Y - lookRange; j <= gridPosition.Y + lookRange; j++)
					{
						if (j > -1 && j < CommonHelper.GridSize.Y && playGrid[i,j].GetCellType() == CellType.Ocean)
						{
							affectGridList.Add(new Vector2i(i,j));
						}
					}
				}
			}
		}
		
		protected void GetAffectCell(ref PlayCell[,] playGrid, Vector2 targetPosition)
		{
			int lookRange = (int)System.Math.Round(impactRadius / CommonHelper.CellSize.X, 0);
			
			Vector2i gridPosition = new Vector2i((int)((targetPosition.X - CommonHelper.ScreenPadding.X) / CommonHelper.CellSize.X), (int)((targetPosition.Y - CommonHelper.ScreenPadding.Y) / CommonHelper.CellSize.Y));
			targetGrid = gridPosition;
			this.targetPosition.X = gridPosition.X * CommonHelper.CellSize.X + CommonHelper.ScreenPadding.X;
			this.targetPosition.Y = gridPosition.Y * CommonHelper.CellSize.Y + CommonHelper.ScreenPadding.Y;
			
			for (int i = gridPosition.X - lookRange; i <= gridPosition.X + lookRange; i++)
			{
				if (i > -1 && i < CommonHelper.GridSize.X)
				{
					for (int j = gridPosition.Y - lookRange; j <= gridPosition.Y + lookRange; j++)
					{
						if (j > -1 && j < CommonHelper.GridSize.Y && playGrid[i,j].GetCellType() == CellType.Ocean)
						{
							affectGridList.Add(new Vector2i(i,j));
						}
					}
				}
			}
		}
		
		protected void GetAffectCell(ref PlayCell[,] playGrid, Vector2 targetPosition, int HalfWidth, int Length)
		{
			Vector2i gridPosition = new Vector2i((int)((targetPosition.X - CommonHelper.ScreenPadding.X) / CommonHelper.CellSize.X), (int)((targetPosition.Y - CommonHelper.ScreenPadding.Y) / CommonHelper.CellSize.Y));
			targetGrid = gridPosition;
			
			for (int i = targetGrid.X; i < targetGrid.X + Length + 1; i++)
			{
				if (i > -1 && i < CommonHelper.GridSize.X)
				{
					for (int j = targetGrid.Y - HalfWidth; j < targetGrid.Y + HalfWidth + 1; j++)
					{
						if (j > -1 && j < CommonHelper.GridSize.Y && playGrid[i,j].GetCellType() == CellType.Ocean)
						{
							affectGridList.Add(new Vector2i(i,j));
						}
					}
				}
			}
		}
		
		public float GetCastDuration()
		{
			return castDuration;
		}
		
		public List<Vector2i> GetAffectList()
		{
			return affectGridList;
		}
		
		public SpellState GetSpellState()
		{
			return spellState;
		}
		
		public float GetCastRange()
		{
			return castRange;
		}
		
		public int	GetPowerCost()
		{
			return powerCost;
		}
	}
}