using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class utils {
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

