using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCTrait
{

}

public class NPC : MonoBehaviour
{
    string Name { get; set; }
    string Race { get; set; }

    NPCTrait Trait { get; set; } // Ư��

    string[] PreferredIngredient { get; set; }
    string[] PreferredBread { get; set; }

    // ������
    public NPC(string name, string race, string[] preferredIngredient, string[] preferredBread)
    {
        Name = name;
        Race = race;
        PreferredIngredient = preferredIngredient;
        PreferredBread = preferredBread;
    }


}
