using UnityEngine;
using System.Collections;

public class ToggleCreatureWindowButton : MonoBehaviour {

	public void TogglePanel (GameObject panel) {
		panel.SetActive(!panel.activeSelf);
	}
}
