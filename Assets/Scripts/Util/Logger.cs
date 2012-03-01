using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;

/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		crl9@aber.ac.uk
 *
 */

 
public class Logger : MonoBehaviour {
	
	public static GameObject container;
	public static Logger instance;
	
	private StreamWriter sw;
	
	private Logger () {
		sw = new StreamWriter(Application.dataPath + "/Logs/log.txt");
	}
	
	public static Logger getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "Logger";
			instance = container.AddComponent(typeof(Logger)) as Logger;
		}
		return instance;
	}
	
	public void write (String str) {
		sw.WriteLine(str);
	}
	
	public void close () {
		sw.Close();
	}

}
