using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class SendEmail : MonoBehaviour {

	public NumberEntry doorEntry;
	public Clickable linkedPrizeZoom;
	public bool displayingKeepShare;
	public string prizeName;

	private int sisterPlaying;
	private string originalFoundCode;
	private string originalKeepShareCode;
	private int ownerNum;
	private string owner;
	private string ownersChoice;
	private string doorNum;
	private string ownershipStatus;

	// Use this for initialization
	void Start () {
		//email_info.text = "Not sending email.";
		originalFoundCode = PlayerPrefs.GetString("foundCode");
		originalKeepShareCode = PlayerPrefs.GetString("keepShareCode");	
		sisterPlaying = FindSisterPlayingNow();
		doorNum = doorEntry.door_number;
		// Find out if this door has been opened already
		int openedOnTurn = originalFoundCode.IndexOf(doorNum);
		if (openedOnTurn >= 0) {
			// Door has been opened
			ownersChoice = ""+originalKeepShareCode[openedOnTurn];
			if (openedOnTurn == 0 || openedOnTurn == 2 || openedOnTurn == 4) {
				ownerNum = 1;
				owner = "Vivian";
			}
			else {	// Must have been Denise
				ownerNum = 2;
				owner = "Denise";
			}
			Debug.Log("Door "+doorNum+" was unlocked by "+owner+" on turn "+openedOnTurn+" result was "+ownersChoice);
			if ( ownerNum == sisterPlaying) {
				// This sister did something with the item
				if ("K".Equals(ownersChoice) ) {
					ownershipStatus = "You chose to keep this for yourself.";
				}
				else if ("S".Equals(ownersChoice) ) {
					ownershipStatus = "You chose to share this with your sister.";
				}
			}
			else {	// Other sister owns it
				if ("K".Equals(ownersChoice) ) {
					ownershipStatus = owner+" chose to keep this for herself.";
				}
				else if ("S".Equals(ownersChoice) ) {
					ownershipStatus = owner+" chose to share this with you.";
				}
			}
			Debug.Log("Ownership status = "+ownershipStatus);
		}
		else {
			// Door hasn't been opened
			ownerNum = -1;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}		
	void OnGUI() {
		if (linkedPrizeZoom.renderer.enabled == true)  {
			// zoomed in on the prize
			if ( doorNum.Equals( PlayerPrefs.GetString("UnlockedThisTime") ))  {
					// This was the prize that was just unlocked
				displayingKeepShare = true;
				if(GUI.Button(new Rect(150,400,280,20), "Keep "+prizeName+" for yourself")) {
					Debug.Log("Player clicked Keep.");
						// Send message
					string body = WWW.EscapeURL(GetBody("K"));
					Debug.Log("Sister = "+sisterPlaying+"; Body is : "+body);
					Application.OpenURL("http://www.jennsand.com/demos/thewill/sendemail.php?sister="+sisterPlaying
					                    +"&emailbody="+body);
				}
				if(GUI.Button(new Rect(450,400,280,20), "Share "+prizeName+" with your sister")) {
					Debug.Log("Player Clicked Share.");
					// Send message
					string body = WWW.EscapeURL(GetBody("S"));
					Debug.Log("Sister = "+sisterPlaying+"; Body is :"+body);
					Application.OpenURL("http://www.jennsand.com/demos/thewill/sendemail.php?sister="+sisterPlaying
					                    +"&emailbody="+body);
				}
			}
			else {
				// This door was unlocked at some other time
				displayingKeepShare = false;
					// Display what the sister did about this item
				GUI.Box(new Rect(250,450,300,30), ownershipStatus);
			}
		}
		else {
				displayingKeepShare = false;
		}
	}

	private string GetBody(string keepShare)
	{
		// Works out the full string of what the body will be.
		string body = "";

		// Put in bit from the player
		body += "I'm playing a game called: The Will. It's a two-player point-and-click game."
				+" In the game we play as sisters dividing the contents of our Dad's study."
				+" It's your turn now. Grab the code from the email below and then go to:"
				+"  http://www.jennsand.com/demos/thewill/TheWill_v1.5.html <br><br>"
				+"----------------------------------------------------<br><br>";

		// Get the saluations to the other sister
		body += GetEmailIntro();

		// Comment about what the other player kept/shared last time
		body += GetResponseToLastKs();

		// Comment about what this player has kept/shared for now
		body += GetCommentOnPlayerAction(keepShare);

		// Comment about an overview poem that has been found
		body += GetOverviewPoemComment();

		// If this is the first turn and the locker with the extra letters about a son turn up, comment
		int currentTurn = FindTurnNum();
		if (currentTurn == 0) {
			body += "Whaaat????!!!! I just found something saying that Mum & Dad had a son?! I don't get it! How could they keep that a secret for so long? I'm in shock!<br>";
		}

		// Comment about sharing/not progress
		body += GetSharingProgress(keepShare);

		// Comment about endings that are/aren't possible anymore
		body += GetEndingsProgress(keepShare);

		// Finishing comment... It's your turn, here's the new code to enter the room
		string newCode = CreateCode(keepShare);
		if (currentTurn == 6) {
			// No more turns to take!
			body += "We've unlocked everything now, but if you want to go back to the room you'll need this code: <br>"
				+ newCode +"<br>";
		}
		else {	// Tell player to take their turn
			body += "It's your turn now, sis. Here's the code to unlock the study door for you: <br>"
					+ newCode +"<br>";
		}

		body += NameSisterPlayingNow()+"<br>";

		//TODO Work out what the previous emails would have said, so can include in the thread.
		// Just do that for the very first email....
		if (currentTurn == 0) {
			body += "<br><br>----------------------------------------------------<br><br>"
				+ "On Tue, Dec 3, 2013 at 12:34 PM, Denise Fong Lau wrote:<br>"
					+ "Hey Viv, <br><br>"
    				+ "Can't believe it's already been a month since the memorial service. The lawyer finally sent notice that we can go in and see Dad's cryptic man cave. If it's anything like the last time I saw it (when I was like, 10), it's so crowded with stuff that I don't even know how Dad fit himself in there. <br>"
					+ "Looks like Dad actually left us something of a treasure hunt, complete with clues. So typical. Couldn't be like normal kids who just find Christmas presents under the tree... Santa requires maths riddles first. Sigh. Apparently there are six special things to find, and we can either keep them for ourselves or share them with each other. <br>"
					+ "Anyway, work is going to be crazy busy over the next little while (like sleeping at the office kind of crazy), so we won't get to see each other. Let's take turns and keep in touch via email. <br>"
					+ "You go first. <br>"
					+ "Denise <br>";
		}
		return body;
	}
	private string GetEmailIntro()
	{
		// Gets the beginning of the email, saluations etc...
		string introText = "";

		int turnNum = FindTurnNum();
		if (turnNum == 0) {
			introText += "Hi Denise!<br>Thanks for letting me go first. I managed to open up one of the lockers! Guess it must be your turn to try next.";
		}
		else if (turnNum == 1) {
			introText += "Hey Vivian!<br>Went to Dad's study today to have a go and managed to find something.";
		}
		else if (turnNum == 2) {
			introText += "Hi Denise!<br>Sorry it's taken me so long to get back to you, I was busy. But I had a go in Dad's study today.";
		}
		else if (turnNum == 3) {
			introText += "Hey Vivian!<br>It's kinda creepy looking through Dad's stuff without him there. But I guess it has to be done...";
		}
		else if (turnNum == 4) {
			introText += "Hi Denise!<br>I guess that's all of my turns done. I was just beginning to enjoy solving some of those puzzles.";
		}
		else if (turnNum == 5) {
			introText += "Hey Vivian!<br>That's it. We're done. That's all the items unlocked. Guess we don't need to worry about giving all the stuff to the Art Gallery.";
		}

		introText += "<br>";
		return introText;
	}
	private string GetResponseToLastKs()
	{
		// Get the response to the last item that was kept/shared
		string response = "";

		string prevDoorOpened = GetPrevDoorNum();
		string prevKsAction = GetPrevKsAction();
		if ("0".Equals(prevDoorOpened) || "N".Equals(prevKsAction) ) {
			// Must be the first turn, no response needed.
			return "";
		}

		// Else... The other player did something, comment on it!
		if (prevDoorOpened == "1") {	// Dresses
			if ("K".Equals(prevKsAction) ) {
				if (sisterPlaying == 1) {
					response += "About the cheong sams... Was one of them the red one with the phoenix on it? Mum wore it at their wedding. I wish you would have let me have it. I used to beg her to let me play dress-up in it.";
				}
				else {
					response += "About those cheong sams.... That's fine if you keep them. Just be careful with them. They're family heirlooms. One day I might want my kid to wear them or something.";
				}
			}
			else {
				if (sisterPlaying == 1) {
					response += "Thanks for sharing the red cheong sam. I think I know which one you're talking about.";
				}
				else {
					response += "Where the hell am I gonna store the cheong same you gave me??! Fine. Whatevs.";
				}
			}
		}
		else if (prevDoorOpened == "2") {	// Goblets
			if ("K".Equals(prevKsAction) ) {
				if (sisterPlaying == 1) {
					response += "Yeah, it's fine you kept the goblets. I wouldn't have anywhere nice to display them.";
				}
				else {
					response += "Re goblets. I don't care. Whatever.";
				}
			}
			else {
				if (sisterPlaying == 1) {
					response += "About splitting the goblets up... Thanks ... how ... thoughtful.";
				}
				else {
					response += "Oh, you should have kept both the goblets. It's lame just to have one.";
				}
			}
		}
		else if (prevDoorOpened == "3") {	// Hair Pieces
			if ("K".Equals(prevKsAction) ) {
				if (sisterPlaying == 1) {
					response += "All right about the hair ornaments, but don't take them off in a bathroom and leave them there.  You know you have a habit of ... that.";
				}
				else {
					response += "I wish you'd asked me about those hair ornaments first. I really like wearing those, and you always keep really short hair.";
				}
			}
			else {
				if (sisterPlaying == 1) {
					response += "Thanks for remembering about Dad combing my hair. Those were good times. Sigh.";
				}
				else {
					response += "Thanks for sharing the hair ornaments! I could use some killer accessories.";
				}
			}
		}
		else if (prevDoorOpened == "4") {	// Fans
			if ("K".Equals(prevKsAction) ) {
				if (sisterPlaying == 1) {
					response += "Just because I don't always dress as nicely as you do doesn't mean I don't like girly things. But it's fine, you can keep the fans.";
				}
				else {
					response += "Ha, that's a funny story about the fans. Dad never told me that. Then again, he didn't tell me a lot of things.";
				}
			}
			else {
				if (sisterPlaying == 1) {
					response += "Yeah, I did do a dance with those fans. Thanks. They made us dress up like peaches. I have no idea why.";
				}
				else {
					response += "I'll have to ask you more about the story of the fans next time I see you.";
				}
			}
		}
		else if (prevDoorOpened == "5") {	// Jewellery
			if ("K".Equals(prevKsAction) ) {
				if (sisterPlaying == 1) {
					response += "I like jewellery, I just ... never receive it and don't think to buy it for myself.";
				}
				else {
					response += "I don't care about the cufflinks but I always liked that ring. Now that I think of it, it's more your style. That's cool.";
				}
			}
			else {
				if (sisterPlaying == 1) {
					response += "About the ring... Yeah, thanks. I've always loved it.";
				}
				else {
					response += "Thanks Viv. That means a lot to me that you would remember how much I loved Mum's ring.";
				}
			}
		}
		else if (prevDoorOpened == "6") {	// Photos
			if ("K".Equals(prevKsAction) ) {
				if (sisterPlaying == 1) {
					response += "You might be good at scanning those photos, but try not to take forever sending the files like you usually do. Sometimes you're just like Dad.";
				}
				else {
					response += "Thanks for offering to restore the photos. Don't forget to get a quote first. You don't want someone ripping you off.";
				}
			}
			else {
				if (sisterPlaying == 1) {
					response += "Thanks for the photos. Even though you didn't have a lot of time with Mum, I'm sure you would have been her favourite if she'd lived to see you grow up.";
				}
				else {
					response += "Thanks for the photos. Some of these are of Mum and me and I don't have a lot of those to frame.";
				}
			}
		}
		response += "<br>";

		return response;
	}

	private string GetCommentOnPlayerAction(string keepShare)
	{
		// Send a comment about what the player has just chosen to do
		// Based on what is behind this door
		string comment = "";

		if (doorNum == "1") {	// Dresses
			if ("K".Equals(keepShare) ) {
				if (sisterPlaying == 1) {
					comment += "I found a set of cheong sams. I've got an event coming up where I want to wear them. I tried them on. I don't think they'll fit you.";
				}
				else {
					comment += "I found these two awesome cheong sams. I think I saw Mum wearing them in a photo once. Hope you don't mind.";
				}
			}
			else {
				if (sisterPlaying == 1) {
					comment += "I found a set of cheong sams. I'll take one, and you can have the other one.";
				}
				else {
					comment += "There's two cheong sams in the set, so I gave you the red one. I'm keeping the black one.";
				}
			}
		}
		else if (doorNum == "2") {	// Goblets
			if ("K".Equals(keepShare) ) {
				if (sisterPlaying == 1) {
					comment += "I found the crystal goblets from Mum and Dad's wedding. I didn't think you'd care.";
				}
				else {
					comment += "I found two crystal goblets. I think they're from Mum and Dad's wedding. They'd go really nicely with my collection of champagne flutes.";
				}
			}
			else {
				if (sisterPlaying == 1) {
					comment += "There were two crystal goblets from Mum and Dad's wedding. I thought you'd like to have one.";
				}
				else {
					comment += "I found two crystal goblets. I think they're from Mum and Dad's wedding. If not, probably their anniversary at some point. They're nice. I thought we could split them.";
				}
			}
		}
		else if (doorNum == "3") {	// Hair Ornaments
			if ("K".Equals(keepShare) ) {
				if (sisterPlaying == 1) {
					comment += "I came across these beautiful hair ornaments. They reminded me of dressing up in Mum's old costumes, so I kept them. Hope you don't mind.";
				}
				else {
					comment += "I found these cool hair ornaments. They might have been Mum's. But I'm keeping them since you always wear your hair short.";
				}
			}
			else {
				if (sisterPlaying == 1) {
					comment += "I thought that you'd like to keep some of the hair ornaments I found. They might go really nicely with one of your cocktail dresses that you like for those fancy work dos.";
				}
				else {
					comment += "I found these hair ornaments and I'm sharing them with you. I think I saw a photo of Dad trying to comb your hair with them once. Wasn't there a story about how your hair was so knotted, you'd only let him do it with that one comb?";
				}
			}
		}
		else if (doorNum == "4") {	// Fans
			if ("K".Equals(keepShare) ) {
				if (sisterPlaying == 1) {
					comment += "I found the set of old carved Chinese fans. I'm keeping them because I remember Dad fanning Mum with them on the night you were born, and Mum swearing about the air conditioner breaking down at the worst possible time ever. She was in labour. You can't blame her.";
				}
				else {
					comment += "I found these great Chinese fans. I decided to keep them since they're probably too girly for you.";
				}
			}
			else {
				if (sisterPlaying == 1) {
					comment += "I thought you might like to have one of the Chinese fans I found. Dad used them to fan Mum on the night you were born, one in each hand. The air conditioner broke down and then kicked in just after everyone left for the hospital.";
				}
				else {
					comment += "I saved you one of the Chinese fans. I think I remember you doing a Chinese dance in high school with one of them.";
				}
			}
		}
		else if (doorNum == "5") {	// Jewellery
			if ("K".Equals(keepShare) ) {
				if (sisterPlaying == 1) {
					comment += "I found these cufflinks and wedding ring in a baggie together. I'm guessing that Mum would have given these to Dad on their wedding day. I'm going to keep them. I always loved her ring.";
				}
				else {
					comment += "I found these cufflinks with Mum's wedding ring. You don't really like jewellery, so I figured I'd keep it.";
				}
			}
			else {
				if (sisterPlaying == 1) {
					comment += "I found the cufflinks and wedding ring together in a baggie. I know you always admired the ring in photos, so you can have it.";
				}
				else {
					comment += "I found Mum's wedding ring with Dad's cufflinks. The ring doesn't do anything for me, but maybe you have some sentimental attachment to it.";
				}
			}
		}
		else if (doorNum == "6") {	// Photos
			if ("K".Equals(keepShare) ) {
				if (sisterPlaying == 1) {
					comment += "I found a box of old photos. The quality isn't really great. I'll get them restored.";
				}
				else {
					comment += "I found a box of old photos. I'm keeping them because I'll be better at scanning them for the both of us.";
				}
			}
			else {
				if (sisterPlaying == 1) {
					comment += "I found a box of loose old photos. I just took a couple that I really wanted. I'm sure Dad has them organized in albums somewhere. ";
				}
				else {
					comment += "I found this old box of photos. I don't really have any memories of Mum, so I thought you would prefer to keep them.";
				}
			}
		}
		comment += "<br>";
		return comment;
	}
	private string GetOverviewPoemComment()
	{
		// Sometimes the player finds a new hint note, if so, comment about it

		string comment = "";

		if (doorNum == "1") {	// Dresses
			// Found hint about hair pieces
			if (sisterPlaying == 1) {
				comment += "Posted note about hair pieces. Dad's not the type to leave us wigs ... or at least, I hope. Gross.";
			}
			else {
				comment += "I found a hint about something that Mum might have worn in her hair. You'll see my note on the locker.";
			}
		}
		else if (doorNum == "2") {	// Goblets
			// Found hint about fans
			if (sisterPlaying == 1) {
				comment += "Found something about Chinese fans. I hope they're pretty. Left note for you.";
			}
			else {
				comment += "You'll see a note on a locker about Chinese fans. I hope it's not lame Chinese bobblehead dolls holding flags. ";
			}
		}
		else if (doorNum == "3") {	// Hair Pieces
			// Found hint about photos
			if (sisterPlaying == 1) {
				comment += "Got something about photos. Posted note for you.";
			}
			else {
				comment += "So photos are a part of this treasure hunt. What of, I dunno. Left you a note.";
			}
		}
		else if (doorNum == "4") {	// Fans
			// Found hint about jewellery
			if (sisterPlaying == 1) {
				comment += "Got a hint about jewellery. Hoping it's what I think it is, but can't tell. You'll see my note.";
			}
			else {
				comment += "Found hint about jewellery, but I have several ideas as to what it is. Left you a note.";
			}
		}

		comment += "<br>";
		return comment;
	}
	private string GetSharingProgress(string keepShare) {
		// Returns a comment about what's been going on with the keeping/sharing situation
		string comment = "";

		int turnNum = FindTurnNum();

		if (turnNum == 0) {	// Vivian's turn
			if ("K".Equals(keepShare)) {
				comment += "Dad pretty much raised us as champion scavenger hunters, so I'm hoping it's awesome.";
			}
			else {
				comment += "So far, so good! I guess if this was Dad's last scavenger hunt, he had to make it the best ever.";
			}
		}
		else if (turnNum == 1) { // Denise's turn
			// Find out what happened on the first turn
			string lastKS = ""+originalKeepShareCode[1];
			if (keepShare.Equals(lastKS)) {	// Both sisters doing the same thing
				comment += "Hopefully there's enough stuff for both of us. Meh, we'll figure it out and see how it goes. I'll go with whatever you're doing.";
			}
			else if ("K".Equals(keepShare)) {
				// Keeping when the other sister shared
				comment += "I know you shared, but I think maybe Dad made it a race, so it's like finders, keepers.";
			}
			else {	// Must be sharing when other kept
				comment += "I know you kept what you found, but I think maybe it's the ultimate sharing is caring test.";
			}
		}
		else if (turnNum == 2) { // Vivian's turn
			if ("K".Equals(keepShare)) {
				// Vivian is keeping, she doesn't care...
				comment += "Most of these things seem to have more sentimental value to me than you.";
			}
			else { // Something more complex happening
				// Vivian chose to share this time, what did she do last time?
				string viv1stKS = ""+originalKeepShareCode[0];
				if ("K".Equals(viv1stKS)) {
					// Kept first time, now sharing
					comment += "You know, I've been keeping a lot of stuff, but I'll try to make a more concerted effort to share what I think you'd like.";
				}
				else {
					// Find out what Denise chose to do last time
					string den1stKS = ""+originalKeepShareCode[1];
					if ("S".Equals(den1stKS)) {	// Denis has been sharing too
						comment += "I appreciate how considerate you've been in sharing the stuff. I'll try to do the same.";
					}
					else {	// Denise kept what she found
						comment += "Um, I don't really know how to say this better, but I kind of thought you'd be sharing more stuff. What's the deal?";
					}
				}
			}
		}
		else if (turnNum == 3) { // Denise's Turn
			int numTotK = CountKS("K","12", keepShare);
			int numVK = CountKS("K","1", keepShare);
			int numDK = CountKS("K","2", keepShare);

			if (numTotK == 4) {	// Kept every time
				comment += "It seems like a shame to break up the pairs of stuff. Let's just keep the stuff we want. No big deal.";
			}
			else if (numTotK == 0) { // Shared every time
				comment += "I like that we each get something if we share. Then it's more to remember Dad by.";
			}
			else if (numVK == 2 && numDK == 0) { // V all K and D all S
				comment += "You've been keeping a lot of stuff, and while there are some days I'm okay with it, there are others when I can't help but think it's a bit selfish. I've totally been sharing with you.";
			}
			else if (numDK == 2) {	// D all K
				comment += "I've been keeping stuff because I feel like I need to have more tangible reminders of Dad. You saw him all the time, so I imagine stuff isn't as important to you.";
			}
			else {
				// Check what Vivian did last time
				string viv2ndKS = ""+originalKeepShareCode[2];
				if ("K".Equals(viv2ndKS) && "K".Equals(keepShare)) {	// Viv kept last time and Denise shared
					comment += "I figured since you kept the last thing, I'm gonna keep this one for myself.";
				}
				else if ("S".Equals(viv2ndKS) && "S".Equals(keepShare)) {	// Viv shared last time and Denise shared
					comment += "Thanks for sharing, I figured I would do the same.";
				}
				else if (numDK==1 && "K".Equals(keepShare)) {	// It was all sharing until now
					comment += "I know we've been sharing a lot lately, but I really love this one, so I'm gonna keep it. Hope that's okay.";
				}
				else {	// Some weird mix
					comment += "I guess it depends on what it is that'll determine whether we wanna keep something or not.";
				}
			}
		}
		else if (turnNum == 4) { // Vivian's Turn
			int numTotK = CountKS("K","12", keepShare);
			int numVK = CountKS("K","1", keepShare);
			int numDK = CountKS("K","2", keepShare);

			if (numTotK == 5) {	// All keeping
				comment += "I'm sorry I've been keeping everything I found. I hope you'll look past the stuff. It's nothing to do with you. He must have known that these things would trigger all these memories for me, and I'm having a really hard time letting go.";
			}
			else if (numTotK == 0) { // All sharing
				comment += "I like sharing with you and it doesn't matter if you reciprocate or not. It's not like I'll love you any less. You're my sister. ";
			}
			else if (numVK==2 && "S".Equals(keepShare)) {	// V kept all other times, but this time sharing
				comment += "I know I kept the last two things. I don't want you to think I'm being selfish, so I'm sharing this one with you.  It's not worth much but I think you'll appreciate the sentimental value as I do.";
			}
			else if (numDK==2) {	// D has been keeping everything
				comment += "I know you've been keeping everything, and I'm okay with it. But you know you have a bit of a baby sister entitlement complex, right?";
			}
			else if ("S".Equals(keepShare)) {	// V just shared that time
				comment += "I think both of us really needed to share this one. It's too special to keep to oneself.";
			}
			else if ("K".Equals(keepShare)) {	// V just kept this time
				comment += "I really want this one. Sorry.  Tough.";
			}
		}
		else if (turnNum == 5) { // Denise's Turn. LAST TURN
			int numTotK = CountKS("K","12", keepShare);
			int numVK = CountKS("K","1", keepShare);
			int numDK = CountKS("K","2", keepShare);

			if (numTotK==6) {	// Everyone kept everything HATE
				comment += "I say this because you're my sister and everyone else is too polite to say it, but sometimes you can be such a domineering, selfish cow.";
			}
			else if (numTotK==0) { // Everyone shared everything LOVE
				comment += "I think that worked out really nicely. I'm glad we didn't have any fights over anything. That would have been awful.";
			}
			else if (numVK > numDK) {	// V kept more than D SPLIT
				comment += "I can't believe you would keep all that stuff for yourself. And if you're thinking, 'You should have said something', well, I'm saying it now. You can't keep EVERYTHING.";
			}
			else if (numVK < numDK) { // D kept more than V Uneven
				comment += "I just kept what I really wanted, and I'm sure you would have said something instead of just getting all grumpy about it by yourself.";
			}
			else if (numDK == numVK) {	// Even keep/share L/H
				comment += "I'm sure it worked out for the best and we're keeping stuff in the family at the very least.";
			}
		}
		
		comment += "<br>";
		return comment;
	}
	private string GetEndingsProgress(string keepShare) {
		// Returns a comment based on which endings are no longer possible
		string comment = "";

		int turnNum = FindTurnNum();

		int numTotK = CountKS("K","12", keepShare);
		int numVK = CountKS("K","1", keepShare);
		int numDK = CountKS("K","2", keepShare);

		if (turnNum == 0) {	// Vivian's turn
			if (numTotK==1) {	// Keep
				comment += "I think we should be practical and share the things that warrant sharing.";
			}
			else {	// Share
				comment += "I'd be really touched if you thought of whether I'd like items when deciding to keep or share stuff.";
			}
		}
		else if (turnNum == 1) { // Denise's turn
			if (numTotK==2) {	// All Keep
				comment += "So to be practical, I guess we keep the stuff we really really want?";
			}
			else if (numTotK==0){	// All Share
				comment += "I like how this is going.";
			}
			else {	// Mix
				comment += "On to the next item ...";
			}
		}
		else if (turnNum == 2) { // Vivian's turn
			if (numTotK==3) {	// All Keep
				comment += "I think Dad designed a really good scavenger hunt, probably knowing that we'd find the items that meant the most to each of us.";
			}
			else if (numTotK==0){	// All Share
				comment += "I have to say that I was a bit worried at first about how this would go, but it's been a great trip down memory lane with you so far.";
			}
			else {	// Mix
				comment += "That's all. Talk to you later.";
			}
		}
		else if (turnNum == 3) { // Denise's turn
			if (numTotK==4) {	// All Keep
				comment += "I think if Dad wanted us to share everything, he would have just said so and split everything down the middle.";
			}
			else if (numTotK==0){	// All Share
				comment += "I know we've had our differences, but I'm glad we're in this together.";
			}
			else if (numVK == 2 && numDK == 0) { // V has been keeping way more than D
				comment += "Look, I know you like being a sentimental packrat sometimes, but this stuff doesn't actually properly belong to you, they're FAMILY belongings that you're supposed to share with your FAMILY.";
			}
			else if (numDK == 2 && numVK == 0) { // D has been keeping way more than V
				comment += "I'm surprised you're not putting up more of a fight for stuff, but I really like what I'm finding to keep.";
			}
			else {	// Mix
				comment += "That's me done. Chat soon.";
			}
		}
		else if (turnNum == 4) { // Vivian's turn
			if (numTotK==5) {	// All Keep
				comment += "I'm glad we're seeing eye-to-eye on what we'd like to keep.";
			}
			else if (numTotK==0){	// All Share
				comment += "I guess what you've been finding has meant a lot to you. I figured, so that's why I've been sharing what I've found too. Maybe it gives us both a better sense of identity and family history.";
			}
			else if ( numVK > (numDK+1) ) {	// V has been keeping more than D
				comment += "You know, I just spent so much time with Dad in his last days, and while you went away for school, and when you did that year abroad, and before you were born ... I just think the stuff means more to me than to you.";
			}
			else if ( numDK > numVK) {// D has been keeping way more than V
				comment += "I guess if the stuff is what you need to cope ... everybody has their vice.";
			}
			else {	// Mix
				comment += "That's all. Talk to you later.";
			}
		}
		else if (turnNum == 5) { // Denise's turn
			if (numTotK==6) {	// Everyone kept everything HATE
				comment += "I also know that you won't share anything because you think I won't take care of it. That's not true, and I know you're aware that you're lying to yourself when you come up with that crap.";
			}
			else if (numTotK==0) { // Everyone shared everything LOVE
				comment += "I don't say it very often, but I love you a lot and I always will.";
			}
			else if (numVK > numDK) {	// V kept more than D SPLIT
				comment += "If you think this is like some kind of bogus reward plan for taking care of Dad, I can't do any more for you than thank you the thousand times I have already. I can't feel guilty for living my life the way I have, it's just the nature of my career. We all make choices. And if you're going to hold this over my head for the rest of my life, then I just can't be around you.";
			}
			else if (numVK < numDK) { // D kept more than V Uneven
				comment += "If we were in opposite places, I'm sure I would have gotten really grumpy but I would have let you have it. Thanks for being so cool about me keeping as much as I did. This whole thing has been kind of cathartic. I blame it on Mum's collector/retail therapy genes.";
			}
			else if (numDK == numVK) {	// Even keep/share L/H
				comment += "It's a pity that we're on such opposite schedules and live so far apart. Things would definitely be different.";
			}
		}

		comment += "<br>";
		return comment;
	}
	private string CreateCode(string keepShare)
	{
		// Works out what the new code will be for the player

		// Find out what turn number we're up to
		int turn = FindTurnNum();

		// Now we just need to add to it

		// Find out which door they unlocked this time.
		string doorUnlocked = PlayerPrefs.GetString("UnlockedThisTime");
		string newFound = originalFoundCode.Substring(0,turn)+doorUnlocked[0]+originalFoundCode.Substring(turn+1);
	
		// Check to see if they kept or shared, based on which button this is that they clicked
		string newKeepShare = originalKeepShareCode.Substring(0,turn)+keepShare+originalKeepShareCode.Substring(turn+1);

		// Encypt the code
		byte[] bytesToEncode = Encoding.UTF8.GetBytes(newFound+newKeepShare);
		string encodedText = Convert.ToBase64String (bytesToEncode);

		return encodedText;
	}

	public int FindTurnNum()
	{
		for (int i=0; i<originalFoundCode.Length; i++) {
			if (originalFoundCode[i] == '0') {
				return i;
			}
		}
		// If got here didn't find "0", must be an error
		return -1;

	}

	public int FindSisterPlayingNow()
	{
		// Return 1 for Vivian
		// Return 2 for Denise

		// Start by finding out which turn we are on
		int turnNum = FindTurnNum();
		if ( turnNum == 0 || turnNum == 2 || turnNum == 4) {
			// Vivian's turn
			return 1;
		}
		else if ( turnNum == 1 || turnNum == 3 || turnNum == 5) {
			// Denise's turn
			return 2;
		}
		// Error to get here!
		return -1;
	}
	public string NameSisterPlayingNow()
	{
		int sisNum = FindSisterPlayingNow();
		if (sisNum == 1) {
			return "Vivian";
		}
		else if (sisNum == 2) {
			return "Denise";
		}
		return "";	// Empty string, some error happening!
	}
	private string GetPrevDoorNum()
	{
		// Returns the number of the last door that was opened before this player's turn
		int currTurn = FindTurnNum();
		if ( currTurn != 0 ) {
			return ""+originalFoundCode[currTurn-1];
		}
		return "0";
	}

	private string GetPrevKsAction()
	{
		// Finds out what the last player did (ie keep/share)
		int currTurn = FindTurnNum();
		if ( currTurn != 0 ) {
			return ""+originalKeepShareCode[currTurn-1];
		}
		return "N";
	}
	private int CountKS(string ksType, string sisterToCheck, string latestResponse)
	{
		// Returns a count of the number of "ksType" that sisterToCheck
		//  has chosen so far
		// ksType = "K" or "S"
		// sisterToCheck = "1", "2" or "12" where "12" means both sisters.
	
		int finalCount = 0;
		// Count up what happened previously
		for (int i=0; i<originalKeepShareCode.Length; i++) {
			string currKS = ""+originalKeepShareCode[i];
			if ("12".Equals(sisterToCheck)) {	// Don't care which sister
				if (currKS.Equals(ksType)) {
					finalCount++;
				}
			}
			else if ("1".Equals(sisterToCheck)) { // Just sister 1
				if (i==0 || i==2 || i==4) {
					if (currKS.Equals(ksType)) {
						finalCount++;
					}
				}
			}
			else if ("2".Equals(sisterToCheck)) { // Just sister 2
				if (i==1 || i==3 || i==5) {
					if (currKS.Equals(ksType)) {
						finalCount++;
					}
				}
			}
		}

		// Add in what has just happened if necessary
		if (sisterPlaying.Equals(sisterToCheck) || "12".Equals(sisterToCheck)) {
			if (latestResponse.Equals(ksType)) {
				finalCount++;
			}
		}

		return finalCount;
	}
	
}
