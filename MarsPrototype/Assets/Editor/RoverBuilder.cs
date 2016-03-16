using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoverBuilder {

	//int curid = 0;

	//public List<ROVER_MODULE_SLOT> l_slots = new List<ROVER_MODULE_SLOT>();
	public List<KeyValuePair<ROVER_MODULE_SLOT, int>> l_loadedrmodules = new List<KeyValuePair<ROVER_MODULE_SLOT, int>>();
	//public List<RoverModule> l_modules = new List<RoverModule>();

	//public List<RoverModule> l_allmods = new List<RoverModule>();

	public void Init() {
		l_loadedrmodules.Clear();
	}

	public void AddMod(ROVER_MODULE_SLOT type, int num) {
		//l_slots.Add(type);
		//l_modules.Add(mod);
		l_loadedrmodules.Add(new KeyValuePair<ROVER_MODULE_SLOT, int>(type, num));
	}

	public void RemoveMod(ROVER_MODULE_SLOT type, int num) {
		//if (l_loadedrmodules.Contains(type)) {
			//l_loadedrmodules.Remove(type);
			//l_modules.Remove(mod);
		//}
	}
}
