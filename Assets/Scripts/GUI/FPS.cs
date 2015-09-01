using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPS : MonoBehaviour {

	float fps;
	public Text text;

	// Use this for initialization
	void Start () {
		InvokeRepeating("printFPS", 0F, 0.5F);
	}

	void Update () {
		fps = 1.0f / Time.deltaTime;
	}

	void printFPS () {
		text.text = "FPS: " + fps.ToString("F0");
	}
}
