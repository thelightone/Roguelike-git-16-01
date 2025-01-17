using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Pool;

public class XPSpawner : MonoBehaviour
{

    [SerializeField] private GameObject _prefab;

    public ObjectPool<GameObject> _pool;
    private GameObject _tempXP;
    public static XPSpawner Instance;
    public List<GameObject> listXP = new List<GameObject>();
    public List<GameObject> poolXP = new List<GameObject>();
    public GameObject parent;
    private void Start()
    {
        Instance = this;

        _pool = new ObjectPool<GameObject>(
         createFunc: () => Instantiate(_prefab, parent.transform),
         actionOnGet: (obj) =>
         {
             obj.SetActive(true);
         }
         ,
         actionOnRelease: (obj) => obj.SetActive(false),
         actionOnDestroy: (obj) => Destroy(obj),
         collectionCheck: false,
         defaultCapacity: 100,
         maxSize: 200
         );

        for (int i = 0; i < 20; i++)
        {
            _tempXP = _pool.Get();
            poolXP.Add( _tempXP );
        }



    }

    public void SpawnXP(Transform enemy)
    {
        if (poolXP.Count == 0)
        {
            _tempXP = _pool.Get(); 
            _tempXP.SetActive(true);
        }
        else
        {
            poolXP[0].SetActive(true);
            poolXP[0].transform.position = enemy.position;
            poolXP.Remove(poolXP[0]);
        }
        //_tempXP.transform.position = enemy.position;
        listXP.Add(_tempXP);
    }

    public void DespawnXP(GameObject xp)
    {
        //_pool.Release(xp);
        xp.transform.position = parent.transform.position;
        xp.SetActive(false);
        poolXP.Add(xp);
        listXP.Remove(xp);
    }

    private void ResetXP()
    {

    }

}
