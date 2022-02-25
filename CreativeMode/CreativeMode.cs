﻿using Il2CppSystem;
using PrimitierModdingFramework;
using PrimitierModdingFramework.Debugging;
using PrimitierModdingFramework.SubstanceModding;
using UnityEngine;


namespace CreativeMode
{
	public class CreativeMode : PrimitierMod
    {
		//Vector3 initialBlockSize = new Vector3(0.5f,0.5f,0.5f);
		Vector3 blockSize = new Vector3(0.5f,0.5f,0.5f);
		float upperBlockSizeLim = 2f;
		float lowerBlockSizeLim = 0.025f;
		float blockSizeInc = 0.025f;
		float blockSizeDec = 0.025f;
		Vector3 creativeBlockGenerationOffset = new Vector3(.75f, 0.0f, -0.75f);

		DateTime sysTime;// = new DateTime();

		long lastTimeBtnPressed = 0;

		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			base.OnSceneWasLoaded(buildIndex, sceneName);

			var creativeMenu = InGameDebugTool.CreateMenu("Creative Menu", "MainMenu");
			var creativeBlockBuilderMenu = CreateCreativeBlockBuilderMenu();
			var creativeGodModeMenu = CreateCreativeGodMenu();
			
			LoadCustomSubstances();
		}

		public override void OnApplicationStart()
		{
			base.OnApplicationStart();
			PMFSystem.EnableSystem<PMFHelper>();
			PMFSystem.EnableSystem<InGameDebuggingSystem>();
			PMFSystem.EnableSystem<CustomSubstanceSystem>();
			PMFSystem.EnableSystem<CustomAssetSystem>();
		}

		public InGameDebugMenu CreateCreativeBlockBuilderMenu(){
			var newmenu = InGameDebugTool.CreateMenu("Blockbuilder Menu", "Creative Menu");

			//Increment Buttons
			newmenu.CreateButton("IncX", new System.Action(() =>
			{
				if(blockSize.x+blockSizeInc < (upperBlockSizeLim+0.001)){
					blockSize.x += blockSizeInc;
					newmenu.SetLabelWidgetText("Xcounter",blockSize.x.ToString("0.000"));
				}
				UpdateLastRecordedButtonPress();
			}));
			newmenu.CreateButton("IncY", new System.Action(() =>
			{
				if(blockSize.y+blockSizeInc < (upperBlockSizeLim+0.001)){
					blockSize.y += blockSizeInc;
					newmenu.SetLabelWidgetText("Ycounter",blockSize.y.ToString("0.000"));
				}
				UpdateLastRecordedButtonPress();
			}));
			newmenu.CreateButton("IncZ", new System.Action(() =>
			{
				if(blockSize.z+blockSizeInc < (upperBlockSizeLim+0.001)){
					blockSize.z += blockSizeInc;
					newmenu.SetLabelWidgetText("Zcounter",blockSize.z.ToString("0.000"));
				}
				UpdateLastRecordedButtonPress();
			}));
			//Label Widgets
			newmenu.CreateLabelWidget("Xcounter", blockSize.x.ToString("0.000"));
			newmenu.CreateLabelWidget("Ycounter", blockSize.y.ToString("0.000"));
			newmenu.CreateLabelWidget("Zcounter", blockSize.z.ToString("0.000"));
			//Decrement Buttons
			newmenu.CreateButton("DecX", new System.Action(() =>
			{
				if(blockSize.x-blockSizeDec > (lowerBlockSizeLim-0.001)){
					blockSize.x -= blockSizeDec;
					newmenu.SetLabelWidgetText("Xcounter",blockSize.x.ToString("0.000"));
				}
				UpdateLastRecordedButtonPress();
			}));
			newmenu.CreateButton("DecY", new System.Action(() =>
			{
				if(blockSize.y-blockSizeDec > (lowerBlockSizeLim-0.001)){
					blockSize.y -= blockSizeDec;
					newmenu.SetLabelWidgetText("Ycounter",blockSize.y.ToString("0.000"));
				}
				UpdateLastRecordedButtonPress();
			}));
			newmenu.CreateButton("DecZ", new System.Action(() =>
			{
				if(blockSize.z-blockSizeDec > (lowerBlockSizeLim-0.001)){
					blockSize.z -= blockSizeDec;
					newmenu.SetLabelWidgetText("Zcounter",blockSize.z.ToString("0.000"));
				}
				UpdateLastRecordedButtonPress();
			}));

			newmenu.CreateButton("Leaf", new System.Action(() =>
			{
				CubeGenerator.GenerateCube(newmenu.transform.position + creativeBlockGenerationOffset, blockSize, Substance.Leaf);
			}));
			newmenu.CreateButton("Wood", new System.Action(() =>
			{
				CubeGenerator.GenerateCube(newmenu.transform.position + creativeBlockGenerationOffset, blockSize, Substance.Wood);
			}));
			newmenu.CreateButton("Set Small\nBlock Size", new System.Action(() =>
			{
				blockSize = new Vector3(0.25f,0.25f,0.25f);;
				newmenu.SetLabelWidgetText("Xcounter",blockSize.x.ToString("0.000"));
				newmenu.SetLabelWidgetText("Ycounter",blockSize.y.ToString("0.000"));
				newmenu.SetLabelWidgetText("Zcounter",blockSize.z.ToString("0.000"));
			}));
			newmenu.CreateButton("Stone", new System.Action(() =>
			{
				CubeGenerator.GenerateCube(newmenu.transform.position + creativeBlockGenerationOffset, blockSize, Substance.Stone);
			}));
			newmenu.CreateButton("Iron", new System.Action(() =>
			{
				CubeGenerator.GenerateCube(newmenu.transform.position + creativeBlockGenerationOffset, blockSize, Substance.Iron);
			}));
			newmenu.CreateButton("Set Large\nBlock Size", new System.Action(() =>
			{
				blockSize = new Vector3(1.5f,1.5f,1.5f);;
				newmenu.SetLabelWidgetText("Xcounter",blockSize.x.ToString("0.000"));
				newmenu.SetLabelWidgetText("Ycounter",blockSize.y.ToString("0.000"));
				newmenu.SetLabelWidgetText("Zcounter",blockSize.z.ToString("0.000"));
			}));
			newmenu.CreateButton("Ancient\nAlloy", new System.Action(() =>
			{
				CubeGenerator.GenerateCube(newmenu.transform.position + creativeBlockGenerationOffset, blockSize, Substance.AncientAlloy);
			}));
			newmenu.CreateButton("Silver", new System.Action(() =>
			{
				CubeGenerator.GenerateCube(newmenu.transform.position + creativeBlockGenerationOffset, blockSize, Substance.Silver);
			}));

			return newmenu;
		}

