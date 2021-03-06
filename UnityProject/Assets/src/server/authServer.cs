﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class authServer : MonoBehaviour {

	float serverCurrentHInput = 0f;
	float serverCurrentVInput = 0f;
	List<Vector3> lineStored = new List<Vector3>();
	//bool hasSpawned = false;
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

		
		/*if (!hasSpawned && sv_spawn.canSpawn) {
			spawnMinion();
			hasSpawned = true;
		}*/

		if (linePoints == null)
			return;

		if (sv_spawn.canSpawn && coolDownSpawn == false) {
			spawnMinion();
			StartCoroutine(onCOOL());
		}

		if (lineStored.Count > 0) {

			lineScript sc = currentLine.GetComponent<lineScript>();
			if (!sc) {
				Debug.LogError("The prefab has no lineScript attached!");
			}

			colors tempColor = sc.lineColor;
			//if (Random.Range(0.0f, 1.0f) > 0.2f) {
			//	tempColor = colors.green;
			//	Debug.Log("GREEN");
			//}
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
		lineScript ls = currentLine.GetComponent<lineScript>();
		ls.SetPosition(linePoints);


		utils.addCollider(currentLine.gameObject,linePoints);

		NetworkView netView = currentLine.GetComponent<NetworkView>();
		netView.RPC("addCollider", RPCMode.AllBuffered, currentLine.networkView.viewID, linePoints.ToArray());

	}


	public Transform linePrefab;
	List<Vector3> linePoints;
	public Transform minionPrefab;
	public Transform bluePrefab;
	public Transform yellowPrefab;
	public Transform redPrefab;
	//int lastLineSize;

	[RPC]
	void lineDrawing(Vector3 mouseWorld) {
		//Debug.Log(mouseWorld.ToString());
		lineStored.Add(mouseWorld);
	}

	public void spawnMinion() {
		GameObject[] spawns = GameObject.FindGameObjectsWithTag("Respawn");
		if (spawns.Length <=  0) {
			return;
		}
		Vector3 pos = spawns[0].renderer.bounds.center;

		pos.y += spawns[0].renderer.bounds.size.y / 2;
		pos.x += spawns[0].renderer.bounds.size.x / 2;

		Transform prefabToInst = minionPrefab;

		colors choosenSpawn = (colors)Random.Range(1, 4);
		switch (choosenSpawn) {
			case colors.red:
				prefabToInst = redPrefab;
				break;
			case colors.blue:
				prefabToInst = bluePrefab;
				break;
			case colors.yellow:
				prefabToInst = yellowPrefab;
				break;
			default: break;
		}

		//prefabToInst.GetComponent<Minion>().setColor((colors)choosenSpawn);
		Transform currentMinion = (Transform)Network.Instantiate(
											prefabToInst,
											pos,
											Quaternion.identity,
											2);//temp

		currentMinion.GetComponent<Minion>().setColor((colors)choosenSpawn);
		
	}

	private bool coolDownSpawn = false;
	IEnumerator onCOOL() {
		coolDownSpawn = true;
		yield return new WaitForSeconds(5.0f);
		coolDownSpawn = false;
	}
}
