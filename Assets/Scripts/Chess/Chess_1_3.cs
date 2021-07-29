using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑马
/// </summary>
public class Chess_1_3 : Chess_x_3 {


    protected override void Awake() {
        base.Awake();
        IsRedChess = false;
    }
}