		public void LoadCustomSubstances(){
			var customSubstance = CustomMaterial();
			CustomSubstanceSystem.LoadCustomSubstance(customSubstance);

			customSubstance = CreateUnobtainium();
			CustomSubstanceSystem.LoadCustomSubstance(customSubstance);

			customSubstance = CreateUnobtainaminium();
			CustomSubstanceSystem.LoadCustomSubstance(customSubstance);
		}

		public SubstanceParameters.Param CustomMaterial()
		{
			var customMat = CustomSubstanceSystem.CreateCustomMaterial("Iron");
			customMat.name = "CustomMat";
			customMat.color = new Color(0, 1, 1);
			

			CustomSubstanceSystem.LoadCustomMaterial(customMat);

			var customSubstance = CustomSubstanceSystem.CreateCustomSubstance(Substance.Iron);

			customSubstance.displayNameKey = "SUB_CUSTOM";
			customSubstance.collisionSound = "metal1";
			customSubstance.isEdible = true;
			customSubstance.material = "CustomMat";
			customSubstance.stiffness = 9999999; //Damage
        	customSubstance.density = 50; //Mass through =m/v relationship?
			customSubstance.strength = 99999; //HP

			return customSubstance;
		}

		public SubstanceParameters.Param CreateUnobtainium()
		{
			var customMat = CustomSubstanceSystem.CreateCustomMaterial("AncientAlloy");
			customMat.name = "Unobtainium";
			customMat.color = new Color(1f, 0.3f, 1f);
			

			CustomSubstanceSystem.LoadCustomMaterial(customMat);

			var customSubstance = CustomSubstanceSystem.CreateCustomSubstance(Substance.AncientAlloy);

			customSubstance.displayNameKey = "SUB_UNOB";
			customSubstance.collisionSound = "papa1";
			customSubstance.isFlammable = false;
			customSubstance.isEdible = false;
			customSubstance.material = "Unobtainium";
			customSubstance.stiffness = 10; //Damage
        	customSubstance.density = 0.5f; //Mass through =m/v relationship?
			customSubstance.strength = 99999; //HP

			return customSubstance;
		}

		public SubstanceParameters.Param CreateUnobtainaminium()
		{
			var customMat = CustomSubstanceSystem.CreateCustomMaterial("Leaf");
			customMat.name = "Unobtainaminium";
			customMat.color = new Color(0.8f,1f,0.6f);
			

			CustomSubstanceSystem.LoadCustomMaterial(customMat);

			var customSubstance = CustomSubstanceSystem.CreateCustomSubstance(Substance.Leaf);

			customSubstance.displayNameKey = "SUB_UNAL";
			customSubstance.collisionSound = "leaf1";
			customSubstance.isFlammable = false;
			customSubstance.isEdible = false;
			customSubstance.material = "Unobtainaminium";
			customSubstance.stiffness = 10; //Damage
        	customSubstance.density = 0.01f; //Mass through =m/v relationship?
			customSubstance.strength = 99999; //HP

			return customSubstance;
		}

