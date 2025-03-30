using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WWFramework;

public class ReferencePoolTest : MonoBehaviour
{
    private ReferencePool referencePool;
    private List<ReferenceTest> referenceTests;
    
    // Start is called before the first frame update
    void Start()
    {
        referencePool = GameEntry.Pool.CreatePool<ReferencePool>(UniqueIDGenerator.IntID, p =>
        {
            p.Init(id =>
            {
                if (id == ReferenceTest1.typeId)
                {
                    return new ReferenceTest1();
                }

                if (id == ReferenceTest2.typeId)
                {
                    return new ReferenceTest2();
                }

                if (id == ReferenceTest3.typeId)
                {
                    return new ReferenceTest3();
                }

                return new ReferenceTest();
            }).Forget();
        });

        Init().Forget();
    }

    private async UniTaskVoid Init()
    {
        referenceTests = new List<ReferenceTest>();
        for (int i = 0; i < 100; i++)
        {
            await Add();
        }
    }

    private async UniTask Add()
    {
        var item = await CreateReferenceTest();
        referenceTests.Add(item);
    }
    
    private async UniTask<ReferenceTest> CreateReferenceTest()
    {
        var random = Random.Range(0, 3);
        if (random == 0)
        {
            return await referencePool.Spawn<ReferenceTest1>(ReferenceTest1.typeId);
        }

        if (random == 1)
        {
            return await referencePool.Spawn<ReferenceTest2>(ReferenceTest2.typeId);
        }

        return await referencePool.Spawn<ReferenceTest3>(ReferenceTest3.typeId);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var random = Random.Range(0, referenceTests.Count);
            var item = referenceTests[random];
            referenceTests.RemoveAt(random);
            referencePool.Despawn(item).Forget();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Add().Forget();
        }
    }
}

public class ReferenceTest : IReferencePoolItem
{
    public int PoolItemTypeId { get; set; }

    public UniTask OnSpawnFromPool()
    {
        return UniTask.CompletedTask;
    }

    public UniTask OnDespawnToPool()
    {
        return UniTask.CompletedTask;
    }
}

public class ReferenceTest1 : ReferenceTest
{
    public static int typeId = UniqueIDGenerator.IntID; 
}

public class ReferenceTest2 : ReferenceTest
{
    public static int typeId = UniqueIDGenerator.IntID; 
}

public class ReferenceTest3 : ReferenceTest
{
    public static int typeId = UniqueIDGenerator.IntID; 
}
