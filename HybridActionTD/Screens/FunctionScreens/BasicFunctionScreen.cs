using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;

namespace HybridActionTD
{
	public class BasicFunctionScreen
	{
		protected List<GameButton> 	menuButtonList;
		protected SpriteTile		blankBackground;
		
		public BasicFunctionScreen ()
		{
		}
		
		public virtual void PositionComponents()
		{
			blankBackground.Scale = new Vector2(CommonHelper.ScreenSize.X, CommonHelper.ScreenSize.Y);
			blankBackground.Position = new Vector2(CommonHelper.ScreenSize.X / 2 - blankBackground.Scale.X / 2, CommonHelper.ScreenSize.Y / 2 - blankBackground.Scale.Y / 2);
			menuButtonList[0].SetPosition(CommonHelper.ScreenSize.X / 2 - CommonHelper.ButtonSize.X / 2, CommonHelper.ScreenSize.Y / 2 - (CommonHelper.ButtonSize.Y * menuButtonList.Count + 10 * (menuButtonList.Count - 1)) / 2);
			for (int i = 1; i < menuButtonList.Count; i++)
			{
				menuButtonList[i].SetPosition(menuButtonList[i - 1].GetPosition().X, menuButtonList[i - 1].GetPosition().Y + CommonHelper.ButtonSize.Y + 10);
			}
		}
		
		public virtual void Display(Scene parentScene, ref SpriteList buttonSpriteList, ref SpriteList spriteList)
		{
			spriteList.AddChild(blankBackground, CommonHelper.DrawOrderMenuDialog);
			for (int i = 0; i < menuButtonList.Count; i++)
			{
				menuButtonList[i].Display(parentScene, ref buttonSpriteList);
			}
		}
		
		public virtual void Remove(Scene parentScene, ref SpriteList buttonSpriteList, ref SpriteList spriteList)
		{
			for (int i = 0; i < menuButtonList.Count - 1; i++)
			{
				menuButtonList[i].Remove(parentScene, ref buttonSpriteList, false, false);
			}
			menuButtonList[menuButtonList.Count - 1].Remove(parentScene, ref buttonSpriteList, true, true);
			spriteList.RemoveChild(blankBackground, true);
		}
		
		public void Update(float dt, List<TouchData> touches)
		{
			for (int i = 0; i < menuButtonList.Count; i++)
				menuButtonList[i].Update (dt, touches);
		}
	}
}

