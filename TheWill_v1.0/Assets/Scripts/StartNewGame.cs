using UnityEngine;
using System.Collections;

public class StartNewGame : MonoBehaviour {
	public Clickable deniseLetter;
	public IntroTutorial tute;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (deniseLetter.renderer.enabled && deniseLetter.isClicked()) {
			// Now show the tutorial
			tute.displayTute = true;
			deniseLetter.renderer.enabled = false;
			deniseLetter.collider2D.enabled = false;
		}
	
	}

	void OnGUI() {
		if (!deniseLetter.renderer.enabled && !tute.displayTute) {
			GUI.Box(new Rect(410,300,140,45), "Start New Game");
			// Make the button.
			if(GUI.Button(new Rect(420,320,120,20), "Start")) {
				// Begin the new game
				PlayerPrefs.SetString("Door1","locked");
				PlayerPrefs.SetString("Door2","locked");
				PlayerPrefs.SetString("Door3","locked");
				PlayerPrefs.SetString("Door4","locked");
				PlayerPrefs.SetString("Door5","locked");
				PlayerPrefs.SetString("Door6","locked");
				PlayerPrefs.SetString("foundCode", "000000");
				PlayerPrefs.SetString("keepShareCode", "NNNNNN");
				PlayerPrefs.SetString("UnlockedThisTime","0");
				// Show Denise's letter
				deniseLetter.renderer.enabled = true;
				deniseLetter.collider2D.enabled = true;
			}
		}
	}

}
