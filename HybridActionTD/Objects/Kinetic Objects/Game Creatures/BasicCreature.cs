using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public class BasicCreature : GameObject
	{
		protected float 			baseMovementSpeed;
		
		protected int				healthPoint;
		
		protected Vector2 			currentMovementSpeed;
		
		protected Vector2i			currentGridPosition;
		
		public BasicCreature (ref Texture2D texture, ref TextureInfo textureInfo, float baseMovementSpeed, int healthPoint) : base (ref texture, ref textureInfo, "Basic Creature")
		{
			this.baseMovementSpeed = baseMovementSpeed;
			this.healthPoint = healthPoint;
			this.currentMovementSpeed = new Vector2(baseMovementSpeed);
			currentGridPosition = new Vector2i(-1, -1);
		}
		
		public BasicCreature (ref Texture2D texture, ref TextureInfo textureInfo, float baseMovementSpeed, int healthPoint, string name) : base (ref texture, ref textureInfo, name)
		{
			this.baseMovementSpeed = baseMovementSpeed;
			this.healthPoint = healthPoint;
			this.currentMovementSpeed = new Vector2(baseMovementSpeed);
			currentGridPosition = new Vector2i(-1, -1);
		}
		
		public BasicCreature (ref Texture2D texture, ref TextureInfo textureInfo, Vector2 position, int gridPositionX, int gridPositionY, float baseMovementSpeed, int healthPoint) : base (ref texture, ref textureInfo, "Basic Creature")
		{
			this.baseMovementSpeed = baseMovementSpeed;
			this.healthPoint = healthPoint;
			this.currentMovementSpeed = new Vector2(baseMovementSpeed);
			currentGridPosition = new Vector2i(gridPositionX, gridPositionY);
			Init(position);
		}
		
		public BasicCreature (ref Texture2D texture, ref TextureInfo textureInfo, Vector2 position, int gridPositionX, int gridPositionY, float baseMovementSpeed, int healthPoint, string name) : base (ref texture, ref textureInfo, name)
		{
			this.baseMovementSpeed = baseMovementSpeed;
			this.healthPoint = healthPoint;
			this.currentMovementSpeed = new Vector2(baseMovementSpeed);
			currentGridPosition = new Vector2i(gridPositionX, gridPositionY);
			Init(position);
		}
		
		public BasicCreature (ref Texture2D texture, ref TextureInfo textureInfo, Vector2 position, Vector2i gridPosition, float baseMovementSpeed, int healthPoint) : base (ref texture, ref textureInfo, "Basic Creature")
		{
			this.baseMovementSpeed = baseMovementSpeed;
			this.healthPoint = healthPoint;
			this.currentMovementSpeed = new Vector2(baseMovementSpeed);
			currentGridPosition = gridPosition;
			Init(position);
		}
		
		public BasicCreature (ref Texture2D texture, ref TextureInfo textureInfo, Vector2 position, Vector2i gridPosition, float baseMovementSpeed, int healthPoint, string name) : base (ref texture, ref textureInfo, name)
		{
			this.baseMovementSpeed = baseMovementSpeed;
			this.healthPoint = healthPoint;
			this.currentMovementSpeed = new Vector2(baseMovementSpeed);
			currentGridPosition = gridPosition;
			Init(position);
		}
	}
}

