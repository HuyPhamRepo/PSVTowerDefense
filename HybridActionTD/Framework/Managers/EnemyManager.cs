using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public class EnemyManager
	{
		List<BasicEnemy> 	enemyList;
		List<EnemyWave> 	enemyWaveList;
		
		float				currentWaitTime;
		float				currentSpawnTime;
		float				spawnTime;
		
		EnemyManagerState	currentState;
		
		int					currentWaveIndex;
		int					spawnEnemyCount;
		int					lastEnemyIndex;
		int					waveUnitsCount;
		
		Vector2i			spawnGridPosision;
		
		public bool			isFinished;
		
		Messenger			messenger;
		
		public EnemyManager (ref SpriteList spriteList, ref Texture2D texture, ref TextureInfo textureInfo, List<EnemyWave> enemyWaveList, Vector2i spawnGridPosition, ref Messenger messenger)
		{
			enemyList = new List<BasicEnemy>(CommonHelper.MAX_ENEMY_COUNT * CommonHelper.ENEMY_TYPE_COUNT);
			this.enemyWaveList = enemyWaveList;
			
			InitEnemyList(ref spriteList, ref texture, ref textureInfo);
			
			currentWaitTime = 0;
			currentSpawnTime = 0;
			spawnTime = -1;
			
			currentWaveIndex = 0;
			spawnEnemyCount = 0;
			
			currentState = EnemyManagerState.Waiting;
			this.spawnGridPosision = spawnGridPosition;
			
			isFinished = false;
			
			this.messenger = messenger;
		}
		
		public void InitEnemyList(ref SpriteList spriteList, ref Texture2D texture, ref TextureInfo textureInfo)
		{
			for (int i = 0; i < CommonHelper.ENEMY_TYPE_COUNT; i++)
			{
				switch ((EnemyType)i)
				{
					case EnemyType.Basic:
						for (int j = 0; j < CommonHelper.MAX_ENEMY_COUNT; j++)
						{
							enemyList.Add(new BasicEnemy(ref spriteList, ref texture, ref textureInfo, CommonHelper.EnemyBasicMS, CommonHelper.EnemyBasicHP));
						}
						break;
						
					case EnemyType.Swift:
						for (int j = 0; j < CommonHelper.MAX_ENEMY_COUNT; j++)
						{
							enemyList.Add(new FastEnemy(ref spriteList, ref texture, ref textureInfo, CommonHelper.EnemySwiftNormalTilePosition, CommonHelper.EnemySwiftMS, CommonHelper.EnemySwiftHP));
						}
						break;
						
					case EnemyType.Tank:
						for (int j = 0; j < CommonHelper.MAX_ENEMY_COUNT; j++)
						{
							enemyList.Add(new TankyEnemy(ref spriteList, ref texture, ref textureInfo, CommonHelper.EnemyTankNormalTilePosition, CommonHelper.EnemyTankMS, CommonHelper.EnemyTankHP));
						}
						break;
				}
			}
		}
		
		public void Update(float dt, ref PlayCell[,] playGrid, ref SpriteList spriteList, ref int lifeCount, ref PlayState playState)
		{
			if (!isFinished)
			{
				switch(currentState)
				{
					case EnemyManagerState.Waiting:
						currentWaitTime += dt;
						if (currentWaitTime >= CommonHelper.EnemyManagerTimeBetweenWaves)
						{
							currentWaitTime = 0;
							currentState = EnemyManagerState.PreparingWave;
						}
						break;
						
					case EnemyManagerState.Spawning:
						if (currentSpawnTime >= spawnTime)
						{
							SpawnEnemy(ref playGrid, ref spriteList);
							currentSpawnTime = 0;
						}
						else
							currentSpawnTime ++;
						break;
						
					case EnemyManagerState.Finished:
						if (waveUnitsCount <= 0)
							isFinished = true;
						break;
						
					case EnemyManagerState.PreparingWave:
						PrepareWave();
						break;
					
					case EnemyManagerState.FinishedWave:
						if (waveUnitsCount <= 0)
							currentState = EnemyManagerState.Waiting;
						break;
				}
				
				for (int i = 0; i < enemyList.Count; i++)
				{
					enemyList[i].Update(dt, ref spriteList, ref playGrid, ref lifeCount, ref waveUnitsCount);
				}
			}
		}
		
		public void PrepareWave()
		{
			switch (enemyWaveList[currentWaveIndex].enemyType)
			{
			case EnemyType.Basic:
				spawnTime = CommonHelper.EnemyManagerWaveSpawnSeperation / CommonHelper.EnemyBasicMS;
				break;
			case EnemyType.Swift:
				spawnTime = CommonHelper.EnemyManagerWaveSpawnSeperation / CommonHelper.EnemySwiftMS;
				break;
			case EnemyType.Tank:
				spawnTime = CommonHelper.EnemyManagerWaveSpawnSeperation / CommonHelper.EnemyTankMS;
				break;
			}
			
			waveUnitsCount = enemyWaveList[currentWaveIndex].enemyCount;
			currentSpawnTime = spawnTime;
			currentState = EnemyManagerState.Spawning;
			
			SavePlayerData();
		}
		
		public void LoadResumeData()
		{
			currentWaveIndex = Player.CurrentWave;
		}
		
		public void SavePlayerData()
		{
			Player.Save();
			Player.SaveTowerData(messenger.GetTowerManager().GetTowerList());
		}
		
		public void SpawnEnemy (ref PlayCell[,] playGrid, ref SpriteList spriteList)
		{
			if (spawnEnemyCount < enemyWaveList[currentWaveIndex].enemyCount)
			{
				for (int i = (int)enemyWaveList[currentWaveIndex].enemyType * CommonHelper.MAX_ENEMY_COUNT; i < (int)enemyWaveList[currentWaveIndex].enemyType * CommonHelper.MAX_ENEMY_COUNT + CommonHelper.MAX_ENEMY_COUNT; i++)
				{
					if (!enemyList[i].isActive)
					{
						enemyList[i].Init(ref playGrid, ref spriteList, spawnGridPosision, i);
						lastEnemyIndex = i;
						break;
					}
				}
				
				spawnEnemyCount++;
			}
			else
			{
				spawnEnemyCount = 0;
				currentWaveIndex++;
				Player.CurrentWave = currentWaveIndex;
				if (currentWaveIndex < enemyWaveList.Count)
					currentState = EnemyManagerState.FinishedWave;
				else
					currentState = EnemyManagerState.Finished;
			}
		}
		
		public List<BasicEnemy> GetEnemyList()
		{
			return enemyList;
		}
		
		public void SetMessenger(ref Messenger messenger)
		{
			this.messenger = messenger;
		}
	}
}

