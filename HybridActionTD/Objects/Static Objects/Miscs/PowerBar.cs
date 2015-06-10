using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public class PowerBar : HealthBar
	{
		public PowerBar (int currentPower, int maxPower, ref TextureInfo textureInfo) : base (currentPower, maxPower, ref textureInfo)
		{
			spriteUV.TileIndex2D = CommonHelper.PowerBarTilePosition;
		}
	}
}

