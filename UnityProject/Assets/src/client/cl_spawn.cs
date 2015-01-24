using UnityEngine;
using System.Collections;
using System;

public class cl_spawn : MonoBehaviour {
	void OnConnectedToServer() {
		Debug.Log("Disabling message queue!");
		Network.isMessageQueueRunning = false;
		Application.LoadLevel(0);
	}

	void OnLevelWasLoaded(int level) {
		if (/*level != 0 && */Network.isClient) { //0 is my menu scene so ignore that.
			Network.isMessageQueueRunning = true;
			Debug.Log("Level was loaded, requesting spawn");
			Debug.Log("Re-enabling message queue!");
			//Request a player instance form the server
			networkView.RPC("requestSpawn", RPCMode.Server, Network.player);
		}
	}
}
