using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public static class CommonHelper
	{
		#region Common stuff
		
		public static Vector2i	ScreenPadding 		= new Vector2i(80, 0);
		public static Vector2i	GridSize 			= new Vector2i(11, 6);
		public static Vector2	CellSize 			= new Vector2(80, 80);
		public static Vector2i	TextureDivision	 	= new Vector2i(11, 20);
		public static Vector2i	NullTexturePosition = new Vector2i(10, 19);
		public static string	ArtDirectory		= "/Application/Art/";
		public static string	MapDirectory		= "/Application/Maps/";
		public static string	FontDirectory		= "/Application/Font/";
		public static string	SaveDirectory		= "/Documents/";
		public static Vector2i	ScreenSize			= new Vector2i(960, 544);
		
		public static Font 		GameFont 			= new Font(CommonHelper.FontDirectory + "EarthFont.ttf", 30, FontStyle.Regular);
		public static Font		SmallFont			= new Font(CommonHelper.FontDirectory + "EarthFont.ttf", 15, FontStyle.Regular);
		
		public static int		DrawOrderTower		= 300;
		public static int		DrawOrderTowerTop	= 400;
		public static int		DrawOrderEnemy		= 100;
		public static int		DrawOrderProjectile	= 200;
		public static int		DrawOrderGod		= 600;
		public static int		DrawOrderHealthBar	= 500;
		public static int		DrawOrderParticle	= 700;
		public static int		DrawOrderMenuDialog	= 800;
		
		public static bool IsInside(Vector2 point, Rectangle rect)
		{
			return !(point.X < rect.X) && !(point.X > rect.Width + rect.X) && !(point.Y < rect.Y) && !(point.Y > rect.Height + rect.Y);
		}
		
		public static bool IsInside(Vector2i point, Rectangle rect)
		{
			return !(point.X < rect.X) && !(point.X > rect.Width + rect.X) && !(point.Y < rect.Y) && !(point.Y > rect.Height + rect.Y);
		}
		
		public static bool IsInside(int X, int Y, Rectangle rect)
		{
			return !(X < rect.X) && !(X > rect.Width + rect.X) && !(Y < rect.Y) && !(Y > rect.Height + rect.Y);
		}
		
		public static bool IsIntersect(Rectangle r1, Rectangle r2)
		{
			if (r1.X >= (r2.X + r2.Width) || (r1.X + r1.Width) <= r2.X || (r1.Y + r1.Height) >= r2.Y || r1.Y <= (r2.Y + r2.Height))
				return false;

			return true;
		}
		
		public static int TouchToScreenX(float X)
		{
			return (int)((X + 0.5f) * ScreenSize.X);
		}
		
		public static int TouchToScreenY(float Y)
		{
			return (ScreenSize.Y -(int)((Y + 0.5f) * ScreenSize.Y));
		}
		
		public static float GetDistance(Vector2 p1, Vector2 p2)
		{
			return FMath.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
		}
		
		public static float GetDistanceSquared(Vector2 p1, Vector2 p2)
		{
			return ((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
		}
		
		public static Texture2D CreateTexture(int w, int h, UInt32 color)
		{
			Texture2D newTex = new Texture2D(w, h, false, PixelFormat.Rgba);
			UInt32[] pix = new UInt32[w*h];
				
			for(int i=0; i<pix.Length; i++)
				pix[i] = color;
				
			newTex.SetPixels(0, pix);
		
			return(newTex);
		}
			
		#endregion
		
		#region Cell
		
		public static Vector2i	CellDefaultTile		= new Vector2i(0,0);
		
		#endregion
		
		#region Tower
		
		#region Basic Tower
		
		public static string	TowerBasicName			= "Earth Canon";
		public static float		TowerBasicAS			= 1.0f;
		public static float		TowerBasicTurnSpeed		= .1f;
		public static int		TowerBasicAP			= 15;
		public static int		TowerBasicAttackRange	= 150;
		public static Vector2i	TowerBasicBaseTileIndex	= new Vector2i(0, 2);
		public static Vector2	TowerBasicPivot			= new Vector2(CellSize.X * 0.4f, CellSize.Y / 2);
		
		#endregion
		
		#region Slow Tower
		
		public static string	TowerSlowName			= "Slime Watergun";
		public static float		TowerSlowcAS			= 1.0f;
		public static float		TowerSlowTurnSpeed		= .1f;
		public static int		TowerSlowAP				= 5;
		public static int		TowerSlowAttackRange	= 150;
		public static Vector2i	TowerSlowBaseTileIndex	= new Vector2i(0, 4);
		
		#endregion
		
		#region Splash Tower
		
		public static string	TowerSplashName				= "Spikey Ball";
		public static float		TowerSplashcAS				= 1.0f;
		public static float		TowerSplashTurnSpeed		= .1f;
		public static int		TowerSplashAP				= 25;
		public static int		TowerSplashAttackRange		= 150;
		public static int		TowerSplashNormalDmgRange	= 25;
		public static int		TowerSplashSplashDmgRange	= 100;
		public static Vector2i	TowerSplashBaseTileIndex	= new Vector2i(0, 6);
		
		#endregion
		
		#endregion
		
		#region Tower Manager
		
		public static int		MAX_TOWER_COUNT			= 56;
		
		#endregion
		
		#region Enemy
		
		#region Basic Enemy
		
		public static int		EnemyBasicHP					= 100;
		public static float		EnemyBasicMS					= 1.0f;
		public static Vector2i	EnemyBasicNormalTilePosition	= new Vector2i(0, 8);
		
		#endregion
		
		#region Swift Enemy
		
		public static int		EnemySwiftHP					= 75;
		public static float		EnemySwiftMS					= 2.0f;
		public static Vector2i	EnemySwiftNormalTilePosition	= new Vector2i(0, 10);
		
		#endregion
		
		#region Tank Enemy
		
		public static int		EnemyTankHP					= 150;
		public static float		EnemyTankMS					= 0.5f;
		public static Vector2i	EnemyTankNormalTilePosition	= new Vector2i(0, 9);
		
		#endregion
		
		#endregion
		
		#region Enemy Manager
		
		public static int		MAX_ENEMY_COUNT					= 50;
		public static int		ENEMY_TYPE_COUNT				= 3;
		
		public static int		EnemyManagerTimeBetweenWaves	= 5;
		public static int		EnemyManagerWaveSpawnSeperation	= 85;
		
		#endregion
		
		#region Projectile
		
		#region Basic Projectile
		
		public static string	ProjectileBasicName					= "Basic Projectile";
		public static float		ProjectileBasicSpeed				= .15f;
		public static Vector2i	ProjectileBasicTilePosition			= new Vector2i(1, 2);
		public static Vector2i	ProjectileBasicImpactTilePosition	= new Vector2i(2, 2);
		public static float		ProjectileBasicDisappearTime		= .3f;
			
		#endregion
		
		#region SLow Projectile
		
		public static string	ProjectileSlowName					= "Slow Projectile";
		public static float		ProjectileSlowSpeed					= .15f;
		public static Vector2i	ProjectileSlowTilePosition			= new Vector2i(1, 4);
		public static Vector2i	ProjectileSlowImpactTilePosition	= new Vector2i(2, 4);
		public static float		ProjectileSlowDisappearTime			= .5f;
		public static float		ProjectileSlowPercentage			= .75f;
		public static float		ProjectileSlowDuration				= 3.0f;
		
		#endregion
		
		#region Splash Projectile
		
		public static string	ProjectileSplashName					= "Splash Projectile";
		public static float		ProjectileSplashSpeed					= .15f;
		public static Vector2i	ProjectileSplashTilePosition			= new Vector2i(1, 6);
		public static Vector2i	ProjectileSplashImpactTilePosition		= new Vector2i(2, 6);
		public static float		ProjectileSplashDisappearTime			= .5f;
		public static float		ProjectileSplashPercentage				= .45f;
		
		#endregion
		
		#endregion
		
		#region Projectile Manager
		
		public static int		MAX_PROJECTILE_COUNT				= 50;
		
		#endregion
		
		#region God
		
		public static string	GodName						= "Mountain God";
		public static Vector2i	GodRightNormalTileIndex		= new Vector2i(0, 17);
		public static Vector2i	GodRightCastingTileIndex	= new Vector2i(1, 17);
		public static Vector2i	GodDownNormalTileIndex		= new Vector2i(2, 17);
		public static Vector2i	GodDownCastingTileIndex		= new Vector2i(3, 17);
		public static Vector2i	GodUpNormalTileIndex		= new Vector2i(4, 17);
		public static Vector2i	GodUpCastingTileIndex		= new Vector2i(5, 17);
		public static Vector2i	GodLeftNormalTileIndex		= new Vector2i(6, 17);
		public static Vector2i	GodLeftCastingTileIndex		= new Vector2i(7, 17);
		public static float		GodMS						= 2.0f;
		public static float		GodPowerRegenInterval		= 1.0f;
		public static int		GodHP						= 1000;
		public static int		GodBaseMaxPower				= 100;
		public static int		GodBasePowerRegen			= 10;
		
		#endregion
		
		#region Spells
		
		#region Basic
		
		public static string	SpellBasicName			= "Basic Spell";
		public static Vector2i	SpellShadowTileIndex	= new Vector2i(0, 14);
		
		#endregion
		
		#region Fire Ball
		
		public static string	SpellFireBallName			= "Fire Ball";
		public static float		SpellFireBallCoolTime		= 1;
		public static float		SpellFireBallCastRange		= 160;
		public static int		SpellFireBallPowerCost		= 15;
		public static float		SpellFireBallCastDuration	= .5f;
		public static float		SpellFireBallMS				= .05f;
		public static int		SpellFireBallRoE			= 160;
		public static int		SpellFireBallDamage			= 20;
		
		public static Vector2i	SpellFireBallTileIndex		= new Vector2i(0, 15);
			
		#endregion
		
		#region Avalanche
		
		public static string	SpellAvalancheName			= "Avalanche";
		public static float		SpellAvalancheCoolTime		= 10.0f;
		public static float		SpellAvalancheCastRange		= 100;
		public static int		SpellAvalanchePowerCost		= 50;
		public static float		SpellAvalancheCastDuration	= .8f;
		public static float		SpellAvalancheAffectTime	= .3f;
		public static int		SpellAvalancheHalfWidth		= 1;
		public static int		SpellAvalancheLength		= 3;
		public static int		SpellAvalancheDamage		= 40;
		public static float		SpellAvalancheStunDuarion	= 1.0f;
		
		public static Vector2i	SpellAvalancheTileIndex		= new Vector2i(2, 15);
			
		#endregion
		
		#region Root
		
		public static string	SpellRootName				= "Root";
		public static float		SpellRootCoolTime			= 8.0f;
		public static float		SpellRootCastRange			= 100;
		public static int		SpellRootPowerCost			= 30;
		public static float		SpellRootCastDuration		= .6f;
		public static int		SpellRootDamage				= 10;
		public static float		SpellRootSlowPercentage		= .4f;
		public static float		SpellRootSlowDuration		= 2.0f;
		public static float		SpellRootAffectTime			= .5f;
		public static float		SpellRootAplifyPercentage	= 1.2f;
		public static float		SpellRootMS					= 0.07f;
		public static int		SpellRootLength				= 3;
		
		public static Vector2i	SpellRootTileIndex		= new Vector2i(3, 15);
			
		#endregion
		
		#endregion
		
		#region Button
		
		public static Vector2	ButtonTowerSize		 		= new Vector2(50, 50);
		public static Vector2i	ButtonTowerTileDivision 	= new Vector2i(5, 5);
		public static Vector2i	ButtonBasicTowerTileIndex	= new Vector2i(2, 0);
		public static Vector2i	ButtonSlowTowerTileIndex	= new Vector2i(4, 0);
		public static Vector2i	ButtonSplashTowerTileIndex	= new Vector2i(0, 0);
		public static Vector2	ButtonSize					= new Vector2(240, 80);
		public static Vector2i	ButtonTileDivision			= new Vector2i(1, 4);
		public static Vector2i	ButtonNormalTileIndex		= new Vector2i(0, 0);
		public static Vector2i	ButtonFocusedTileIndex		= new Vector2i(0, 1);
		
		#endregion
		
		#region Miscs
		
		public static Vector2i	HealthBarTilePosition		= new Vector2i(1, 14);
		public static Vector2i	BlackTexturePosition		= new Vector2i(2, 14);
		public static Vector2i	PowerBarTilePosition		= new Vector2i(3, 14);
		
		#endregion
	}
}

