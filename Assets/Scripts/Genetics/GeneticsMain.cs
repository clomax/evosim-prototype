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


public class GeneticsMain : MonoBehaviour
{
	private Ether eth;
	private Settings settings;
	public static GameObject container;
	public static GeneticsMain instance;
	
	public Chromosome chromosome;
	
	int starting_creatures;
	float creature_spread;
	
	Vector3 max_root_scale;
	Vector3 min_root_scale;

	Vector3 max_segment_scale;
	Vector3 min_segment_scale;
	
	void Start ()
    {
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



		max_segment_scale		= new Vector3();
		max_segment_scale.x 	= float.Parse( settings.contents["creature"]["segment"]["max_segment_scale"]["x"].ToString() );
		max_segment_scale.y 	= float.Parse( settings.contents["creature"]["segment"]["max_segment_scale"]["y"].ToString() );
		max_segment_scale.z 	= float.Parse( settings.contents["creature"]["segment"]["max_segment_scale"]["z"].ToString() );

		min_segment_scale 		= new Vector3();
		min_segment_scale.x 	= float.Parse( settings.contents["creature"]["segment"]["min_segment_scale"]["x"].ToString() );
		min_segment_scale.y 	= float.Parse( settings.contents["creature"]["segment"]["min_segment_scale"]["y"].ToString() );
		min_segment_scale.z 	= float.Parse( settings.contents["creature"]["segment"]["min_segment_scale"]["z"].ToString() );


		starting_creatures	= (int) 			settings.contents["ether"]["starting_creatures"];
		creature_spread		= float.Parse(		settings.contents["ether"]["creature_spread"].ToString() );
		decimal creature_init_energy = decimal.Parse(settings.contents["creature"]["init_energy"].ToString());
		int limb_limit 	= (int)				    settings.contents["creature"]["limb_limit"];
		int segment_limit = (int)			settings.contents["creature"]["segment_limit"];


		for (int i=0; i<starting_creatures; i++)
        {
			chromosome = new Chromosome();
			
			// random root and limb colours
            for (int c=0; c<6; ++c)
            {
                chromosome.genes.Add((float)Random.Range(0.0F, 1.0F));
            }

            // random root scale
            chromosome.genes.Add((float)Random.Range(min_root_scale.x, max_root_scale.x));
            chromosome.genes.Add((float)Random.Range(min_root_scale.y, max_root_scale.y));
            chromosome.genes.Add((float)Random.Range(min_root_scale.z, max_root_scale.z));

            chromosome.genes.Add(float.Parse(settings.contents["creature"]["hunger_threshold"].ToString()));

            // base frequency, amp, phase
            chromosome.genes.Add(Random.Range(3, 20));
            chromosome.genes.Add(Random.Range (3,6));
            chromosome.genes.Add(Random.Range(0, 360));

            // random initial limbs
            int num_limbs = Random.Range(1, limb_limit);
            int[] num_segments = new int[num_limbs];

            for(int l=0; l<num_limbs; l++)
                num_segments[l] = Random.Range(1, segment_limit);


            for (int l=0; l<num_limbs; ++l)
            {
                Vector3[] segment_scales = new Vector3[num_segments[l]];
                for (int s=0; s<num_segments[l]; s++)
                {
                    if (s == 0)
                    {
                        // random scale
                        Vector3 scale = new Vector3 (
                            Random.Range(min_segment_scale.x, max_segment_scale.x),
                            Random.Range(min_segment_scale.y, max_segment_scale.y),
                            Random.Range(min_segment_scale.z, max_segment_scale.z)
                        );

                        segment_scales[0] = scale;

                        chromosome.genes.Add(scale.x);
                        chromosome.genes.Add(scale.y);
                        chromosome.genes.Add(scale.z);
                    }
                    else
                    {
                        //TODO: Make a gene out of the uniform scaling of segments
                        Vector3 scale = new Vector3(
                                segment_scales[s - 1].x * 0.8F,
                                segment_scales[s - 1].y * 0.8F,
                                segment_scales[s - 1].z * 0.8F
                            );

                        if (s >= segment_scales.Length)
                            print("test");
                        segment_scales[s] = scale;

                        chromosome.genes.Add(scale.x);
                        chromosome.genes.Add(scale.y);
                        chromosome.genes.Add(scale.z);
                    }
                }
            }

            for(int s=0; s<num_limbs; ++s)
            {
                chromosome.genes.Add((float)num_segments[s]);
            }

            chromosome.genes.Add((float)num_limbs);

            if (eth.enoughEnergy(creature_init_energy))
            {
				eth.spawner.spawn(Utility.RandomVec(-creature_spread,creature_spread,creature_spread), Utility.RandomRotVec(), creature_init_energy, chromosome);
                eth.subtractEnergy(creature_init_energy);
            }
        }
	}
	
	public static GeneticsMain getInstance ()
    {
		if(!instance)
        {
			container = new GameObject();
			container.name = "GeneticsMain";
			instance = container.AddComponent<GeneticsMain>();
		}
		return instance;
	}
}
