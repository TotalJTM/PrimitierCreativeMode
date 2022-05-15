using PrimitierModdingFramework;
using PrimitierModdingFramework.Debugging;
using PrimitierModdingFramework.SubstanceModding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CreativeMode
{
	public static class BlockBuilderMenu
	{
		public static Vector3 blockSize = new Vector3(0.5f, 0.5f, 0.5f);
		public static float upperBlockSizeLim = 2f;
		public static float lowerBlockSizeLim = 0.025f;
		public static float blockSizeInc = 0.025f;
		public static float blockSizeDec = 0.025f;
		public static Vector3 creativeBlockGenerationOffset = new Vector3(.75f, 0.0f, -0.75f);

		public static GameObject SpawnCursor;
		public static InGameDebugMenu BlockbuilderMenu;

		private static int _spawnCursorBlinkCounter = 0;

		public static InGameDebugMenu CreateBlockBuilderMenu()
		{
			SpawnCursor = GameObject.CreatePrimitive(PrimitiveType.Cube);
			UnityEngine.Object.Destroy(SpawnCursor.GetComponent<BoxCollider>());
			SpawnCursor.GetComponent<MeshRenderer>().material.color = new Color(0, 0.5f, 1f);

			BlockbuilderMenu = InGameDebugTool.CreateMenu("Blockbuilder Menu", "Creative Menu");

			//Increment Buttons
			BlockbuilderMenu.CreateButton("IncX", new System.Action(() =>
			{
				if (blockSize.x + blockSizeInc < (upperBlockSizeLim + 0.001))
				{
					blockSize.x += blockSizeInc;
				}
			}));
			BlockbuilderMenu.CreateButton("IncY", new System.Action(() =>
			{
				if (blockSize.y + blockSizeInc < (upperBlockSizeLim + 0.001))
				{
					blockSize.y += blockSizeInc;
				}
			}));
			BlockbuilderMenu.CreateButton("IncZ", new System.Action(() =>
			{
				if (blockSize.z + blockSizeInc < (upperBlockSizeLim + 0.001))
				{
					blockSize.z += blockSizeInc;
				}
			}));
			////Label Widgets
			//newmenu.CreateLabelWidget("Xcounter", blockSize.x.ToString("0.000"));
			//newmenu.CreateLabelWidget("Ycounter", blockSize.y.ToString("0.000"));
			//newmenu.CreateLabelWidget("Zcounter", blockSize.z.ToString("0.000"));
			
			//Decrement Buttons
			BlockbuilderMenu.CreateButton("DecX", new System.Action(() =>
			{
				if (blockSize.x - blockSizeDec > (lowerBlockSizeLim - 0.001))
				{
					blockSize.x -= blockSizeDec;
					//newmenu.SetLabelWidgetText("Xcounter", blockSize.x.ToString("0.000"));
				}
			}));
			BlockbuilderMenu.CreateButton("DecY", new System.Action(() =>
			{
				if (blockSize.y - blockSizeDec > (lowerBlockSizeLim - 0.001))
				{
					blockSize.y -= blockSizeDec;
					//newmenu.SetLabelWidgetText("Ycounter", blockSize.y.ToString("0.000"));
				}
			}));
			BlockbuilderMenu.CreateButton("DecZ", new System.Action(() =>
			{
				if (blockSize.z - blockSizeDec > (lowerBlockSizeLim - 0.001))
				{
					blockSize.z -= blockSizeDec;
					//newmenu.SetLabelWidgetText("Zcounter", blockSize.z.ToString("0.000"));
				}
			}));

			BlockbuilderMenu.CreateButton("Set Small\nBlock Size", new System.Action(() =>
			{
				blockSize = new Vector3(0.25f, 0.25f, 0.25f); ;
			}));
			BlockbuilderMenu.CreateButton("Set Large\nBlock Size", new System.Action(() =>
			{
				blockSize = new Vector3(1.5f, 1.5f, 1.5f); ;

			}));


			//SpawningObjects
			if (SubstanceManager.instance == null)
			{
				SubstanceManager.instance = Resources.Load<SubstanceParameters>(SubstanceManager.scriptableObjectPath);
			}


			for (int i = 0; i < SubstanceManager.instance.param.Count; i++)
			{
				var sub = SubstanceManager.instance.param[i];
				var name = Localizer.GetLocalizedString(sub.displayNameKey);
				Substance blockIdSubstance = (Substance)i;

				BlockbuilderMenu.CreateButton(name, new System.Action(() => 
				{
					CubeGenerator.GenerateCube(SpawnCursor.transform.position, SpawnCursor.transform.localScale, blockIdSubstance);

				}));

			}

			//Other objects
			BlockbuilderMenu.CreateButton("Generate\nDrone", new System.Action(() =>
			{
				CubeGenerator.GenerateDrone(SpawnCursor.transform.position, SpawnCursor.transform.localScale.magnitude);

			}));
			BlockbuilderMenu.CreateButton("Generate\nEngine", new System.Action(() =>
			{
				CubeGenerator.GenerateEngine(SpawnCursor.transform.position, Quaternion.identity);
			}));

			
			foreach (var treeType in Enum.GetValues(typeof(CubeGenerator.TreeType)))
			{
				BlockbuilderMenu.CreateButton($"Generate\nTree ({Enum.GetName(typeof(CubeGenerator.TreeType), treeType)})", new System.Action(() =>
				{
					CubeGenerator.GenerateTree(SpawnCursor.transform.position, SpawnCursor.transform.localScale.magnitude, (CubeGenerator.TreeType)treeType);

				}));
			}
			
			BlockbuilderMenu.CreateButton("Generate\nCactus", new System.Action(() =>
			{
				CubeGenerator.GenerateCactus(SpawnCursor.transform.position, SpawnCursor.transform.localScale.magnitude);

			}));

			BlockbuilderMenu.CreateButton("Generate\nSlime", new System.Action(() =>
			{
				CubeGenerator.GenerateSlime(SpawnCursor.transform.position, SpawnCursor.transform.localScale.magnitude);

			}));
			BlockbuilderMenu.CreateButton("Generate\nRed slime", new System.Action(() =>
			{
				CubeGenerator.GenerateRedSlime(SpawnCursor.transform.position, SpawnCursor.transform.localScale.magnitude);

			}));
			BlockbuilderMenu.CreateButton("Generate\nGreen slime", new System.Action(() =>
			{
				CubeGenerator.GenerateGreenSlime(SpawnCursor.transform.position, SpawnCursor.transform.localScale.magnitude);

			}));
			BlockbuilderMenu.CreateButton("Generate\nMonument", new System.Action(() =>
			{
				CubeGenerator.GenerateMonument(SpawnCursor.transform.position);
			}));
			BlockbuilderMenu.CreateButton("Generate\nRespawn point", new System.Action(() =>
			{
				CubeGenerator.GenerateRespawnPoint(SpawnCursor.transform.position);
			}));

			return BlockbuilderMenu;
		}





		public static void UpdateCursor()
		{
			if (SpawnCursor == null)
			{
				return;
			}

			SpawnCursor.SetActive(InGameDebugTool.IsShown);

			SpawnCursor.transform.localScale = blockSize;
			SpawnCursor.transform.position = BlockbuilderMenu.transform.position + creativeBlockGenerationOffset;


			if (_spawnCursorBlinkCounter >= 30)
			{
				SpawnCursor.GetComponent<MeshRenderer>().enabled = !SpawnCursor.GetComponent<MeshRenderer>().enabled;
				_spawnCursorBlinkCounter = 0;
			}


			_spawnCursorBlinkCounter++;
		}

	}
}
