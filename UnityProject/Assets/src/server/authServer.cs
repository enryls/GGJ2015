using UnityEngine;

public class authServer : MonoBehaviour {

	float serverCurrentHInput = 0f;
	float serverCurrentVInput = 0f;

	void Update() {
		if (Network.isClient)
			return;

		Vector3 moveDirection = new Vector3(serverCurrentHInput, 0, serverCurrentVInput);
		float speed = 5;
		transform.Translate(speed * moveDirection * Time.deltaTime);
	}

	/**
	* The client calls this to notify the server about new motion data
	* @param	motion
	*/
	[RPC]
	void updateClientMotion(float HInput, float VInput) {
		serverCurrentHInput = HInput;
		serverCurrentVInput = VInput;
	}
}
