using System;
using System.Collections.Generic;

using Sce.PlayStation.HighLevel.GameEngine2D;

namespace HybridActionTD
{
	public class ScreenManager
	{
		List<Scene> gameSceneList;
		
		public ScreenManager ()
		{
			gameSceneList = new List<Scene>();
			//gameSceneList.Add(new SplashScreen(this));
		}
		
		public void AddScreen(Scene screen)
		{
			gameSceneList.Add(screen);
		}
		
		public void RemoveScreen(int index)
		{
			gameSceneList.RemoveAt(index);
		}
		
		public void RemoveScreen(Scene screen)
		{
			gameSceneList.Remove(screen);
		}
		
		public void MoveToScreen(GameScreenEnum screen)
		{
			Director.Instance.ReplaceScene(gameSceneList[(int)screen]);
		}
		
		public void StartWithScreen(GameScreenEnum screen)
		{
			Director.Instance.RunWithScene(gameSceneList[(int)screen]);
		}
		
		public void StartWithScreen(int screen)
		{
			Director.Instance.RunWithScene(gameSceneList[screen]);
		}
	}
}

