using UnityEngine;
using System.Collections;

public abstract class RoverModule {

	public string sName {protected set; get;}
	public ROVER_MODULE_SLOT eSlot {protected set; get;}
	public Texture2D texIcon {protected set; get;}
	//public GameObject goRover;
	//public Rover roverdata;

	public abstract void Init();

	public abstract void OnEquip();

	public abstract void OnUnEquip();

	public abstract void OnActivate();
}
