using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Selectable : MonoBehaviour {

	private Creature crt_script;
	public Selection sm;
	public ToggleCreatureWindowButton tw;

	void Start () {
		sm = Selection.getInstance();
		tw = GameObject.Find ("UIController").GetComponent<ToggleCreatureWindowButton>();
	}

	public void select (GameObject go) {
		sm.select (go);
	}
	

}


