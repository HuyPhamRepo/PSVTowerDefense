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
	public class LoseScreen : BasicFunctionScreen
	{
		private	Label	loseText;
		
		private	Font		font;
		private	Font		tempFont;
		private	FontMap		fontMap;
		
		public LoseScreen (ref TextureInfo textureInfo, ref TextureInfo blankTextureInfo)
		{
			loseText = new Label();
			
			font = (Font)CommonHelper.GameFont.ShallowClone();
			tempFont = (Font)font.ShallowClone();
			fontMap = new FontMap(tempFont);
			
			loseText = new Label("You failed to protect the village", fontMap);
				
			menuButtonList = new List<GameButton>(2);
			menuButtonList.Add(new GameButton(ref textureInfo, "Quit", CommonHelper.GameFont));
			menuButtonList.Add(new GameButton(ref textureInfo, "Replay", CommonHelper.GameFont));
			blankBackground = new SpriteTile(blankTextureInfo);
			blankBackground.TileIndex2D = CommonHelper.BlackTexturePosition;
			PositionComponents();
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
			loseText.Position = new Vector2(CommonHelper.ScreenSize.X / 2 - font.GetTextWidth(loseText.Text) / 2, menuButtonList[menuButtonList.Count - 1].GetPosition().Y + CommonHelper.ButtonSize.Y + 10);
		}
		
		public override void Display (Scene parentScene, ref SpriteList buttonSpriteList, ref SpriteList spriteList)
		{
			base.Display (parentScene, ref buttonSpriteList, ref spriteList);
			parentScene.AddChild(loseText);
		}
		
		public override void Remove(Scene parentScene, ref SpriteList buttonSpriteList, ref SpriteList spriteList)
		{
			parentScene.RemoveChild(loseText, false);
			base.Remove (parentScene, ref buttonSpriteList, ref spriteList);
		}
		
		public GameButton GetQuitButton()
		{
			return menuButtonList[0];
		}
		
		public GameButton GetReplayButton()
		{
			return menuButtonList[1];
		}
	}
}

