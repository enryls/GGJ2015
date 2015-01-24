﻿using System.Collections.Generic;
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

			lineScript sc = currentLine.GetComponent<lineScript>();
			if (!sc) {
				Debug.LogError("The prefab has no lineScript attached!");
			}

			colors tempColor = sc.lineColor;
			if (Random.Range(0.0f, 1.0f) > 0.2f) {
				tempColor = colors.green;
				Debug.Log("GREEN");
			}
			//TODO Color mixing!

			NetworkView netView = currentLine.GetComponent<NetworkView>();
			netView.RPC("updateLine", RPCMode.AllBuffered, currentLine.networkView.viewID, lineStored.ToArray(), (int)tempColor);

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

		lineScript sc = currentLine.GetComponent<lineScript>();
		if (!sc) {
			Debug.LogError("The prefab has no lineScript attached!");
		}

		//Reset the renderer size
		currentRenderer = currentLine.gameObject.GetComponent<LineRenderer>();
		currentRenderer.SetVertexCount(0);


		//Set the line color!
		authPlayer ap = networkView.GetComponent<authPlayer>();
		sc.lineColor = ap.playerColor;

		Color c = utils.getColor(sc.lineColor);
		currentRenderer.SetColors(c,c);
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
