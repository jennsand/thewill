using UnityEngine;
using System.Collections;

public class Clickable : MonoBehaviour {
	// An item that is only clickable if it is visible

	public string itemName;
	public bool clickableInZoomMode;

	private bool clicked;

	// Use this for initialization
	void Start () {
		clicked = false;	
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnMouseDown() {
			ProcessMouseDown();
	}
	void OnMouseUp(){
		//Debug.Log (itemName+ ": I'm unclicked");
		clicked = false;
	}
	
	private void ProcessMouseDown()
	{
			if ((clickableInZoomMode && Items.inCloseUpNow()) 
			    || (!clickableInZoomMode && !Items.inCloseUpNow()) ) {
				Debug.Log (itemName+ ": I'm clicked, and nothing else in close up");
				clicked = true;
			}
			else {
				Debug.Log (itemName+ ": I'm not clicked... No, I'm not in denial!");
			}
	}


	public bool isClicked()
	{
		return clicked;
	}
}
