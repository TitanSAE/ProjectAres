using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class RoverModuleDataHolder {

	public List<RoverModule> l_modules = new List<RoverModule>();

	public RoverModuleDataHolder() {
		Debug.Log("Eggnog");

		l_modules = new List<RoverModule>();

		//Camera
		l_modules.Add(new BasicCamera());

		//Battery
		l_modules.Add(new SolarPanel());
		l_modules.Add(new NuclearGenerator());

		//Locomotion
		l_modules.Add(new Wheels());
		l_modules.Add(new CaterpillarTracks());

		//Misc

	}
}
