using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Minion : MonoBehaviour {
	List<Vector3> linePoints;
	bool run = false;
	int i = 0;
	bool inCD = false;
	public enum color {
		Red = 0, blue = 1, yellow = 2

	};
	private color minionColor;
	// Use this for initialization
	void Start() {
		minionColor = color.Red;
	}

	// Update is called once per frame
	void Update() {

		if (run) {

			Debug.Log("enemy");
			transform.position = new Vector3(linePoints[i].x * 3f, (linePoints[i].y + 0.02f) * 3f, 1f);
			i++;
			StartCoroutine(onCOOL());
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		lineScript line = other.gameObject.GetComponent<lineScript>();
		if (line == null) {
			//No line but minion
			return;
		}

		Debug.Log("lineea");

		if (line.lineColor == colors.red) {
			linePoints = line.getPosition();
			run = true;
		} else
			run = false;

	}

	IEnumerator onCOOL() {
		inCD = true;
		yield return new WaitForSeconds(0.10f);
		inCD = false;
	}
}
