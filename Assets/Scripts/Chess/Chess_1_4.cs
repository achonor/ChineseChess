using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑车
/// </summary>
public class Chess_1_4 : Chess_x_4 {
    protected override void Awake() {
        base.Awake();
        IsRedChess = false;
    }
}
