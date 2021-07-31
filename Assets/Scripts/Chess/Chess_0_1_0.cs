using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红左仕
/// </summary>
public class Chess_0_1_0 : Chess_0_1 {

    protected override void Awake() {
        base.Awake();
        SetPosPoint(new Vector2Byte(-1, -4));
    }
}
