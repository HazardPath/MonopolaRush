using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// This attaches itself to prefabs. The prefabs that get created, therefore, create cargo that goes with them.
public class Cargo : MonoBehaviour{
	// Unique identifier for this type of cargo. Cargo with the same ID will be lumped together in the compiled view.
	public Cargotypes ID;

	// Position of the piece of cargo in the WORLD. Can be initially set manually, or by the Hold.PlaceCargo() function.
	public Vector3 pos = Vector3.zero;

	// Numbers of items in the current stack
	public int quantity;

	// Maximum number of items in the stack
	public int maxQuantity;

	// A list of tags about this specific piece of cargo.
	public List<Tags> tags;

	// What hold am I in?
	public Hold hold;

	// List of every hold I'm touching.
	private List<GameObject> holdHitList = new List<GameObject>();

	// List of every non-hold I'm touching.
	private List<GameObject> hitList = new List<GameObject>();

	// How big am I in grid units? This is determined by the offset of the hitbox.
	private Vector2 size;

	// Use this for initialization
	void Start () {
		BoxCollider2D collide = GetComponent<BoxCollider2D>();
		//This is kind of magic numbers.
		size = collide.offset / 0.16f;
		size.y *= -1;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Hold") {
			holdHitList.Add (coll.gameObject);
		} else {
			hitList.Add (coll.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		if (coll.gameObject.tag == "Hold") {
			holdHitList.Remove (coll.gameObject);
		} else {
			hitList.Remove (coll.gameObject);
		}
	}

	void OnMouseDown(){
		//I don't think I care.
	}

    void OnMouseDrag(){
        //You still got me, so I should probably draw myself centered where the mouse is.
		transform.position = new Vector3 (((Input.mousePosition.x-5)/100)-4, ((Input.mousePosition.y+5)/100)-3, transform.position.z);
    }

    void OnMouseUp(){
        //Alright, you dropped me here. Now I gotta do a lot of work.
		if (holdHitList.Count > 0) {
			Hold hitHold = holdHitList[0].GetComponent<Hold>();
			//Snap this to the grid of that hold.
			//Delta from the corner of the hold, in world units
			Vector3 difference = transform.position - hitHold.offset;
			//Multiply by 100 to get pixels, then divide by 32 to get grid units
			difference = difference * 100.0f / 32.0f;
			//Add rounding buffer
			difference += new Vector3(0.16f, 0.16f);
			//Floor to nearest grid space
			difference.x = Mathf.Ceil(difference.x);
			difference.y = Mathf.Ceil(difference.y);
			//Check if you're fully inside the hold here, cuz it's easy in these units
			if(difference.x<0 || difference.x+size.x>hitHold.holdSize || difference.y-size.y<0 || difference.y>hitHold.holdSize){
				GoHome();
				return;
			}
			//Multiply by 32 to get pixels, then divide by 100 to get world units
			difference = difference * 32.0f / 100.0f;
			//Add the difference to the offset, and set it as my position, and fudge to clean up
			transform.position = hitHold.offset + difference;
			transform.position += new Vector3(-0.16f, -0.16f); //this is largely magic just don't worry about it
			//Now check if you're hitting any other cargo.
			bool hittingSomething = false;
			foreach(GameObject obj in hitList){
				if(obj.GetComponent<BoxCollider2D>().IsTouching(GetComponent<BoxCollider2D>())){
					hittingSomething = true;
					break;
				}
			}
			if(hittingSomething){
				GoHome();
				return;
			}else{
				//I just rounded to the grid and I'm not hitting anything. I should stop here.
				//Make this my new home
				pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
				//Remove myself from the old hold
				if(hold!=null) hold.RemoveCargo(this);
				//Add myself to the new hold
				hitHold.AddCargo(this);
				//Recompile the world
				PlayerShip.FindObjectOfType<PlayerShip>().Recompile();
			}
		} else {
			GoHome();
			return;
		}
    }

	void GoHome(){
		//kludge
		pos.z = -2.5f;
		transform.position = pos;
	}
}
