using UnityEngine;
using System.Collections;

public class FPS : MonoBehaviour {

	float fps;

	// Use this for initialization
	void Start () {
		InvokeRepeating("printFPS", 0F, 0.5F);
	}

	void Update () {
		fps = 1.0f / Time.deltaTime;
	}

	void printFPS () {
		guiText.text = "FPS: " + fps.ToString("F0");
	}
}
