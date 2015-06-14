using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConsumableTracker : MonoBehaviour {

	public string prefix;
	public Cargotypes whatITrack;

	private Text myTextBox;
	private PlayerShip ship;

	// Use this for initialization
	void Start () {
		myTextBox = GetComponent<Text>();
		ship = GameObject.FindWithTag ("Player Ship").GetComponent<PlayerShip> ();
		Update ();
	}
	
	// Update is called once per frame
	void Update () {
		if (ship.inv.ContainsKey (whatITrack)) {
			myTextBox.text = prefix + ship.inv [whatITrack];
		} else {
			myTextBox.text = prefix + "0";
		}
	}
}
