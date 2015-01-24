using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	Vector3 lastPosition;
	float minimumMovement = .05f;


	// Update is called once per frame
	void Update () {
		if (networkView.isMine){ 
			Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); 
			float speed = 5; 
			transform.Translate(speed * moveDir * Time.deltaTime);

			//if (Vector3.Distance(transform.position, lastPosition) > minimumMovement)			{
			//	lastPosition = transform.position;
			//	networkView.RPC("SetPosition", RPCMode.Others, transform.position);
			//}
		}
	}

	//[RPC]
	//void SetPosition(Vector3 newPosition){
	//	Debug.Log("RPC!\n");
	//	transform.position = newPosition;
	//}

	//void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
	//	//If not the server
	//	if (stream.isWriting) {
	//		Vector3 pos = transform.position;
	//		stream.Serialize(ref pos);
	//	}
	//}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info){ 
		if (stream.isWriting) { 
			Vector3 myPosition = transform.position;
			stream.Serialize(ref myPosition); 
		} else { 
			Vector3 receivedPosition = Vector3.zero; 
			stream.Serialize(ref receivedPosition); 
			//"Decode" it and receive it 
			transform.position = receivedPosition; 
		}
	}

	void Awake(){ 
		if (!networkView.isMine) enabled = false;
	}
}
