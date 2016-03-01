using UnityEngine;
using System.Collections;

public class RandomQuadTerrain : MonoBehaviour {

	private MeshRenderer rend;
	private MeshCollider collide;
	private MeshFilter filter;

	private MeshBuilder construct;

	public int m_SegmentCount = 10;
	public int m_Width = 10;
	public int m_Length = 10;
	public int m_MaxHeight = 15;
	public int m_MinHeight = -30;
	public int m_StepCount = 10;

	public float mountain_chance = 25.00f;
	public float plain_chance = 1.00f;

	// Use this for initialization
	void Start () {
		rend = this.GetComponent<MeshRenderer>();
		collide = this.GetComponent<MeshCollider>();
		filter = this.GetComponent<MeshFilter>();
		construct = new MeshBuilder();

		Generate();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.M)) {
			construct = new MeshBuilder();
			Generate();
		}
	}

	public void BuildQuad(MeshBuilder meshBuilder, Vector3 offset) {
		meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, 0.0f) + offset);
		meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
		meshBuilder.Normals.Add(Vector3.up);

		meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, m_Length) + offset);
		meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
		meshBuilder.Normals.Add(Vector3.up);

		meshBuilder.Vertices.Add(new Vector3(m_Width, 0.0f, m_Length) + offset);
		meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
		meshBuilder.Normals.Add(Vector3.up);

		meshBuilder.Vertices.Add(new Vector3(m_Width, 0.0f, 0.0f) + offset);
		meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
		meshBuilder.Normals.Add(Vector3.up);

		int baseIndex = meshBuilder.Vertices.Count - 4;

		meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
		meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
	}

	void BuildQuadForGrid(MeshBuilder meshBuilder, Vector3 position, Vector2 uv, bool buildTriangles, int vertsPerRow) {
		meshBuilder.Vertices.Add(position);
		meshBuilder.UVs.Add(uv);

		if (buildTriangles) {
			int baseIndex = meshBuilder.Vertices.Count - 1;

			int index0 = baseIndex;
			int index1 = baseIndex - 1;
			int index2 = baseIndex - vertsPerRow;
			int index3 = baseIndex - vertsPerRow - 1;

			meshBuilder.AddTriangle(index0, index2, index1);
			meshBuilder.AddTriangle(index2, index3, index1);
		}
	}

	public void Generate() {
		int iKeepHigh = 0;
		int iKeepFlat = 0;

		for (int i = 0; i <= m_SegmentCount; i++) {
			float z = m_Length * i;
			float v = (1.0f / m_SegmentCount) * i;

			for (int j = 0; j <= m_SegmentCount; j++) {
				float x = m_Width * j;
				float u = (1.0f / m_SegmentCount) * j;

				if (iKeepHigh > 0) {
					iKeepHigh--;
				}

				if (iKeepFlat > 0) {
					iKeepFlat--;
				}

				float finalheight = Random.Range(m_MinHeight, m_MaxHeight);
				float ismount = Random.Range(0.0f, 100.0f);
				float isplain = Random.Range(0.0f, 100.0f);

				//Mountains
				if (ismount < mountain_chance) {
					finalheight *= Random.Range(1, 6);
					finalheight = Mathf.Clamp(finalheight, 0, Mathf.Infinity);

					iKeepHigh += Random.Range(4, 16);
				}

				if (iKeepHigh > 0) {
					finalheight *= Random.Range(1, 3);
					finalheight = Mathf.Clamp(finalheight, 0, Mathf.Infinity);
				}

				//Plains
				if (isplain < plain_chance && iKeepHigh == 0) {
					finalheight = 0;
					finalheight += Random.Range(-1.0f, 1.0f);

					iKeepFlat += Random.Range(4, 16);
				}

				if (iKeepFlat > 0) {
					finalheight = 0;
					finalheight += Random.Range(-1.0f, 1.0f);
				}

				Vector3 offset = new Vector3(x, finalheight, z);

				Vector2 uv = new Vector2(u, v);
				bool buildTriangles = i > 0 && j > 0;

				BuildQuadForGrid(construct, offset, uv, buildTriangles, m_StepCount + 1);
			}
		}

		filter.mesh = construct.CreateMesh();
		filter.mesh.RecalculateNormals();
		collide.sharedMesh = filter.mesh;
	}
}
