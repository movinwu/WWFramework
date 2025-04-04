using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WWFramework;

public class SubscriberTest : MonoBehaviour
{
    public static ulong eventId1 = UniqueIDGenerator.ULongID;
    public static ulong eventId2 = UniqueIDGenerator.ULongID;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameEntry.Event.Subscribe(eventId1, Test1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameEntry.Event.Subscribe<int>(eventId2, Test2);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameEntry.Event.Unsubscribe(eventId1, Test1);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameEntry.Event.Unsubscribe<int>(eventId2, Test2);
        }
    }

    private void Test1()
    {
        Log.LogDebug((sb) =>
        {
            sb.Append("test1");
        });
    }

    private static int frameCount;
    private void Test2(int a)
    {
        if (frameCount != Time.frameCount)
        {
            frameCount = Time.frameCount;
            Log.LogDebug((sb) =>
            {
                sb.Append("test2 ");
                sb.Append(a);
                sb.Append(" ");
                sb.Append(Time.frameCount);
            });
        }
    }
}
