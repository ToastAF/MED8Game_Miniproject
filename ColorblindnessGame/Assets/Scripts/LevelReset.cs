using System.Collections.Generic;
using UnityEngine;

public class LevelReset : MonoBehaviour
{
    private List<Resettable> resettableObjects = new List<Resettable>();

    void Start()
    {
        MonoBehaviour[] allScripts = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach(MonoBehaviour script in allScripts)
        {
            if (script is Resettable resettable)
            {
                resettableObjects.Add(resettable);
            }
        }
    }

    public void ResetEverything()
    {
        foreach (Resettable resettable in resettableObjects)
        {
            resettable.Death();
        }
    }
}
