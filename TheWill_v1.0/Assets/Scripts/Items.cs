using UnityEngine;
using System.Collections;

public class Items : MonoBehaviour {
	//public ItemGroup [] itemsList;
	private static bool inCloseUp;
	private static SpriteRenderer greyBackground;
	public GameObject greyInbetweenObject;

	// Use this for initialization
	void Start () {
		inCloseUp = false;
		greyBackground = greyInbetweenObject.GetComponent<SpriteRenderer>();
		greyBackground.renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static bool attemptCloseUp()
	{	// Called by itemGroup when it wants to set one of its items into a close up
		if (inCloseUp ) {
			// Already in a close up, can't go into a new one
			return false;
		}
		else {
			inCloseUp = true;
			greyBackground.renderer.enabled = true;
		}
		return true;
	}
	public static void exitCloseUp()
	{	// Remove the grey background.
		greyBackground.renderer.enabled = false;
		inCloseUp = false;
	}
	public static bool inCloseUpNow()
	{	// Allows other functions to test whether in a zoom now or not.
		return inCloseUp;
	}
}
