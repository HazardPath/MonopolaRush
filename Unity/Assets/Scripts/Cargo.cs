using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// This attaches itself to prefabs. The prefabs that get created, therefore, create cargo that goes with them.
public class Cargo : MonoBehaviour{
	// Unique identifier for this type of cargo. Cargo with the same ID will be lumped together in the compiled view.
	public string ID;

	// Position of the piece of cargo in the WORLD.
	public Vector3 pos;

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

	// Use this for initialization
	void Start () {
		Log ("ok");
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Hold") {
			holdHitList.Add (coll.gameObject);
			Log ("HI");
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
			Vector3 difference = hitHold.offset - transform.position;
			//Multiply by 100 to get pixels, then divide by 32 to get grid units
			difference = difference * 100.0f / 32.0f;
			//Floor to nearest grid space
			difference.x = Mathf.Floor(difference.x);
			difference.y = Mathf.Floor(difference.y);
			//Note this for later
			Vector3 offsetInGrid = new Vector3(difference.x, difference.y, difference.z);
			//Multiply by 32 to get pixels, then divide by 100 to get world units
			difference = difference * 32.0f / 100.0f;
			//Add the difference to the offset, and set it as my position, and fudge to clean up
			transform.position = -1*(hitHold.offset + difference);
			transform.position += new Vector3(-0.16f, -0.16f);
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
		}
    }

	void GoHome(){
		transform.position = pos;
	}

	void Log(string txt){
		GameObject.FindWithTag("Console").GetComponent<Text>().text=txt;
	}
}
