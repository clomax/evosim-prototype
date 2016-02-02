using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeltaTime : MonoBehaviour
{
    public Text text;
    char delta = '\u0394';

    // Use this for initialization
    void Start ()
    {
		InvokeRepeating("printDeltaTime", 0F, 0.5F);
	}

	void printDeltaTime ()
    {
		text.text = delta +"T: " + Time.deltaTime.ToString();
	}
}
