using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface EventInterface {
	void eventExecute(PlayerShip ship);
	float chanceToHappen(PlayerShip ship);
}