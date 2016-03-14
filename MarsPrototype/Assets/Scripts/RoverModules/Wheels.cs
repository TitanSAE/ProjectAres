using UnityEngine;
using System.Collections;

public class Wheels : RoverModule {

	public override void Init() {
		sName = "Wheels";
		eSlot = ROVER_MODULE_SLOT.LOCOMOTION;
		texIcon = Resources.Load<Texture2D>("Icons/icon-wheel");
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
