using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using System.IO;

public abstract class EventInterface {
	List<Dialogue> dialogues;

	public abstract void eventExecute(PlayerShip ship);
	public abstract float chanceToHappen(PlayerShip ship);

	public EventInterface(PlayerShip ship, params string[] filenames)
	{
		// Xml processor.
		XmlSerializer xmlLevelReader;
		
		dialogues = new List<Dialogue> ();
		
		// This has the potential to throw an exception, but it shouldn't.
		//try {
			xmlLevelReader = new XmlSerializer(typeof(Dialogue));// }
		//catch (InvalidOperationException e) {
		//	Application.Quit(); }
		
		// Read all dialogue files.
		foreach (string filePath in filenames) {
			using (Stream fileStream = new FileStream(filePath, FileMode.Open)) {
				Dialogue newDialogue = (Dialogue)(xmlLevelReader.Deserialize(fileStream));
				fileStream.Close();
				dialogues.Add(newDialogue);
			}
		}
	}
}