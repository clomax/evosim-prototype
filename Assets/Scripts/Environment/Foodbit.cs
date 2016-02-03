using UnityEngine;
using System.Collections;

/*
 *		Author: 	Craig Lomax
 *		Date: 		31.08.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */

public class Foodbit : MonoBehaviour
{
	public static float foodbitHeight = 1.0F;
	
	Settings settings;
	Ether eth;
    MeshRenderer mr;

    public decimal energy;
	float decay_amount;
	float destroy_at;
	float decay_time;
	float decay_rate;

    void Start ()
    {
		name = "Foodbit";
		settings = Settings.getInstance();
		
		eth = Ether.getInstance();

        mr = GetComponent<MeshRenderer>();
        mr.sharedMaterial = (Material)Resources.Load("Materials/Foodbit");

		Collider co = GetComponent<SphereCollider>();
		co.isTrigger = true;
    }

	public void destroy ()
    {
		eth.removeFoodbit(this.gameObject);
		Destroy(gameObject);
	}



}
