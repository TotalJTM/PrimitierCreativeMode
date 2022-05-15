using PrimitierModdingFramework.SubstanceModding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CreativeMode.Substances
{
	public class UnobtainaminiumBuilder : CustomSubstanceBuilder
	{
		public override void OnBuild()
		{
			var customMat = CustomSubstanceSystem.CreateCustomMaterial("Leaf");
			customMat.name = "Unobtainaminium";
			customMat.color = new Color(0.8f, 1f, 0.6f);


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

			CustomSubstanceSystem.LoadCustomSubstance(customSubstance, new CustomSubstanceSettings()
			{
				EnName = "Unobtainium",
				JpName = "ウンオブタミニウム"

			});
		}
	}
}
