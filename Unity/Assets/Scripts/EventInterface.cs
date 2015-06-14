using UnityEngine;
using System.Collections;

interface EventInterface {
	void eventExecute(PlayerShip ship);
	float chanceToHappen(PlayerShip ship);
}
