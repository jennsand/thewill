using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class ContinueGame : MonoBehaviour {
	public string entryString = "Enter Code";
	public Clickable deniseLetter;
	public IntroTutorial tute;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnGUI() {
		if ( !tute.displayTute && !deniseLetter.renderer.enabled) {
			GUI.Box(new Rect(410,380,140,75), "Continue Game");
			
			// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
			entryString = GUI.TextField(new Rect(420, 405, 120,20), entryString, 20);
			
			// Make the button.
			if(GUI.Button(new Rect(420,430,120,20), "Continue")) {
				// Start a game based on code input string
				// First unencrypt it
				string unencyptedCode = "";
				try {
					byte[] decodedBytes = Convert.FromBase64String(entryString);
					unencyptedCode = Encoding.UTF8.GetString(decodedBytes);
				}
				catch (System.FormatException) {
					// Player entered the code wrong or messed with it.
					entryString = "Invalid Code";
				}
				if (unencyptedCode.Length<12 || unencyptedCode.Length > 12) {
					entryString = "Invalid Code";
				}
				else {
					string foundCode = unencyptedCode.Substring(0,6);
					string keepShareCode = unencyptedCode.Substring(6);
					int foundTurn = isValidFoundCode(foundCode);
					int ksTurn = isValidKSCode(keepShareCode);
					Debug.Log("Checking code ("+entryString+" aka "+unencyptedCode+")): found "+foundCode+"; turn = "+foundTurn
					          +"; keepShare = "+keepShareCode+"; turn = "+ksTurn);
					// Check that these are valid codes
					if ((foundTurn > 0) && (ksTurn > 0) && (foundTurn == ksTurn) ) {
						PlayerPrefs.SetString("foundCode",foundCode);
						PlayerPrefs.SetString("keepShareCode", keepShareCode);
						Debug.Log("Found = "+foundCode+". Keep/Share = "+keepShareCode);
						if (foundCode.Contains("1") ) {
							PlayerPrefs.SetString("Door1","unlocked");
						}
						else {
							PlayerPrefs.SetString("Door1","locked");
						}
						if (foundCode.Contains("2") ) {
							PlayerPrefs.SetString("Door2","unlocked");
						}
						else {
							PlayerPrefs.SetString("Door2","locked");
						}
						if (foundCode.Contains("3") ) {
							PlayerPrefs.SetString("Door3","unlocked");
						}
						else {
							PlayerPrefs.SetString("Door3","locked");
						}
						if (foundCode.Contains("4") ) {
							PlayerPrefs.SetString("Door4","unlocked");
						}
						else {
							PlayerPrefs.SetString("Door4","locked");
						}
						if (foundCode.Contains("5") ) {
							PlayerPrefs.SetString("Door5","unlocked");
						}
						else {
							PlayerPrefs.SetString("Door5","locked");
						}
						if (foundCode.Contains("6") ) {
							PlayerPrefs.SetString("Door6","unlocked");
						}
						else {
							PlayerPrefs.SetString("Door6","locked");
						}
						PlayerPrefs.SetString("UnlockedThisTime","0");
						// Check to see if this is player 2's first time playing
						if (foundTurn == 1) {
							// Show tutorial
							tute.displayTute = true;
						}
						else {	// Load straight into the game
							Application.LoadLevel("Wall_Safe");
						}
					}
					else {
						entryString = "Invalid Code";
					}
				}

			}
		}
	}

	public int isValidFoundCode(string codeToCheck)
	{
		// Checks to see if the formatting for the found code is suitable
		int turnNum = -1;
		for (int i=0;i<codeToCheck.Length;i++) {
			if ( turnNum == -1 ) {// Haven't found the end of the turn
				if ( codeToCheck[i] == '0') {
					// This is now the end turn, everything else should be 0s
					turnNum = i;
				}
				else {	// Need to have a number between 1 and 6
					if (( codeToCheck[i] != '1') && ( codeToCheck[i] != '2') &&
					    ( codeToCheck[i] != '3') && ( codeToCheck[i] != '4') &&
					    ( codeToCheck[i] != '5') && ( codeToCheck[i] != '6') ){
						// Invalid number, exit early
						Debug.Log("Invalid at i"+i+". Not a valid number. "+codeToCheck[i]);
						return -1;
					}
				}
			}
			else { // Turn is over, need to find a 0
				if (codeToCheck[i] != '0') {
					Debug.Log("Invalid at i"+i+". Didn't get a zero when supposed to. "+codeToCheck[i]);
					return -1;
				}
				// else, still valid, keep checking
			}
		}
		if (turnNum == -1 ) { 
			turnNum = codeToCheck.Length;	/// Must be the last turn, everything was valid until here.
		}

		return turnNum;	// Got to here, must have something valid
	}

	public int isValidKSCode(string codeToCheck)
	{
		// Checks to see if the formatting for the found code is suitable
		int turnNum = -1;
		for (int i=0;i<codeToCheck.Length;i++) {
			if ( turnNum == -1 ) {// Haven't found the end of the turn
				if ( codeToCheck[i] == 'N') {
					// This is now the end turn, everything else should be Ns
					turnNum = i;
				}
				else {	// Need to have either K or S
					if (( codeToCheck[i] != 'K') && ( codeToCheck[i] != 'S') ){
						// Invalid letter
						Debug.Log("Invalid at i"+i+". Not a valid letter. "+codeToCheck[i]);
						return -1;
					}
				}
			}
			else { // Turn is over, need to find a 0
				if (codeToCheck[i] != 'N') {
					Debug.Log("Invalid at i"+i+". Didn't get a N when supposed to. "+codeToCheck[i]);
					return -1;
				}
				// else, still valid, keep checking
			}
		}
		if (turnNum == -1 ) { 
			turnNum = codeToCheck.Length;	/// Must be the last turn, everything was valid until here.
		}

		return turnNum;	// Got to here, must have something valid
	}


}
