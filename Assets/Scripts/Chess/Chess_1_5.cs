using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑炮
/// </summary>
public class Chess_1_5 : Chess_x_5 {
    protected override void Awake() {
        base.Awake();
        IsRedChess = false;
    }
}
