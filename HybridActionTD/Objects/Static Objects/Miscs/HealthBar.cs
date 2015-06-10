using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public class HealthBar
	{
		protected SpriteTile	spriteUV;
		
		protected int			currentDisplayHealth;
		protected int			maxHealth;
		
		protected float		healthDisplayRatio;
		
		public HealthBar (int currentHealth, int maxHealth, ref TextureInfo textureInfo)
		{
			currentDisplayHealth = currentHealth;
			this.maxHealth = maxHealth;
			
			healthDisplayRatio = 40.0f / maxHealth;
			
			spriteUV = new SpriteTile(textureInfo);
			spriteUV.TileIndex2D = new Vector2i(1, 14);
			
			HealthToDisplay();
		}
		
		public void SetPosition(float x, float y)
		{
//			blackSprite.Position = new Vector2(x - 1, y - 1);
//			greenSprite.Position = new Vector2(x, y);
			spriteUV.Position = new Vector2(x + CommonHelper.CellSize.X / 2 - 40 / 2, y);
		}
		
//		public void SetPosition(Vector2 position)
//		{
////			blackSprite.Position = position - 1;
////			greenSprite.Position = position;
//			spriteUV.Position = position;
//		}
		
		public void SetHealth(int health)
		{
			currentDisplayHealth = health;
			HealthToDisplay();
		}
		
		public SpriteTile GetHealthBar()
		{
			return spriteUV;
		}
		
//		public SpriteUV GetHealthOutline()
//		{
//			return blackSprite;
//		}
		
		public void HealthToDisplay()
		{
			float scaleX = healthDisplayRatio * currentDisplayHealth;
			spriteUV.Scale = new Vector2(scaleX, 5);
		}
		
		public void Update(float dt, int health)
		{
			currentDisplayHealth = health;
			HealthToDisplay();
		}
	}
}

