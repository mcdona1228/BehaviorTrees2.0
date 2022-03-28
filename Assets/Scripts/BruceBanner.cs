using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruceBanner : MonoBehaviour
{
    public Door theDoor;
    public GameObject theTreasure;
    public GameObject test;
    bool executingBehavior = false;
    Task myCurrentTask;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!executingBehavior)
            {
                executingBehavior = true;
                myCurrentTask = BuildTask_GetTreasue();

                EventBus.StartListening(myCurrentTask.TaskFinished, OnTaskFinished);
                myCurrentTask.run();
            }
        }
    }

    void OnTaskFinished()
    {
        EventBus.StopListening(myCurrentTask.TaskFinished, OnTaskFinished);
        //Debug.Log("Behavior complete! Success = " + myCurrentTask.succeeded);
        executingBehavior = false;
    }
    
    Task BuildTask_GetTreasue()
    {
        // create our behavior tree based on Millington pg. 344
        // building from the bottom up
        List<Task> taskList = new List<Task>();

        // if door isn't locked, open it
        Task isDoorNotLocked = new IsFalse(theDoor.isLocked);
        Task waitABeat = new Wait(0.5f);
        Task openDoor = new OpenDoor(theDoor);
        taskList.Add(isDoorNotLocked);
        taskList.Add(waitABeat);
        taskList.Add(openDoor);
        Sequence openUnlockedDoor = new Sequence(taskList);

        // barge a closed door
        taskList = new List<Task>();
        Task isDoorClosed = new IsTrue(theDoor.isClosed);
        Task hulkOut = new HulkOut(this.gameObject);
        Task bargeDoor = new BargeDoor(theDoor.transform.GetChild(0).GetComponent<Rigidbody>());
        Task isDoorBargable = new IsFalse(theDoor.isSteel);
        Task isDoorThrowable = new IsFalse(theDoor.isTitanium);
        Task isDoorUnbreakable = new IsFalse(theDoor.isVibranium);
        taskList.Add(isDoorClosed);
        taskList.Add(waitABeat);
        taskList.Add(hulkOut);
        taskList.Add(waitABeat);
        taskList.Add(isDoorBargable);
        taskList.Add(waitABeat);
        taskList.Add(bargeDoor);
        Sequence bargeClosedDoor = new Sequence(taskList);

        // Squish steel door
        
        taskList = new List<Task>();
        Task isDoorSteel = new IsTrue(theDoor.isSteel);
        //Task hulkOut = new HulkOut(this.gameObject);
        Task squishDoor = new SquishDoor(theDoor.transform.GetChild(0).GetComponent<Rigidbody>());
        taskList.Add(isDoorSteel);
        taskList.Add(waitABeat);
        taskList.Add(hulkOut);
        taskList.Add(waitABeat);
        taskList.Add(isDoorBargable);
        taskList.Add(waitABeat);
        taskList.Add(squishDoor);
        Sequence squishSteelDoor = new Sequence(taskList);

        // Titanium door brings hulk into a rage
        taskList = new List<Task>();
        Task isDoorTitanium = new IsTrue(theDoor.isTitanium);
        Task hulkRage = new HulkRage(this.gameObject);
        Task titaniumDoor = new BargeDoor(theDoor.transform.GetChild(0).GetComponent<Rigidbody>());
        taskList.Add(isDoorTitanium);
        taskList.Add(waitABeat);
        taskList.Add(hulkOut);
        taskList.Add(waitABeat);
        taskList.Add(hulkRage);
        taskList.Add(waitABeat);
        taskList.Add(titaniumDoor);
        Sequence throwTitaniumDoor = new Sequence(taskList);

        //Giving up
        taskList = new List<Task>();
        Task isDoorVibranium = new IsTrue(theDoor.isVibranium);
        Task giveUp = new GiveUp(this.gameObject);
        taskList.Add(isDoorVibranium);
        taskList.Add(waitABeat);
        taskList.Add(hulkOut);
        taskList.Add(waitABeat);
        taskList.Add(hulkRage);
        taskList.Add(waitABeat);
        taskList.Add(giveUp);
        Sequence hulkgivesUp = new Sequence(taskList);

        // open a closed door, one way or another
        taskList = new List<Task>();
        taskList.Add(openUnlockedDoor);
        taskList.Add(bargeClosedDoor);
        taskList.Add(squishSteelDoor);
        taskList.Add(throwTitaniumDoor);
        taskList.Add(hulkgivesUp);
        Selector openTheDoor = new Selector(taskList);

        // get the treasure when the door is closed
        taskList = new List<Task>();
        Task moveToDoor = new MoveKinematicToObject(this.GetComponent<Kinematic>(), theDoor.gameObject);
        Task moveToTreasure = new MoveKinematicToObject(this.GetComponent<Kinematic>(), theTreasure.gameObject);
        taskList.Add(moveToDoor);
        taskList.Add(waitABeat);
        taskList.Add(openTheDoor); // one way or another
        taskList.Add(waitABeat);
        taskList.Add(moveToTreasure);
        Sequence getTreasureBehindClosedDoor = new Sequence(taskList);

        // get the treasure when the door is open 
        taskList = new List<Task>();
        Task isDoorOpen = new IsFalse(theDoor.isClosed);
        taskList.Add(isDoorOpen);
        taskList.Add(moveToTreasure);
        Sequence getTreasureBehindOpenDoor = new Sequence(taskList);

        // get the treasure, one way or another
        taskList = new List<Task>();
        taskList.Add(getTreasureBehindOpenDoor);
        taskList.Add(getTreasureBehindClosedDoor);
        Selector getTreasure = new Selector(taskList);

        return getTreasure;
    }
}
