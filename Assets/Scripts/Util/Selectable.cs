using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Selectable : MonoBehaviour
{
	public Selection sm;
    public CreaturePane cp;

	void Start () {
		sm = Selection.getInstance();
        cp = GameObject.Find("Canvas/CreaturePanel").GetComponent<CreaturePane>();
	}

	public void select (GameObject go) {
		sm.select (go);
        cp.set_data(go.GetComponent<Creature>());
	}
	

}


