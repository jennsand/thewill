using UnityEngine;
using System.Collections;

public class NumberEntry : MonoBehaviour {

	public Clickable door_locked;
	public Clickable door_unlocked;
	public Clickable hintPoem;
	public Clickable prizeHint;
	public Clickable prize;
	public Digit [] digits;
	public GUIText door_name;
	public Clickable enter_button;
	public GameObject padlock_image;
	public string door_number;
	public int correctAnswer;
	public LetterLocker letterLocker;

	private bool doorUnlocked;
	private bool showingError;

	// Use this for initialization
	void Start () {
		// Check to see if the door is locked
		string doorState = PlayerPrefs.GetString("Door"+door_number);
		Debug.Log("Door "+door_number+": is currently "+doorState);
		if ("locked".Equals(doorState) ) {
			makeVC(door_locked);
			makeUnVC(door_unlocked);
			makeUnVC(prize);
			doorUnlocked = false;
			// Check to see if this note has been found yet
			if ("1".Equals(door_number) || "2".Equals(door_number) ) {
				// A starting clue
				makeVC (hintPoem);
			}
			else if ("3".Equals(door_number) && "unlocked".Equals(PlayerPrefs.GetString("Door1"))) {
				Debug.Log("I'm door 3 and door 1 has already been unlocked");
				makeVC (hintPoem);	// Correct door unlocked
			}
			else if ("4".Equals(door_number) && "unlocked".Equals(PlayerPrefs.GetString("Door2"))) {
				Debug.Log("I'm door 4 and door 2 has already been unlocked");
				makeVC (hintPoem);	// Correct door unlocked
			}
			else if ("5".Equals(door_number) && "unlocked".Equals(PlayerPrefs.GetString("Door3"))) {
				Debug.Log("I'm door 5 and door 3 has already been unlocked");
				makeVC (hintPoem);	// Correct door unlocked
			}
			else if ("6".Equals(door_number) && "unlocked".Equals(PlayerPrefs.GetString("Door4"))) {
				Debug.Log("I'm door 6 and door 4 has already been unlocked");
				makeVC (hintPoem);	// Correct door unlocked
			}
			else {
				makeUnVC(hintPoem);	// Don't show it yet
			}
		}
		else {	// Door must have been unlocked
			makeUnVC(door_locked);
			makeVisible(door_unlocked);
			makeUnclickable(door_unlocked);
			makeUnVC(hintPoem);
			makeVC(prize);
			doorUnlocked = true;
		}
		if (prizeHint!=null) {	// Don't show the prize hint until unlock this door.
			makeUnVC(prizeHint);
		}
		makeDigitsUnVC();
		showingError = false;
	}
	
	// Update is called once per frame
	void Update () {
	  string doorUnlockedThisTime = PlayerPrefs.GetString("UnlockedThisTime");
	  if ("0".Equals(doorUnlockedThisTime) ) {	// Haven't opened a door yet
		if ( Input.GetKeyDown(KeyCode.Mouse0) ) {
			// Someone is clicking something
			if ( enter_button.renderer.enabled ) { // In close up mode
				if ( enter_button.isClicked() ) {	
					// Player has tried to enter a number
					Debug.Log("Checking answers: player says: "+playerAnswer()+" correct answer is: "+correctAnswer);
					if (playerAnswer() == correctAnswer) {
						//	Check to see if the number is correct
						Debug.Log("Door "+door_number+": Entered a number");
						makeUnVC(door_locked);	// Can't interact with locked door anymore.
						makeVisible(door_unlocked);	// Can see, but not click the unlocked door
						makeVC(prize);	// Make prize visible
						if (prizeHint != null ) {
								makeVC(prizeHint);	// Make prize hint visible
						}
						makeUnVC(hintPoem);  // Remove hint poem.
						makeDigitsUnVC();
						Items.exitCloseUp();
						doorUnlocked = true;
						PlayerPrefs.SetString("Door"+door_number, "unlocked");
						PlayerPrefs.SetString("UnlockedThisTime",door_number);
						// Unlock special locker with extra documents if this is the first turn
						if (letterLocker!=null) {
								letterLocker.enableLetter();
						}
					}
					// If not correct, do nothing
				}
				else if (!buttonClicked()) { 
					//Player has clicked elsewhere on the screen, not on a button
					// Exit close up mode
					Debug.Log("Door "+door_number+": Exit the close up screen.");
					makeDigitsUnVC();
					makeClickable(door_locked);	// Door still locked.
					Items.exitCloseUp();
				}
			}
			else if ( door_locked.isClicked() ) { // Not in close up mode and clicked our item
				Debug.Log("Door "+door_number+": I know "+door_locked.itemName+" is clicked.");
				// Check to see if already in a close up
				if (Items.attemptCloseUp() ) { // Check nothing else in close up first
					makeUnclickable(door_locked);
					makeDigitsVC();
				}
				// else do nothing, don't go into close up
			}
		}
	  }
	  else if ( Input.GetKeyDown(KeyCode.Mouse0) && door_locked.isClicked() ) {
			// ELSE We've already opened a door, we shouldn't allow opening another door
			// Give an error message to tell the player to collect their prize.
			showingError = true;
	  }
	  
	}
	void OnGUI() {
		if (showingError) {
			GUI.skin.button.wordWrap = true;
			if(GUI.Button(new Rect(150,200,280,80), "You can only unlock one prize per turn. Click on the prize to decide whether to keep or share the item and notify your sister.")) {
				showingError = false;
			}
		}
	}

	public bool isUnlocked()
	{
		return doorUnlocked;
	}

	public bool buttonClicked()
	{
		// Returns true if any of the UI buttons have been clicked
		if (enter_button.isClicked()) {
			return true;
		}
		else {	// Check all the digits
			for (int i=0; i<digits.Length; i++) {
				if ( digits[i].upButton.isClicked() || digits[i].downButton.isClicked() ) {
					return true;
				}
			}
		}
		return false;
	}

	private int playerAnswer()
	{
		// Returns the current answer that the player has set
		float answer = 0f;
		for (int i=0; i<digits.Length; i++) {
			answer += digits[i].numShowing()*Mathf.Pow(10.0f,(float) i);
			//Debug.Log("Adding "+digits[i].numShowing()*Mathf.Pow(10.0f,(float) i));
		}
		return (int) answer;
	}

	private void makeDigitsVC() {
		door_name.enabled = true;
		for (int i=0; i<digits.Length; i++) {
			digits[i].makeVC();
		}
		makeVC(enter_button);
		padlock_image.renderer.enabled = true;
	}
	private void makeDigitsUnVC() {
		door_name.enabled = false;
		for (int i=0; i<digits.Length; i++) {
			digits[i].makeUnVC();
		}
		makeUnVC(enter_button);
		padlock_image.renderer.enabled = false;
	}

	public void makeVC(Clickable item) {
		// Makes an item both visible and clickable
		//Debug.Log("Door "+door_number+": Making item "+item.itemName+" visible.");
		makeClickable(item);
		makeVisible(item);
	}
	public void makeUnVC(Clickable item) {
		// Makes an item invisible and unclickable
		makeUnclickable(item);
		makeInvisible(item);
	}

	public void makeClickable(Clickable item) {
		item.collider2D.enabled = true;
	}

	public void makeUnclickable(Clickable item) {
		item.collider2D.enabled = false;
	}

	public void makeVisible(Clickable item)
	{
		item.renderer.enabled = true;

	}
	public void makeInvisible(Clickable item)
	{
		//Debug.Log("Door "+door_number+": Making item "+item.itemName+" INvisible.");
		item.renderer.enabled = false;
	}

}
