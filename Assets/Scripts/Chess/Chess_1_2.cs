using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑象
/// </summary>
public class Chess_1_2 : Chess_x_2 {

    protected override void Awake() {
        base.Awake();
        IsRedChess = false;
    }
}
