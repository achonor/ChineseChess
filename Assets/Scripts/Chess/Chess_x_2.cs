using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 相
/// </summary>
public class Chess_x_2 : ChessBase {

    protected override void Awake() {
        base.Awake();
        ChType = ChessType.Xiang;
    }
}
