using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Selectable : MonoBehaviour {

	private Creature crt_script;
	public Selection sm;

	void Start () {
		sm = Selection.getInstance();
	}

	public void select (GameObject go) {
		sm.select (go);

	}
	

}


