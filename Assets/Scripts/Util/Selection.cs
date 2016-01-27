using UnityEngine;
using System.Collections;

public class Selection : MonoBehaviour
{			
	public GameObject selected;

	public static GameObject container;
	public static Selection instance;

    public delegate void SelectionDelegate(Creature c);
    public static event SelectionDelegate Selected;

    private Ray ray;
    private RaycastHit hit;

	public static Selection getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "SelectionManager";
			instance = container.AddComponent<Selection>();
		}
		return instance;
	}

    void Update ()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonUp(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                Creature c = null;
                if (hit.transform.tag == "Creature")
                {
                    c = hit.transform.GetComponentInParent<Creature>();
                    selected = hit.transform.parent.gameObject;
                    Selected(c);
                }
            }
            else
            {
                Selected(null);
            }
        }
    }

	public void select (GameObject go) {
        selected = go;
    }
	
	public bool isSelected (GameObject go) {
		return selected == go;
	}

}
