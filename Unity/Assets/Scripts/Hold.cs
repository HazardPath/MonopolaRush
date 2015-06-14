using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hold : MonoBehaviour {

	// The ship.
	private PlayerShip ship;

	// What we draw with.
	private SpriteRenderer spriteRenderer;

	// An empty square of the grid.
	private GameObject emptySquare;

	// A list of all our squares, just to keep track of them.
	private List<Object> grid = new List<Object>();

	public Vector3 offset;

	public HoldType holdType;

	public int holdSize;

	private List<Cargo> hold;

	// Use this for initialization
	void Start () {
		tag = "Hold";
		offset /= 100;

		ship = GameObject.FindWithTag ("Player Ship").GetComponent<PlayerShip> ();
		spriteRenderer = GetComponent<SpriteRenderer>(); // we are accessing the SpriteRenderer that is attached to the Gameobject
		emptySquare = Resources.Load<GameObject> ("gridSquare");

		switch (holdType) {
		case HoldType.mainHold:
			holdSize = ship.MAIN_HOLD_SIZE;
			hold = ship.mainHold;
			break;
		case HoldType.hiddenHold:
			holdSize = ship.HIDDEN_HOLD_SIZE;
			hold = ship.hiddenHold;
			break;
		case HoldType.temporaryHold:
			holdSize = ship.TEMP_HOLD_SIZE;
			hold = ship.tempHold;
			break;
		}

		// Draw all of the grid squares
		for(int i=0; i<holdSize; i++){
			for(int j=0; j<holdSize; j++){
				//Why 32/100? The default pixels per unit is 100, and each square is 32 pixels.
				grid.Add(Instantiate(emptySquare, offset+(32*new Vector3(i,j,0)/100), Quaternion.identity));
			}
		}

		BoxCollider2D collide = GetComponent<BoxCollider2D>();
		collide.size = new Vector2 (holdSize * .32f, holdSize * .32f);
		Vector3 halfBoxSize = new Vector3 ((collide.size / 2).x-.16f, (collide.size / 2).y-.16f, 0);
		collide.transform.position = offset+halfBoxSize;
		//collide.offset = offset;

		Update ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	// Adds that cargo to this hold. Does not move the cargo, just adds ownership.
	public void AddCargo(Cargo c){
		hold.Add (c);
		c.hold = this;
	}

	// Removes that cargo from this hold. Does not delete the cargo, just remove ownership.
	public void RemoveCargo(Cargo toremove){
		hold.Remove (toremove);
		toremove.hold = null;
	}
	
	// Given an x,y in grid coordinates, returns the position vector in world coordinates to place the cargo.
	// Then there's a bunch of helper methods below.
	public Vector3 PlaceCargo(int x, int y){
		return offset + new Vector3 (x*.32f, y*.32f, 0);
	}
	
	public Vector3 PlaceCargo(Vector3 gridPos){
		return PlaceCargo ((int)Mathf.Floor(gridPos.x), (int)Mathf.Floor(gridPos.y));
	}
	
	public Vector3 PlaceCargo(Vector2 gridPos){
		return PlaceCargo ((int)Mathf.Floor(gridPos.x), (int)Mathf.Floor(gridPos.y));
	}
}

public enum HoldType{
	mainHold,
	hiddenHold,
	temporaryHold
}