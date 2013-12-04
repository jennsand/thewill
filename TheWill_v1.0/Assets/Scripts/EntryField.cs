using UnityEngine;
using System.Collections;

public class EntryField : MonoBehaviour {
	public string entryString = "Enter Code";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
	void OnGUI() {
		GUI.Box(new Rect(10,10,100,90), "Enter Code");
		
		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(20,40,80,20), "Level 1")) {
			Application.LoadLevel(1);
		}
		
		// Make the second button.
		if(GUI.Button(new Rect(20,70,80,20), "Level 2")) {
			Application.LoadLevel(2);
		}

		entryString = GUI.TextField(new Rect(250, 270, 120,20), entryString, 12);
		//bool clicked = GUI.Button(new Rect(20,70,80,20), "Level 2");
	}
	public void invalidCode() {
		entryString = "Invalid Code";
	}

}
