using UnityEngine;
using System.Collections;

public class LetterLocker : MonoBehaviour {
	public Clickable letter;
	public bool sonLetter;

	// Use this for initialization
	void Start () {
		string foundCode = PlayerPrefs.GetString("foundCode");
		bool door1or2open = foundCode.Contains("1") || foundCode.Contains("2");
		bool door3or4open = foundCode.Contains("3") || foundCode.Contains("4");

		if (( sonLetter && door1or2open ) || (!sonLetter && door3or4open)) {
			letter.renderer.enabled = true;
			letter.collider2D.enabled = true;
		}
		else {
			letter.renderer.enabled = false;
			letter.collider2D.enabled = false;
		}


	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void enableLetter() {
		letter.renderer.enabled = true;
		letter.collider2D.enabled = true;

	}
}
