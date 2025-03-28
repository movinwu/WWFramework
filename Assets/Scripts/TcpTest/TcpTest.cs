using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WWFramework;

public class TcpTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEntry.NetworkClient.OnConnectedSuccess = () =>
        {
            Debug.Log("Connected Success");
        };
        GameEntry.NetworkClient.OnConnectedFailed = () =>
        {
            Debug.Log("Connected Failed");
        };
        GameEntry.NetworkClient.OnDisconnected = () =>
        {
            Debug.Log("Disconnected");
        };
        GameEntry.NetworkClient.OnReconnectSuccess = () =>
        {
            Debug.Log("Reconnect Success");
        };
        GameEntry.NetworkClient.OnReconnectFail = (code) =>
        {
            Debug.Log($"Reconnect Fail: {code}");
        };
        GameEntry.NetworkClient.OnReconnectStart = () =>
        {
            Debug.Log("Reconnect Start");
        };
        GameEntry.NetworkClient.OnReconnectFail = (code) =>
        {
            Debug.Log($"Reconnect Fail: {code}");
        };
        GameEntry.NetworkClient.OnDisconnected = () =>
        {
            Debug.Log("Disconnected");
        };
        StartCoroutine(Connect());
    }

    IEnumerator Connect()
    {
        yield return new WaitForSeconds(1);
        GameEntry.NetworkClient.Connect().Forget();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameEntry.NetworkClient.Send(System.Text.Encoding.UTF8.GetBytes(Input.mousePosition.ToString())).Forget();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameEntry.NetworkClient.Disconnect().Forget();
        }

        if (!string.IsNullOrEmpty(GameEntry.NetworkClient.TEMP))
        {
            Log.LogDebug(sb => { sb.Append(GameEntry.NetworkClient.TEMP); }, ELogType.Network);
            GameEntry.NetworkClient.TEMP = string.Empty;
        }
    }
}
