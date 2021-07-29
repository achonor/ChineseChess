using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleObject<T> : System.Object where T : new()
{
    private static Dictionary<Type, System.Object> mInstanceDict = new Dictionary<Type, System.Object>();

    public static T Instance {
        get {
            Type classType = typeof(T);
            if (!mInstanceDict.ContainsKey(classType)) {
                mInstanceDict.Add(classType, new T());
            }
            return (T)mInstanceDict[classType];
        }
        protected set {
            Type classType = typeof(T);
            if (!mInstanceDict.ContainsKey(classType)) {
                mInstanceDict.Add(classType, value);
            } else {
                mInstanceDict[classType] = value;
            }
        }
    }
}
