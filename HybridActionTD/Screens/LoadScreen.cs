using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;

namespace HybridActionTD
{
	public class LoadScreen : Scene
	{
		private TextureInfo textureInfo;
        private Texture2D 	texture;
		private SpriteUV	screenUV;
		
		private TintTo 		tintFromBlack;
		private TintTo 		tintToBlack;
		private bool		nextScreen;
		
		private GameScreen gamescreen;
		private	int			map;
		
		public LoadScreen (int map = 0)
		{
			this.Camera.SetViewFromViewport();
			
			this.map = map;
			
			texture = new Texture2D(CommonHelper.ArtDirectory + "LoadScreen.png", false);
            textureInfo = new TextureInfo(texture);
            screenUV = new SpriteUV(textureInfo);
            screenUV.Scale = textureInfo.TextureSizef;
            screenUV.Position = new Vector2(0,0);
			
            this.AddChild(screenUV);
            
			Vector4 origColor = screenUV.Color;
            screenUV.Color = new Vector4(0,0,0,0);
				
            tintFromBlack = new TintTo(origColor, 1.0f);
			
			ActionManager.Instance.AddAction(tintFromBlack, screenUV);
			tintFromBlack.Run();
			
			nextScreen = false;
            
            Scheduler.Instance.ScheduleUpdateForTarget(this,0,false);

            // Clear any queued clicks so we dont immediately exit if coming in from the menu
            Touch.GetData(0).Clear();
		}
		
		public override void Update (float dt)
        {
            base.Update (dt);
			
            List<TouchData> touches = Touch.GetData(0);
			
			if (!nextScreen && !tintFromBlack.IsRunning)
			{
				tintToBlack = new TintTo(new Vector4(0,0,0,0), 2.0f);
				ActionManager.Instance.AddAction(tintToBlack, screenUV);
				gamescreen = new GameScreen(map);
				nextScreen = true;
				tintToBlack.Run();
			}
			if (nextScreen && !tintToBlack.IsRunning)
			{
				Director.Instance.ReplaceScene(gamescreen);
			}
        }
    
        ~LoadScreen()
        {
            texture.Dispose();
            textureInfo.Dispose ();
        }
	}
}

