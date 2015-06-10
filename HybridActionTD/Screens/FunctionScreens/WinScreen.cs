using System;
using System.Collections.Generic;

using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;

namespace HybridActionTD
{
	public class WinScreen : BasicFunctionScreen
	{
		private	Label	winText;
		private	Label	winScore;
		
		private	Font		font;
		private	Font		tempFont;
		private	FontMap		fontMap;
		
		public WinScreen (ref TextureInfo textureInfo, ref TextureInfo blankTextureInfo)
		{
			winText = new Label();
			winScore = new Label();
			
			font = (Font)CommonHelper.GameFont.ShallowClone();
			tempFont = (Font)font.ShallowClone();
			fontMap = new FontMap(tempFont);
			
			winText = new Label("Congratulations, you win!", fontMap);
			winScore = new Label("10 villagers were saved", fontMap);
				
			menuButtonList = new List<GameButton>(3);
			menuButtonList.Add(new GameButton(ref textureInfo, "Quit", CommonHelper.GameFont));
			menuButtonList.Add(new GameButton(ref textureInfo, "Replay", CommonHelper.GameFont));
			menuButtonList.Add(new GameButton(ref textureInfo, "Next Mission", CommonHelper.GameFont));
			blankBackground = new SpriteTile(blankTextureInfo);
			blankBackground.TileIndex2D = CommonHelper.BlackTexturePosition;
			PositionComponents();
		}
		
		public void SetWinScore(int score)
		{
			winScore.Text = score + " villagers were saved";
		}
		
		public override void PositionComponents()
		{
			blankBackground.Scale = new Vector2(CommonHelper.ScreenSize.X, CommonHelper.ScreenSize.Y);
			blankBackground.Position = new Vector2(CommonHelper.ScreenSize.X / 2 - blankBackground.Scale.X / 2, CommonHelper.ScreenSize.Y / 2 - blankBackground.Scale.Y / 2);
			
			menuButtonList[0].SetPosition(CommonHelper.ScreenSize.X / 2 - CommonHelper.ButtonSize.X / 2, CommonHelper.ScreenSize.Y / 2 - (fontMap.CharPixelHeight * 2 + CommonHelper.ButtonSize.Y * menuButtonList.Count + 10 * (menuButtonList.Count - 1 + 2)) / 2);
			for (int i = 1; i < menuButtonList.Count; i++)
			{
				menuButtonList[i].SetPosition(menuButtonList[i - 1].GetPosition().X, menuButtonList[i - 1].GetPosition().Y + CommonHelper.ButtonSize.Y + 10);
			}
			winText.Position = new Vector2(CommonHelper.ScreenSize.X / 2 - font.GetTextWidth(winText.Text) / 2, menuButtonList[menuButtonList.Count - 1].GetPosition().Y + CommonHelper.ButtonSize.Y + 10);
			winScore.Position = new Vector2(CommonHelper.ScreenSize.X / 2 - font.GetTextWidth(winScore.Text) / 2, winText.Position.Y +  fontMap.CharPixelHeight + 10);
		}
		
		public override void Display (Scene parentScene, ref SpriteList buttonSpriteList, ref SpriteList spriteList)
		{
			base.Display (parentScene, ref buttonSpriteList, ref spriteList);
			parentScene.AddChild(winText);
			parentScene.AddChild(winScore);
		}
		
		public override void Remove(Scene parentScene, ref SpriteList buttonSpriteList, ref SpriteList spriteList)
		{
			parentScene.RemoveChild(winText, false);
			parentScene.RemoveChild(winScore, false);
			base.Remove(parentScene, ref buttonSpriteList, ref spriteList);
		}
		
		public GameButton GetQuitButton()
		{
			return menuButtonList[0];
		}
		
		public GameButton GetReplayButton()
		{
			return menuButtonList[1];
		}
		
		public GameButton GetNextButton()
		{
			return menuButtonList[2];
		}
	}
}

