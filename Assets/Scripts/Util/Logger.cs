using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;

/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */

 
public class Logger : MonoBehaviour {
	
	public static GameObject container;
	public static Logger instance;
	
	public static Logger getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "Logger";
			instance = container.AddComponent(typeof(Logger)) as Logger;
		}
		return instance;
	}
	
	public void write (String str, String file) {
		File.AppendAllText(Application.dataPath + "/data/" + file + ".csv", str);
	}
	
	public void close () {
		//sw.Close();
	}

}
