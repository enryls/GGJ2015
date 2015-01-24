using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


	public class updateLineElement {
		public List<Vector3> pointsList;
		public colors pointsColor = colors.none;

		public updateLineElement() {
			pointsList = new List<Vector3>();
		}

		public updateLineElement(Vector3[] _pointsList, colors _pointsColor) {
			pointsColor = _pointsColor;
			pointsList = new List<Vector3>(_pointsList);
		}
	}

