using UnityEngine;
using System.Collections;

public class Selection : MonoBehaviour {
			
	public GameObject selected;

	public static GameObject container;
	public static Selection instance;

	public static Selection getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "SelectionManager";
			instance = container.AddComponent<Selection>();
		}
		return instance;
	}

	public GameObject select (GameObject go) {
		return selected = go;
	}
	
	public bool isSelected (GameObject go) {
		return selected == go;
	}

}
