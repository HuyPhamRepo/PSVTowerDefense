using System;

namespace HybridActionTD
{
	public class EnemyWave
	{
		public EnemyType 	enemyType;
		public int			enemyCount;
		
		public EnemyWave (EnemyType type, int enemyCount)
		{
			this.enemyType = type;
			this.enemyCount = enemyCount;
		}
		
		public EnemyWave()
		{
		}
		
		public void SetWave(EnemyType type, int enemyCount)
		{
			this.enemyType = type;
			this.enemyCount = enemyCount;
		}
	}
}

