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

    private static Step Result = null;
    /// <summary>
    /// 搜索过的状态标记,byte表示搜索深度
    /// </summary>
    private static Dictionary<ulong, int> mVisited = new Dictionary<ulong, int>();

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
                    result = SearchRoot(chart, curDepth);
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

    public static Step SearchRoot(Chart chart, int depth) {
        Step result = new Step();
        int bestScore = MIN_VALUE;
        //获取所有移动步骤
        List<MovePoint> movePoints = chart.GetAllMovePoints(chart.IsRedPlayChess);
        for (int i = 0; i < movePoints.Count; i++) {
            int curScore;
            if (MIN_VALUE == bestScore) {
                curScore = -DfsSearch(chart, depth - 1, false, MIN_VALUE, MAX_VALUE, true);
            } else {
                curScore = DfsSearch(chart, depth - 1, false, -bestScore - 1, -bestScore);
                if (bestScore < curScore) {
                    curScore = -DfsSearch(chart, depth - 1, false, MIN_VALUE, -bestScore, true);
                }
            }
            if (bestScore < curScore) {
                bestScore = curScore;
                result.SetValue(movePoints[i].ChessID, movePoints[i].PointKey, depth, bestScore);
            }
        }
        return result;
    }


    public static int DfsSearch(Chart chart, int lastDepth, bool isMax, int alpha, int beta, bool NoNULL = false) {
        int result = MAX_VALUE;
        if (-1 == chart.GetChessPoint((chart.IsRedPlayChess ? 0 : 16))) {
            return result;
        }
        //空步裁剪
        if ((!NoNULL) && beta != int.MaxValue && chart.NullOkay()) {
            chart.NullMove();
            int value = -DfsSearch(chart, lastDepth - 3, !isMax, -beta, 1 - beta, true);
            chart.BackNullMove();
            if (beta <= value) {
                return value;
            }
        }

        List<MovePoint> movePoints = chart.GetAllMovePoints(chart.IsRedPlayChess);
        for (int k = 0; k < movePoints.Count; k++) {
            MovePoint move = movePoints[k];
            chart.MoveChess(move.ChessID, move.PointKey);
            ulong newChartKey = chart.GetChartKey();
            int stepResult;
            if (!UpdateVisited(newChartKey, lastDepth)) {
                //已经计算过更深的
                chart.BackStep();
                continue;
            } else {
                if (lastDepth <= 0) {
                    runCount++;
                    //直接计算分数，不能往下搜索了
                    stepResult = chart.GetScore(!chart.IsRedPlayChess);
                } else {
                    stepResult = -DfsSearch(chart, (byte)(lastDepth - 1), !isMax, alpha, beta, NoNULL);
                }
            }
            chart.BackStep();

            if (result < stepResult) {
                result = stepResult;
            }
            if (isMax) {
                alpha = Math.Max(alpha, stepResult);
            } else {
                beta = Math.Min(beta, -stepResult);
            }
            if (beta <= alpha) {
                //剪枝
                return result;
            }
        }
        return result;
    }
}