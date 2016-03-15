using UnityEngine;
using System.Collections;

//public enum ROVER_MODULE {
//	COMMUNICATIONS,
//	POWER_SOURCE,
//	SCANNER,
//	WHEELS,
//	TREADS,
//	DRONE_LAUNCHER,
//	ARM,
//	CAMERA
//}
//
//public enum ROVER_WHEELS {
//	SMALL,
//	MEDIUM,
//	LARGE
//}

public enum ROVER_MODULE_SLOT {
	NONE,
	LOCOMOTION,
	CHASSIS,
	BATTERY,
	SENSOR,
	CAMERA,
	MISC
}

public enum ROVER_MODULE_LOCOMOTION {
	NONE,
	Wheels,
	CaterpillarTracks
}

public enum ROVER_MODULE_BATTERY {
	NONE,
	SolarPanelling,
	NuclearBattery
}

public enum ROVER_MODULE_CHASSIS {
	NONE,
	ThinChassis
}

public enum ROVER_MODULE_SENSOR {
	NONE,
	RadiationSensor
}

public enum ROVER_MODULE_CAMERA {
	NONE,
	BasicCamera,
	MkIICamera
}

public enum ROVER_MODULE_MISC {
	NONE,
	HappyBirthday
}