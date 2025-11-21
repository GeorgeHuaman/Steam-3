using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteTask : MonoBehaviour
{
    public SpatialQuest quest;

    public void FinishTask(int index)
    {
        quest.tasks[index].CompleteTask();
    }
}
