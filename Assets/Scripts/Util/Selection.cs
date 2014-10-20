using UnityEngine;
using System.Collections;

public class Selection {
			
	private static GameObject selected;
	
	public static GameObject getSelected {
		get { return selected; }
		set { selected = value; }
	}
	
	public static void select (GameObject go) {
		getSelected = go;
	}
	
	public static bool isSelected (GameObject go) {
		return selected == go;
	}

}
