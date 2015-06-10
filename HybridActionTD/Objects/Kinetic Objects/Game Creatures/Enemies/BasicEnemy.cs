using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public class BasicEnemy : BasicCreature
	{
		protected 	Vector2i		nextGridPosition;
		
		protected 	EnemyType	 	enemyType;
		protected	SpellEffect		currentEffect;
		protected 	EnemyState 		currentState;
		
		protected 	int 			currentListIndex;
		protected	int				baseHealthPoint;
		
		protected	Rectangle		hitBox;
		
		public		float			travelledDistance;
		public		float			slowDuration;
		public		float			currentSlowDuration;
		protected	float			normalMovementSpeed;
		
		public 		float			stunDuration;
		public 		float			currentStunDuration;
		
		public		bool			isActive;
		public		bool			swappedCell;
		
		protected	HealthBar		healthBar;
		
		public BasicEnemy (ref SpriteList spriteList, ref Texture2D texture, ref TextureInfo textureInfo, float baseMovementSpeed, int healthPoint) : base(ref texture, ref textureInfo, baseMovementSpeed, healthPoint)
		{
			enemyType = EnemyType.Basic;
			currentState = EnemyState.Moving;
			
			spriteTile.TileIndex2D = CommonHelper.EnemyBasicNormalTilePosition;
			
			normalMovementSpeed = baseMovementSpeed;
			baseHealthPoint = healthPoint;
			
			healthBar = new HealthBar(healthPoint, baseHealthPoint, ref textureInfo);
			
			swappedCell = false;
			isActive = false;
		}
		
		public BasicEnemy (EnemyType type, ref SpriteList spriteList, ref Texture2D texture, ref TextureInfo textureInfo, Vector2i defaultTileIndex, float baseMovementSpeed, int healthPoint) : base(ref texture, ref textureInfo, baseMovementSpeed, healthPoint)
		{
			enemyType = type;
			currentState = EnemyState.Moving;
			
			spriteTile.TileIndex2D = defaultTileIndex;
			
			normalMovementSpeed = baseMovementSpeed;
			baseHealthPoint = healthPoint;
			
			healthBar = new HealthBar(healthPoint, baseHealthPoint, ref textureInfo);
			
			swappedCell = false;
			isActive = false;
		}
		
		public SpriteTile GetHealthBar()
		{
			return healthBar.GetHealthBar();
		}
				
		public void Init(ref PlayCell[,] playGrid, ref SpriteList spriteList, Vector2i gridPosition, int currentListIndex)
		{
			this.currentGridPosition = gridPosition;
			this.nextGridPosition = playGrid[currentGridPosition.X,currentGridPosition.Y].GetNextGrid();
			this.currentListIndex = currentListIndex;
			
			baseMovementSpeed = normalMovementSpeed;
			
			playGrid[currentGridPosition.X, currentGridPosition.Y].GetEnemyList().Add(currentListIndex);
			currentMovementSpeed = new Vector2(playGrid[currentGridPosition.X, currentGridPosition.Y].GetMoveDirection().X * baseMovementSpeed,
			                                   playGrid[currentGridPosition.X, currentGridPosition.Y].GetMoveDirection().Y * baseMovementSpeed);
			
			Init(playGrid[currentGridPosition.X, currentGridPosition.Y].GetPosition());
			
			hitBox = new Rectangle(centerPosition.X - 20, centerPosition.Y - 20, 40, 40);
			
			Rotate(playGrid[currentGridPosition.X, currentGridPosition.Y].GetEnumMoveDirection());
			
			spriteList.AddChild(spriteTile, CommonHelper.DrawOrderEnemy);
			
			travelledDistance = 0;
			slowDuration = 0;
			currentSlowDuration = 0;
			
			healthPoint = baseHealthPoint;
			
			spriteList.AddChild(healthBar.GetHealthBar(), CommonHelper.DrawOrderHealthBar);
			healthBar.SetHealth(healthPoint);
			healthBar.SetPosition(position.X, centerPosition.Y + 5);
			
			currentEffect = SpellEffect.Normal;
			
			swappedCell = false;
			isActive = true;
		}
		
		public void Reset(ref SpriteList spriteList, ref PlayCell[,] playGrid)
		{
			isActive = false;
			spriteList.RemoveChild(spriteTile, false);
			spriteList.RemoveChild(healthBar.GetHealthBar(), true);
			
			playGrid[currentGridPosition.X, currentGridPosition.Y].GetEnemyList().Remove(currentListIndex);
			
			currentGridPosition = new Vector2i(-1, -1);
			nextGridPosition = new Vector2i(-1, -1);
			
			spriteTile.Rotation = new Vector2(1, 0);
			
			Init(new Vector2(-100, -100));
		}
		
		public void Update (float dt, ref SpriteList spriteList, ref PlayCell[,] playGrid, ref int lifeCount, ref int waveUnitsCount)
		{
			if (isActive)
			{
				if (healthPoint <= 0)
				{
					waveUnitsCount --;
					Reset(ref spriteList, ref playGrid);
				}
				else
				{
					float d = CommonHelper.GetDistanceSquared(centerPosition, playGrid[nextGridPosition.X, nextGridPosition.Y].GetCenterPosition());
					if (playGrid[nextGridPosition.X, nextGridPosition.Y].GetCellType() != CellType.End)
					{
						if (d < 4)
						{
							currentGridPosition = nextGridPosition;
							nextGridPosition = playGrid[currentGridPosition.X, currentGridPosition.Y].GetNextGrid();
							
							Rotate(playGrid[currentGridPosition.X, currentGridPosition.Y].GetEnumMoveDirection());
							
							currentMovementSpeed = new Vector2(playGrid[currentGridPosition.X, currentGridPosition.Y].GetMoveDirection().X * baseMovementSpeed,
							                                   playGrid[currentGridPosition.X, currentGridPosition.Y].GetMoveDirection().Y * baseMovementSpeed);
							swappedCell = false;
						}
						else
						{
							if (d < 1900 && d > 1500)
							{
								if(!swappedCell)
								{
									playGrid[currentGridPosition.X, currentGridPosition.Y].GetEnemyList().Remove(currentListIndex);
									playGrid[nextGridPosition.X, nextGridPosition.Y].GetEnemyList().Add(currentListIndex);
									swappedCell = true;
								}
							}
						}
					}
					else
					{
						if (d < 4)
						{
							lifeCount --;
							waveUnitsCount --;
							Reset(ref spriteList, ref playGrid);
						}
					}
					
					switch (currentEffect)
					{
						case SpellEffect.Slowed: 
						{
							if (currentSlowDuration > 0)
								currentSlowDuration -= dt;
							else
							{
								currentEffect = SpellEffect.Normal;
								currentSlowDuration = 0;
								slowDuration = 0;
								baseMovementSpeed = normalMovementSpeed;
								currentMovementSpeed = new Vector2(playGrid[currentGridPosition.X, currentGridPosition.Y].GetMoveDirection().X * baseMovementSpeed,
			                                   playGrid[currentGridPosition.X, currentGridPosition.Y].GetMoveDirection().Y * baseMovementSpeed);
								currentMovementSpeed *= baseMovementSpeed;
							}
						}
						break;
						
						case SpellEffect.Stunned:
						{
							if (currentStunDuration > 0)
								currentStunDuration -= dt;
							else
							{
								currentEffect = SpellEffect.Normal;
								currentStunDuration = 0;
								stunDuration = 0;
								currentMovementSpeed = new Vector2(playGrid[currentGridPosition.X, currentGridPosition.Y].GetMoveDirection().X * baseMovementSpeed,
			                                   playGrid[currentGridPosition.X, currentGridPosition.Y].GetMoveDirection().Y * baseMovementSpeed);
							}
						}
						break;
					}
					
					SetPosition(position + currentMovementSpeed);
					healthBar.SetPosition(position.X, centerPosition.Y + 5);
					hitBox.Position += currentMovementSpeed;
					travelledDistance += baseMovementSpeed;
				}
			}
		}
		
		public void Rotate(MoveDirection moveDirection)
		{
			switch(moveDirection)
			{
				case MoveDirection.Left: 
					spriteTile.Rotation = new Vector2(1, 0);
					break;
				case MoveDirection.Down:
					spriteTile.Rotation = new Vector2(0, 1);
					break;
				case MoveDirection.Right:
					spriteTile.Rotation = new Vector2(-1, 0);
					break;
				case MoveDirection.Up:
					spriteTile.Rotation = new Vector2(0, -1);
					break;
			}
		}
		
		public void GetHit(int damage)
		{
			healthPoint -= damage;
			healthBar.SetHealth(healthPoint);
		}
		
		public void GetSlowed(float percentage, float slowDuration)
		{
			if (currentEffect != SpellEffect.Slowed)
			{
				this.slowDuration = slowDuration;
				currentSlowDuration = slowDuration;
				currentMovementSpeed *= percentage;
				baseMovementSpeed *= percentage;
				currentEffect = SpellEffect.Slowed;
			}
			else
			{
				this.slowDuration = slowDuration;
				currentSlowDuration = slowDuration;
				currentEffect = SpellEffect.Slowed;
			}
		}
		
		public void GetStunned(float stunDuration)
		{
			if (currentEffect != SpellEffect.Stunned)
			{
				this.stunDuration = stunDuration;
				currentStunDuration = stunDuration;
				currentMovementSpeed = new Vector2(0,0);
				currentEffect = SpellEffect.Stunned;
			}
			else
			{
				this.stunDuration = stunDuration;
				currentStunDuration = stunDuration;
				currentEffect = SpellEffect.Stunned;
			}
		}
		
		public Rectangle GetHitBox()
		{
			return hitBox;
		}
		
		public Vector2i GetCurrentGrid()
		{
			return currentGridPosition;
		}
	}
}

