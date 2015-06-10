using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;

namespace HybridActionTD
{
	public class PauseScreen : BasicFunctionScreen
	{
		public PauseScreen (ref TextureInfo textureInfo, ref TextureInfo blankTextureInfo)
		{
			menuButtonList = new List<GameButton>(4);
			menuButtonList.Add(new GameButton(ref textureInfo, "Quit", CommonHelper.GameFont));
			menuButtonList.Add(new GameButton(ref textureInfo, "Mute", CommonHelper.GameFont));
			menuButtonList.Add(new GameButton(ref textureInfo, "Resume", CommonHelper.GameFont));
			blankBackground = new SpriteTile(blankTextureInfo);
			blankBackground.TileIndex2D = CommonHelper.BlackTexturePosition;
			PositionComponents();
		}
		
		public GameButton GetResumeButton()
		{
			return menuButtonList[2];
		}
		
		public GameButton GetMuteButton()
		{
			return menuButtonList[1];
		}
		
		public GameButton GetQuitButton()
		{
			return menuButtonList[0];
		}
	}
}

