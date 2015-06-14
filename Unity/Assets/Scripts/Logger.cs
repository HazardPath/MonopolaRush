using UnityEngine;
using UnityEngine.UI;
public class Logger {
	//Utility method for logging to a console for debug purposes. This should be deleted before the final version.
	public static void Log(string txt){
		GameObject.FindWithTag("Console").GetComponent<Text>().text=txt;
	}
}