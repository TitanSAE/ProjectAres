using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class FlowsheetNode {

	public int id {private set; get;}
	public string sTitle {set; get;}
	//public string sDescription;
	public Rect position;
	public bool bMarkedForDelete = false;

	//GUI Elements
	private Vector2 vScrollPos;
	public bool bShowEditPane = true;
	public bool bShowInputs = true;
	public bool bShowOutputs = true;
	public bool bShowSizeBoxes = false;
	public bool bParentResizing = false;

	public DIALOG_LOCATION eLocation = DIALOG_LOCATION.NONE;

	private Texture2D stretchicon;

	public FlowsheetNode parentSelectedNode = null;
	//public Vector2 vParentScrollPos;

	public List<FlowsheetNode> l_inputs {private set; get;}
	public List<FlowsheetNode> l_outputs {private set; get;}

	public Dictionary<FlowsheetNode, FLOWSHEET_LINK_TYPE> d_linktypes_in {private set; get;}
	public Dictionary<FlowsheetNode, FLOWSHEET_LINK_TYPE> d_linktypes_out {private set; get;}

	public Texture2D texTalkAvatar = null;
	public string sSpeakerName= "";
	public string sDialog = "";

	public FlowsheetNode() {
		l_inputs = new List<FlowsheetNode>();
		l_outputs = new List<FlowsheetNode>();

		d_linktypes_in = new Dictionary<FlowsheetNode, FLOWSHEET_LINK_TYPE>();
		d_linktypes_out = new Dictionary<FlowsheetNode, FLOWSHEET_LINK_TYPE>();

		stretchicon = Resources.Load<Texture2D>("resize");
	}

	public FlowsheetNode(int winid, Rect winrect, string title) {
		id = winid;
		position = winrect;
		sTitle = title;

		l_inputs = new List<FlowsheetNode>();
		l_outputs = new List<FlowsheetNode>();

		d_linktypes_in = new Dictionary<FlowsheetNode, FLOWSHEET_LINK_TYPE>();
		d_linktypes_out = new Dictionary<FlowsheetNode, FLOWSHEET_LINK_TYPE>();

		stretchicon = Resources.Load<Texture2D>("resize");
	}

	public void AddInput(FlowsheetNode node, FLOWSHEET_LINK_TYPE type) {
		if (!l_inputs.Contains(node)) {
			d_linktypes_in.Add(node, FLOWSHEET_LINK_TYPE.NONE);
			l_inputs.Add(node);
		}
	}

	public void RemoveInput(FlowsheetNode node) {
		if (l_inputs.Contains(node)) {
			d_linktypes_in.Remove(node);
			l_inputs.Remove(node);
		}
	}

	public void AddOutput(FlowsheetNode node, FLOWSHEET_LINK_TYPE type) {
		if (!l_outputs.Contains(node)) {
			d_linktypes_out.Add(node, FLOWSHEET_LINK_TYPE.NONE);
			l_outputs.Add(node);
		}
	}

	public void RemoveOutput(FlowsheetNode node) {
		if (l_outputs.Contains(node)) {
			d_linktypes_out.Remove(node);
			l_outputs.Remove(node);
		}
	}

	public void SetInputClassification(FlowsheetNode node, FLOWSHEET_LINK_TYPE type) {
		if (l_inputs.Contains(node)) {
			d_linktypes_in[node] = type;
		}
	}

	public void SetOutputClassification(FlowsheetNode node, FLOWSHEET_LINK_TYPE type) {
		if (l_outputs.Contains(node)) {
			d_linktypes_out[node] = type;
		}
	}

//	public void DetectEvents(ref bool bResize, ref FlowsheetNode selnode, Vector2 mousepos) {
//		Rect corner = new Rect(position.width - 20, position.height - 20, 20, 20);
//
//		if (mousepos.x > corner.x && mousepos.x < corner.x + 20) {
//			if (mousepos.y > corner.y && mousepos.y < corner.y + 20) {
//				bResize = true;
//				selnode = this;
//				Debug.Log("FOIUDN IT");
//			}
//		}
//	}

	public void Draw(int winid) {
		//Begin auto layout
		vScrollPos = GUILayout.BeginScrollView(vScrollPos);

		GUILayout.BeginVertical();
		bShowEditPane = EditorGUILayout.Foldout(bShowEditPane, "Editable data");
		if (bShowEditPane) {
			EditorGUIUtility.labelWidth = 35.0f;
			sTitle = EditorGUILayout.TextField("Name", sTitle);
			//sDescription = EditorGUILayout.TextField("Desc", sDescription);
			//GUILayout.Space(4.0f);

			bShowSizeBoxes = EditorGUILayout.Foldout(bShowSizeBoxes, "Size: " + position.width.ToString() + "x" + position.height.ToString());
			if (bShowSizeBoxes) {
				GUILayout.BeginHorizontal();
				EditorGUIUtility.labelWidth = 15.0f;
				position.width = EditorGUILayout.FloatField("W", position.width);
				position.height = EditorGUILayout.FloatField("H", position.height);
				EditorGUIUtility.labelWidth = 0;
				GUILayout.EndHorizontal();
			}
			EditorGUIUtility.labelWidth = 75.0f;
			eLocation = (DIALOG_LOCATION)EditorGUILayout.EnumPopup("Screen Loc:", eLocation);
			EditorGUIUtility.labelWidth = 0;

			GUILayout.Label("Avatar:");
			texTalkAvatar = (Texture2D)EditorGUILayout.ObjectField((Object)texTalkAvatar, typeof(Texture2D), false);

			sSpeakerName = EditorGUILayout.TextField("Speaker", sSpeakerName);

			GUILayout.Label("Dialog:");
			sDialog = EditorGUILayout.TextArea(sDialog, GUILayout.ExpandHeight(true));

			GUILayout.Label("Links", EditorStyles.boldLabel);
			bShowInputs = EditorGUILayout.Foldout(bShowInputs, "Inputs");
			if (bShowInputs && l_inputs.Count > 0) {
				for (int i = l_inputs.Count - 1; i > -1; i--) {
					GUILayout.BeginHorizontal();

					GUILayout.Label(l_inputs[i].sTitle);

					//d_linktypes_in[l_inputs[i]] = (ClassificationType)EditorGUILayout.EnumPopup(d_linktypes_in[l_inputs[i]]);
					GUILayout.Label(l_inputs[i].d_linktypes_out[this].ToString());

					if (GUILayout.Button("Sever")) {
						FlowsheetNode tempnode = l_inputs[i];
						l_inputs[i].RemoveOutput(this);
						RemoveInput(tempnode);
					}
					GUILayout.EndHorizontal();
				}
			}

			bShowOutputs = EditorGUILayout.Foldout(bShowOutputs, "Outputs");
			if (bShowOutputs) {
				for (int i = l_outputs.Count - 1; i > -1; i--) {
					GUILayout.BeginHorizontal();

					GUILayout.Label(l_outputs[i].sTitle);

					d_linktypes_out[l_outputs[i]] = (FLOWSHEET_LINK_TYPE)EditorGUILayout.EnumPopup(d_linktypes_out[l_outputs[i]]);

					if (GUILayout.Button("Sever")) {
						FlowsheetNode tempnode = l_outputs[i];
						l_outputs[i].RemoveInput(this);
						RemoveOutput(tempnode);
					}
					GUILayout.EndHorizontal();
				}
			}
		}
		else {
			GUILayout.Label("Inputs: " + l_outputs.Count.ToString());
			GUILayout.Label("Outputs: " + l_outputs.Count.ToString());
		}
		GUILayout.EndVertical();

		GUILayout.EndScrollView();
		//End auto layout

		//Fixed layout
		if (GUI.Button(new Rect(position.width - 15, 0, 15, 15), "X")) {
			bMarkedForDelete = true;

			for (int i = l_outputs.Count - 1; i > -1; i--) {
				FlowsheetNode tempnode = l_outputs[i];
				l_outputs[i].RemoveInput(this);
				RemoveOutput(tempnode);
			}

			for (int i = l_inputs.Count - 1; i > -1; i--) {
				FlowsheetNode tempnode = l_inputs[i];
				l_inputs[i].RemoveOutput(this);
				RemoveInput(tempnode);
			}
		}

		if (bParentResizing && parentSelectedNode == this && Event.current.type == EventType.MouseDrag) {
			if ((position.width + Event.current.delta.x) > 80) { 
				position.xMax += Event.current.delta.x; 
			}

			if ((position.height + Event.current.delta.y) > 80) { 
				position.yMax += Event.current.delta.y; 
			}

			Event.current.Use();
		}

		//Resize
		Rect corner = new Rect(position.width - 20, position.height - 20, 20, 20);
		GUI.DrawTexture(corner, stretchicon);

		//Drag
		if (!bParentResizing) {
			GUI.DragWindow();
		}
		//End fixed layout

		//Don't let it become too small
		position.height = Mathf.Clamp(position.height, 80, 1500);
		position.width = Mathf.Clamp(position.width, 80, 1500);
	}
}
