using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerShip : MonoBehaviour {

	/*
	 * Constants
	 */

	// Size of the holds. Holds are assumed to be square.
	public static readonly int MAIN_HOLD_SIZE = 8;
	public static readonly int HIDDEN_HOLD_SIZE = 4;
	public static readonly int TEMP_HOLD_SIZE = 16;

	// Conversion of in-game locations and distances to UI locations and distances.
	// Distance traveled (across the UI) every jump, in units.
	public static readonly float UNITS_PER_JUMP = .206f;
	// Location of home in UI space.
	public static readonly float HOME_IN_UNITS = -2.38f;
	// Location of mining spot in UI space.
	public static readonly float MOTHERLOAD_IN_UNITS = 3.81f;

	/*
	 * Independent variables
	 */

	// List of everything in inventory. These are referenced elsewhere in code; do NOT assign new ones.
	public List<Cargo> mainHold = new List<Cargo>();
	public List<Cargo> hiddenHold = new List<Cargo>();
	public List<Cargo> tempHold = new List<Cargo>();

	// List of crew on ship - if inactive crew is a thing, they'll be in cargo, not in here.
	public Crew[] crew = new Crew[6]{null, null, null, null, null, null};

	// Current distance from home as marked on the screen.
	private float _distance = HOME_IN_UNITS;
	public float distance {
		get { return _distance; }
		set { _distance = value; }
	}

	// Amount of fuel used for each jump
	public int fuelPerJump = 2;
	
	// Current morale of the ship. Percentage.
	public float crewMorale = 100.0f;
	
	// A list of tags about the ship.
	public List<Tags> tags;

	/*
	 * Depndent variables
	 */

	// Compiled list of inventory - the above inventory compiled into human readable things. The key is <Cargo>.ID
	public Dictionary<Cargotypes, int> inv = new Dictionary<Cargotypes, int>();

	// How many people are on board (active crew or otherwise)
	public int mouthsToFeed = 0;

	public int numActive(Jobs job) {
		int result = 0;
		foreach (Crew cur in crew) {
			if (cur.job == job)
				result ++;
		}
		return result;
	}

	/*
	 * Internal variables
	 */

	// What we draw with.
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>(); // we are accessing the SpriteRenderer that is attached to the Gameobject
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI() {
		if (GUI.Button(new Rect(10, 10, 150, 100), Time.time+""))
			print("You clicked the button!");
		
	}

	// Updates all GUI elements and dependent variables (like `inv`) based on independent variables (like the cargo lists).
	// Guaranteed to have no side effects, and can be called frequently without breaking the game. Calling Recompile() if
	// no changes have been made since the last recompile should make no changes to the world.
	public void Recompile () {
		// Update the inv dictionary & mouthsToFeed
		inv.Clear ();
		mouthsToFeed = 0;

		List<Cargo> allCargo = new List<Cargo>(mainHold);
		if(hiddenHold!=null) allCargo.AddRange (hiddenHold);
		foreach (Cargo cur in allCargo) {
			// Add to inventory
			if(inv.ContainsKey(cur.ID)){
				inv[cur.ID] += cur.quantity;
			}else{
				inv[cur.ID] = cur.quantity;
			}

			//If it eats, add it to the number of mouths to feed
			if(cur.tags.Contains(Tags.hungry)) mouthsToFeed++;
		}
		foreach (Crew cur in crew) {
			if(cur!=null) mouthsToFeed++;
		}
	}

	// Checks if $amount of $string are onboard. If so, decreases available stores by $amount and returns true. If not, returns false.
	public bool consumeSupplies(Cargotypes ID, int amount) {
		//Check cache tosee if ship has enough
		if (inv [ID] < amount)
			return false;
		else {
			// Making a list of every stack of Cargo<ID>
			int amountRemaining = amount;
			foreach (List<Cargo> hold in new List<Cargo>[]{mainHold, hiddenHold}) {
				List<Cargo> allSupplies = new List<Cargo>();
				foreach(Cargo cur in hold) {
					if (cur.ID == ID)	allSupplies.Add(cur);
				}
				allSupplies.Sort((e1, e2) => e1.quantity - e2.quantity);
				while (amountRemaining > 0 && allSupplies.Count > 0) {
					if (allSupplies[0].quantity > amountRemaining) allSupplies[0].quantity -= amountRemaining;
					else if (allSupplies[0].quantity == amountRemaining) allSupplies.RemoveAt(0);
					else {
						amountRemaining -= allSupplies[0].quantity;
						allSupplies.RemoveAt(0);
					}
				}
			}

			Recompile();
			return true;
		}
	}

	// int jumpDirection positive = towards motherload, negative = towards home, 0 = waiting in place.
	public void jump(int jumpDirection) {
		//decrease food by this.mouthsToFeed;
		if (!consumeSupplies(Cargotypes.food, mouthsToFeed)) {
			int shortfall = mouthsToFeed - inv[Cargotypes.food];
			Recompile ();
			crewMorale -= 12.5f * shortfall;
			if (!tags.Contains(Tags.outoffood)) tags.Add(Tags.outoffood);
		}
		if (!tags.Contains(Tags.broken) && jumpDirection != 0) {
			//decrease fuel by this.fuelPerJumps;
			if (consumeSupplies(Cargotypes.fuel, fuelPerJump)) {
				if (jumpDirection > 0) distance += UNITS_PER_JUMP;
				else distance -= UNITS_PER_JUMP;
			}
			else if (!tags.Contains(Tags.outoffuel)) tags.Add(Tags.outoffuel);
		}
		//call event generator
		EventList.eventGenerator (this);
	}

	void ChangeSprite (Sprite newSprite) {
		spriteRenderer.sprite = newSprite;
	}
}