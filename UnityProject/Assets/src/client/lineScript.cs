using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class lineScript : MonoBehaviour {

	void OnNetworkInstantiate(NetworkMessageInfo info) {
		Debug.Log("LS: " + networkView.viewID + " spawned");

		authPlayer.lineDict.Add(networkView.viewID, transform);
	}

	[RPC]
	public void updateLine(NetworkViewID instance, Vector3[] tempStored) {
		//Debug.Log("updateLine " + instance);
		if (!authPlayer.lol.ContainsKey(instance))
			authPlayer.lol.Add(instance, null);
		authPlayer.lol[instance] = new List<Vector3>(tempStored);
	}
}
