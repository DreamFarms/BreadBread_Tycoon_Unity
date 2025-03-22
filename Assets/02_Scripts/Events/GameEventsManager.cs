using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager Instance { get; private set; }

    public MiscEvents miscEvents;
    public QuestEvent questEvents;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;

        // init all events
        miscEvents = new MiscEvents();
        questEvents = new QuestEvent();
    }
}
