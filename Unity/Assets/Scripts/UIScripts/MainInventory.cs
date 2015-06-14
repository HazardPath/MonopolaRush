using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainInventory : MonoBehaviour {

	// The ship.
	private PlayerShip ship;

	// What we draw with.
	private SpriteRenderer spriteRenderer;

	// An empty square of the grid.
	private GameObject emptySquare;

	// A list of all our squares, just to keep track of them.
	private List<Object> grid = new List<Object>();

	public Vector3 offset;

	// Use this for initialization
	void Start () {
		ship = GameObject.FindWithTag ("Player Ship").GetComponent<PlayerShip> ();
		spriteRenderer = GetComponent<SpriteRenderer>(); // we are accessing the SpriteRenderer that is attached to the Gameobject
		emptySquare = Resources.Load<GameObject> ("gridSquare");

		// Draw all of the grid squares
		for(int i=0; i<ship.MAIN_HOLD_SIZE; i++){
			for(int j=0; j<ship.MAIN_HOLD_SIZE; j++){
				grid.Add(Instantiate(emptySquare, offset+(2*new Vector3(i,j,0)/7), Quaternion.identity));
			}
		}

		Update ();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
