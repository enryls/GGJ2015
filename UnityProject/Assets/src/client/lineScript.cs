using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class lineScript : MonoBehaviour {

	public colors lineColor = colors.none;

	void OnNetworkInstantiate(NetworkMessageInfo info) {
		//Debug.Log("LS: " + networkView.viewID + " spawned");

		authPlayer.lineDict.Add(networkView.viewID, transform);
	}

	[RPC]
	public void updateLine(NetworkViewID instance, Vector3[] tempStored, int color) {
		colors tc = (colors)color;
		//Debug.Log("updateLine " + instance);
		if (!authPlayer.lol.ContainsKey(instance))
			authPlayer.lol.Add(instance, null);

		authPlayer.lol[instance] = new updateLineElement(tempStored, tc);
	}

	List<Vector3> Position;
	public void SetPosition(List<Vector3> position) {
		Position = position;
	}

	public List<Vector3> getPosition() {
		return Position;
	}


	[RPC]
	public void addCollider(NetworkViewID instance, Vector3[] tempStored) {
		utils.addCollider(new List<Vector3>(tempStored));
	}
}
