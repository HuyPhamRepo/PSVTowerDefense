using System;
using System.Collections.Generic;
using System.IO;
using Sce.PlayStation.HighLevel.GameEngine2D;
namespace HybridActionTD
{
	public static class Player
	{
		public	static int				FinishedMission;
		public 	static int				CurrentMission;
		public	static int				CurrentWave;
		public	static int				CurrentVillagers;
		public	static bool				IsResumed;
		
		public static void Load()
		{
			if (File.Exists(CommonHelper.SaveDirectory + "ATDGame.sav"))
			{
				BinaryReader reader = new BinaryReader(File.OpenRead(CommonHelper.SaveDirectory + "ATDGame.sav"));
				FinishedMission = reader.ReadInt32();
				CurrentMission = reader.ReadInt32();
				CurrentWave = reader.ReadInt32();
				CurrentVillagers = reader.ReadInt32();
				reader.Close();
			}
			else
			{
				FinishedMission = 0;
				CurrentMission = 0;
				CurrentWave = 0;
				CurrentVillagers = 10;
			}
		}
		
		public static void LoadTowerData(List<int> towerList)
		{			
			if (File.Exists(CommonHelper.SaveDirectory + "Tower.sav"))
			{
				BinaryReader reader = new BinaryReader(File.OpenRead(CommonHelper.SaveDirectory + "Tower.sav"));
				int towerCount = reader.ReadInt32();
				for (int i = 0; i < towerCount; i++)
				{
					towerList.Add(reader.ReadInt32());
				}
				reader.Close();
			}
		}
		
		public static void Delete()
		{
//			if (File.Exists(CommonHelper.SaveDirectory + "ATDGame.sav"))
//			{
//				
//			}
		}
		
		public static void Save()
		{
			BinaryWriter writer = new BinaryWriter(File.Create(CommonHelper.SaveDirectory + "ATDGame.sav"));
			writer.Write(FinishedMission);
			writer.Write(CurrentMission);
			writer.Write(CurrentWave);
			writer.Write(CurrentVillagers);
			writer.Close();
		}
		
		public static void SaveTowerData(List<BasicTower> towerList)
		{
			BinaryWriter writer = new BinaryWriter(File.Create(CommonHelper.SaveDirectory + "Tower.sav"));
			writer.Write(towerList.Count * 3);
			for (int i = 0; i < towerList.Count; i++)
			{
				writer.Write((int)towerList[i].GetTowerType());
				writer.Write(towerList[i].GetGridPosition().X);
				writer.Write(towerList[i].GetGridPosition().Y);
			}
			writer.Close();
		}
	}
}