using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 仕
/// </summary>
public class Chess_x_1 : ChessBase {

    protected override void Awake() {
        base.Awake();
        ChType = ChessType.Shi;
    }
}
