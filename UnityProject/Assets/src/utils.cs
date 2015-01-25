using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class utils {

	public static readonly float globalZ = -14.7f;

	public static void addCollider(GameObject gm,List<Vector3>linePoints){

		/*
		GameObject collin = new GameObject();
		collin.gameObject.transform.position = linePoints[0];
		*/
		BoxCollider2D BX = gm.AddComponent<BoxCollider2D>();

		BX.size = new Vector2(0.1f, 0.1f);

		BX.center = new Vector2(linePoints[0].x - gm.transform.position.x, linePoints[0].y + gm.transform.position.y);

		/*
		GameObject collinf = new GameObject();
		collinf.gameObject.transform.position = linePoints[linePoints.Count - 1];
		*/
		BoxCollider2D BXf = gm.AddComponent<BoxCollider2D>();
		BXf.size = new Vector2(0.1f, 0.1f);

		BXf.center = new Vector2(linePoints[linePoints.Count - 1].x - gm.transform.position.x, linePoints[linePoints.Count - 1].y + gm.transform.position.y);
	}

	public static Color getColor(colors inCol) {
		Color c1 = Color.white;
		Color c2 = Color.black;
		switch (inCol) {
			case colors.red:
				c1 = c2 = Color.red;
				break;
			case colors.yellow:
				c1 = c2 = Color.yellow;
				break;
			case colors.blue:
				c1 = c2 = Color.cyan;
				break;
			case colors.green:
				c1 = c2 = Color.green;
				break;
			default: break;
		}

		return c1;
	}
}

