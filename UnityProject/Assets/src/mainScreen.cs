using UnityEngine;
using System.Collections;

public class mainScreen : MonoBehaviour {

	public string connectionIP = "192.168.43.43";
	public int connectionPort = 25001;

	// Use this for initialization
	void Start() {
		Application.runInBackground = true ;
	}

	void OnGUI() {
		if (Network.peerType == NetworkPeerType.Disconnected) {
			GUI.Label(new Rect(10, 10, 300, 20), "Status: Disconnected");
			if (GUI.Button(new Rect(10, 30, 120, 20), "Client Connect")) {
				Network.Connect(connectionIP, connectionPort);
			}
			if (GUI.Button(new Rect(10, 50, 120, 20), "Initialize Server")) {
				Network.InitializeServer(32, connectionPort, false);
			}
			if (GUI.Button(new Rect(10, 70, 120, 20), "Reset")) {
				var objects = GameObject.FindObjectsOfType<GameObject>();
				foreach (GameObject o in objects) {
					if (o.name == "floor")
						continue;
					if (o.GetComponent<lineScript>())
						Destroy(o.gameObject);
					if (o.GetComponent<Minion>())
						Destroy(o.gameObject);
					if (o.GetComponent<BoxCollider2D>())
						Destroy(o.gameObject);

					//Continue resetting things
					sv_spawn svs = o.GetComponent<sv_spawn>();
					if (svs) {
						svs.usedColors = 0;
						svs.playerTracker.Clear();
						svs.scheduledSpawns.Clear();
					}
					authPlayer aut = o.GetComponent<authPlayer>();
					if (aut) {
						aut.linePointsDict.Clear();
					}
					authPlayer.lol.Clear();
					authPlayer.lineDict.Clear();
				}
			}
		} else if (Network.peerType == NetworkPeerType.Client) {
			GUI.Label(new Rect(10, 10, 300, 20), "Status: Connected as Client");
			if (GUI.Button(new Rect(10, 30, 120, 20), "Disconnect")) {
				Network.Disconnect(200);
			}
		} else if (Network.peerType == NetworkPeerType.Server) {
			GUI.Label(new Rect(10, 10, 300, 20), "Status: Connected as Server");
			if (GUI.Button(new Rect(10, 30, 120, 20), "Disconnect")) {
				Network.Disconnect(200);
			}
		}
	}
}
