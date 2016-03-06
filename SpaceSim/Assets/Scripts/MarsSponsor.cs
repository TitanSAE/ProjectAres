using UnityEngine;
using System.Collections;

using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Text;

public class MarsSponsor : MonoBehaviour {

	public string sSponsorName = "";
	public string sSponsorDescription = "";
	public Texture2D texSponsorAvatar;

	public string sCurRoverFilename = "none";
	public TextAsset taRoverSource = null;
	public string sRoverName = "";
	public string sRoverDescription = "";
	public Texture2D texRoverPreview;

	void Start() {
		//
	}

	void Update() {
		//
	}

	public void LoadRoverInfo(string path) {
		taRoverSource = Resources.Load<TextAsset>(path);

		//TextAsset[] tas = Resources.LoadAll<TextAsset>("Rovers/");

		//foreach (TextAsset ta in tas) {
			//Debug.Log(ta.name);
		//}
		//Debug.Log(tas.Count());

		//Debug.Log(path);
		//Debug.Log(taRoverSource.text);

		XDocument s_xmlDoc = XDocument.Parse(taRoverSource.text);

		foreach (XElement xroot in s_xmlDoc.Elements()) {
			foreach (XElement xlayer1 in xroot.Elements()) {
				if (xlayer1.Name == "name") {
					sRoverName = xlayer1.Value;
				}
				else if (xlayer1.Name == "description") {
					sRoverDescription = xlayer1.Value;
				}
				else if (xlayer1.Name == "avatar") {
					string stemp = xlayer1.Value;
					texRoverPreview = Resources.Load<Texture2D>("Rovers/Images/" + stemp);
				}
			}
		}
	}
}
