using PrimitierModdingFramework.SubstanceModding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CreativeMode.Substances
{
	public class UnobtainiumBuilder : CustomSubstanceBuilder
	{
		public override void OnBuild()
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

			CustomSubstanceSystem.LoadCustomSubstance(customSubstance, new CustomSubstanceSettings() 
			{
				EnName = "Unobtainium",
				JpName = "アンオブタニウム"

			});
		}
	}
}
