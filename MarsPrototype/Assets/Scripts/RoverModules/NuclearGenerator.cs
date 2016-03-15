using UnityEngine;
using System.Collections;

public class NuclearGenerator : RoverModule {

	public override void Init() {
		sName = "Nuclear Generator";
		eSlot = ROVER_MODULE_SLOT.BATTERY;
		texIcon = Resources.Load<Texture2D>("Icons/icon-nuke");
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
