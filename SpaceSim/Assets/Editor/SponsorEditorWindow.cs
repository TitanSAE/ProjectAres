using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Linq;
using System.Linq;

public enum SPONSOR_WINDOW_TABS {
	SPONSORS,
	ROVERS
}

public class SponsorEditorWindow : EditorWindow {

	SPONSOR_WINDOW_TABS eCurTab = SPONSOR_WINDOW_TABS.SPONSORS;
	Vector2 vScrollPos;

	//Sponsor bits
	string sSponsorName = "";
	string sSponsorDescription = "";
	Texture2D texSponsorAvatar;
	string sCurRover = "none";

	//Rover bits
	string sRoverName = "";
	string sRoverDescription = "";
	Texture2D texRoverPreview;

	RoverBuilder builder = new RoverBuilder();
	ROVER_MODULE_SLOT eCurSlot;

	List<DropdownList> l_dropdowns = new List<DropdownList>();
	List<int> l_dropchoices = new List<int>();
	List<string> l_loadedmodules = new List<string>();
	bool bLoadModulesFromList = false;
	Texture texNoChoice;
	bool bRemoveSlot;
	int iDeadSlot;

	//Window creation
	[MenuItem("Window/Sponsor and Rover Editor")]
	static void CreateWindow() {
		SponsorEditorWindow editor = EditorWindow.GetWindow<SponsorEditorWindow>();

		editor.Init();

		editor.titleContent = new GUIContent("Sponsor/Rover Editor");

		editor.Show();
	}

	public void ClearRover() {
		sRoverName = "";
		sRoverDescription = "";
		texRoverPreview = null;

		builder.Init();
		l_dropchoices.Clear();
		l_dropdowns.Clear();
		l_loadedmodules.Clear();
		bLoadModulesFromList = false;
	}

	public void ClearSponsor() {
		sSponsorName = "";
		sSponsorDescription = "";
		texSponsorAvatar = null;
		sCurRover = "none";
	}

	public void Init() {
		builder.Init();
		texNoChoice = Resources.Load<Texture2D>("Icons/icon-none");
		l_dropchoices.Clear();
		l_dropdowns.Clear();
		l_loadedmodules.Clear();
		bLoadModulesFromList = false;
	}

	void Update () {
		//
	}


	public int DrawTabs(int selected, params string[] options) {
		const float DarkGray = 0.4f;
		const float LightGray = 0.9f;
		const float StartSpace = 10.0f;

		GUILayout.Space(StartSpace);
		Color storeColor = GUI.backgroundColor;
		Color highlightCol = new Color(LightGray, LightGray, LightGray);
		Color bgCol = new Color(DarkGray, DarkGray, DarkGray);

		GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
		buttonStyle.padding.bottom = 8;

		GUILayout.BeginHorizontal(); {   //Create a row of buttons
			for (int i = 0; i < options.Length; ++i) {
				GUI.backgroundColor = i == selected ? highlightCol : bgCol;
				if (GUILayout.Button(options[i], buttonStyle)) {
					selected = i; //Tab click
				}
			}
		} GUILayout.EndHorizontal();
		//Restore color
		GUI.backgroundColor = storeColor;
		//Draw a line over the bottom part of the buttons
		var texture = new Texture2D(1, 1);
		texture.SetPixel(0, 0, highlightCol);
		texture.Apply();
		GUI.DrawTexture(new Rect(0, buttonStyle.lineHeight + buttonStyle.border.top + buttonStyle.margin.top + StartSpace,  Screen.width, 4),texture);

		return selected;
	}

