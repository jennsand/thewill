using UnityEngine;
using System.Collections;

public class ItemGroup : MonoBehaviour {
	public Clickable farItem;
	public Clickable closeItem;
	public Clickable closeItem02;
	public string groupName;
	public SendEmail keepShareButtons;

	// Use this for initialization
	void Start () {
		closeItem.renderer.enabled = false;
		if ( closeItem02 != null ) {
			closeItem02.renderer.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetKeyDown(KeyCode.Mouse0) ) {
			// Someone is clicking something
			if ( closeItem.renderer.enabled || ((closeItem02 != null) && closeItem02.renderer.enabled) ) { // In close up mode
				if (keepShareButtons == null
				    	|| (!keepShareButtons.displayingKeepShare) ) {
					if (closeItem02 != null && !closeItem02.renderer.enabled) {
						closeItem.renderer.enabled = false;
						closeItem02.renderer.enabled = true;
					}
					else {
						// Exit close up mode
						Debug.Log(groupName+": Button down when in close up.");
						closeItem.renderer.enabled = false;
						if ( closeItem02 != null ) {
							closeItem02.renderer.enabled = false;
						}
						Items.exitCloseUp();
					}
				}
			}
			else if ( farItem.isClicked() ) { // Not in close up mode and clicked our item
				Debug.Log(groupName+": I know "+farItem.itemName+" is clicked.");
				// Check to see if already in a close up
				if (Items.attemptCloseUp() ) { // Successful
					closeItem.renderer.enabled = true;
				}
				// else do nothing, don't go into close up
			}
		}
	
	}

}
