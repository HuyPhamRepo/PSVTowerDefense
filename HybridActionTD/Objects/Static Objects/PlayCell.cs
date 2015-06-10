using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public class PlayCell : GameObject
	{
		protected	Vector2i			gridPosition;
		protected	Vector2i			nextGridPosition;
		
		protected	CellType			cellType;
		
		protected	List<int>			enemyList;
		
		protected	int					towerIndex;
		
		protected	Vector2i			moveDirection;
		
		protected	MoveDirection		enumMoveDirection;
		
		public PlayCell (ref Texture2D inputTexture, ref TextureInfo textureInfo, CellType type, int x, int y) : base(ref inputTexture, ref textureInfo, "Play Cell")
		{
			gridPosition = new Vector2i(x, y);
			nextGridPosition = new Vector2i(-1, -1);
			enemyList = new List<int>(5);
			towerIndex = -1;
			Init(type, new Vector2(x * CommonHelper.CellSize.X + CommonHelper.ScreenPadding.X, y * CommonHelper.CellSize.Y + CommonHelper.ScreenPadding.Y));
		}
		
		public void Init(CellType type, Vector2 position)
		{
			ChangeCellType(type);
			base.Init(position);
		}
		
		public void ChangeCellType (CellType type)
		{
			this.cellType = type;
			switch(cellType)
			{
			case CellType.Ocean: 
				spriteTile.TileIndex2D = CommonHelper.CellDefaultTile;
				break;
			case CellType.Sea:
				spriteTile.TileIndex2D = new Vector2i((int)CellType.Sea, 0);
				break;
			case CellType.Land:
			case CellType.Occupied: 
				spriteTile.TileIndex2D = new Vector2i((int)CellType.Land, 0);
				break;
			case CellType.Start:
				spriteTile.TileIndex2D = new Vector2i((int)CellType.Start, 0);
				break;
			case CellType.End:
				spriteTile.TileIndex2D = new Vector2i((int)CellType.Start, 0);
				break;
			}
		}
		
		public CellType GetCellType()
		{
			return cellType;
		}
		
		public List<int> GetEnemyList()
		{
			return enemyList;
		}
		
		public void SetDirection(Vector2i dir)
		{
			this.moveDirection = dir;
		}
		
		public Vector2i GetMoveDirection()
		{
			return moveDirection;
		}
		
		public void SetNextGrid(Vector2i next)
		{
			nextGridPosition = next;
		}
		
		public Vector2i GetNextGrid()
		{
			return nextGridPosition;
		}
		
		public void SetTowerIndex(int index)
		{
			this.towerIndex = index;
		}
			
		public int GetTowerIndex()
		{
			return towerIndex;
		}
		
		public MoveDirection GetEnumMoveDirection()
		{
			return enumMoveDirection;
		}
		
		public void SetEnumMoveDirection(MoveDirection moveDir)
		{
			this.enumMoveDirection = moveDir;
		}
	}
}

