using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WWFramework;

public class ReadOnlyTest : MonoBehaviour
{
    [ReadOnly]
    public List<int> list = new List<int>() { 1, 2, 3 };
    
    [SerializeField, ReadOnly]
    private int[] array = new int[] { 1, 2, 3 };
    
    public List<int> newList = new List<int>() { 1, 2, 3 };
    
    [SerializeField]
    private int[] newArray = new int[] { 1, 2, 3 };
    
    [Range(0, 1)]
    public float floatValue = 1.0f;
    
    /// <summary>
    /// 只读属性不能对range属性生效
    /// </summary>
    [Range(1, 100), ReadOnly]
    public int intValue = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(intValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
