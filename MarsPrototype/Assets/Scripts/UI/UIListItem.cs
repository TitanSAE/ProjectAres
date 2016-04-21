using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Text;

public class UIListItem : MonoBehaviour {

	public TextAsset taSource;
	public Text txtDisplay;
	public BoxCollider boxcol2d;

	public MarsSponsor sponsor;

	bool bUpdateRectSize;

	public UISceneManager uiMng;

	void Start() {
		//
	}

	public void Init() {
		txtDisplay = this.GetComponent<Text>();
		boxcol2d = this.GetComponent<BoxCollider>();
		sponsor = this.GetComponent<MarsSponsor>();

		uiMng = GameObject.FindGameObjectWithTag("GameController").GetComponent<UISceneManager>();
	}

	public void LoadSponsorSettings() {
		LoadSponsorFromXML();
		txtDisplay.text = sponsor.sSponsorName;
		bUpdateRectSize = true;
	}

	void OnMouseDown() {
		DisplaySponsorData();
	}

	public void DisplaySponsorData() {
		sponsor.LoadRoverInfo("Rovers/" + sponsor.sCurRoverFilename);
		uiMng.imgRoverPic.sprite = Sprite.Create(sponsor.texRoverPreview, new Rect(0, 0, sponsor.texRoverPreview.width, sponsor.texRoverPreview.height), uiMng.imgSponsorPic.rectTransform.pivot);
		uiMng.imgSponsorPic.sprite = Sprite.Create(sponsor.texSponsorAvatar, new Rect(0, 0, sponsor.texSponsorAvatar.width, sponsor.texSponsorAvatar.height), uiMng.imgSponsorPic.rectTransform.pivot);
		uiMng.txtRoverInfo.text = sponsor.sRoverDescription;
		uiMng.txtSponsorInfo.text = sponsor.sSponsorDescription;
		uiMng.sRoverName = sponsor.sRoverName;
		uiMng.sSponsorName = sponsor.sSponsorName;
	}

	void Update() {
		//
	}

	void OnGUI() {
		if (bUpdateRectSize) {
			GridLayoutGroup prt = transform.parent.gameObject.GetComponent<GridLayoutGroup>();
			this.transform.localScale = Vector3.one;
			boxcol2d.size = new Vector3(prt.cellSize.x, prt.cellSize.y, 1);
			bUpdateRectSize = false;
		}
	}

	public void LoadSponsorFromXML() {
		XDocument s_xmlDoc = XDocument.Parse(taSource.text);

		foreach (XElement xroot in s_xmlDoc.Elements()) {
			foreach (XElement xlayer1 in xroot.Elements()) {
				if (xlayer1.Name == "name") {
					sponsor.sSponsorName = xlayer1.Value;
				}
				else if (xlayer1.Name == "description") {
					sponsor.sSponsorDescription = xlayer1.Value;
				}
				else if (xlayer1.Name == "avatar") {
					string stemp = xlayer1.Value;
					sponsor.texSponsorAvatar = Resources.Load<Texture2D>("Sponsors/Images/" + stemp);
				}
				else if (xlayer1.Name == "rover") {
					string stemp = xlayer1.Value;
					sponsor.sCurRoverFilename = stemp;
				}
			}
		}
	}
}
