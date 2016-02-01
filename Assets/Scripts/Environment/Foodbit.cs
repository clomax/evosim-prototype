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
    //MeshRenderer mr;

    float init_energy_min;
    float init_energy_max;
    float init_scale_min;
    float init_scale_max;
    public double energy;
	float decay_amount;
	float destroy_at;
	float decay_time;
	float decay_rate;

	void Start ()
    {
		name = "Foodbit";
		settings = Settings.getInstance();
		init_energy_min = float.Parse(settings.contents["foodbit"]["init_energy_min"].ToString());
        init_energy_max = float.Parse(settings.contents["foodbit"]["init_energy_max"].ToString());

        init_scale_min = float.Parse(settings.contents["foodbit"]["init_scale_min"].ToString());
        init_scale_min = float.Parse(settings.contents["foodbit"]["init_scale_min"].ToString());

        energy = (double)Random.Range(init_energy_min, init_energy_max);
        float scale = Utility.ConvertRange((float)energy, init_energy_min, init_energy_max, init_scale_min, init_scale_max);
        transform.localScale = new Vector3(scale,scale, scale);

		eth = Ether.getInstance();
		Collider co = GetComponent<SphereCollider>();
		co.isTrigger = true;
		
	}

	public void destroy ()
    {
		eth.removeFoodbit(this.gameObject);
		Destroy(gameObject);
	}



}
