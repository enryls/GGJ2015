using System.Collections.Generic;
using UnityEngine;

public class authServer : MonoBehaviour {

	float serverCurrentHInput = 0f;
	float serverCurrentVInput = 0f;
	List<Vector3> lineStored = new List<Vector3>();

	//LineRenderer ren;

	void Start() {
		//linePrefab.gameObject.AddComponent<LineRenderer>();

		LineRenderer ren = linePrefab.gameObject.GetComponent<LineRenderer>();
		ren.material = new Material(Shader.Find("Particles/Additive"));
		//r.SetColors(c1, c2);
		ren.SetWidth(0.01f, 0.01f);

	}

	void Update() {
		if (Network.isClient)
			return;

		if (linePoints == null)
			return;

		if (lineStored.Count > 0) {
			NetworkView netView = currentLine.GetComponent<NetworkView>();
			netView.RPC("updateLine", RPCMode.AllBuffered, currentLine.networkView.viewID, lineStored.ToArray());

			int lastLineSize = linePoints.Count;
			linePoints.AddRange(lineStored);
			lineStored.Clear();
			currentRenderer.SetVertexCount(linePoints.Count);

			for (int i = lastLineSize; i < linePoints.Count; i++) {
				currentRenderer.SetPosition(i, linePoints[i]);
			}
		}
	}

	/**
	* The client calls this to notify the server about new motion data
	* @param	motion
	*/
	[RPC]
	void updateClientMotion(float HInput, float VInput) {
		serverCurrentHInput = HInput;
		serverCurrentVInput = VInput;
	}

	Transform currentLine;
	bool isDrawing;
	LineRenderer currentRenderer;

	[RPC]
	void startDrawing(Vector3 mouseWorld) {
		//isDrawing = true;
		linePoints = new List<Vector3>();
		currentLine = (Transform)Network.Instantiate(
												linePrefab,
												mouseWorld,
												Quaternion.identity,
												2);//temp
		//Debug.Log("Down Mouse");
		currentRenderer = currentLine.gameObject.GetComponent<LineRenderer>();
		currentRenderer.SetVertexCount(0);
	}

	[RPC]
	void stopDrawing() {
		//isDrawing = false;
		//Debug.Log("Up Mouse");
	}


	[SerializeField] Transform linePrefab;
	List<Vector3> linePoints;
	//int lastLineSize;

	[RPC]
	void lineDrawing(Vector3 mouseWorld) {
		//Debug.Log(mouseWorld.ToString());
		lineStored.Add(mouseWorld);
	}
}
