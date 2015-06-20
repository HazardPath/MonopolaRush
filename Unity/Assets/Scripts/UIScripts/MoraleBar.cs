using UnityEngine;
using System.Collections;

public class MoraleBar : MonoBehaviour {

	private PlayerShip ship;

	private float MORALE_SCALE = 11.0f/100.0f;

	// Use this for initialization
	void Start () {
		ship = PlayerShip.FindObjectOfType<PlayerShip> ();
		Update ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 scale = transform.localScale;
		scale.x = ship.crewMorale * MORALE_SCALE;
		transform.localScale = scale;
	}
}
