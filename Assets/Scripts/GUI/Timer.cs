using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	void Update () {
		guiText.text = "Time: " + Time.timeSinceLevelLoad.ToString("F2");
	}

	void OnPostRender () {
		GL.Begin(GL.LINES);
		GL.End();
	}
}
