using System;
using System.Collections.Generic;
using Sce.PlayStation.Core;

namespace HybridActionTD
{
	public class QuadTreeNode
	{
		private		bool				IsLeafNode;
		private		bool				IsRootNode;
		private		QuadTreeNode		rootNode;
		private		List<QuadTreeNode>	childNodeList;
		private		List<int>			containedObjectIndex;
		private		Rectangle			nodeRect;
		
		public QuadTreeNode ()
		{
			IsLeafNode = true;
			IsRootNode = true;
			Initialize();
		}
		
		public QuadTreeNode (int childNodeCount)
		{
			IsLeafNode = false;
			IsRootNode = true;
			Initialize(childNodeCount);
		}
		
		public QuadTreeNode (ref QuadTreeNode rootNode)
		{
			IsRootNode = false;
			IsLeafNode = true;
			
			this.rootNode = rootNode;
			
			Initialize();
		}
		
		public QuadTreeNode (ref QuadTreeNode rootNode, int childNodeCount)
		{
			IsRootNode = false;
			IsLeafNode = false;
			
			this.rootNode = rootNode;
			
			Initialize(childNodeCount);
		}
		
		private void Initialize()
		{
			Initialize(0);
		}
		
		private void Initialize(int childNodeCount)
		{
			childNodeList = new List<QuadTreeNode>(childNodeCount);	
			
			containedObjectIndex = new List<int>(10);
		}
		
		public void InitializeRectangle(Vector2 position, Vector2 size)
		{
			this.nodeRect = new Rectangle(position, size);
		}
		
		public int GetChildNodeCount()
		{
			return childNodeList.Count;
		}
		
		public bool CheckRootNode()
		{
			return IsRootNode;
		}
		
		public bool CheckLeafNode()
		{
			return IsLeafNode;
		}
	}
}

