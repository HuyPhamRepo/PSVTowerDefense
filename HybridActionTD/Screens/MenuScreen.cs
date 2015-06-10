using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;

namespace HybridActionTD
{
	public class MenuScreen : Scene
	{
		private Texture2D texture;
		private TextureInfo tInfo;
		
		private GameButton	playButton;
		private GameButton	resumeButton;
		private GameButton	creditsButton;
		
		private SpriteList spriteList;
		
		List<TouchData> touchData;
		
		public MenuScreen ()
		{
			this.Camera.SetViewFromViewport();
			
			texture = new Texture2D(CommonHelper.ArtDirectory + "Button.png", false);
			tInfo = new TextureInfo(texture, CommonHelper.ButtonTileDivision);
			spriteList = new SpriteList(tInfo);
			this.AddChild(spriteList);
			
			creditsButton = new GameButton(this, ref spriteList, ref tInfo, "Credits", CommonHelper.GameFont);
			creditsButton.SetPosition(CommonHelper.ScreenSize.X - creditsButton.GetWidth() - 50, (CommonHelper.ScreenSize.Y - creditsButton.GetHeight() * 3 - 20 * 2) / 2);
			
			resumeButton = new GameButton(this, ref spriteList, ref tInfo, "Resume", CommonHelper.GameFont);
			resumeButton.SetPosition(creditsButton.GetPosition().X, creditsButton.GetPosition().Y + resumeButton.GetHeight() + 20);
			
			playButton = new GameButton(this, ref spriteList, ref tInfo, "New Game", CommonHelper.GameFont);
			playButton.SetPosition(resumeButton.GetPosition().X, resumeButton.GetPosition().Y + playButton.GetHeight() + 20);
			
            Scheduler.Instance.ScheduleUpdateForTarget(this,0,false);
		}
		
		public override void Update (float dt)
        {
			touchData = Touch.GetData(0);
			playButton.Update(dt, touchData);
			resumeButton.Update(dt, touchData);
			if (playButton.isSelected)
			{
				Director.Instance.ReplaceScene(new LoadScreen());
				Player.CurrentMission = 0;
				Player.CurrentWave = 0;
			}
			else if (resumeButton.isSelected)
			{
				Director.Instance.ReplaceScene(new LoadScreen(Player.CurrentMission));
				Player.IsResumed = true;
			}
			
            base.Update (dt);
			
        }
        
        public override void Draw ()
        {
			base.Draw();
        }
        
        ~MenuScreen()
        {
            
        }
	}
}

