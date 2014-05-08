using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *		Author: 	Craig Lomax
 *		Date: 		07.09.2013
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */

public class Chromosome {
	
	Color colour;
	Color limb_colour;
	Vector3 root_scale;
	
	MultiDimList branches;

	public Color getColour () {
		return colour;	
	}

	public Color getLimbColour () {
		return limb_colour;
	}
	
	public Vector3 getRootScale () {
		return root_scale;	
	}
	
	public MultiDimList getBranches () {
		return branches;
	}

	public void setBranches (MultiDimList bs) {
		branches = bs;
	}
	
	public void setColour (float r, float g, float b) {
		colour = new Color(r,g,b);
	}

	public void setLimbColour (float r, float g, float b) {
		limb_colour = new Color(r,g,b);
	}
	
	public void setRootScale (Vector3 rs) {
		root_scale = rs;
	}

}
