using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour {

	public Text text;

	void Update () {
		text.text = "Time: " + Time.timeSinceLevelLoad.ToString("F2");
	}

	void OnPostRender () {
		GL.Begin(GL.LINES);
		GL.End();
	}
}
