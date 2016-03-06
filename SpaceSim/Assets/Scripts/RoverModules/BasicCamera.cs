using UnityEngine;
using System.Collections;

public class BasicCamera : RoverModule {

	public override void Init() {
		sName = "Basic Camera";
		eSlot = ROVER_MODULE_SLOT.CAMERA;
		texIcon = Resources.Load<Texture2D>("Icons/icon-camera");
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
