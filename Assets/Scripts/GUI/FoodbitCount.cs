using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FoodbitCount : MonoBehaviour
{
	Text text;

    void Start ()
    {
        text = GetComponent<Text>();
    }
	
	void OnEnable ()
    {
        Ether.FoodbitsUpdated += OnUpdated;
    }

    void OnDisable()
    {
        Ether.FoodbitsUpdated -= OnUpdated;
    }

    void OnUpdated(int count)
    {
        text.text = "Foodbits: " + count;
    }

    void Update ()	{
	}
}
