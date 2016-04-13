using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarsDeliveryManager : MonoBehaviour {

	public List<Transform> l_tSpawnPoints = new List<Transform>();
	public Stack<BASE_ATTACHMENT> st_ePackages = new Stack<BASE_ATTACHMENT>();

	public GameObject goProtoPackage;
	public GameObject goTowbar;


	int [] spawnarray = new int[]{0,1,2,3,4,5};
	List<int>spawnlist = new List<int>();

	int [] spawnarray2 = new int[]{0,1,2,3,4,5};
	List<int>spawnlist2 = new List<int>();





	int GetUniqueRandom(bool reloadEmptyList){
		if(spawnlist.Count == 0 ){
			if(reloadEmptyList){
				spawnlist.AddRange(spawnarray); 
			}
			else{
				return -1; // here is up to you 
			}
		}
		int rand = Random.Range(0, spawnlist.Count);
		int value = spawnlist[rand];
		spawnlist.RemoveAt(rand);
		return value;
	}
	int GetUniqueRandomSpawn(bool reloadEmptyList){
		if(spawnlist2.Count == 0 ){
			if(reloadEmptyList){
				spawnlist2.AddRange(spawnarray2); 
			}
			else{
				return -1; // here is up to you 
			}
		}
		int rand = Random.Range(0, spawnlist2.Count);
		int value = spawnlist2[rand];
		spawnlist2.RemoveAt(rand);
		return value;
	}




	void Start() {
		spawnlist.AddRange(spawnarray);
		spawnlist2.AddRange(spawnarray2);
		for (int i = 0; i < 6; i++) {
			st_ePackages.Push((BASE_ATTACHMENT)GetUniqueRandom(true));
		}

	}


	void Update() {
	
	}

//	public void SpawnPackage() {
//		int rspot = GetUniqueRandomSpawn(true);
//
//		GameObject goTemp = (GameObject)GameObject.Instantiate(goProtoPackage, l_tSpawnPoints[rspot].position, Quaternion.identity);
//		MarsDelivery crate = goTemp.GetComponent<MarsDelivery>();
//		crate.goPlayerTowbar = goTowbar;
//		crate.eDeliveryType = st_ePackages.Pop();
//	}
	public void SpawnPackage(Vector3 Location) {
		int rspot = GetUniqueRandomSpawn(true);

		GameObject goTemp = (GameObject)GameObject.Instantiate(goProtoPackage, Location, Quaternion.identity);
		MarsDelivery crate = goTemp.GetComponent<MarsDelivery>();
		crate.goPlayerTowbar = goTowbar;
		crate.eDeliveryType = st_ePackages.Pop();
	}
}
