using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑卒
/// </summary>
public class Chess_1_6 : Chess_x_6 {
    protected override void Awake() {
        base.Awake();
        IsRedChess = false;
    }
}
