using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Minion : MonoBehaviour {
	List<Vector3> linePoints;
	bool run = false;
	int i = 0;
	bool inCD = false;
	bool end = false;
	bool down = false;

	private colors minionColor;

	public void setColor(colors color) {
		minionColor = color;
	}

	// Update is called once per frame
	void Update() {

		if (run && !inCD) {

			if (linePoints == null)
				return;

			if (i < linePoints.Count) {
				transform.position = new Vector3(linePoints[i].x * 3f, (linePoints[i].y + 0.02f) * 3f, 1f);

				i++;
				StartCoroutine(onCOOL());
			} else
				StartCoroutine(onEnd());
		}
		/*
	    if (run) 
		{
		if (i >= linePoints.Count && !end) 
			{
					gameObject.rigidbody2D.gravityScale = 1f;
			}

		}
		*/
	}

	void OnTriggerStay2D(Collider2D other) {

		if (other.gameObject.tag == "Finish" && run) {
			Debug.Log("kill");
			end = true;
			gameObject.SetActive(false);
			kill();
			return;
		}

		lineScript line = other.gameObject.GetComponent<lineScript>();
		if (line == null) {
			//No line but minion
			return;
		}

		if (line.lineColor == minionColor) {
			linePoints = line.getPosition();
			//i=0;
			run = true;
		}

	}

	//void OnTriggerEnter2D(Collider2D other) {
	//	//Debug.Log("lineea");
	//	if (down && other.gameObject.tag == "line" && !run) {
	//		gameObject.rigidbody2D.gravityScale = 0f;
	//		lineScript line = other.gameObject.GetComponent<lineScript>();
	//		if (line.lineColor == minionColor) {
	//			linePoints = line.getPosition();
	//			i = 0;
	//			run = true;
	//			down = false;
	//		}
	//	}
	//}

	IEnumerator onCOOL() {
		inCD = true;
		yield return new WaitForSeconds(0.10f);
		inCD = false;
	}

	IEnumerator onEnd() {
		yield return new WaitForSeconds(0.5f);
		if (!end) {
			gameObject.rigidbody2D.gravityScale = 1;
			down = true;
			run = false;
		}
	}


	void kill() {
		Network.RemoveRPCs(transform.networkView.viewID);
		//Network.Destroy(transform.gameObject);
	}
}
