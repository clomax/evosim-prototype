using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {

	private Rect selectedWindow;
	private Creature crt_script;
	private Vector3 crt_pos;

	void Start () {
		selectedWindow = new Rect (20,555,250,115);
	}

	public void select (GameObject go) {
		Selection.select (go);
	}

	void OnGUI () {
		if (Selection.isSelected (gameObject)) {
			selectedWindow = GUI.Window(GetInstanceID(), selectedWindow, SelectionWindow, gameObject.name);
		}
	}

	void SelectionWindow (int id) {
		crt_script = gameObject.GetComponent<Creature>();
		GUILayout.Box("State: " + crt_script.state +
					  "\nEnergy: " + crt_script.energy.ToString("F1") +
		              "\nAge: " + crt_script.age.ToString("F0") +
		              "\nFood eaten: " + crt_script.times_eaten + 
		              "\nOffspring: " + crt_script.times_mated +
		              "\nGoal: " + crt_script.eye_script.distance_to_goal.ToString("F1"));
		GUI.DragWindow();
	}
	
}


