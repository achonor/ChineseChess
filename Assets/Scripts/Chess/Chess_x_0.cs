using Assets.Scripts;
using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 帅将
/// </summary>
public class Chess_x_0 : ChessBase {

    protected override void Awake() {
        base.Awake();
        ChType = ChessType.Shuai;
    }
}
