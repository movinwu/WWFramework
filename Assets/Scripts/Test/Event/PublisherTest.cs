using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WWFramework;

public class PublisherTest : MonoBehaviour
{
    public int loopNum;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            GameEntry.Event.PublishDelay(SubscriberTest.eventId1);
        }

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            loopNum = Mathf.Max(1, loopNum);
            for (int i = 0; i < loopNum; i++)
            {
                GameEntry.Event.PublishDelay(SubscriberTest.eventId2, 0);
                GameEntry.Event.PublishDelay(SubscriberTest.eventId2, 1);
                GameEntry.Event.PublishDelay(SubscriberTest.eventId2, 2);
                GameEntry.Event.PublishDelay(SubscriberTest.eventId2, 3);
                GameEntry.Event.PublishDelay(SubscriberTest.eventId2, 4);
                GameEntry.Event.PublishDelay(SubscriberTest.eventId2, 5);
                GameEntry.Event.PublishDelay(SubscriberTest.eventId2, 6);
                GameEntry.Event.PublishDelay(SubscriberTest.eventId2, 7);
                GameEntry.Event.PublishDelay(SubscriberTest.eventId2, 8);
                GameEntry.Event.PublishDelay(SubscriberTest.eventId2, 9);
            }
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            GameEntry.Event.PublishDelay(SubscriberTest.eventId2, 1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            GameEntry.Event.PublishDelay(SubscriberTest.eventId2, 2);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            GameEntry.Event.PublishDelay(SubscriberTest.eventId2, 3);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            GameEntry.Event.PublishDelay(SubscriberTest.eventId2, 4);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            GameEntry.Event.PublishDelay(SubscriberTest.eventId2, 5);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            GameEntry.Event.PublishDelay(SubscriberTest.eventId2, 6);
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            GameEntry.Event.PublishDelay(SubscriberTest.eventId2, 7);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            GameEntry.Event.PublishDelay(SubscriberTest.eventId2, 8);
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            GameEntry.Event.PublishDelay(SubscriberTest.eventId2, 9);
        }
    }
}
