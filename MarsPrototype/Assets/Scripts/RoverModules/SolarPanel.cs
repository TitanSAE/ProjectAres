using UnityEngine;
using System.Collections;

public class SolarPanel : RoverModule {

	public override void Init() {
		sName = "Solar Panel";
		eSlot = ROVER_MODULE_SLOT.BATTERY;
		texIcon = Resources.Load<Texture2D>("Icons/icon-sun");
	}

	public override void OnEquip() {
		//
	}

	public override void OnUnEquip() {
		//
	}

	public override void OnActivate() {
		//
	}
}
