using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WWFramework;

public class RectTransformPoolTest : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    
    GameObjectRectTransformPool pool;
    
    private List<GameObject> list = new List<GameObject>();
    
    private List<GameObject> list2 = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        pool = GameEntry.Pool.CreatePool<GameObjectRectTransformPool>(UniqueIDGenerator.IntID, p =>
        {
            var trans = this.GetComponent<RectTransform>();
            p.Init(prefab, trans, trans, preCreateNum: 0);
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

        if (Input.GetKeyDown(KeyCode.D))
        {
            var deleteRandom = Random.Range(0, 2);
            var l = list;
            if (deleteRandom == 0)
            {
                l = list2;
            }

            if (l.Count == 0)
            {
                return;
            }
            var random = Random.Range(0, l.Count);
            var item = l[random];
            l.RemoveAt(random);
            item.SafeDestroy();
        }
    }
    
    private async UniTask Add()
    {
        var item = await pool.Spawn();
        item.GetComponent<RectTransform>().anchoredPosition = Random.insideUnitSphere * 100;
        list.Add(item);
        list2.Remove(item);
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
        list2.Add(item);
        await pool.Despawn(item);
    }
}
