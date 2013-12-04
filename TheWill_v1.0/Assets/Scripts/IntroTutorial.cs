using UnityEngine;
using System.Collections;

public class IntroTutorial : MonoBehaviour {
	public bool displayTute;

	// Use this for initialization
	void Start () {
		displayTute = false;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		if (displayTute) {
			GUIStyle boxStyle = "box";
			boxStyle.wordWrap = true;	
			GUI.Box(new Rect(310,220,320,110), "This is a two player, turn based, point-and-click adventure game."
			        +" On your turn, use the clues to find a number code to unlock a safe."
			        +" Then choose whether to keep the item you find for yourself or share with your friend.", boxStyle);
			// Make the button.
			if(GUI.Button(new Rect(420,305,100,20), "Begin Game")) {
				Application.LoadLevel("Wall_Safe");
			}
		}
	}
}
