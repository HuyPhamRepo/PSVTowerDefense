using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public class TankyEnemy : BasicEnemy
	{
		public TankyEnemy (ref SpriteList spriteList, ref Texture2D texture, ref TextureInfo textureInfo, Vector2i defaultTileIndex, float baseMovementSpeed, int healthPoint) : base (EnemyType.Tank, ref spriteList, ref texture, ref textureInfo, defaultTileIndex, baseMovementSpeed, healthPoint)
		{
		}
	}
}

