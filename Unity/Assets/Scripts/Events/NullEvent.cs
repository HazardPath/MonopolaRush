using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using System.IO;

public class NullEvent : EventInterface {

	public NullEvent(PlayerShip ship, params string[] filenames) : base(ship, filenames)
	{
	}

	public override void eventExecute(PlayerShip ship) {

	}

	public override float chanceToHappen(PlayerShip ship) {
		return .5f;
	}
}
