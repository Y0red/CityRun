using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
[ExecuteInEditMode]
public class ObjPool : MonoBehaviour
{
    [SerializeField] bool canGrow = false;

    [SerializeField] private List<_Pool> _poolsX;

    [SerializeField] private Dictionary<string, List<GameObject>> _poolDictionary = new Dictionary<string, List<GameObject>>();

    GameObject objParent;
    GameObject pooledPrifab;
    protected void Awake()
    {
        InitPool();
    }
    public void InitPool()
    {
        objParent = new GameObject("All_PRIFABS");
        objParent.transform.SetParent(this.transform);
        foreach (_Pool pol in _poolsX)
        {
            List<GameObject> poolsList = new List<GameObject>();

            for (int i = 0; i < pol.size; i++)
            {
                Addressables.InstantiateAsync(pol.prifabReference.AssetGUID, objParent.transform).Completed += (c) =>
                {
                    pooledPrifab = c.Result.gameObject;

                    pooledPrifab.SetActive(false);
                    poolsList.Add(pooledPrifab);
                };
            }
            _poolDictionary.Add(pol.tag, poolsList);
        }
    }
    public GameObject GetAvailablePrifabFromDictionary(string tag)
    {
        if (!_poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("plool with tag " + tag + "not found");

            return null;
        }
        for (int i = 0; i < _poolDictionary[tag].Count; i++)
        {
            if (_poolDictionary[tag][i].activeInHierarchy == false) return _poolDictionary[tag][i];
        }
        if (canGrow)
        {
            List<GameObject> poolsList = new List<GameObject>();
            Addressables.InstantiateAsync(_poolsX[0].prifabReference, objParent.transform).Completed += (c) =>
            {
                pooledPrifab = c.Result.gameObject;

                pooledPrifab.SetActive(false);
                poolsList.Add(pooledPrifab);

                _poolDictionary.Add(tag, poolsList);
   
            };
            return pooledPrifab;
        }
        //else if (!canGrow)
        //{
        //    for (int i = 0; i < _poolDictionary[tag].Count; i++)
        //    {
        //        if (!_poolDictionary[tag][i].GetComponent<Platform>().isLast && !_poolDictionary[tag][i].GetComponent<Platform>().isActive && !_poolDictionary[tag][i].GetComponent<Platform>().isFirst)
        //        {
        //            _poolDictionary[tag][i].gameObject.SetActive(false);
        //            _poolDictionary[tag][i].gameObject.transform.position = Vector3.zero;
        //            Debug.Log("clinULp");
        //            return _poolDictionary[tag][i];
        //        }

        //    }

        //    return GetAvailablePrifabFromDictionary(tag);
        //}
        else
        {
            return null;
        }
    }
    public void ResetAll(string tag)
    {
        for (int i = 0; i < _poolDictionary[tag].Count; i++)
        {
            _poolDictionary[tag][i].gameObject.SetActive(false);
            _poolDictionary[tag][i].gameObject.transform.position = Vector3.zero;
        }
    }
}
[System.Serializable]
public class _Pool
{
    public string tag;
    public AssetReference prifabReference;
    public int size;
}