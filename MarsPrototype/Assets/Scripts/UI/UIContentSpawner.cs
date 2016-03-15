using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

public class UIContentSpawner : MonoBehaviour {

	public GameObject protoContent;
	public string sContentPath = "/Sponsors/";

	List<GameObject> l_gameobjs = new List<GameObject>();
	List<UIListItem> l_items = new List<UIListItem>();

	public GridLayoutGroup layoutg;

	void Start() {
		layoutg = this.GetComponent<GridLayoutGroup>();
		layoutg.cellSize = new Vector2(this.GetComponent<RectTransform>().rect.width, 20);
		LoadContent(sContentPath);
	}

	void Update() {
	
	}

	public void LoadContent(string path) {
		TextAsset[] getall = Resources.LoadAll<TextAsset>(path);

		foreach (TextAsset ta in getall) {
			GameObject gotemp = GameObject.Instantiate(protoContent);
			UIListItem item = gotemp.GetComponent<UIListItem>();

			gotemp.transform.SetParent(this.transform);

			item.taSource = ta;
			item.Init();
			item.LoadSponsorSettings();

			gotemp.name = "Sponsor " + item.sponsor.sSponsorName;

			l_gameobjs.Add(gotemp);
			l_items.Add(item);
		}
	}
}
