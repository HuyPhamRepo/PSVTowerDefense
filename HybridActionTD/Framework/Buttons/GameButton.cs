using System;
using System.Collections.Generic;

using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.Core.Input;

namespace HybridActionTD
{
	public class GameButton
	{
		public		Vector2		position;
		
		protected	Rectangle	boundingBox;
		
		protected 	SpriteTile	spriteTile;
		
		protected	ButtonState	buttonState;
		
		protected	Label		label;
		protected	Label		labelShadow;
		protected	Font		font;
		protected	Font		tempFont;
		protected	FontMap		fontMap;
		
		public		bool		isSelected;
		
		public GameButton (ref TextureInfo textureInfo, string text, Font font) 
		{
			spriteTile = new SpriteTile(textureInfo);
			spriteTile.Quad.S = CommonHelper.ButtonSize;
			
			this.font = (Font)font.ShallowClone();
			tempFont = (Font)font.ShallowClone();
			
			fontMap = new FontMap(tempFont);
			this.label = new Label(text, fontMap);
			this.labelShadow = new Label(text, fontMap);
			labelShadow.Color = new Vector4(0, 0, 0, 0.75f);
			
			CenterText();
			
			boundingBox = new Rectangle(spriteTile.Position.X, spriteTile.Position.Y, spriteTile.Quad.S.X, spriteTile.Quad.S.Y);
			
			isSelected = false;
			
			buttonState = ButtonState.Normal;
		}
		
		public GameButton (Scene parentScene, ref SpriteList spriteList, ref TextureInfo textureInfo, string text, Font font) 
		{
			spriteTile = new SpriteTile(textureInfo);
			spriteTile.Quad.S = CommonHelper.ButtonSize;
			
			this.font = (Font)font.ShallowClone();
			tempFont = (Font)font.ShallowClone();
			
			fontMap = new FontMap(tempFont);
			this.label = new Label(text, fontMap);
			this.labelShadow = new Label(text, fontMap);
			labelShadow.Color = new Vector4(0, 0, 0, 0.75f);
			
			CenterText();
			
			parentScene.AddChild(labelShadow);
			parentScene.AddChild(label);
			
			boundingBox = new Rectangle(spriteTile.Position.X, spriteTile.Position.Y, spriteTile.Quad.S.X, spriteTile.Quad.S.Y);
			
			spriteList.AddChild(spriteTile);
			isSelected = false;
			
			buttonState = ButtonState.Normal;
		}
						
		public void CenterText()
		{
			float length = font.GetTextWidth(label.Text);
			label.Position = new Vector2(spriteTile.Position.X + spriteTile.Quad.S.X / 2 - length / 2, spriteTile.Position.Y + spriteTile.Quad.S.Y / 2 - (label.HeightScale * label.FontMap.CharPixelHeight) / 2);
			labelShadow.Position = label.Position + new Vector2(2, -2);
		}
		
		public void Display(Scene parentScene, ref SpriteList spriteList)
		{
			parentScene.AddChild(labelShadow, CommonHelper.DrawOrderMenuDialog);
			parentScene.AddChild(label, CommonHelper.DrawOrderMenuDialog);
			spriteList.AddChild(spriteTile, CommonHelper.DrawOrderMenuDialog);
		}
		
		public void Remove(Scene parentScene, ref SpriteList spriteList)
		{
			parentScene.RemoveChild(label, false);
			parentScene.RemoveChild(labelShadow, true);
			spriteList.RemoveChild(spriteTile, true);
		}
		
		public void Remove(Scene parentScene, ref SpriteList spriteList, bool sceneCleanup, bool spriteListCleanup)
		{
			parentScene.RemoveChild(label, sceneCleanup);
			parentScene.RemoveChild(labelShadow, sceneCleanup);
			spriteList.RemoveChild(spriteTile, spriteListCleanup);
		}
		
		public void Update(float dt, List<TouchData> touchData)
		{
			if (touchData.Count > 0)
			{
				if (CommonHelper.IsInside(new Vector2(CommonHelper.TouchToScreenX(touchData[0].X), CommonHelper.TouchToScreenY(touchData[0].Y)), boundingBox))
				{
					if (touchData[0].Status == TouchStatus.Up)
					{
						isSelected = true;
						buttonState = ButtonState.Touched;
						spriteTile.TileIndex2D = CommonHelper.ButtonNormalTileIndex;
					}
					else if (touchData[0].Status == TouchStatus.Down)
					{
						buttonState = ButtonState.Down;
						spriteTile.TileIndex2D = CommonHelper.ButtonFocusedTileIndex;
					}
//					else
//					{
//						buttonState = ButtonState.Normal;
//						spriteTile.TileIndex2D = CommonHelper.ButtonNormalTileIndex;
//					}
				}
			}
			else
			{
				spriteTile.TileIndex2D = CommonHelper.ButtonNormalTileIndex;
				isSelected = false;
			}
		}
		
		public void SetPosition(float x, float y)
		{
			spriteTile.Position = new Sce.PlayStation.Core.Vector2(x, y);
			CenterText();
			
			boundingBox.Position = spriteTile.Position;
		}
		
		public void SetHeight(float height)
		{
			spriteTile.Quad.S = new Sce.PlayStation.Core.Vector2(spriteTile.Quad.S.X, height);
		}
		
		public void SetWidth(float width)
		{
			spriteTile.Quad.S = new Sce.PlayStation.Core.Vector2(width, spriteTile.Quad.S.Y);
		}
		
		public void SetButtonSize(float width, float height)
		{
			spriteTile.Quad.S = new Sce.PlayStation.Core.Vector2(width, height);
		}
		
		public void SetButtonText(string text)
		{
			label.Text = text;
		}
		
		public void SetTextSize(float size)
		{
			label.HeightScale = size;
		}
		
		public SpriteTile GetSpriteTile()
		{
			return spriteTile;
		}
		
		public Label GetButtonLabel()
		{
			return label;
		}
		
		public Label GetButtonShadow()
		{
			return	labelShadow;
		}
		
		public float GetWidth()
		{
			return spriteTile.Quad.S.X;
		}
		
		public float GetHeight()
		{
			return spriteTile.Quad.S.Y;
		}
		
		public Vector2 GetPosition()
		{
			return spriteTile.Position;
		}
		
		~GameButton()
		{
			font.Dispose();
			tempFont.Dispose();
			fontMap.Dispose();
		}
	}
}

