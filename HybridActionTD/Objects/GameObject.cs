using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace HybridActionTD
{
	public class GameObject
	{
		protected 	string 		name;
		
		protected	Vector2 	position;
		protected	Vector2		centerPosition;
		
		protected 	Texture2D 	texture;
		protected 	TextureInfo textureInfo;
		protected 	SpriteUV	spriteUV;
		protected 	SpriteTile	spriteTile;
		
		protected 	Rectangle	boundingBox;
		
		public GameObject(ref Texture2D inputTexture, ref TextureInfo textureInfo, Vector2 position)
		{
			name = "GameObject";
			this.texture = inputTexture;
			//textureInfo = new TextureInfo(texture, CommonHelper.TextureDivision);
			this.textureInfo = textureInfo;
			spriteTile = new SpriteTile(textureInfo);
			spriteTile.Pivot = CommonHelper.CellSize/2;
			spriteTile.Quad.S = CommonHelper.CellSize;
			Init(position);
		}
		
		public GameObject(ref Texture2D inputTexture, ref TextureInfo textureInfo, Vector2 position, string name)
		{
			this.name = name;
			this.texture = inputTexture;
//			textureInfo = new TextureInfo(texture, CommonHelper.TextureDivision);
			this.textureInfo = textureInfo;
			spriteTile = new SpriteTile(textureInfo);
			spriteTile.Pivot = CommonHelper.CellSize/2;
			spriteTile.Quad.S = CommonHelper.CellSize;
			Init(position);
		}
		
		public GameObject(ref Texture2D inputTexture, ref TextureInfo textureInfo)
		{
			name = "GameObject";
			this.texture = inputTexture;
//			textureInfo = new TextureInfo(texture, CommonHelper.TextureDivision);
			this.textureInfo = textureInfo;
			spriteTile = new SpriteTile(textureInfo);
			spriteTile.Pivot = CommonHelper.CellSize/2;
			spriteTile.Quad.S = CommonHelper.CellSize;
		}
		
		public GameObject(ref Texture2D inputTexture, ref TextureInfo textureInfo, string name)
		{
			this.name = name;
			this.texture = inputTexture;
//			textureInfo = new TextureInfo(texture, CommonHelper.TextureDivision);
			this.textureInfo = textureInfo;
			spriteTile = new SpriteTile(textureInfo);
			spriteTile.Pivot = CommonHelper.CellSize/2;
			spriteTile.Quad.S = CommonHelper.CellSize;
		}
		
		public virtual void InitDefault()
		{
			this.position = new Vector2 (-100, -100);
			this.centerPosition = position + CommonHelper.CellSize/2;
			spriteTile.Position = position;
			boundingBox = new Rectangle ((int)position.X, (int)position.Y, CommonHelper.CellSize.X, CommonHelper.CellSize.Y);
		}
		
		public virtual void Init(Vector2 position)
		{
			this.position = position;
			this.centerPosition = position + CommonHelper.CellSize/2;
			spriteTile.Position = position;
			boundingBox = new Rectangle ((int)position.X, (int)position.Y, CommonHelper.CellSize.X, CommonHelper.CellSize.Y);
		}
		
//		public void SetMessenger(ref Messenger messenger)
//		{
//			this.messenger = messenger;
//		}
		
		public SpriteTile GetSpriteTile()
		{
			return spriteTile;
		}
		
		public Rectangle GetBoundingBox()
		{
			return boundingBox;
		}
		
		public Vector2 GetPosition()
		{
			return position;
		}
		
		public Vector2 GetCenterPosition()
		{
			return centerPosition;
		}
		
		public virtual void SetPosition(Vector2 position)
		{
			this.position = position;
			this.centerPosition = position + CommonHelper.CellSize/2;
			spriteTile.Position = position;
			boundingBox.X = (int)position.X;
			boundingBox.Y = (int)position.Y;
		}
	}
}

