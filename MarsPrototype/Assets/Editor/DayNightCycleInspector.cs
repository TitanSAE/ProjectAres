//using UnityEngine;
//using UnityEditor;
//using System.Collections;
//
//[CustomEditor(typeof(DayNightCycle))]
//public class DayNightCycleInspector : Editor {
//
//	private bool bShowCustom = true;
//	private bool bShowDefault = false;
//
//	private float fSetTime = 0.5f;
//
//	void OnEnable() {
//		//
//	}
//	
//	public override void OnInspectorGUI() {
//		DayNightCycle DNC = target as DayNightCycle;
//
//		bShowCustom = EditorGUILayout.Foldout(bShowCustom, "Custom Inspector");
//		if (bShowCustom) {
//			GUILayout.BeginVertical("box");
//			DNC.fCycleDuration = EditorGUILayout.FloatField("Transition length", DNC.fCycleDuration);
//			DNC.fHoldDuration = EditorGUILayout.FloatField("Day/Night length", DNC.fHoldDuration);
//			GUILayout.Space(10.0f);
//			GUILayout.BeginHorizontal();
//			fSetTime = EditorGUILayout.FloatField(fSetTime);
//			fSetTime = Mathf.Clamp01(fSetTime);
//			if (GUILayout.Button("Set Time")) {
//				DNC.SetTime(fSetTime);
//			}
//			if (GUILayout.Button("Reset Time")) {
//				DNC.ResetTime();
//			}
//			GUILayout.EndHorizontal();
//			GUILayout.EndVertical();
//
//			GUILayout.BeginVertical("box");
//			GUILayout.Label("Is it daytime? " + DNC.bDaytime.ToString());
//			GUILayout.Label("Are we holding? " + DNC.bHold.ToString());
//			GUILayout.Space(10.0f);
//			GUILayout.Label("Days passed: " + DNC.iDayCount.ToString());
//			GUILayout.Label("World Time: " + DNC.fDayPortion.ToString("F"));
//			GUILayout.Label("Seconds Elapsed: " + DNC.fTimePassed.ToString("F") + " of " + ((DNC.fHoldDuration * 2) + (DNC.fCycleDuration * 2)).ToString());
//			GUILayout.EndVertical();
//
//			GUILayout.BeginVertical("box");
//			GUILayout.Label("Scene-Based Light Intensity");
//			DNC.fIntensityDay = EditorGUILayout.FloatField("Daytime: ", DNC.fIntensityDay);
//			DNC.fIntensityNight = EditorGUILayout.FloatField("Nightime: ", DNC.fIntensityNight);
//			GUILayout.Space(10.0f);
//
//			GUILayout.Label("Ambient Light Intensity");
//			DNC.fAmbientIntensityDay = EditorGUILayout.FloatField("Daytime: ", DNC.fAmbientIntensityDay);
//			DNC.fAmbientIntensityNight = EditorGUILayout.FloatField("Nightime: ", DNC.fAmbientIntensityNight);
//			GUILayout.EndVertical();
//
//			GUILayout.BeginVertical("box");
//			GUILayout.Label("Scene-Based Light");
//			DNC.colDay = EditorGUILayout.ColorField("Daytime: ", DNC.colDay);
//			DNC.colNight = EditorGUILayout.ColorField("Nightime: ", DNC.colNight);
//			GUILayout.Space(10.0f);
//
//			GUILayout.Label("Ambient Sky Light");
//			DNC.colAmbientSkyDay = EditorGUILayout.ColorField("Daytime: ", DNC.colAmbientSkyDay);
//			DNC.colAmbientSkyNight = EditorGUILayout.ColorField("Nightime: ", DNC.colAmbientSkyNight);
//			GUILayout.Space(10.0f);
//
//			GUILayout.Label("Ambient Equator Light");
//			DNC.colAmbientEquatorDay = EditorGUILayout.ColorField("Daytime: ", DNC.colAmbientEquatorDay);
//			DNC.colAmbientEquatorNight = EditorGUILayout.ColorField("Nightime: ", DNC.colAmbientEquatorNight);
//			GUILayout.Space(10.0f);
//
//			GUILayout.Label("Ambient Ground Light");
//			DNC.colAmbientGroundDay = EditorGUILayout.ColorField("Daytime: ", DNC.colAmbientGroundDay);
//			DNC.colAmbientGroundNight = EditorGUILayout.ColorField("Nightime: ", DNC.colAmbientGroundNight);
//			GUILayout.EndVertical();
//		}
//
//		bShowDefault = EditorGUILayout.Foldout(bShowDefault, "Default Inspector");
//		if (bShowDefault) {
//			DrawDefaultInspector();
//		}
//	}
//}
