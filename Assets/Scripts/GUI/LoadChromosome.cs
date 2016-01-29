using UnityEngine;
using System.Collections;

public class LoadChromosome : MonoBehaviour
{
    public Chromosome c;
    public UIElement parent;

    public void OnClick ()
    {
        Spawner spawner = Spawner.getInstance();
        spawner.spawn(
            Camera.main.transform.position + new Vector3(0, 0, 10),
             Utility.RandomRotVec(),
             75.0,
             c
        );
        parent.make_invisible();
        GetComponentInParent<CreatureList>().DepopulateMenu();
    }
}
