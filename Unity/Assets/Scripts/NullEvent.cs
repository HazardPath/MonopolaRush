using UnityEngine;
using System.Collections;

public class NullEvent : EventInterface {
	public void eventExecute(PlayerShip ship) {

	}
	public float chanceToHappen(PlayerShip ship) {
		return .5f;
	}
}
