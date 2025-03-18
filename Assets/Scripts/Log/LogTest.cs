using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WWFramework;

[ExecuteAlways]
public class LogTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Log.Debug(sb =>
        {
            sb.AppendLine("Debug");
        });
        Log.Debug(sb =>
        {
            sb.AppendLine("Debug");
        }, ELogType.Config);
        Log.Debug(sb =>
        {
            sb.AppendLine("Debug");
        }, ELogType.Resource);
        Log.Debug(sb =>
        {
            sb.AppendLine("Debug");
        }, ELogType.DataTable);
    }

    // Update is called once per frame
    void Update()
    {
        // Log.LogWarning(sb =>
        // {
        //     sb.AppendLine("Warning");
        // });
        // Log.LogError(sb =>
        // {
        //     sb.AppendLine("Error");
        // });
        // Log.Debug(sb =>
        // {
        //     sb.AppendLine("Debug");
        // });
    }
}
