using System;

namespace HybridActionTD
{
	public enum CellType
	{
		Ocean = 0,
		Sea,
		Land,
		Start,
		End,
		Occupied
	}
	
	public enum MoveDirection
    {
        Left = 0,
        Down,
        Up, 
        Right,
        None
    }
	
	public enum TowerType
	{
		Basic,
		Slow,
		Splash
	}
	
	public enum TowerState
	{
		Idle,
		Attacking,
		Cooling
	}
	
	public enum EnemyType
	{
		Basic,
		Swift,
		Tank
	}
	
	public enum EnemyState
	{
		Moving,
		Attacking
	}
	
	public enum EnemyManagerState
	{
		Waiting,
		Spawning,
		Finished,
		PreparingWave,
		FinishedWave
	}
	
	public enum ProjectileType
	{
		Basic,
		Slow,
		Splash
	}
	
	public enum SpellType
	{
		Avalanche,
		FireBall,
		Root
	}
	
	public enum SpellEffect
	{
		Normal,
		Slowed,
		Burned,
		Snared,
		Stunned
	}
	
	public enum GodState
	{
		Walking,
		CastingFire,
		CastingEarth,
		CastingNature
	}
	
	public enum SpellState
	{
		Cooling,
		Ready,
		Casting
	}
	
	public enum GameScreenEnum
	{
		SplashScreen,
		LoadScreen,
		MenuScreen,
		GameScreen,
		OptionScreen,
		LevelSelectScreen,
		CreditsScreen
	}
	
	public enum PlayState
	{
		Normal,
		Paused,
		Win,
		Lose,
		Reset
	}
}

