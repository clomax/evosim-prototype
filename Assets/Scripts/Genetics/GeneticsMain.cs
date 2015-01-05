using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */


public class GeneticsMain : MonoBehaviour {

	private Ether eth;
	private Settings settings;
	private Spawner spw;
	public static GameObject container;
	public static GeneticsMain instance;
	
	Chromosome chromosome;
	
	int starting_creatures;
	float creature_spread;
	
	Vector3 max_root_scale;
	Vector3 min_root_scale;

	Vector3 max_limb_scale;
	Vector3 min_limb_scale;
	
	
	void Start () {
		spw = Spawner.getInstance();
		settings = Settings.getInstance();
		eth = Ether.getInstance();
		
		max_root_scale 		= new Vector3();
		max_root_scale.x 	= float.Parse( settings.contents["creature"]["root"]["max_root_scale"]["x"].ToString() );
		max_root_scale.y 	= float.Parse( settings.contents["creature"]["root"]["max_root_scale"]["y"].ToString() );
		max_root_scale.z 	= float.Parse( settings.contents["creature"]["root"]["max_root_scale"]["z"].ToString() );
		
		min_root_scale 		= new Vector3();
		min_root_scale.x 	= float.Parse( settings.contents["creature"]["root"]["min_root_scale"]["x"].ToString() );
		min_root_scale.y 	= float.Parse( settings.contents["creature"]["root"]["min_root_scale"]["y"].ToString() );
		min_root_scale.z 	= float.Parse( settings.contents["creature"]["root"]["min_root_scale"]["z"].ToString() );



		max_limb_scale		= new Vector3();
		max_limb_scale.x 	= float.Parse( settings.contents["creature"]["limb"]["max_limb_scale"]["x"].ToString() );
		max_limb_scale.y 	= float.Parse( settings.contents["creature"]["limb"]["max_limb_scale"]["y"].ToString() );
		max_limb_scale.z 	= float.Parse( settings.contents["creature"]["limb"]["max_limb_scale"]["z"].ToString() );

		min_limb_scale 		= new Vector3();
		min_limb_scale.x 	= float.Parse( settings.contents["creature"]["limb"]["min_limb_scale"]["x"].ToString() );
		min_limb_scale.y 	= float.Parse( settings.contents["creature"]["limb"]["min_limb_scale"]["y"].ToString() );
		min_limb_scale.z 	= float.Parse( settings.contents["creature"]["limb"]["min_limb_scale"]["z"].ToString() );



		starting_creatures	= (int) 		settings.contents["ether"]["starting_creatures"];
		creature_spread		= float.Parse(	settings.contents["ether"]["creature_spread"].ToString() );
		double creature_init_energy	= (double) settings.contents["creature"]["init_energy"];
		int branch_limit 	= (int)			settings.contents["creature"]["branch_limit"];
		int recurrence_limit = (int)		settings.contents["creature"]["recurrence_limit"];


		/*
		 * For each new creature, generate random genes and spawn the bugger
		 */
		for (int i=0; i<starting_creatures; i++) {
			chromosome = new Chromosome();
			
			// random colours
			Color col = new Color( (float)Random.Range(0.0F,1.0F),
								   (float)Random.Range(0.0F,1.0F),
								   (float)Random.Range(0.0F,1.0F)
								 );
			chromosome.setColour(col.r, col.g, col.b);
			chromosome.setLimbColour(col.r, col.g, col.b);
			
			// random root scale
			Vector3 rootScale = new Vector3((float) Random.Range(min_root_scale.x,max_root_scale.x),
											(float) Random.Range(min_root_scale.y,max_root_scale.y),
											(float) Random.Range(min_root_scale.z,max_root_scale.z)
								  		   );
			chromosome.setRootScale(rootScale);
			
			// random initial limbs
			int bs = Random.Range (1,branch_limit+1);
			ArrayList branches = new ArrayList();

			for (int j=0; j<bs; j++) {
				ArrayList limbs = new ArrayList();

				int recurrences = Random.Range(0,recurrence_limit);
				for (int k=0; k<=recurrences; k++) {

					Vector3 scale = new Vector3 ((float) Random.Range(min_limb_scale.x,max_limb_scale.x),
		                                 (float) Random.Range(min_limb_scale.y,max_limb_scale.y),
		                                 (float) Random.Range(min_limb_scale.z,max_limb_scale.z)
		                                );

					Vector3 position = Utility.RandomPointInsideCube(rootScale);

					chromosome.setBaseFequency (Random.Range (1,10));
					chromosome.setBaseAmplitude (Random.Range (1,5));
					chromosome.setBasePhase (Random.Range (0,180));

					ArrayList limb = new ArrayList();
					limb.Add  (position);
					limb.Add  (scale);
					limbs.Add (limb);
				}
				branches.Add(limbs);
			}

			chromosome.setBranches(branches);

			if (eth.enoughEnergy(creature_init_energy)) {
				spw.spawn(Utility.RandomVec(-creature_spread,creature_spread,creature_spread), Utility.RandomRotVec(), creature_init_energy, chromosome);
				eth.subtractEnergy(creature_init_energy);
			}
		}
	}
	
	public static GeneticsMain getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "GeneticsMain";
			instance = container.AddComponent<GeneticsMain>();
		}
		return instance;
	}
}
