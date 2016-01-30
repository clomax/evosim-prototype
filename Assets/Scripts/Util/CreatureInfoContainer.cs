using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatureInfoContainer : MonoBehaviour
{
    public static CreatureInfoContainer instance;
    public static GameObject container;

    public SortedList<string, Chromosome> creatures;

    public static CreatureInfoContainer getInstance ()
    {
        if (!instance)
        {
            container = new GameObject();
            container.name = "CreatureInfo";
            instance = container.AddComponent<CreatureInfoContainer>();
        }
        return instance;
    }

    void Start ()
    {
        creatures = new SortedList<string, Chromosome>();
    }
}
