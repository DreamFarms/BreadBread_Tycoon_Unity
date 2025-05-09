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

    NPCTrait Trait { get; set; } // 특성

    string[] PreferredIngredient { get; set; }
    string[] PreferredBread { get; set; }

    // 생성자
    public NPC(string name, string race, string[] preferredIngredient, string[] preferredBread)
    {
        Name = name;
        Race = race;
        PreferredIngredient = preferredIngredient;
        PreferredBread = preferredBread;
    }


}
