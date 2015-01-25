using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	public bool isStart1 = false;
	public bool isStart2 = false;
	// Use this for initialization
	Color onMouseEntercolor1;
	Color onMouseEntercolor2;
	Color maincolor1;
	Color maincolor2;
	
	void Start () {
		maincolor1 = new Color (0.2509f, 0.0862f, 0.0392f, 1f);
		maincolor2 = new Color (0.4509f, 0.32941f, 0.2039f, 1f);
		onMouseEntercolor1 = new Color (0.7490f, 0.6666f, 0.4509f, 1f);
		onMouseEntercolor2 = new Color (0.5490f, 0.4509f, 0.2941f, 1f);
		if(isStart1)
			renderer.material.color = maincolor1;
		else if(isStart2)
			renderer.material.color = maincolor2;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnMouseEnter(){
		if(isStart1)
			renderer.material.color = onMouseEntercolor1;
		else if(isStart2)
			renderer.material.color = onMouseEntercolor2;
	}
	
	void OnMouseExit(){
		if(isStart1)
			renderer.material.color = maincolor1;
		else if(isStart2)
			renderer.material.color = maincolor2;
		
	}
	void OnMouseUp(){
		if (isStart1) {
			Application.LoadLevel ("Connessione");
		}
	}
}
