using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public class MountainGod : BasicCreature
	{
		private	GodState	currentState;
		
		private	SpellType	currentSpell;
		
		private FireBall	fireBallSpell;
		private Avalanche	avalancheSpell;
		private	Root		rootSpell;
		
		private float		channelTime;
		private float		currentChannelTime;
		private float		regenInterval;
		private	float		regenIntervalTrigger;
		
		private int			maxPower;
		private int			currentPower;
		private int			powerRegenRate;
		
		private	HealthBar	healthBar;
		private PowerBar	powerBar;
		
		private Vector2		castDirection;
		private Vector2		castOrigin;
		
		private Messenger 	messenger;
		
		public MountainGod (ref Texture2D texture, ref TextureInfo textureInfo, ref Messenger messenger) : base (ref texture, ref textureInfo, CommonHelper.GodMS, CommonHelper.GodHP, CommonHelper.GodName)
		{
			currentState = GodState.Walking;
			
			currentSpell = SpellType.FireBall;
			
			fireBallSpell = new FireBall(ref texture, ref textureInfo);
			Console.WriteLine("FireBall initiated");
			avalancheSpell = new Avalanche(ref texture, ref textureInfo);
			Console.WriteLine("Avalanche initiated");
			rootSpell = new Root(ref texture, ref textureInfo);
			Console.WriteLine("Root initiated");
			
			maxPower = CommonHelper.GodBaseMaxPower;
			currentPower = maxPower;
			powerRegenRate = CommonHelper.GodBasePowerRegen;
			regenInterval = 0;
			regenIntervalTrigger = CommonHelper.GodPowerRegenInterval;
			
			//healthBar = new HealthBar(healthPoint, CommonHelper.GodHP, ref textureInfo);
			powerBar = new PowerBar(currentPower, CommonHelper.GodBaseMaxPower, ref textureInfo);
			
			castDirection = new Vector2(1, 0);
			castOrigin = new Vector2(0, 120);
			
			this.messenger = messenger;
		}
		
		public HealthBar GetHealthBar()
		{
			return healthBar;
		}
		
		public PowerBar GetPowerBar()
		{
			return powerBar;
		}
		
		public void Init(Vector2 position, ref SpriteList spriteList)
		{
			this.spriteTile.TileIndex2D = CommonHelper.GodRightNormalTileIndex;
			this.spriteTile.Quad.S = CommonHelper.CellSize;
			this.spriteTile.Pivot = CommonHelper.CellSize/2;
			
			SetPosition(position);
			powerBar.SetPosition(position.X, position.Y + CommonHelper.CellSize.Y - 5);
			
			boundingBox = new Rectangle ((int)position.X, (int)position.Y, CommonHelper.CellSize.X, CommonHelper.CellSize.Y);
			
			spriteList.AddChild(spriteTile, CommonHelper.DrawOrderGod);
			spriteList.AddChild(powerBar.GetHealthBar(), CommonHelper.DrawOrderHealthBar);
			
			currentChannelTime = 0;
		}
		
		public override void SetPosition(Vector2 position)
		{
			this.position = position;
			this.centerPosition = position + CommonHelper.CellSize/2;
			spriteTile.Position = position;
			boundingBox.X = (int)position.X;
			boundingBox.Y = (int)position.Y;
		}
		
		public void Update(float dt, ref PlayCell[,] playGrid, ref SpriteList spriteList, ref EnemyManager enemyManager)
		{
			switch(currentState)
			{
				case GodState.Walking:
					if (Input2.GamePad0.Dpad.X < 0 || Input2.GamePad0.AnalogLeft.X < -0.05f)
					{
						SetPosition(position - new Vector2(baseMovementSpeed,0));
//						healthBar.SetPosition(position.X, position.Y);
						powerBar.SetPosition(position.X, position.Y + CommonHelper.CellSize.Y - 5);
						CheckRegion3Rotation();
						CheckRegion4Rotation();
					}
					else if (Input2.GamePad0.Dpad.X > 0 || Input2.GamePad0.AnalogLeft.X > 0.05f)
					{
						SetPosition(position + new Vector2(baseMovementSpeed,0));
//						healthBar.SetPosition(position.X, position.Y);
						powerBar.SetPosition(position.X, position.Y + CommonHelper.CellSize.Y - 5);
						CheckRegion1Rotation();
						CheckRegion2Rotation();
					}
					
					if (Input2.GamePad0.Dpad.Y > 0 || Input2.GamePad0.AnalogLeft.Y < -0.05f)	
					{
						SetPosition(position + new Vector2(0,baseMovementSpeed));
//						healthBar.SetPosition(position.X, position.Y);
						powerBar.SetPosition(position.X, position.Y + CommonHelper.CellSize.Y - 5);
						CheckRegion2Rotation();
						
						CheckRegion3Rotation();
					}
					else if (Input2.GamePad0.Dpad.Y < 0 || Input2.GamePad0.AnalogLeft.Y > 0.05f)
					{
						SetPosition(position - new Vector2(0,baseMovementSpeed));
//						healthBar.SetPosition(position.X, position.Y);
						powerBar.SetPosition(position.X, position.Y + CommonHelper.CellSize.Y - 5);
						CheckRegion1Rotation();
						CheckRegion4Rotation();
					}
				
					if (Input2.GamePad0.Square.Release)
						currentSpell = SpellType.FireBall;
					else if (Input2.GamePad0.Triangle.Release)
						currentSpell = SpellType.Avalanche;
					else if (Input2.GamePad0.Circle.Release)
						currentSpell = SpellType.Root;
					else if (Input2.GamePad0.Cross.Release)
						CheckCurrentStatus();
				
					if (Input2.GamePad0.R.Release)
					{
						CastSpell();
					}
				
					if (regenInterval < regenIntervalTrigger)
					{
						regenInterval += dt;
					}
					else
					{
						regenInterval = 0;
						currentPower += powerRegenRate;
						if (currentPower > maxPower)
						{
							currentPower = maxPower;
						}
					}
					
					break;
				
				case GodState.CastingFire:
					if (currentChannelTime < channelTime)
					{
						currentChannelTime += dt;
						SetChannelingTileIndex();
					}
					else
					{
						fireBallSpell.CastSpell(ref spriteList, position + castOrigin, ref playGrid, centerPosition + castDirection * fireBallSpell.GetCastRange());
						currentChannelTime = 0;
						currentState = GodState.Walking;
					}
					break;
				
				case GodState.CastingEarth:
					if(currentChannelTime < channelTime)
					{
						currentChannelTime += dt;
						SetChannelingTileIndex();
					}
					else
					{
						avalancheSpell.CastSpell(ref spriteList, ref playGrid, position + castDirection * avalancheSpell.GetCastRange());
						currentChannelTime = 0;
						currentState = GodState.Walking;
					}
					break;
				
				case GodState.CastingNature:
					if (currentChannelTime < channelTime)
					{
						currentChannelTime += dt;
						SetChannelingTileIndex();
					}
					else
					{
						rootSpell.CastSpell(ref spriteList, ref playGrid, position + castDirection * rootSpell.GetCastRange());
						currentChannelTime = 0;
						currentState = GodState.Walking;
					}
					break;
			}
			fireBallSpell.Update(dt, ref playGrid, ref spriteList, ref enemyManager);
			avalancheSpell.Update(dt, ref playGrid, ref spriteList, ref enemyManager);
			rootSpell.Update(dt, ref playGrid, ref spriteList, ref enemyManager);
			powerBar.Update(dt, currentPower);
		}
		
		public void SetChannelingTileIndex()
		{
			if (spriteTile.TileIndex2D == CommonHelper.GodDownNormalTileIndex)
				spriteTile.TileIndex2D = CommonHelper.GodDownCastingTileIndex;
			else if (spriteTile.TileIndex2D == CommonHelper.GodLeftNormalTileIndex)
				spriteTile.TileIndex2D = CommonHelper.GodLeftCastingTileIndex;
			else if (spriteTile.TileIndex2D == CommonHelper.GodRightNormalTileIndex)
				spriteTile.TileIndex2D = CommonHelper.GodRightCastingTileIndex;
			else if (spriteTile.TileIndex2D == CommonHelper.GodUpNormalTileIndex)
				spriteTile.TileIndex2D = CommonHelper.GodUpCastingTileIndex;
		}
		
		public void CheckRegion1Rotation()
		{
			if ((Input2.GamePad0.AnalogLeft.X >= 0.7f && Input2.GamePad0.AnalogLeft.Y >= -0.7f)
			    || (Input2.GamePad0.Dpad.X >= 0.7f && Input2.GamePad0.Dpad.Y >= -0.7f))
			{
				spriteTile.TileIndex2D = CommonHelper.GodRightNormalTileIndex;
				castDirection = new Vector2(1, 0);
				castOrigin = new Vector2(0, 120);
			}
			else
			{
				spriteTile.TileIndex2D = CommonHelper.GodUpNormalTileIndex;
				castDirection = new Vector2(0, 1);
				castOrigin = new Vector2(0, 0);
			}
		}
		
		public void CheckRegion2Rotation()
		{
			if ((Input2.GamePad0.AnalogLeft.X >= 0.7f && Input2.GamePad0.AnalogLeft.Y <= 0.7f) 
			    || (Input2.GamePad0.Dpad.X >= 0.7f && Input2.GamePad0.Dpad.Y <= 0.7f))
			{
				spriteTile.TileIndex2D = CommonHelper.GodRightNormalTileIndex;
				castDirection = new Vector2(1, 0);
				castOrigin = new Vector2(0, 120);
			}
			else
			{
				spriteTile.TileIndex2D = CommonHelper.GodDownNormalTileIndex;
				castDirection = new Vector2(0, -1);
				castOrigin = new Vector2(0, 0);
			}
		}
		
		public void CheckRegion3Rotation()
		{
			if ((Input2.GamePad0.AnalogLeft.X <= -0.7f && Input2.GamePad0.AnalogLeft.Y <= 0.7f)
			    || (Input2.GamePad0.Dpad.X <= -0.7f && Input2.GamePad0.Dpad.Y <= 0.7f))
			{
				spriteTile.TileIndex2D = CommonHelper.GodLeftNormalTileIndex;
				castDirection = new Vector2(-1, 0);
				castOrigin = new Vector2(0, 120);
			}
			else
			{
				spriteTile.TileIndex2D = CommonHelper.GodUpNormalTileIndex;
				castDirection = new Vector2(0, 1);
				castOrigin = new Vector2(0, 0);
			}
		}
		
		public void CheckRegion4Rotation()
		{
			if ((Input2.GamePad0.AnalogLeft.X <= -0.7f && Input2.GamePad0.AnalogLeft.Y >= -0.7f)
			    || (Input2.GamePad0.Dpad.X <= -0.7f && Input2.GamePad0.Dpad.Y >= -0.7f))
			{
				spriteTile.TileIndex2D = CommonHelper.GodLeftNormalTileIndex;
				castDirection = new Vector2(-1, 0);
				castOrigin = new Vector2(0, 120);
			}
			else
			{
				spriteTile.TileIndex2D = CommonHelper.GodDownNormalTileIndex;
				castDirection = new Vector2(0, -1);
				castOrigin = new Vector2(0, 0);
			}
		}
		
		public void CastSpell()
		{
			switch(currentSpell)
			{
				case SpellType.FireBall:
					if (fireBallSpell.GetSpellState() == SpellState.Ready)
					{
						currentState = GodState.CastingFire;
					
						if (currentPower < fireBallSpell.GetPowerCost())
						{
							currentState = GodState.Walking;
							return;
						}
						
						channelTime = fireBallSpell.GetCastDuration();
						currentPower -=  fireBallSpell.GetPowerCost();
					}
					break;
				case SpellType.Avalanche:
					if (avalancheSpell.GetSpellState() == SpellState.Ready)
					{
						currentState = GodState.CastingEarth;
					
						if (currentPower < avalancheSpell.GetPowerCost())
						{
							currentState = GodState.Walking;
							return;
						}
					
						channelTime = avalancheSpell.GetCastDuration();
						currentPower -=  avalancheSpell.GetPowerCost();
					}
					break;
				case SpellType.Root:
					if (rootSpell.GetSpellState() == SpellState.Ready)
					{
						currentState = GodState.CastingNature;
					
						if (currentPower < rootSpell.GetPowerCost())
						{
							currentState = GodState.Walking;
							return;
						}
					
						channelTime = rootSpell.GetCastDuration();
						currentPower -=  rootSpell.GetPowerCost();
					}
					break;
			}
		}
		
		public void CheckCurrentStatus()
		{
		}
	}
}

