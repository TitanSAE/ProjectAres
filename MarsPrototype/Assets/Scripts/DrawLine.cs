using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLine : MonoBehaviour {

	public int segments = 72;
	public float radius = 6f;
	public float zOffset = 2f;
	public bool overrideColor = true;
	public Color32 lineColor = new Color32(255, 204, 129, 255);
	public bool alwaysUp = true;
	public bool rotate = false;
	LineRenderer lineRenderer;

	public GameObject cube;

	Vector3[] points;
	public TextAsset presetCoordinatesFile;
	void Start()
	{
		

		Vector3[] positionArray = StringUtility.Vector3ArrayWithFile(presetCoordinatesFile);
		points = positionArray;
//

		int counter = 0;


			foreach (Vector3 i in points) {

				Vector3 pos = i;
				pos.y = Terrain.activeTerrain.SampleHeight (i);

				Material newMaterial = new Material(Shader.Find("Oculus/Unlit Transparent Color"));
				
				newMaterial.color = Heatmap.colorizedArray[counter] ;
				cube.GetComponent<Renderer> ().material = newMaterial;
				Instantiate(cube, i+ new Vector3(0,pos.y - 2.5f,0), Quaternion.identity);
				++counter;
			}
		Debug.Log ("first");
//				
			
	}



	IEnumerator Spawn3DHeatmap(float waitTime) {
		
		yield return new WaitForSeconds(waitTime);
		print("WaitAndPrint " + Time.time);
	}



	public Vector3[] MakeSmoothCurve(Vector3[] arrayToCurve,float smoothness){
		List<Vector3> points;
		List<Vector3> curvedPoints;
		int pointsLength = 0;
		int curvedLength = 0;

		if(smoothness < 1.0f) smoothness = 1.0f;

		pointsLength = arrayToCurve.Length;

		curvedLength = (pointsLength*Mathf.RoundToInt(smoothness))-1;
		curvedPoints = new List<Vector3>(curvedLength);

		float t = 0.0f;
		for(int pointInTimeOnCurve = 0;pointInTimeOnCurve < curvedLength+1;pointInTimeOnCurve++){
			t = Mathf.InverseLerp(0,curvedLength,pointInTimeOnCurve);

			points = new List<Vector3>(arrayToCurve);

			for(int j = pointsLength-1; j > 0; j--){
				for (int i = 0; i < j; i++){
					points[i] = (1-t)*points[i] + t*points[i+1];
				}
			}

			curvedPoints.Add(points[0]);
		}

		return(curvedPoints.ToArray());
	}


}
