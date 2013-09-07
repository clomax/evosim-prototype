using UnityEngine;
using System.Collections;

/*
 *		Author: 	Craig Lomax
 *		Date: 		26.07.2013
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */


public class GeneticsUtils {
	
	static System.Random rnd = new System.Random();

	public static Chromosome mutate (Chromosome c, double rate, float factor) {
		// Mutate colour
		float[] cs = new float[3];
		Color cc = c.getColour();
		cs[0] = cc.r;
		cs[1] = cc.g;
		cs[2] = cc.b;
		for (int i=0; i<3; i++) {
			double rand = rnd.NextDouble();
			if (rand < rate)
				cs[i] += randomiseGene(factor);
		}
		c.setColour(cs[0], cs[1], cs[2]);
		
		// Mutate root scale
		float[] rs = new float[3];
		Vector3 rc = c.getRootScale();
		rs[0] = rc.x;
		rs[1] = rc.y;
		rs[2] = rc.z;
		for (int i=0; i<3; i++) {
			double rand = rnd.NextDouble();
			if (rand < rate)
				rs[i] += randomiseGene(factor);
		}
		c.setRootScale(rs[0], rs[1], rs[2]);
		
		return c;
	}
	
	public static Chromosome crossover (Chromosome c1, Chromosome c2, double rate) {
		Chromosome newChromosome = c1;
		for (int i=0; i<length; i++) {
			double rand = rnd.NextDouble();
			if (rand < rate)
				newChromosome[i] = c2[i];
		}
		
		return newChromosome;
	}
	
	private static float randomiseGene(float factor) {
		return (float) rnd.NextDouble() * ( Mathf.Abs(factor-(-factor)) ) + (-factor);
	}
	
}
