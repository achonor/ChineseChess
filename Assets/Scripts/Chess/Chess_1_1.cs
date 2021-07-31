using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑士
/// </summary>
public class Chess_1_1 : Chess_x_1 {

    protected override void Awake() {
        base.Awake();
        IsRedChess = false;
    }
}
