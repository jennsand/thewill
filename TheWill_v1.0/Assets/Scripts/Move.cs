using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	public Items itemList;
	public Clickable leftArrow;
	public Clickable rightArrow;
	public string leftScene;
	public string rightScene;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetKeyDown(KeyCode.Mouse0) ) {
			if ( !Items.inCloseUpNow() ) {	// Not in a close up
				if ( leftArrow.isClicked() ) {
					Application.LoadLevel(leftScene);
				}
				else if (rightArrow.isClicked() ) {
					Application.LoadLevel(rightScene);
				}
			}
		}
	}
}
