using UnityEngine;
using System.Collections;

public class CreatureWindow: MonoBehaviour {

	private Rect windowRect = new Rect(15,15,250,250);

	void OnGUI () {
		windowRect = GUI.Window (0, windowRect, WindowFunction, "Creature");
	}

	void WindowFunction (int WindowID) {
		GUI.Label (new Rect(20,20,100,50), "Creature Window");
		GUI.DragWindow (new Rect (0,0,100,100));
	}

}
