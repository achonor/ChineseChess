using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    private static Dictionary<Type, MonoBehaviour> mInstanceDict = new Dictionary<Type, MonoBehaviour>();

    public static T Instance {
        get {
            Type classType = typeof(T);
            if (!mInstanceDict.ContainsKey(classType)) {
                GameObject newGo = new GameObject(classType.ToString());
                return newGo.AddComponent<T>();
            }
            return (T)mInstanceDict[classType];
        }
    }

    protected virtual void Awake() {
        Type classType = typeof(T);
        if (!mInstanceDict.ContainsKey(classType)) {
            mInstanceDict.Add(classType, this);
        }
        else {
            mInstanceDict[classType] = this;
        }
    }
}
