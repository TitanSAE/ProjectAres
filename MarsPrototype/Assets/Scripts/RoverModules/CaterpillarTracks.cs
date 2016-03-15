using UnityEngine;
using System.Collections;

public class CaterpillarTracks : RoverModule {

	public override void Init() {
		sName = "Caterpillar Tracks";
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
