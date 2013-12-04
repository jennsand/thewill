using UnityEngine;
using System.Collections;

public class Digit : MonoBehaviour {

	public Clickable upButton;
	public Clickable downButton;
	public GUIText digitText;

	private int digitNumber;

	// Use this for initialization
	void Start () {
		digitNumber = 0;
		digitText.text = digitNumber.ToString();
		makeUnVC();
	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetKeyDown(KeyCode.Mouse0) ) {
			if (upButton.isClicked()) {
				digitNumber++;
				if (digitNumber > 9 ) {
					digitNumber = 0;
				}
				Debug.Log("New number is "+digitNumber);
				digitText.text = digitNumber.ToString();
			}
			else if (downButton.isClicked()) {
				digitNumber--;
				if (digitNumber < 0 ) {
					digitNumber = 9;
				}
				digitText.text = digitNumber.ToString();
			}
		}
	}

	public void makeVC() {
		// Makes this digit (the number AND arrows) both visible and clickable
		digitText.enabled = true;
		upButton.renderer.enabled = true;
		upButton.collider2D.enabled = true;
		downButton.renderer.enabled = true;
		downButton.collider2D.enabled = true;
	}
	public void makeUnVC() {
		// Makes this digit (the number AND arrows) invisible and unclickable
		digitText.enabled = false;
		upButton.renderer.enabled = false;
		upButton.collider2D.enabled = false;
		downButton.renderer.enabled = false;
		downButton.collider2D.enabled = false;
	}

	public int numShowing()
	{
		return digitNumber;
	}

}
