using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 记录操作的类
/// 表示A棋子从A点走到B点吃掉了B棋子
/// </summary>
public class Record {
    public sbyte AChessID;
    public sbyte APoint;
    public sbyte BChessID;
    public sbyte BPoint;
}
