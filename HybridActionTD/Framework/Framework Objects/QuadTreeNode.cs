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
			Initialize();
		}
		
		public QuadTreeNode (ref QuadTreeNode rootNode, int childNodeCount)
		{
			IsRootNode = false;
			IsLeafNode = false;
			Initialize(childNodeCount);
		}
		
		private void Initialize()
		{
			childNodeList = new List<QuadTreeNode>();
		}
		
		private void Initialize(int childNodeCount)
		{
			childNodeList = new List<QuadTreeNode>(childNodeCount);	
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

