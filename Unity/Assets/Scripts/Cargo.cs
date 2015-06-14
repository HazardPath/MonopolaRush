using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This attaches itself to prefabs. The prefabs that get created, therefore, create cargo that goes with them.
public class Cargo : MonoBehaviour{
	// Unique identifier for this type of cargo. Cargo with the same ID will be lumped together in the compiled view.
	public string ID;

	// Position of the piece of cargo in the hold.
	public Vector2 pos;

	// Size of the piece of cargo when in the hold.
	public Vector2 size;

	// Numbers of items in the current stack
	public int quantity;

	// Maximum number of items in the stack
	public int maxQuantity;

	// A list of tags about this specific piece of cargo.
	public List<Tags> tags;

	// Who is my daddy?
	public readonly GameObject parent;

	// Use this for initialization
	void Start () {
		parent = GetComponent<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
