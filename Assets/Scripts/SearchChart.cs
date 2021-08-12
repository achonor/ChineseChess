using Assets.Scripts;
using Assets.Scripts.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


/// <summary>
/// 搜索最优走法
/// </summary>
public class SearchChart {
    public const int SByte_0 = 0;
    public const int SByte_1 = 1;
    public const int SByte_16 = 16;

    public const int MAX_VALUE = 100000;
    public const int MIN_VALUE = -100000;

    public class Step {
        public int chessID;
        public int point;

        public int searchDepth;
        /// <summary>
        /// 往后搜索searchDepth步的最优分数
        /// </summary>
        public int mostScore = MIN_VALUE;

        public void SetValue(int _chessID, int _point, int _searchDepth, int _mostScore) {
            this.chessID = _chessID;
            this.point = _point;
            this.searchDepth = _searchDepth;
            this.mostScore = _mostScore;
        }
    }
    /// <summary>
    /// 搜索过的状态标记,byte表示搜索深度
    /// </summary>
    private static Dictionary<ulong, int> mVisited = new Dictionary<ulong, int>();

    private static Dictionary<string, int> mVisitedScore = new Dictionary<string, int>();

    public static bool UpdateVisited(ulong chartKey, int depth) {
        if (mVisited.ContainsKey(chartKey) && depth <= mVisited[chartKey]) {
            return false;
        }
        return true;
    }
    private static int runCount = 0;
    public static void Search(Chart chart, Action<Step> callback) {
        runCount = 0;
        mVisited.Clear();
        Step result = null;
        Thread thread = new Thread(()=> {
            try {
                byte curDepth = 3;
                long startTime = Achonor.Function.GetLocaLTime();
                while (curDepth <= 64) {
                    result = DfsSearch(chart, curDepth, true, MIN_VALUE, MAX_VALUE);
                    long newTime = Achonor.Function.GetLocaLTime();
                    if (15000 < (newTime - startTime)) {
                        break;
                    }
                    curDepth++;
                }
                Debug.LogFormat("搜索深度：{0} 搜索局面数：{1}", (curDepth + 1), runCount);
            } catch (Exception ex) {
                Debug.LogError(ex.ToString());
            }
        });
        thread.Start();
        thread.IsBackground = true;
        // 等待搜索完成
        Achonor.Scheduler.CreateScheduler("WaitSearchFinished", 1.0f, 0, 0.5f, () => {
            if (!thread.IsAlive) {
                Debug.LogFormat("棋子：{0}走到{1}", result.chessID, result.point);
                Achonor.Function.CallCallback(callback, result);
                Achonor.Scheduler.Stop("WaitSearchFinished");
            }
        });
    }


    public static Step DfsSearch(Chart chart, int lastDepth, bool isMax, int alpha, int beta, bool NoNULL = false) {
        Step result = new Step();
        result.searchDepth = lastDepth;
        if (-1 == chart.GetChessPoint((chart.IsRedPlayChess ? 0 : 16))) {
            return result;
        }
        //空步裁剪
        //if ((!NoNULL) && beta != int.MaxValue && chart.NullOkay()) {
        //    chart.NullMove();
        //    Step value = DfsSearch(chart, lastDepth - 3, !isMax, -beta, 1 - beta, true);
        //    value.mostScore *= -1;
        //    chart.BackNullMove();
        //    if (value.mostScore >= beta) {
        //        return value;
        //    }
        //}


        List<MovePoint> movePoints = chart.GetAllMovePoints(chart.IsRedPlayChess);
        for (int k = 0; k < movePoints.Count; k++) {
            MovePoint move = movePoints[k];
            chart.MoveChess(move.ChessID, move.PointKey);
            ulong newChartKey = chart.GetChartKey();
            Step step;
            if (!UpdateVisited(newChartKey, lastDepth)) {
                //已经计算过更深的
                chart.BackStep();
                continue;
            } else {
                if (lastDepth <= 0) {
                    runCount++;
                    //直接计算分数，不能往下搜索了
                    step = new Step();
                    step.SetValue(move.ChessID, move.PointKey, lastDepth, chart.GetScore(!chart.IsRedPlayChess));
                } else {
                    step = DfsSearch(chart, (byte)(lastDepth - 1), !isMax, alpha, beta, NoNULL);
                    step.mostScore *= -1;
                }
            }
            chart.BackStep();

            if (result.mostScore < step.mostScore) {
                result.SetValue(move.ChessID, move.PointKey, lastDepth, step.mostScore);
            }
            if (isMax) {
                alpha = Math.Max(alpha, step.mostScore);
            } else {
                beta = Math.Min(beta, -step.mostScore);
            }
            if (beta <= alpha) {
                //剪枝
                return result;
            }
        }
        return result;
    }
}