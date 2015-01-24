using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class spawning : MonoBehaviour {

	public Transform cubePrefab;

	private List<authPlayer> playerTracker = new List<authPlayer>();
	//private List<NetworkPlayer> scheduledSpawns = new List<NetworkPlayer>();


	void OnServerInitialized() {
		SpawnPlayer();
	}
	void OnConnectedToServer() {
		SpawnPlayer();
	}

	void SpawnPlayer() {
		GameObject sc = (GameObject)Network.Instantiate(
			cubePrefab, transform.position, transform.rotation, 0);

		/*if (!sc) {
			Debug.LogError("The prefab has no C_PlayerManager attached!");
		}
		playerTracker.Add(sc);*/
	}


	public void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Player " + player.guid + " disconnected.");
		authPlayer found = null;
		/*foreach (authPlayer man in playerTracker) {
			if (man.getOwner() == player) {
				Network.RemoveRPCs(man.gameObject.networkView.viewID);
				Network.Destroy(man.gameObject);
			}
		}
		if (found) {
			playerTracker.Remove(found);
		}*/
	}
}