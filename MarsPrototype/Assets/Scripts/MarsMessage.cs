using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MarsMessage {

    public string sTitle;
	public string sBody;
	public bool bRead;
	public bool bCompleted;

	public Texture2D texAvatar;

	public int iDayReceived;

	public GameObject goQuestObject;
	public float fProximityToFinish = 5.0f;

//	void Start() {
//	
//	}
//
//	void Update() {
//	
//	}

	public void LoadAvatar(string name) {
		texAvatar = Resources.Load<Texture2D>("Sponsors/Images/" + name);
		if (texAvatar == null) {
			Debug.LogError(sTitle + " loaded a null image!");
		}
		//Debug.Log("Loaded " + texAvatar.name);
	}
}
