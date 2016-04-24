using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NaturalResourceSpawner : MonoBehaviour {

	public bool bSpawnRocks = true;
	public Vector2 vRockSpawnBounds = new Vector2(50, 50);
	public int iRockAmount = 35;
	public int iRockVariance = 10;
	public GameObject goProtoRock;
	public List<MarsResource> l_rocks = new List<MarsResource>();

	void Start() {
		if (bSpawnRocks) {
			int maxrock = iRockAmount + Random.Range(-iRockVariance, iRockVariance);

			for (int i = 0; i < maxrock; i++) {
				float rx = Random.Range(-vRockSpawnBounds.x, vRockSpawnBounds.x) + this.transform.position.x;
				float rz = Random.Range(-vRockSpawnBounds.y, vRockSpawnBounds.y) + this.transform.position.z;

				float th = Terrain.activeTerrain.SampleHeight(new Vector3(rx, 0, rz)) + Terrain.activeTerrain.GetPosition().y;

				GameObject gotemp = (GameObject)GameObject.Instantiate(goProtoRock, new Vector3(rx, th, rz), goProtoRock.transform.rotation);
				gotemp.transform.parent = this.transform;
				l_rocks.Add(gotemp.GetComponent<MarsResource>());
			}
		}
	}

	void Update() {
	
	}
}
