using UnityEngine;
using System.Collections;

public class LoadChromosome : MonoBehaviour
{
    public Chromosome c;
    public UIElement parent;

    Settings s;

    decimal init_energy;

    void Start ()
    {
        s = Settings.getInstance();
        init_energy = decimal.Parse(s.contents["creature"]["init_energy"].ToString());
    }

    public void OnClick ()
    {
        Spawner spawner = Spawner.getInstance();
        spawner.spawn(
            Camera.main.transform.position + new Vector3(0, 0, 10),
             Utility.RandomRotVec(),
             init_energy,
             c
        );
        Ether.getInstance().subtractEnergy(init_energy);
        parent.make_invisible();
        GetComponentInParent<CreatureList>().DepopulateMenu();
    }
}
