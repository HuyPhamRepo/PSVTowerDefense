using System;

using Sce.PlayStation.HighLevel.GameEngine2D;

namespace HybridActionTD
{
	public class AppMain
	{
		public static void Main (string[] args)
        {
            Director.Initialize();
			//Player.Load();
			Player.IsResumed = false;
			Director.Instance.RunWithScene(new SplashScreen());
        }
	}
}
