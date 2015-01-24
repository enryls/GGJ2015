using UnityEngine;
using System.Collections.Generic;

public class render : MonoBehaviour {

	List<Vector3> linePoints = new List<Vector3>();
	LineRenderer lineRenderer;
	public float startWidth = 0.1f;
	public float endWidth = 0.1f;
	public float threshold = 0.001f;
	Camera thisCamera;
	int lineCount = 0;
	bool newLine = false;
	List<LineRenderer> lines = new List<LineRenderer>();
	GameObject ob;

	Vector3 lastPos;


	void Awake() {
		thisCamera = Camera.main;
		//lineRenderer = GetComponent<LineRenderer>();
	}

	void Update() {
		if (Input.GetMouseButton(0)) {

			if (!newLine) {

				ob = new GameObject();

				Vector3 mouse = Input.mousePosition;
				mouse.z = utils.globalZ;
				Vector3 mouseW = thisCamera.ScreenToWorldPoint(mouse);

				ob.transform.position = mouse;  // posiziono l' oggetto nel punto in cui clicco

				BoxCollider2D col = ob.AddComponent<BoxCollider2D>();
				col.size = new Vector2(0.5f, 0.5f);
				col.center = ob.transform.position;

				ob.AddComponent<LineRenderer>();
				lineRenderer = ob.GetComponent<LineRenderer>();
				lastPos = Vector3.one * float.MaxValue;
				newLine = true;
			}
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = utils.globalZ;
			Vector3 mouseWorld = thisCamera.ScreenToWorldPoint(mousePos);

			float dist = Vector3.Distance(lastPos, mouseWorld);

			if (dist <= threshold)
				return;

			lastPos = mouseWorld;
			if (linePoints == null)
				linePoints = new List<Vector3>();
			linePoints.Add(mouseWorld);

			UpdateLine();
		} else {
			newLine = false;
			linePoints = null;
			lineCount = 0;

		}

	}


	void UpdateLine() {
		lineRenderer.SetWidth(startWidth, endWidth);
		lineRenderer.SetVertexCount(linePoints.Count);

		for (int i = lineCount; i < linePoints.Count; i++) {
			lineRenderer.SetPosition(i, linePoints[i]);
		}
		lineCount = linePoints.Count;
	}
}
