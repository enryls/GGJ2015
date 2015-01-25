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
			GUI.Label(new Rect(500, 250, 300, 100), "Status: Disconnected");
			if (GUI.Button(new Rect(450, 300, 220, 100), "Client Connect")) {
				Network.Connect(connectionIP, connectionPort);
				Application.LoadLevel("networkScene");
			}
			if (GUI.Button(new Rect(450, 400, 220, 100), "Initialize Server")) {
				Network.InitializeServer(32, connectionPort, false);
				Application.LoadLevel("networkScene");
			}
			if (GUI.Button(new Rect(450, 500, 220, 100), "Reset")) {
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
			GUI.Label(new Rect(500, 10, 300, 100), "Status: Connected as Client");
			if (GUI.Button(new Rect(500, 300, 220, 100), "Disconnect")) {
				Network.Disconnect(200);
			}
		} else if (Network.peerType == NetworkPeerType.Server) {
			GUI.Label(new Rect(500, 10, 300, 100), "Status: Connected as Server");
			if (GUI.Button(new Rect(500, 300, 220, 100), "Disconnect")) {
				Network.Disconnect(200);
			}
		}
	}
}
