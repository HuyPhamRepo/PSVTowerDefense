using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public class SplashTower : BasicTower
	{
		public SplashTower (ref Texture2D inputTexture, ref TextureInfo textureInfo, Vector2i gridPosition, ref PlayCell[,] playGrid, ref SpriteList spriteList) : base(TowerType.Splash, ref inputTexture, ref textureInfo, gridPosition, ref playGrid, ref spriteList)
		{
		}
		
		public override void InitTowerSprite (ref SpriteList spriteList)
		{
			spriteTile.TileIndex2D = CommonHelper.TowerSplashBaseTileIndex;
			spriteTile.Pivot = CommonHelper.CellSize/2;
			
			spriteList.AddChild(spriteTile, CommonHelper.DrawOrderTower);
		}
	}
}

