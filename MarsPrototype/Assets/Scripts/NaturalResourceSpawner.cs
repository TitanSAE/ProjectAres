using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NaturalResourceSpawner : MonoBehaviour {

	public bool bSpawnRocks = true;
	public Vector2 vRockSpawnMinBounds = new Vector2(10, 10);
	public Vector2 vRockSpawnBounds = new Vector2(50, 50);
	public int iRockAmount = 35;
	public int iRockVariance = 10;
	public GameObject goProtoRock;
	public List<MarsResource> l_rocks = new List<MarsResource>();

	void Start() {
		if (bSpawnRocks) {
			int maxrock = iRockAmount + Random.Range(-iRockVariance, iRockVariance);

			for (int i = 0; i < maxrock; i++) {
				float rx = 0;
				float rz = 0;

				while (rx == 0 || rz == 0) {
					rx = Random.Range(-vRockSpawnBounds.x, vRockSpawnBounds.x) + this.transform.position.x;
					rz = Random.Range(-vRockSpawnBounds.y, vRockSpawnBounds.y) + this.transform.position.z;

					if ((rx - this.transform.position.x) > -vRockSpawnMinBounds.x && (rx - this.transform.position.x) < vRockSpawnMinBounds.x) {
						rx = 0;
					}

					if ((rz - this.transform.position.z) > -vRockSpawnMinBounds.y && (rz - this.transform.position.z) < vRockSpawnMinBounds.y) {
						rz = 0;
					}
				}

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
