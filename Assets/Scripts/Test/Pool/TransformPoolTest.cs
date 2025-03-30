using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WWFramework;

public class TransformPoolTest : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    
    GameObjectTransformPool pool;
    
    private List<GameObject> list = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        pool = GameEntry.Pool.CreatePool<GameObjectTransformPool>(UniqueIDGenerator.IntID, p =>
        {
            p.Init(prefab, preCreateNum: 10);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Add().Forget();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Remove().Forget();
        }
    }
    
    private async UniTask Add()
    {
        var item = await pool.Spawn();
        item.transform.position = Random.insideUnitSphere * 10;
        list.Add(item);
    }
    
    private async UniTask Remove()
    {
        if (list.Count == 0)
        {
            return;
        }
        var index = Random.Range(0, list.Count);
        var item = list[index];
        list.RemoveAt(index);
        await pool.Despawn(item);
    }
}
