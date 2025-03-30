using UnityEngine;
using WWFramework;

[ExecuteAlways]
public class LogTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Log.StartTimeMonitor();
        Log.LogDebug(sb =>
        {
            sb.AppendLine("Debug");
        });
        Log.LogDebug(sb =>
        {
            sb.AppendLine("Debug");
        }, ELogType.Config);
        Log.LogDebug(sb =>
        {
            sb.AppendLine("Debug");
        }, ELogType.Resource);
        Log.LogDebug(sb =>
        {
            sb.AppendLine("Debug");
        }, ELogType.DataTable);
        Log.EndTimeMonitor();
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
