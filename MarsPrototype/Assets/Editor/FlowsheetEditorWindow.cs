using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class FlowsheetEditorWindow : EditorWindow {

	private static Texture icon;
	private static Texture arrowhead;
	private static int curid = 1;

	//Scroll area
	private const int MAX_SCROLL_AREA = 10000;
	private const float TOOLBAR_OFFSET = 100.0f;
	private Texture2D texGrid;

	public GameObject goFlowLoc;
	public GameObject goFlowOut;

	public Vector2 vScrollPos;
	public Vector2 vScrollPosSidebar;
	private Vector2 vMousePos;
	private bool bSelectingNode = false;
	private bool bDoAttachment = false;
	private FlowsheetNode nodeToAttach = null;
	private bool bJustLinked = false;
	private bool bResizing = false;
	private Rect scrollview = new Rect(0, 0, MAX_SCROLL_AREA, MAX_SCROLL_AREA);

	//Nodes
	private List<FlowsheetNode> l_nodes;
	private List<FlowsheetNode> l_lastdeadnodes;
	private bool bAnyRecentDead = false;
	private FlowsheetNode selectednode;
	//private FlowsheetNode drawingnode;

	//Window creation
	[MenuItem("Window/FlowSheet Editor %&F")]
	static void CreateWindow() {
		FlowsheetEditorWindow editor = EditorWindow.GetWindow<FlowsheetEditorWindow>();

		editor.Init();

		if (icon == null) {
			icon = Resources.Load<Texture2D>("flowchart-icon");
		}

		if (arrowhead == null) {
			arrowhead = Resources.Load<Texture2D>("arrowhead");
		}

		editor.titleContent = new GUIContent("Flowsheet Editor", icon);

		editor.Show();
	}

	public void Init() {
		vScrollPos = Vector2.zero;
		texGrid = Resources.Load<Texture2D>("flowsheet-backdrop");

		l_nodes = new List<FlowsheetNode>();
		l_lastdeadnodes = new List<FlowsheetNode>();

		bSelectingNode = false;
		bResizing = false;
		selectednode = null;
		nodeToAttach = null;

		goFlowLoc = Resources.Load<GameObject>("Prefabs/FlowLocation");
		goFlowOut = Resources.Load<GameObject>("Prefabs/FlowOutput");
	}

	void Update() {
		if (bSelectingNode) {
			Repaint();
		}

		if (bJustLinked) {
			Repaint();
			bJustLinked = false;
		}
	}

	void OnGUI() {
		FlowsheetEditorWindow editor = this as FlowsheetEditorWindow;

		Event e = Event.current;
		vMousePos = e.mousePosition;

		//Weird mouse error?
		vMousePos = new Vector2(vMousePos.x - 100.0f, vMousePos.y);

		if (e.type == EventType.Layout) {
			if (bDoAttachment && selectednode != null) {
				if (!selectednode.l_outputs.Contains(nodeToAttach)) {
					selectednode.AddOutput(nodeToAttach, ClassificationType.NONE);
					nodeToAttach.AddInput(selectednode, ClassificationType.NONE);
				}
				bSelectingNode = false;
				//Debug.Log("Linked!");
				bJustLinked = true;
				bDoAttachment = false;
			}
			else if (selectednode == null) {
				bDoAttachment = false;
				bSelectingNode = false;
			}
		}

		//Check for window clicks
		if (e.button == 1 && e.type == EventType.MouseDown && !bResizing) {
			bool bfoundany = false;
			foreach (FlowsheetNode node in l_nodes) {
				//We're attaching a link
				if (bSelectingNode && node.position.Contains(vMousePos) && node != selectednode && !bDoAttachment) {
					bDoAttachment = true;
					nodeToAttach = node;
					break;
				}

				if (node.position.Contains(vMousePos) && !bSelectingNode && !bJustLinked && !bDoAttachment) {
					//Debug.Log("Linking!");
					selectednode = node;
					bSelectingNode = true;
					bfoundany = true;
					break;
				}
			}

			if (!bfoundany && !bJustLinked && !bDoAttachment) {
				bSelectingNode = false;
				selectednode = null;
				//Debug.Log("Missed!");
				bJustLinked = true;
			}
		}

		//Drag to resize
		if (e.button == 0 && e.type == EventType.MouseDown && !bResizing) {
			foreach (FlowsheetNode node in l_nodes) {
				Rect corner = new Rect(node.position.x + node.position.width - 20 - vScrollPos.x, node.position.y + node.position.height - 20 - vScrollPos.y, 20, 20);

				if (corner.Contains(vMousePos)) {
					bResizing = true;
					selectednode = node;
					//Debug.Log("RESIZING");
					break;
				}
			}
		}

		if (e.button == 0 && e.type == EventType.MouseUp && bResizing) {
			bResizing = false;
			selectednode = null;
		}

		//Auto GUI
		GUILayout.BeginArea(new Rect(0, 0, TOOLBAR_OFFSET, editor.position.height));
		GUILayout.BeginVertical();
		if (GUILayout.Button("New Node")) {
			FlowsheetNode tempNode = new FlowsheetNode(curid, new Rect(30, 30, 170, 180), "Window " + curid.ToString());
			l_nodes.Add(tempNode);
			curid++;
		}
		if (GUILayout.Button("Delete All")) {
			foreach (FlowsheetNode node in l_nodes) {
				node.bMarkedForDelete = true;
			}
		}
		if (GUILayout.Button("Undo Delete (" + l_lastdeadnodes.Count.ToString() + ")")) {
			if (l_lastdeadnodes != null && l_lastdeadnodes.Count > 0) {
				foreach (FlowsheetNode node in l_lastdeadnodes) {
					l_nodes.Add(node);
				}
				l_lastdeadnodes.Clear();
			}
			else {
				Debug.Log("Nothing to undo!");
			}
		}
		if (GUILayout.Button("Load from scene")) {
			if (EditorUtility.DisplayDialog("Are you sure?", "This will overwrite any current Flowsheet nodes.", "OK", "Cancel")) {
				LoadFromScene();
			}
		}
		if (GUILayout.Button("Save to scene")) {
			if (EditorUtility.DisplayDialog("Are you sure?", "This will overwrite any current Flowsheet GameObjects.", "OK", "Cancel")) {
				SaveToScene();
			}
		}
		GUILayout.EndVertical();

		//Clickable list of nodes
		GUILayout.BeginVertical("box");
		GUILayout.BeginScrollView(vScrollPosSidebar);
		if (l_nodes.Count > 0) {
			foreach (FlowsheetNode node in l_nodes) {
				if (GUILayout.Button(node.sTitle)) {
					vScrollPos = new Vector2(node.position.x, node.position.y);
				}
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();

		GUILayout.EndArea();

		//Regular GUI only
		GUI.DrawTexture(new Rect(TOOLBAR_OFFSET, 0, editor.position.width - TOOLBAR_OFFSET, editor.position.height), texGrid);
		vScrollPos = GUI.BeginScrollView(new Rect(TOOLBAR_OFFSET, 0, editor.position.width - TOOLBAR_OFFSET, editor.position.height), vScrollPos, scrollview, true, true);

		//Curves
		if (bSelectingNode && selectednode != null) {
			DrawCurve(selectednode.position, new Rect(vMousePos.x, vMousePos.y, 1, 1), Color.yellow);
		}
		foreach (FlowsheetNode node in l_nodes) {
			foreach (FlowsheetNode child in node.l_outputs) {
				//float xoffset = 0.0f;
				//float yoffset = 0.0f;
				//				float index = (float)node.l_outputs.IndexOf(child);
				//
				//				xoffset = index * 3;
				//				yoffset = index * 3;

				DrawCurve(node.position, child.position, Color.white);
			}
		}

		//Windows
		BeginWindows();
		foreach (FlowsheetNode node in l_nodes) {
			//drawingnode = node;
			node.bParentResizing = bResizing;
			node.parentSelectedNode = selectednode;
			node.position = GUI.Window(node.id, node.position, node.Draw, node.sTitle);
		}
		EndWindows();
		GUI.EndScrollView();

		//Don't get lost off screen!
		foreach (FlowsheetNode node in l_nodes) {
			if (node.position.xMin < 0 || node.position.yMin < 0 || node.position.xMax > MAX_SCROLL_AREA || node.position.yMax > MAX_SCROLL_AREA) {
				node.position.position = new Vector2(Mathf.Clamp(node.position.x, 0, MAX_SCROLL_AREA - node.position.width), Mathf.Clamp(node.position.y, 0, MAX_SCROLL_AREA - node.position.width));
			}
		}

		//Find dead nodes
		if (!bAnyRecentDead) {
			foreach (FlowsheetNode node in l_nodes) {
				if (node.bMarkedForDelete) {
					bAnyRecentDead = true;
					bSelectingNode = false;
					break;
				}
			}
		}

		if (bAnyRecentDead) {
			l_lastdeadnodes.Clear();

			for (int i = l_nodes.Count - 1; i > -1; i--) {
				if (l_nodes[i].bMarkedForDelete) {
					l_nodes[i].bMarkedForDelete = false;
					l_lastdeadnodes.Add(l_nodes[i]);
					l_nodes.Remove(l_nodes[i]);
				}
			}

			bAnyRecentDead = false;
		}

		//Cancel resize etc. if we lost focus
		if (e.rawType == EventType.MouseUp && GUIUtility.hotControl != editor.GetInstanceID()) {
			GUIUtility.hotControl = 0;
		}

		//		if (l_nodes.Count > 0) {
		//			GUI.DrawTexture(new Rect(l_nodes[0].position.x - 11, l_nodes[0].position.y - 11, 21, 21), arrowhead);
		//			float yoff2 = l_nodes[0].position.y - 11 - ((vScrollPos.y / (float)MAX_SCROLL_AREA) * l_nodes[0].position.height);
		//			GUI.DrawTexture(new Rect(l_nodes[0].position.width - 20, yoff2, 11, 11), arrowhead);
		//		}
	}

	public void DrawCurve(Rect source, Rect dest, Color color, float width = 3.0f, float xoffset = 0.0f, float yoffset = 0.0f) {
		Vector3 source_pos;
		Vector3 end_pos;

		float source_fx;
		float source_fy;
		float end_fx;
		float end_fy;

		//float source_centre_tolerance_x = source.width / 3;
		//float source_centre_tolerance_xmax = source_centre_tolerance_x * 2;

		//float source_centre_tolerance_y = source.height / 3;
		//float source_centre_tolerance_ymax = source_centre_tolerance_y * 2;

		float dest_centre_tolerance_x = source.width / 3;
		float dest_centre_tolerance_xmax = dest_centre_tolerance_x * 2;

		float dest_centre_tolerance_y = source.height / 3;
		float dest_centre_tolerance_ymax = dest_centre_tolerance_y * 2;

		if (source.x + source.width / 2 < dest.x) {
			source_fx = source.x + source.width;
			end_fx = dest.x;
		}
		else if (source.x + source.width / 2 > dest.x + dest_centre_tolerance_x && source.x + source.width / 2 < dest.x + dest_centre_tolerance_xmax) {
			source_fx = source.x + source.width / 2;
			end_fx = dest.x + dest.width / 2;
		}
		else {
			source_fx = source.x;
			end_fx = dest.x + dest.width;
		}

		if (source.y + source.height / 2 < dest.y) {
			source_fy = source.y + source.height;
			end_fy = dest.y;
		}
		else if (source.y + source.height / 2 > dest.y + dest_centre_tolerance_y && source.y + source.height / 2 < dest.y + dest_centre_tolerance_ymax) {
			source_fy = source.y + source.height / 2;
			end_fy = dest.y + dest.height / 2;
		}
		else {
			source_fy = source.y;
			end_fy = dest.y + dest.height;
		}

		source_pos = new Vector3(source_fx, source_fy, 0);
		end_pos = new Vector3(end_fx, end_fy, 0);

		float dist = Vector3.Distance(source_pos, end_pos);

		Vector3 start_tang = source_pos + Vector3.right * (dist / 2f) ;
		Vector3 end_tang = end_pos + Vector3.left * (dist / 2f);

		Rect arrowpoint = new Rect(end_pos.x - 11, end_pos.y - 11, 21, 21);

		Handles.BeginGUI();
		Handles.DrawBezier(source_pos, end_pos, start_tang, end_tang, color, null, width);
		GUI.DrawTexture(arrowpoint, arrowhead);
		Handles.EndGUI();
	}

	public void LoadFromScene() {
//		l_nodes.Clear();
//		l_lastdeadnodes.Clear();
//		curid = 0;
//
//		GameObject flowmaster = GameObject.FindGameObjectWithTag("FlowsheetMaster");
//
//		if (flowmaster == null) {
//			Debug.LogError("No FlowsheetMaster was found!");
//			return;
//		}
//
//		Dictionary<FlowsheetObject, List<FlowsheetObject>> d_flowlinks_out = new Dictionary<FlowsheetObject, List<FlowsheetObject>>();
//
//		List<FlowsheetObject> l_flowobj = new List<FlowsheetObject>();
//		l_flowobj.AddRange(flowmaster.GetComponentsInChildren<FlowsheetObject>());
//
//		//l_flowobj.Reverse();
//
//		if (l_flowobj.Count == 0) {
//			Debug.LogError("Nothing was attached to the FlowsheetMaster!");
//			return;
//		}
//
//		//Generate outputs
//		foreach (FlowsheetObject obj in l_flowobj) {
//			d_flowlinks_out.Add(obj, new List<FlowsheetObject>());
//			if (obj.GetComponentInChildren<MineralProcessingLink>() != null) {
//				//Store all the outputs
//				foreach (MineralProcessingLink link in obj.GetComponentsInChildren<MineralProcessingLink>()) {
//					d_flowlinks_out[obj].Add(link.flowObjDest);
//				}
//			}
//		}
//
//		Dictionary<FlowsheetObject, FlowsheetNode> d_flowbridge = new Dictionary<FlowsheetObject, FlowsheetNode>();
//
//		//What node goes with what object?
//		foreach (FlowsheetObject obj in l_flowobj) {
//			FlowsheetNode tempnode = new FlowsheetNode(curid, new Rect(30 + 200 * curid, 30 + 90 * curid, 170, 180), obj.name);
//			l_nodes.Add(tempnode);
//			d_flowbridge.Add(obj, tempnode);
//
//			curid++;
//		}
//
//		//Tie them all together
//		foreach (KeyValuePair<FlowsheetObject, List<FlowsheetObject>> outpair in d_flowlinks_out) {
//			foreach (FlowsheetObject outpair_link in outpair.Value) {
//				d_flowbridge[outpair.Key].AddOutput(d_flowbridge[outpair_link], ClassificationType.NONE);
//				d_flowbridge[outpair_link].AddInput(d_flowbridge[outpair.Key], ClassificationType.NONE);
//
//				if (d_flowbridge[outpair.Key].eLocation == FlowLocation.NONE) {
//					d_flowbridge[outpair.Key].eLocation = outpair.Key.outputLinks[0].flowSrc;
//				}
//			}
//		}
//
//		//Find "terminators" - ones with no outputs. They won't have a Location set.
//		foreach (FlowsheetObject obj in l_flowobj) {
//			if (d_flowbridge[obj].eLocation == FlowLocation.NONE) {
//				d_flowbridge[obj].eLocation = obj.inputLinks[0].flowDst;
//			}
//		}
	}

	public void SaveToScene() {
		//l_lastdeadnodes.Clear();

//		GameObject flowmaster = GameObject.FindGameObjectWithTag("FlowsheetMaster");
//
//		if (flowmaster == null) {
//			Debug.LogError("No FlowsheetMaster was found!");
//			return;
//		}
//
//		if (l_nodes.Count == 0) {
//			Debug.LogError("There's nothing to write!");
//			return;
//		}
//
//		//Clean up old one
//		for (int i = flowmaster.transform.childCount - 1; i > -1; i--) {
//			GameObject.DestroyImmediate(flowmaster.transform.GetChild(i).gameObject);
//		}
//
//		List<GameObject> l_gonodes = new List<GameObject>();
//		Dictionary<FlowsheetNode, FlowsheetObject> d_flowbridge = new Dictionary<FlowsheetNode, FlowsheetObject>();
//
//		//Make FlowsheetObjects
//		foreach(FlowsheetNode node in l_nodes) {
//			GameObject gotemp = GameObject.Instantiate(goFlowLoc);
//
//			gotemp.name = node.sTitle;
//			l_gonodes.Add(gotemp);
//
//			d_flowbridge.Add(node, gotemp.GetComponent<FlowsheetObject>());
//		}
//
//		l_gonodes.Reverse();
//
//		foreach (GameObject go in l_gonodes) {
//			go.transform.SetParent(flowmaster.transform);
//		}
//
//		//Make MineralProcessingLinks
//		foreach (KeyValuePair<FlowsheetNode, FlowsheetObject> pair in d_flowbridge) {
//			foreach (FlowsheetNode outnode in pair.Key.l_outputs) {
//				GameObject mplTemp = GameObject.Instantiate(goFlowOut);
//				mplTemp.transform.parent = pair.Value.gameObject.transform;
//
//				MineralProcessingLink MPL = mplTemp.GetComponent<MineralProcessingLink>();
//				MPL.flowObjDest = d_flowbridge[outnode];
//				MPL.flowObjSrc = d_flowbridge[pair.Key];
//				MPL.flowSrc = pair.Key.eLocation;
//				MPL.flowDst = outnode.eLocation;
//				MPL.linkType = ProcessingLinkType.OUTPUT;
//
//				pair.Value.outputLinks.Add(MPL);
//				MPL.flowObjDest.inputLinks.Add(MPL);
//			}
//		}
	}
}

