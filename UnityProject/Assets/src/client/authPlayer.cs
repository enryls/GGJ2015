using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class authPlayer : MonoBehaviour {

	//That's actually not the owner but the player,
	//the server instantiated the prefab for, where this script is attached
	public NetworkPlayer theOwner;
	public colors playerColor = colors.none;

	//Those are stored to only send RPCs to the server when the 
	//data actually changed.
	float lastClientHInput = 0f;
	float lastClientVInput = 0f;
	Vector3 lastMousePos = Vector3.zero;


	void Awake() {
		if (Network.isServer)
			return;

		//Disable this by default for now
		//Just to make sure no one can use this until we didn't
		//find the right player. (see setOwner())
		enabled = false;
	}

	[RPC]
	void setOwner(NetworkPlayer player) {
		theOwner = player;
		if (player == Network.player) {
			//So it just so happens that WE are the player in question,
			//which means we can enable this control again
			enabled = true;
		} else {
			//Disable a bunch of other things here that are not interesting:
			/*if (GetComponent<Camera>()) {
				GetComponent<Camera>().enabled = false;
			}

			if (GetComponent<AudioListener>()) {
				GetComponent<AudioListener>().enabled = false;
			}

			if (GetComponent<GUILayer>()) {
				GetComponent<GUILayer>().enabled = false;
			}*/
		}
	}

	[RPC]
	public NetworkPlayer getOwner() {
		return theOwner;
	}

	void Update() {
		if (Network.isServer) {
			return; //get lost, this is the client side!
		}

		//Check if this update applies for the current client
		/*if ((theOwner != null) && Network.player == theOwner) {
			float HInput = Input.GetAxis("Horizontal");
			float VInput = Input.GetAxis("Vertical");
			if (lastClientHInput != HInput || lastClientVInput != VInput) {
				//Store latest data
				lastClientHInput = HInput;
				lastClientVInput = VInput;
				//Send update
				networkView.RPC("updateClientMotion", RPCMode.Server, HInput, VInput);
			}
		}*/

		if ((theOwner == null) || Network.player != theOwner)
			return;
		
		if (Input.GetMouseButtonUp(0)) {
			networkView.RPC("stopDrawing", RPCMode.Server);
		}

		Vector3 mousePos = Input.mousePosition;
		mousePos.z = Camera.main.nearClipPlane; // utils.globalZ;//

		if (Input.GetMouseButtonDown(0)) {
			Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mousePos);

			networkView.RPC("startDrawing", RPCMode.Server, mouseWorld);
		} else if (Input.GetMouseButton(0)) {
			//LeftClick
			if (mousePos != lastMousePos) {
				lastMousePos = mousePos;
				Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mousePos);

				networkView.RPC("lineDrawing", RPCMode.Server, mouseWorld);
			}
		}

		foreach (NetworkViewID instance in lol.Keys) {

			if (lol[instance].pointsList.Count > 0) {
				//Get the line from the line array
				Transform t = lineDict[instance];
				LineRenderer currentRenderer = t.GetComponent<LineRenderer>();

				if (!linePointsDict.ContainsKey(instance)) {
					linePointsDict.Add(instance, new List<Vector3>());
				}

				//Debug.Log("ASD " + instance + " " + lol[instance].pointsColor);

				int lastLineSize = linePointsDict[instance].Count;
				linePointsDict[instance].AddRange(lol[instance].pointsList);
				lol[instance].pointsList.Clear();

				Color c = utils.getColor(lol[instance].pointsColor);
				currentRenderer.SetColors(c, c);
				currentRenderer.SetVertexCount(linePointsDict[instance].Count);

				for (int i = lastLineSize; i < linePointsDict[instance].Count; i++) {
					currentRenderer.SetPosition(i, linePointsDict[instance][i]);
				}
			}
		}
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		Debug.Log(info.ToString());
		if (stream.isWriting) {
			Vector3 pos = transform.position;
			stream.Serialize(ref pos);
		} else {
			Vector3 posReceive = Vector3.zero;
			stream.Serialize(ref posReceive);
			transform.position = posReceive;
		}
	}

	Dictionary<NetworkViewID, List<Vector3>> linePointsDict = new Dictionary<NetworkViewID, List<Vector3>>();
	public static Dictionary<NetworkViewID, updateLineElement> lol = new Dictionary<NetworkViewID, updateLineElement>();

	public static Dictionary<NetworkViewID, Transform> lineDict = new Dictionary<NetworkViewID, Transform>();
}
