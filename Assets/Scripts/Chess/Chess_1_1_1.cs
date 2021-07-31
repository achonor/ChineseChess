using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑右士
/// </summary>
public class Chess_1_1_1 : Chess_1_1 {

    protected override void Awake() {
        base.Awake();
        SetPosPoint(new Vector2Byte(-1, 5));
    }
}
