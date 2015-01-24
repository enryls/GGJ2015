using UnityEngine;
using System.Collections;

public class authPlayer : MonoBehaviour {

	//That's actually not the owner but the player,
	//the server instantiated the prefab for, where this script is attached
	public NetworkPlayer theOwner;

	//Those are stored to only send RPCs to the server when the 
	//data actually changed.
	float lastClientHInput = 0f;
	float lastClientVInput = 0f;

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
			if (GetComponent<Camera>()) {
				GetComponent<Camera>().enabled = false;
			}

			if (GetComponent<AudioListener>()) {
				GetComponent<AudioListener>().enabled = false;
			}

			if (GetComponent<GUILayer>()) {
				GetComponent<GUILayer>().enabled = false;
			}
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
		if (theOwner != null && Network.player == theOwner) {
			float HInput = Input.GetAxis("Horizontal");
			float VInput = Input.GetAxis("Vertical");
			if (lastClientHInput != HInput || lastClientVInput != VInput) {
				//Store latest data
				lastClientHInput = HInput;
				lastClientVInput = VInput;
				//Send update
				networkView.RPC("updateClientMotion", RPCMode.Server, HInput, VInput);
			}
		}
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		if (stream.isWriting) {
			Vector3 pos = transform.position;
			stream.Serialize(ref pos);
		} else {
			Vector3 posReceive = Vector3.zero;
			stream.Serialize(ref posReceive);
			transform.position = posReceive;
		}
	}
}
