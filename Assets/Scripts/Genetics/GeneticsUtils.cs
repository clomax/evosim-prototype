using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *		Author: 	Craig Lomax
 *		Date: 		26.07.2013
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */


public class GeneticsUtils
{

	static double rand;
	static System.Random rnd = new System.Random();
	
	public static Chromosome mutate (Chromosome c, double rate, float factor)
    {
        Chromosome result = c;

        int size = result.genes.Count;
        int num_limbs = (int)result.genes[size-1];
        int limit = size - (num_limbs+1);

        int current_index;
		for (current_index = 0; current_index < limit; current_index++)
        {
		    rand = rnd.NextDouble();
            if (rand < rate)
                result.genes[current_index] += randomiseGene(factor);
        }

		return result;
	}
	
	public static Chromosome crossover (Chromosome c1, Chromosome c2, double rate)
    {
		Chromosome new_c = c1;

        int current_index;
        for (current_index = 0; current_index < 13; current_index++)
        {
            rand = rnd.NextDouble();
            new_c.genes[current_index] = (rand < 0.5f) ? c1.genes[current_index] : c2.genes[current_index];
        }

        //TODO: Crossover limbs

        return (new_c);
	}

	public static float similar_colour (Chromosome c1, Chromosome c2)
    {
		Color colour1 = new Color(c1.genes[0], c1.genes[1], c1.genes[2]);
        Color colour2 = new Color(c2.genes[0], c2.genes[1], c2.genes[2]);

        return Mathf.Abs((colour1.r * colour2.r) - (colour1.g * colour2.g) - (colour1.b * colour2.g));
	}
	
	private static float randomiseGene(float factor)
    {
		return (float) rnd.NextDouble() * ( Mathf.Abs(factor-(-factor)) ) + (-factor);
	}
	
}
