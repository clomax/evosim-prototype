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
	Vector3 root_scale;
	
	ArrayList limbs = new ArrayList();	
	ArrayList limb;

	public Color getColour () {
		return colour;	
	}
	
	public Vector3 getRootScale () {
		return root_scale;	
	}
	
	public void setColour (float r, float g, float b) {
		colour = new Color(r,g,b);
	}
	
	public void setRootScale (float x, float y, float z) {
		root_scale = new Vector3(x,y,z);
	}
	
	public void addLimb (Color col, Vector3 point, Vector3 rot, Vector3 scale, int recur) {
		limb = new ArrayList();
		limb.Add(col);
		limb.Add(point);
		limb.Add(rot);
		limb.Add(scale);
		limb.Add(recur);
		limbs.Add(limb);
	}
	
	public void delLimb () {
		
	}
	
	public ArrayList getLimbs () {
		return limbs;
	}

}
