using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    string Name { get; set; }
    string Race { get; set; }
    string[] PreferredIngredient { get; set; }
    string[] PreferredBread { get; set; }

    // »ý¼ºÀÚ
    public NPC(string name, string race, string[] preferredIngredient, string[] preferredBread)
    {
        Name = name;
        Race = race;
        PreferredIngredient = preferredIngredient;
        PreferredBread = preferredBread;
    }


}