		public bool godmodeFlying = false;
		public bool regenHealth = false;
		public bool infiniteHealth = false;
		public bool antigravity = false;
		public InGameDebugMenu CreateCreativeGodMenu(){
			var newmenu = InGameDebugTool.CreateMenu("God Modes", "Creative Menu");

			//Activate flying mode
			/* newmenu.CreateToggleWidget("fly", "Flying", false, new System.Action(() =>
			{
				godmodeFlying = !godmodeFlying;
				newmenu.UpdateToggleWidgetState("fly", godmodeFlying);
			})); */
			//Enable regenerating health over time
			newmenu.CreateToggleWidget("regen", "Regen HP", false, new System.Action(() =>
			{
				regenHealth = !regenHealth;
				newmenu.UpdateToggleWidgetState("regen", regenHealth);
			}));
			//Activate infinite HP mode
			newmenu.CreateToggleWidget("infiniteHP", "Infinite HP", false, new System.Action(() =>
			{
				infiniteHealth = !infiniteHealth;
				newmenu.UpdateToggleWidgetState("infiniteHP", infiniteHealth);
			}));
			//Activate infinite HP mode
			newmenu.CreateToggleWidget("antigrav", "AngtiGravity", false, new System.Action(() =>
			{
				antigravity = !antigravity;
				newmenu.UpdateToggleWidgetState("antigrav", antigravity);
			}));
			return newmenu;
		}
		public void UpdateLastRecordedButtonPress(){
			lastTimeBtnPressed = sysTime.Ticks;
		}
		public long TimeSinceLastButtonPress(){
			return sysTime.Ticks - lastTimeBtnPressed;
		}

		private bool last_infiniteHealth = false;
		private float lastRegenHealth = 1000f;
		private float regenDelay = 1.0f;
		private float regenIncrease = 10.0f;
		public float currTime = 0.0f;
		private float lastTime = 0.0f;
		private byte regenCooldownCounter = 0;
		private byte regenCooldownCounterResetVal = 7;
		private float lastZHeight = 0.0f;
		//private float lastSystemTime = 0.0;
		
		public override void OnUpdate()
		{
			base.OnUpdate();
			sysTime = DateTime.Now;

			if(regenHealth)
			{
				float currTime = Time.fixedUnscaledTime;
				//PMFLog.Message("CurrTime: " + currTime.ToString() + " LastRecord: " + lastTime.ToString() + " Delta: " + (currTime - lastTime).ToString());
				if(currTime - lastTime > regenDelay)
				{
					if(lastRegenHealth != PlayerLife.Life)
					{
						regenCooldownCounter = regenCooldownCounterResetVal;
					}
					if(regenCooldownCounter == 0)
					{
						if(PlayerLife.Life+regenIncrease > 1000f)
						{
							PlayerLife.Life = 1000f;
						}
						else
						{
							PlayerLife.Life += regenIncrease;
						}
					}
					if(regenCooldownCounter > 0)
					{
						//PMFLog.Message("regencooldown subtraction: " + regenCooldownCounter.ToString());
						regenCooldownCounter--;
					}
					lastRegenHealth = PlayerLife.Life;
					lastTime = currTime;
				}
			}

			if(infiniteHealth)
			{
				PlayerLife.MaxLife = float.MaxValue;
				PlayerLife.Life = float.MaxValue;
				last_infiniteHealth = true;
			}
			if(last_infiniteHealth == true && infiniteHealth == false)
			{
				PlayerLife.MaxLife = 1000;
				PlayerLife.Life = 1000;
				last_infiniteHealth = false;
			}

			if(antigravity)
			{ 
				PMFHelper.PlayerMovement.rb.useGravity = false;
			}
			else
			{
				PMFHelper.PlayerMovement.rb.useGravity = true;
			}
			//PMFLog.Message("Max air move: " + PlayerMovement.maxAirMovePower.ToString() + "   Max air move: " + PlayerMovement.airMovePowerMlp.ToString());
			//PMFLog.Message("Moving threshold: " + PlayerMovement.movingThreshold.ToString() + "   Ground Move Power: " + PlayerMovement.groundMovePowerMlp.ToString());

		}

		public override void OnFixedUpdate()
		{
			
			/* if(godmodeFlying)
			{ 
				//PMFLog.Message("Jump cool time " + PlayerMovement.jumpCoolTime.ToString()); //default 0.5
				
				//for some reason +y is up, +x is to left of spawn (death respawn attitude) and +z is straight ahead of spawn
				Vector3 tempPosition = PMFHelper.PlayerMovement.rb.position;
				PMFLog.Message("Position: (" + tempPosition.x.ToString() + "," + tempPosition.y.ToString() + "," + tempPosition.z.ToString() + ")");
				//tempPosition.y = 15.0f;
				PMFHelper.PlayerMovement.rb.useGravity = false;
				PMFHelper.PlayerMovement.rb.velocity = new Vector3(0f,0f,1f);
				//PMFHelper.PlayerMovement.rb.position = tempPosition;
				if(lastZHeight > tempPosition.y)
				{
					tempPosition.y = lastZHeight;
					//PMFHelper.PlayerMovement.rb.position = tempPosition;
				}
				else
				{
					lastZHeight = tempPosition.y;
				}
			}
			else
			{
				PMFHelper.PlayerMovement.rb.useGravity = true;
			} */
		}
	}
}
