using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int password;
    public Bar actualBar;
    public int newAmount;

    public SpatialQuest quest;
    public int indexTask;
    public void DoorVeri()
    {
        PanelNumerico.instance.Open(this.gameObject,password);
    }


    public void Completed()
    {
        actualBar.ChangeAmount(newAmount);
        quest.tasks[indexTask].CompleteTask();
    }
}
