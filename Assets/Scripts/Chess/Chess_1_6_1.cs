using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑7路兵
/// </summary>
public class Chess_1_6_1 : Chess_1_6 {
    protected override void Awake() {
        base.Awake();
        SetPosPoint(new Vector2Byte(2, 2));
    }
}
