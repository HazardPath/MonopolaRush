using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShip : MonoBehaviour {

	/*
	 * Constants
	 */

	// Size of the holds. Holds are assumed to be square.
	public readonly int MAIN_HOLD_SIZE = 8;
	public readonly int HIDDEN_HOLD_SIZE = 4;

	/*
	 * Independent variables
	 */

	// List of everything in inventory.
	public List<Cargo> mainHold = new List<Cargo>();
	public List<Cargo> hiddenHold = new List<Cargo>();

	// List of crew on ship - if inactive crew is a thing, they'll be in cargo, not in here.
	public Crew[] crew = new Crew[4];
	
	// Current number of jumps away from home
	public int distance = 0;
	
	// Is the ship armed?
	public bool isArmed = false;
	
	// Does the ship have better manuverability than normal?
	public bool isNimble = false;
	
	// Amount of fuel used for each jump
	public int fuelPerJump = 2;
	
	// Current morale of the ship. Percentage.
	public double crewMorale = 100.0;

	/*
	 * Depndent variables
	 */

	// Compiled list of inventory - the above inventory compiled into human readable things. The key is <Cargo>.ID
	public Dictionary<string, int> inv = new Dictionary<string, int>();

	// How many people are on board (active crew or otherwise)
	public int mouthsToFeed = 0;

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

	// Updates all GUI elements and dependent variables (like `inv`) based on independent variables (like the cargo lists).
	// Guaranteed to have no side effects, and can be called frequently without breaking the game. Calling Recompile() if
	// no changes have been made since the last recompile should make no changes to the world.
	void Recompile () {
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

	void ChangeSprite (Sprite newSprite) {
		spriteRenderer.sprite = newSprite;
	}
}
