using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public class SlowTower : BasicTower
	{
		public SlowTower (ref Texture2D inputTexture, ref TextureInfo textureInfo, Vector2i gridPosition, ref PlayCell[,] playGrid, ref SpriteList spriteList) : base(TowerType.Slow, ref inputTexture, ref textureInfo, gridPosition, ref playGrid, ref spriteList)
		{
		}
		
		public override void InitTowerSprite (ref SpriteList spriteList)
		{
			spriteTile.TileIndex2D = CommonHelper.TowerSlowBaseTileIndex;
			spriteTile.Pivot = CommonHelper.CellSize/2;
			
			spriteList.AddChild(spriteTile, CommonHelper.DrawOrderTower);
		}
	}
}

