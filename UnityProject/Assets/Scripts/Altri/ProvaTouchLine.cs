
using UnityEngine;
using System.Collections;

public class ProvaTouchLine : MonoBehaviour {

	public Color c1 = Color.yellow;
	public Color c2 = Color.red;
	public int lengthOfLineRenderer = 20;
	LineRenderer lineRenderer;
	int i = 0;

	void Start() {

		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(c1, c2);
		lineRenderer.SetWidth(1F, 1F);
		lineRenderer.SetVertexCount(lengthOfLineRenderer);
	}

	void Update() {
		int touchcount = 0;
		if (Input.GetMouseButtonDown (0)) {
			touchcount++;		
		}
				if (Input.touchCount == 1) {
						if (Input.GetTouch (0).phase == TouchPhase.Moved) {

						lineRenderer.SetPosition(i, Input.GetTouch (0).position);
						i++;

						}
						else if (Input.GetTouch (0).phase == TouchPhase.Ended)
							i=0;
		}


	}

}
