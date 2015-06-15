using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour {

	private PlayerShip ship;

	// Use this for initialization
	void Start () {
		ship = PlayerShip.FindObjectOfType<PlayerShip> ();
		Update ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (ship.distance, transform.position.y, transform.position.z);
	}
}
