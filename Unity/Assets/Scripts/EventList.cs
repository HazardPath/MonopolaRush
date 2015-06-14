using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EventList{
	public static List<EventInterface> allEvents;
	
	static EventList() {
		allEvents = new List<EventInterface>();
		allEvents.Add(new NullEvent());
	}
	
	public static void eventGenerator(PlayerShip ship) {
		float chancesum = 0.0f;
		int eventindex = 0;
		List<EventInterface> candidateEvents = new List<EventInterface> ();
		foreach (EventInterface cur in allEvents) {
			float curchance = cur.chanceToHappen(ship);
			if (curchance > 0) {
				chancesum += curchance;
				candidateEvents.Add(cur);
			}
		}
		float chance = Random.value * chancesum;
		while (chance > 0 && eventindex < (candidateEvents.Count - 1)) {
			chance -= candidateEvents[eventindex].chanceToHappen(ship);
			eventindex ++;
		}
		candidateEvents [eventindex].eventExecute (ship);
	}
}
