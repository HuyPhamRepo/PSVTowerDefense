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
	public class CircleButton
	{
		protected	Vector2		position;
		protected	Vector2i	normalTileIndex;
		protected	Vector2i	focusedTileIndex;
						
		protected	Rectangle	boundingBox;

		protected 	SpriteTile	spriteTile;
		
		protected	ButtonState	buttonState;
		
		protected	Label		label;
		protected	Label		labelShadow;
		protected	Font		font;
		protected	Font		tempFont;
		protected	FontMap		fontMap;
		
		public		bool		isSelected;
		
		public CircleButton (Scene parentScene, ref SpriteList spriteList, ref TextureInfo textureInfo, string text, Font font, Vector2i normalTileIndex, Vector2i focusedTileIndex)
		{
			spriteTile = new SpriteTile(textureInfo);
			spriteTile.Quad.S = CommonHelper.ButtonTowerSize;
			spriteTile.Pivot = CommonHelper.ButtonTowerSize/2;
			spriteTile.Rotation = new Vector2(0, -1);
			
			this.font = (Font)font.ShallowClone();
			tempFont = (Font)font.ShallowClone();
			
			fontMap = new FontMap(tempFont);
			this.label = new Label(text, fontMap);
			this.labelShadow = new Label(text, fontMap);
			
			
			labelShadow.Color = new Vector4(0, 0, 0, 0.75f);
			
			this.normalTileIndex = normalTileIndex;
			this.focusedTileIndex = focusedTileIndex;
			
			PositionText();
			
			boundingBox = new Rectangle(spriteTile.Position.X, spriteTile.Position.Y, spriteTile.Quad.S.X, spriteTile.Quad.S.Y);
			
			parentScene.AddChild(labelShadow, CommonHelper.DrawOrderMenuDialog);
			parentScene.AddChild(label, CommonHelper.DrawOrderMenuDialog);
			spriteList.AddChild(spriteTile, CommonHelper.DrawOrderMenuDialog);
			isSelected = false;
			
			buttonState = ButtonState.Normal;
		}
		
		public void PositionText()
		{
			float length = font.GetTextWidth(label.Text);
			label.Position = new Vector2(spriteTile.Position.X + GetWidth() - length, spriteTile.Position.Y);
			labelShadow.Position = label.Position + new Vector2(2, -2);
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
						spriteTile.TileIndex2D = focusedTileIndex;
					}
					else if (touchData[0].Status == TouchStatus.Down)
					{
						buttonState = ButtonState.Down;
						spriteTile.TileIndex2D = normalTileIndex;
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
				spriteTile.TileIndex2D = normalTileIndex;
				isSelected = false;
			}
		}
		
		public void SetPosition(float x, float y)
		{
			spriteTile.Position = new Sce.PlayStation.Core.Vector2(x, y);
			PositionText();
			
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
		
		~CircleButton()
		{
			font.Dispose();
			tempFont.Dispose();
			fontMap.Dispose();
		}
	}
}

