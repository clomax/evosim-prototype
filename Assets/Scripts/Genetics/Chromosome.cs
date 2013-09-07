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
	
	int size;
	
	Color colour;
	Vector3 root_scale;
	
	List<Vector3> joint_connections;
	List<Vector3> limb_rotations;
	// ...etc...
	
	
	public Color getColour () {
		return colour;	
	}
	
	public Vector3 getRootScale () {
		return root_scale;	
	}
	
	public void setColour (float r, float g, float b) {
		colour = new Color(r,g,b);
	}
	
	public void setRootScale(float x, float y, float z) {
		root_scale = new Vector3(x,y,z);
	}

}
