using UnityEngine;
using System.Collections;

public class MenuItem : MonoBehaviour {

	 void OnMouseEnter () {
        renderer.material.color = Color.green;
    }
	
	void OnMouseExit () {
		renderer.material.color = Color.white;
	}
	
	void OnMouseUp () {
		switch (gameObject.name) {
			case "Quit":  Application.Quit(); break;
			case "Start": Application.LoadLevel(1); break;
		}
	}
	
}
