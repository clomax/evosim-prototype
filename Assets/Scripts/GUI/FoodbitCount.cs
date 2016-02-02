using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FoodbitCount : MonoBehaviour
{
	public Text text;
    int fbs = 0;
	
	void OnEnable ()
    {
        Ether.FoodbitCreated += OnCreated;
        Ether.FoodbitDestroyed += OnDestroyed;
    }

    void OnDisable()
    {
        Ether.FoodbitCreated -= OnCreated;
        Ether.FoodbitDestroyed -= OnDestroyed;
    }

    void OnCreated()
    {
        fbs += 1;
    }

    void OnDestroyed()
    {
        fbs -= 1;
    }

    void Update ()	{
		text.text = "Foodbits: " + fbs;
	}
}
