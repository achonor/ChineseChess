using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConst : SingleObject<GameConst>
{
    /// <summary>
    /// 最低搜索深度
    /// </summary>
    public int MinSearchDepth = 7;
    /// <summary>
    /// 最长搜索时间
    /// </summary>
    public int MaXSearchDuration = 6000;
}
