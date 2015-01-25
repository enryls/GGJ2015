﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Minion : MonoBehaviour {
	List<Vector3> linePoints;
	bool run = false;
	int i = 0;
	bool inCD = false;
	bool end = false;
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

		if (run && !inCD) {

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

		if (other.gameObject.tag == "Finish") {
			end = true;
			gameObject.SetActive(false);
			return;
		}

		lineScript line = other.gameObject.GetComponent<lineScript>();
		if (line == null) {
			//No line but minion
			return;
		}

		if (line.lineColor == colors.red) {
			linePoints = line.getPosition();
			//i=0;
			run = true;
		}

	}

	IEnumerator onCOOL() {
		inCD = true;
		yield return new WaitForSeconds(0.10f);
		inCD = false;
	}

	IEnumerator onEnd() {
		yield return new WaitForSeconds(0.5f);
		if (!end)
			gameObject.rigidbody2D.gravityScale = 1;
	}
}
