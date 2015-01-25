using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class sv_spawn : MonoBehaviour {
	public static string levelName = "networkScene";

	public readonly int maxPlayers = 3;

	public List<authPlayer> playerTracker = new List<authPlayer>();
	public List<NetworkPlayer> scheduledSpawns = new List<NetworkPlayer>();

	private bool processSpawnRequests = false;

	public Transform playerPrefab;
	public int usedColors = 0;

	public static bool canSpawn = false;

	void OnPlayerConnected(NetworkPlayer player){
		if (playerTracker.Count > maxPlayers) {
			//TODO force disconnect
			return;
		}

		Debug.Log("Spawning prefab for new client");
		scheduledSpawns.Add(player);
		processSpawnRequests = true;

		if (!canSpawn)
			canSpawn = true;
	}

	[RPC]
	public void requestSpawn(NetworkPlayer requester) {
		//Called from client to the server to request a new entity
		if (Network.isClient) {
			Debug.LogError("Client tried to spawn itself! Revise logic!");
			return; //Get lost! This is server business
		}

		if (!processSpawnRequests) {
			return; //silently ignore this
		}

		//Process all scheduled players
		foreach (NetworkPlayer spawn in scheduledSpawns) {
			Debug.Log("Checking player " + spawn.guid);

			if (spawn == requester) { //That is the one, lets make him an entity!
				Transform handle = (Transform)Network.Instantiate(
													playerPrefab,
													transform.position,
													transform.rotation, 
													1);//temp
				authPlayer sc = handle.GetComponent<authPlayer>();
				if (!sc) {
					Debug.LogError("The prefab has no authPlayer attached!");
				}

				//Set the player color!
				sc.playerColor = (colors)(++usedColors);
				
				playerTracker.Add(sc);
				//Get the network view of the player and add its owner
				NetworkView netView = handle.GetComponent<NetworkView>();
				netView.RPC("setOwner", RPCMode.AllBuffered, spawn);
			}
		}

		scheduledSpawns.Remove(requester); //Remove the guy from the list now
		if (scheduledSpawns.Count == 0) {
			Debug.Log("spawns is empty! stopping spawn request processing");
			//If we have no more scheduled spawns, stop trying to process spawn requests
			processSpawnRequests = false;
		}
	}

	public void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Player " + player.guid + " disconnected.");
		authPlayer found = null;
		foreach (authPlayer man in playerTracker) {
			if (man.getOwner() == player) {
				Network.RemoveRPCs(man.gameObject.networkView.viewID);
				Network.Destroy(man.gameObject);
			}
		}

		if (found) {
			playerTracker.Remove(found);
			--usedColors;
		}

		if (playerTracker.Count <= 0)
			canSpawn = false;
	}

}