	void OnGUI() {
		eCurTab = (SPONSOR_WINDOW_TABS)DrawTabs((int)eCurTab, "Sponsors", "Rovers");

		vScrollPos = GUILayout.BeginScrollView(vScrollPos);
		//Sponsors
		if (eCurTab == SPONSOR_WINDOW_TABS.SPONSORS) {
			GUILayout.BeginHorizontal(EditorStyles.helpBox);
			if (GUILayout.Button("Save Sponsor")) {
				string savepath = EditorUtility.SaveFilePanelInProject("Save Sponsor", sSponsorName, "xml", "Huh?", Application.dataPath + "/Resources/Sponsors/");
				try {
					SaveSponsorToXML(savepath);
				}
				catch (UnityException e) {
					EditorUtility.DisplayDialog("Error", e.Message, "OK");
				}
			}
			if (GUILayout.Button("Load Sponsor")) {
				if (EditorUtility.DisplayDialog("Are you sure?", "Are you sure you want to load? You will lose any unsaved changes.", "OK", "Cancel")) {
					string loadpath = EditorUtility.OpenFilePanelWithFilters("Load Sponsor", Application.dataPath + "/Resources/Sponsors/", new string[]{"XML", "xml"});
					try {
						LoadSponsorFromXML(loadpath);
					}
					catch (UnityException e) {
						EditorUtility.DisplayDialog("Error", e.Message, "OK");
					}
				}
			}
			if (GUILayout.Button("New")) {
				if (EditorUtility.DisplayDialog("Are you sure?", "Are you sure you want to make a new sponsor? You will lose any unsaved changes.", "OK", "Cancel")) {
					ClearSponsor();
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginVertical(EditorStyles.helpBox);
			GUILayout.Label("Name");
			sSponsorName = GUILayout.TextField(sSponsorName);
			GUILayout.Label("Description");
			sSponsorDescription = GUILayout.TextArea(sSponsorDescription);
			GUILayout.Label("Rover");

			GUILayout.BeginHorizontal(EditorStyles.helpBox);
			GUILayout.Label(sCurRover);
			if (GUILayout.Button("Edit")) {
				if (sCurRover != null && sCurRover != "" && sCurRover != "none") {
					eCurTab = SPONSOR_WINDOW_TABS.ROVERS;
					LoadRoverFromXML(Application.dataPath + "/Resources/Rovers/" + sCurRover + ".xml");
				}
			}
			if (GUILayout.Button("Change")) {
				string temprover = sCurRover;
				sCurRover = EditorUtility.OpenFilePanelWithFilters("Select Rover", Application.dataPath + "/Resources/Rovers/",  new string[]{"XML", "xml"}); 
				if (sCurRover.Length > 4) {
					System.IO.FileInfo finfo = new System.IO.FileInfo(sCurRover);
					sCurRover = finfo.Name.Remove(finfo.Name.Length - 4);
				}
				else {
					sCurRover = temprover;
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.Label("Avatar");
			texSponsorAvatar = (Texture2D)EditorGUILayout.ObjectField(texSponsorAvatar, typeof(Texture2D), false);
			if (texSponsorAvatar != null) {
				GUILayout.Box(texSponsorAvatar, GUILayout.Width(200), GUILayout.Height(200));
			}
			else {
				GUILayout.Box("Preview Pane", GUILayout.Width(200), GUILayout.Height(200));
			}
			GUILayout.EndVertical();
		}

		//Rovers
		if (eCurTab == SPONSOR_WINDOW_TABS.ROVERS) {
			GUILayout.BeginHorizontal(EditorStyles.helpBox);
			if (GUILayout.Button("Save Rover")) {
				string savepath = EditorUtility.SaveFilePanelInProject("Save Rover", sRoverName, "xml", "Huh?", Application.dataPath + "/Resources/Rovers/");
				if (savepath.Length > 4) {
					SaveRoverToXML(savepath);
				}
				else {
					EditorUtility.DisplayDialog("Error", "Nothing was saved - filename was invalid!", "OK");
				}
			}
			if (GUILayout.Button("Load Rover")) {
				if (EditorUtility.DisplayDialog("Are you sure?", "Are you sure you want to load? You will lose any unsaved changes.", "OK", "Cancel")) {
					string loadpath = EditorUtility.OpenFilePanelWithFilters("Load Rover", Application.dataPath + "/Resources/Rovers/", new string[]{"XML", "xml"});
					if (loadpath.Length > 4) {
						LoadRoverFromXML(loadpath);
					}
				}
			}
			if (GUILayout.Button("New")) {
				if (EditorUtility.DisplayDialog("Are you sure?", "Are you sure you want to make a new rover? You will lose any unsaved changes.", "OK", "Cancel")) {
					ClearRover();
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginVertical(EditorStyles.helpBox);
			GUILayout.Label("Name");
			sRoverName = GUILayout.TextField(sRoverName);
			GUILayout.Label("Description");
			sRoverDescription = GUILayout.TextArea(sRoverDescription);
			GUILayout.Label("Preview Image");
			texRoverPreview = (Texture2D)EditorGUILayout.ObjectField(texRoverPreview, typeof(Texture2D), false);
			if (texRoverPreview != null) {
				GUILayout.Box(texRoverPreview, GUILayout.Width(400), GUILayout.Height(200));
			}
			else {
				GUILayout.Box("Preview Pane", GUILayout.Width(400), GUILayout.Height(200));
			}
			GUILayout.EndVertical();

			GUILayout.Space(10.0f);

			GUILayout.BeginVertical(EditorStyles.helpBox);
			GUILayout.BeginHorizontal();
			eCurSlot = (ROVER_MODULE_SLOT)EditorGUILayout.EnumPopup(eCurSlot);
			if (GUILayout.Button("Add Slot")) {
				builder.AddMod(eCurSlot);
			}
			if (GUILayout.Button("Remove Slot") && !bRemoveSlot) {
				//builder.RemoveMod(eCurSlot);
				if (builder.l_slots.Contains(eCurSlot)) {
					iDeadSlot = builder.l_slots.FindLastIndex(findnum => findnum == eCurSlot);
					bRemoveSlot = true;
				}
			}
			if (GUILayout.Button("Refresh")) {
				RefreshModules();
			}
			GUILayout.EndHorizontal();

			RefreshModules();
			DrawModules();
			GUILayout.EndVertical();
		}
		GUILayout.EndScrollView();
	}

	public void RefreshModules() {
		while (l_dropdowns.Count < builder.l_slots.Count) {
			if (l_dropdowns.Count < builder.l_slots.Count) {
				l_dropdowns.Add(new DropdownList());
				l_dropchoices.Add(0);
			}
		}

		if (bRemoveSlot) {
			l_dropdowns.RemoveAt(iDeadSlot);
			builder.l_slots.RemoveAt(iDeadSlot);
			bRemoveSlot = false;
		}
	}

	public void DrawModules() {
		for (int i = 0; i < builder.l_slots.Count; i++) {
			GUILayout.Label("Slot " + (i + 1).ToString() + " (" + builder.l_slots[i].ToString() + "):");
			List<GUIContent> items = new List<GUIContent>();

			items.Add(new GUIContent("Empty", texNoChoice));
			foreach (RoverModule modf in builder.l_allmods) {
				if (modf.eSlot == builder.l_slots[i]) {
					items.Add(new GUIContent(modf.ToString(), modf.texIcon));
				}
			}

			if (bLoadModulesFromList) {
				//l_loadedmodules.Add("dummy");
				bLoadModulesFromList = false;

				int iload = 0;
				foreach (string mod in l_loadedmodules) {
					if (mod != "dummy") {
						int ifound = builder.l_allmods.FindIndex(findtype => findtype.sName == mod);
						l_dropdowns[iload].selectedItemIndex = ifound;
					}

					iload++;
				}

				Repaint();
			}

			if (items.Count > 0) {
				l_dropchoices[i] = l_dropdowns[i].List(new Rect(GUILayoutUtility.GetLastRect().x,
					GUILayoutUtility.GetLastRect().y + GUILayoutUtility.GetLastRect().height, 200, 20),
					items.ToArray(),
					EditorStyles.toolbarDropDown, EditorStyles.toolbarTextField);
				//Debug.Log(i.ToString() + " was " + l_dropdowns[i].GetSelectedItemIndex().ToString());
			}

			if (l_dropdowns[i].isVisible) {
				GUILayout.Space(20.0f * items.Count);
			}
			GUILayout.Space(25);
		}
	}

	public void SaveSponsorToXML(string path) {
		XmlWriterSettings xsettings = new XmlWriterSettings();
		xsettings.Encoding = System.Text.Encoding.ASCII;
		xsettings.OmitXmlDeclaration = true;

		XmlWriter writer = XmlWriter.Create(path, xsettings);

		writer.WriteStartDocument();
		writer.WriteStartElement("MarsSponsor");
		writer.WriteWhitespace("\n");

		//Name
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("name");
		writer.WriteValue(sSponsorName);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Desc
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("description");
		writer.WriteValue(sSponsorDescription);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Avatar
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("avatar");
		if (texSponsorAvatar != null) {
			writer.WriteValue(texSponsorAvatar.name);
		}
		else {
			writer.WriteValue("null");
		}
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Rover
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("rover");
		writer.WriteValue(sCurRover);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//End doc

		writer.WriteEndElement();
		writer.WriteEndDocument();

		writer.Close();
	}

	public void SaveRoverToXML(string path) {
		XmlWriterSettings xsettings = new XmlWriterSettings();
		xsettings.Encoding = System.Text.Encoding.ASCII;
		xsettings.OmitXmlDeclaration = true;

		XmlWriter writer = XmlWriter.Create(path, xsettings);

		writer.WriteStartDocument();
		writer.WriteStartElement("MarsRover");
		writer.WriteWhitespace("\n");

		//Name
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("name");
		writer.WriteValue(sRoverName);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Desc
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("description");
		writer.WriteValue(sRoverDescription);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Avatar
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("avatar");
		if (texSponsorAvatar != null) {
			writer.WriteValue(texRoverPreview.name);
		}
		else {
			writer.WriteValue("null");
		}
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Modules
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("modules");
		writer.WriteWhitespace("\n");

		if (builder.l_slots != null && builder.l_slots.Count != 0) {
			for (int i = 0; i < builder.l_slots.Count; i++) {
				string slotname = "NONE";
				string itemname = "RoverModule";

				if (builder.l_slots[i] != ROVER_MODULE_SLOT.NONE && l_dropchoices[i] != 0) {
					writer.WriteWhitespace("\t\t");
					writer.WriteStartElement("mod");

					//int ifound = builder.l_allmods.FindIndex(findtype => findtype.sName == mod);
					//l_dropdowns[iload].selectedItemIndex = ifound;

					//RoverModule outmod = builder.l_allmods.Find(findrov => findrov.sName == l_dropdowns[i].lastgui[l_dropdowns[i].GetSelectedItemIndex()].text);
					string match = l_dropdowns[i].lastgui[l_dropdowns[i].GetSelectedItemIndex()].text;
					match = match.ToLower();
					//Debug.Log("match: " + match);

					slotname = "NONE";
					itemname = "RoverModule";

					foreach (RoverModule mod in builder.l_allmods) {
						if (match == mod.sName.Replace(" ", "").ToLower()) {
							slotname = mod.eSlot.ToString();
							itemname = mod.sName;
							break;
						}
					}

					//Debug.Log(slotname + "|" + itemname);
					//Debug.Log("===");

					//slotname = outmod.eSlot.ToString();
					//itemname = outmod.sName;

					writer.WriteAttributeString("slot", slotname);
					writer.WriteValue(itemname);
					writer.WriteEndElement();
					writer.WriteWhitespace("\n");
				}
				else {
					//Debug.Log(i.ToString() + " WAS NONE");
				}
			}
		}

		writer.WriteWhitespace("\t");
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//End doc
		writer.WriteEndElement();
		writer.WriteEndDocument();

		writer.Close();
	}

	public void LoadRoverFromXML(string path, bool bParseInstead = false) {
		builder.Init();

		XDocument s_xmlDoc;
		if (!bParseInstead) {
			s_xmlDoc = XDocument.Load(path);
		}
		else {
			s_xmlDoc = XDocument.Parse(path);
		}

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
				else if (xlayer1.Name == "modules") {
					l_loadedmodules.Clear();
					foreach (XElement xlayer2 in xlayer1.Elements()) {
						ROVER_MODULE_SLOT slot = (ROVER_MODULE_SLOT)System.Enum.Parse(typeof(ROVER_MODULE_SLOT), xlayer2.Attribute("slot").Value.ToString());
						builder.AddMod(slot);
						l_loadedmodules.Add(xlayer2.Value);
						bLoadModulesFromList = true;
					}
				}
			}
		}
	}

	public void LoadSponsorFromXML(string path) {
		XDocument s_xmlDoc = XDocument.Load(path);

		foreach (XElement xroot in s_xmlDoc.Elements()) {
			foreach (XElement xlayer1 in xroot.Elements()) {
				if (xlayer1.Name == "name") {
					sSponsorName = xlayer1.Value;
				}
				else if (xlayer1.Name == "description") {
					sSponsorDescription = xlayer1.Value;
				}
				else if (xlayer1.Name == "avatar") {
					string stemp = xlayer1.Value;
					texSponsorAvatar = Resources.Load<Texture2D>("Sponsors/Images/" + stemp);
				}
				else if (xlayer1.Name == "rover") {
					string stemp = xlayer1.Value;
					sCurRover = stemp;
				}
			}
		}
	}
}
