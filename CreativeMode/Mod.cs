using Il2CppSystem;
using PrimitierModdingFramework;
using PrimitierModdingFramework.Debugging;
using PrimitierModdingFramework.SubstanceModding;
using UnityEngine;


namespace CreativeMode
{
	public class Mod : PrimitierMod
    {
		//Vector3 initialBlockSize = new Vector3(0.5f,0.5f,0.5f);


		DateTime sysTime;// = new DateTime();

		long lastTimeBtnPressed = 0;

		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			base.OnSceneWasLoaded(buildIndex, sceneName);

			var creativeMenu = InGameDebugTool.CreateMenu("Creative Menu", "MainMenu");
			var creativeGodModeMenu = CreateCreativeGodMenu();

		}

		public override void OnRealyLateStart()
		{
			base.OnRealyLateStart();
			var creativeBlockBuilderMenu = BlockBuilderMenu.CreateBlockBuilderMenu(); //This is here so that modded substances that are created in OnSceneWasLoaded will be added to the spawn list
		}


		public override void OnApplicationStart()
		{
			base.OnApplicationStart();
			PMFSystem.EnableSystem<PMFHelper>();
			PMFSystem.EnableSystem<InGameDebuggingSystem>();
			PMFSystem.EnableSystem<CustomSubstanceSystem>();
			PMFSystem.EnableSystem<CustomAssetSystem>();

			PMFHelper.AutoInject(System.Reflection.Assembly.GetExecutingAssembly());
		}



		public InGameDebugToolToggleButton RegenHealthToggle;
		public InGameDebugToolToggleButton InfiniteHealthToggle;
		public InGameDebugToolToggleButton AntigravityToggle;
		
		public InGameDebugMenu CreateCreativeGodMenu()
		{
			var newmenu = InGameDebugTool.CreateMenu("God Modes", "Creative Menu");

			//Activate flying mode
			/* newmenu.CreateToggleWidget("fly", "Flying", false, new System.Action(() =>
			{
				godmodeFlying = !godmodeFlying;
				newmenu.UpdateToggleWidgetState("fly", godmodeFlying);
			})); */
			//Enable regenerating health over time
			RegenHealthToggle = newmenu.CreateToggleButton("Regen HP");

			//Activate infinite HP mode
			InfiniteHealthToggle = newmenu.CreateToggleButton("Infinite HP");

			//Activate infinite HP mode
			AntigravityToggle = newmenu.CreateToggleButton("AngtiGravity");

			return newmenu;
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

			if (RegenHealthToggle == null)
			{
				return;
			}


			if(RegenHealthToggle.Value)
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

			if(InfiniteHealthToggle.Value)
			{
				PlayerLife.MaxLife = float.MaxValue;
				PlayerLife.Life = float.MaxValue;
				last_infiniteHealth = true;
			}
			if(last_infiniteHealth == true && InfiniteHealthToggle.Value == false)
			{
				PlayerLife.MaxLife = 1000;
				PlayerLife.Life = 1000;
				last_infiniteHealth = false;
			}

			if(AntigravityToggle.Value)
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
			base.OnFixedUpdate();
			BlockBuilderMenu.UpdateCursor();
			
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
