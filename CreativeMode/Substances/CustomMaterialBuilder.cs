using PrimitierModdingFramework.SubstanceModding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CreativeMode.Substances
{
	public class CustomMaterialBuilder : CustomSubstanceBuilder
	{
		public override void OnBuild()
		{
			var customMat = CustomSubstanceSystem.CreateCustomMaterial("Iron");
			customMat.name = "CustomMat_creativeMod"; //Changed this because it will conflict with the PMF demo mod
			customMat.color = new Color(0, 1, 1);


			CustomSubstanceSystem.LoadCustomMaterial(customMat);

			var customSubstance = CustomSubstanceSystem.CreateCustomSubstance(Substance.Iron);

			customSubstance.displayNameKey = "SUB_CUSTOM_CREATIVEMOD";
			customSubstance.collisionSound = "metal1";
			customSubstance.isEdible = true;
			customSubstance.material = "CustomMat_creativeMod";
			customSubstance.stiffness = 9999999; //Damage
			customSubstance.density = 50; //Mass through =m/v relationship?
			customSubstance.strength = 99999; //HP

			CustomSubstanceSystem.LoadCustomSubstance(customSubstance, new CustomSubstanceSettings()
			{
				EnName = "Custom material",
				JpName = "カスタム素材"

			});
		}
	}
}
