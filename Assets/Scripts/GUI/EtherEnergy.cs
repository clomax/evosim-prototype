using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 *		Author: 	Craig Lomax
 *		Date: 		31.08.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */
 
public class EtherEnergy : MonoBehaviour
{
	Text text;

    void Start ()
    {
        text = GetComponent<Text>();
    }

    void OnEnable ()
    {
        Ether.EnergyUpdated += OnUpdated;
    }

    void OnDisable()
    {
        Ether.EnergyUpdated -= OnUpdated;
    }

    void OnUpdated (decimal n)
    {
		text.text = "Ether energy: " + n.ToString("0.0");
	}
}
