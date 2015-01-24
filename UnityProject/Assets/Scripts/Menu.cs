using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	public bool isStart = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseUp(){
		if (isStart)
			Application.LoadLevel ("Prova");
	}
}
