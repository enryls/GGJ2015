using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProvaTouch : MonoBehaviour {

	private LineRenderer line;
	private bool isTouch;
	private List<Vector3> pointList = new List<Vector3> ();
	private Vector3 touchPos;
	private Ray ray;
	// Use this for initialization

	void Start () {	
		line = gameObject.AddComponent<LineRenderer>();   
		line.material =  new Material(Shader.Find("Particles/Additive"));    
		line.SetVertexCount(0);
		line.SetWidth (0.1F, 0.1F);
		line.SetColors (Color.green, Color.green);         
		isTouch = false; 
	}
	
	// Update is called once per frame
	void Update () {	
		LineRenderer line = GetComponent<LineRenderer>();
		if (Input.GetMouseButtonDown (0)) {
			isTouch = true;
			line.SetVertexCount (0);
			pointList.RemoveRange (0, pointList.Count);
		} 
		else if (Input.GetMouseButtonUp (0)) {
			isTouch=false;

		}
		if (isTouch) {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			touchPos = ray.origin;
			touchPos.z=0;	
			Debug.Log(pointList.Count);

			if(!pointList.Contains(touchPos)){
				pointList.Add(touchPos);
				line.SetVertexCount (pointList.Count);
				line.SetPosition (pointList.Count - 1, pointList [pointList.Count - 1]);
			}
		}
	
	}
}
