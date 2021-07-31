using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : SingleInstance<PrefabManager>
{
    [SerializeField]
    private Transform mPool;

    private Dictionary<GameObject, string> mPrefab2ResPath = new Dictionary<GameObject, string>();
    private Dictionary<string, List<GameObject>> mPoolPrefabDict = new Dictionary<string, List<GameObject>>();


    public GameObject LoadPrefab(string resPath) {
        GameObject result;
        List<GameObject> prefabs;
        if (mPoolPrefabDict.ContainsKey(resPath)) {
            prefabs = mPoolPrefabDict[resPath];
            if (0 < prefabs.Count) {
                result = prefabs[0];
                prefabs.RemoveAt(0);
                return result;
            }
        }else{
            prefabs = new List<GameObject>();
            mPoolPrefabDict.Add(resPath, prefabs);
        }
        //–Ë“™º”‘ÿ
        GameObject res = Resources.Load<GameObject>(resPath);
        result = GameObject.Instantiate(res);
        mPrefab2ResPath.Add(result, resPath);
        return result;
    }

    public void RemovePrefab(GameObject gameObject) {
        gameObject.transform.SetParent(mPool);
        string resPath = mPrefab2ResPath[gameObject];
        mPoolPrefabDict[resPath].Add(gameObject);
    }
}
