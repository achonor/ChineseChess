using Achonor;
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

    public const int MAX_DEPTH = 64;

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
    private Dictionary<ulong, int> mVisited = new Dictionary<ulong, int>();
    private Dictionary<ulong, int> mVisitedScore = new Dictionary<ulong, int>();

    private static int runCount = 0;

    //private static int[] AlphaStep = new int[] { 0, -40, 41, -80, 81, MIN_VALUE };
    //private static int[] BetaStep = new int[] { 40, -1, 80, -41, MAX_VALUE, -81 };

    private static int[] AlphaStep = new int[] { MIN_VALUE, MIN_VALUE };
    private static int[] BetaStep = new int[] { MAX_VALUE, MAX_VALUE };

    public static void Search(Chart chart, Action<Step> callback) {
        runCount = 0;
        Step result = null;
        new Thread(() => {
            //需要等待的线程数量
            int lastThreadCount = AlphaStep.Length;
            int curScore = chart.GetScore(chart.IsRedPlayChess);
            for (int i = 0; i < AlphaStep.Length; i++) {
                ThreadSearch(new Chart(chart), curScore + AlphaStep[i], curScore + BetaStep[i], (param) => {
                    lastThreadCount--;
                    if (null == result || result.mostScore < param.mostScore) {
                        result = param;
                    }
                });
            }
            while (0 < lastThreadCount) {
                //等待所有线程完成
            }
            Debug.LogFormat("深度：{0} 结果：{1} 搜索局面数：{2}", result.searchDepth, result.mostScore, runCount);
            Scheduler.MainThreadRun(() => {
                Function.CallCallback(callback, result);
            });
        }).Start();
    }

    /// <summary>
    /// 开启线程搜索
    /// </summary>
    /// <param name="chart"></param>
    /// <param name="depth"></param>
    /// <param name="alpha"></param>
    /// <param name="beta"></param>
    /// <param name="callback"></param>
    public static void ThreadSearch(Chart chart, int alpha, int beta, Action<Step> callback) {
        int curDepth = GameConst.Instance.MinSearchDepth - 1;
        Thread thread = new Thread(() => {
            Step result = null;
            try {
                long startTime = Function.GetLocaLTime();
                while (curDepth < MAX_DEPTH) {
                    curDepth++;
                    result = new SearchChart().SearchRoot(chart, curDepth, alpha, beta);
                    if (GameConst.Instance.MaXSearchDuration < (Function.GetLocaLTime() - startTime)) {
                        break;
                    }
                }
            } catch (Exception ex) {
                Debug.LogError(ex.ToString());
            }
            Function.CallCallback(callback, result);
        });
        thread.IsBackground = true;
        thread.Start();
    }

    public Step SearchRoot(Chart chart, int depth, int alpha, int beta) {
        Step result = new Step();
        int bestScore = alpha;
        //获取所有移动步骤
        List<MovePoint> movePoints = chart.GetAllMovePoints(chart.IsRedPlayChess);
        for (int i = 0; i < movePoints.Count; i++) {
            MovePoint move = movePoints[i];
            chart.MoveChess(move.ChessID, move.PointKey);
            int curScore;
            if (alpha == bestScore) {
                curScore = -DfsSearch(chart, depth - 1, alpha, beta, true);
            } else {
                //尝试空步裁剪
                curScore = -DfsSearch(chart, depth - 1, -bestScore - 1, -bestScore);
                if (bestScore < curScore) {
                    //验证空不裁剪
                    curScore = -DfsSearch(chart, depth - 1, alpha, -bestScore, true);
                }
            }
            chart.BackStep();
            if (bestScore < curScore) {
                bestScore = curScore;
                result.SetValue(movePoints[i].ChessID, movePoints[i].PointKey, depth, bestScore);
            }
        }
        return result;
    }


    public int DfsSearch(Chart chart, int lastDepth, int alpha, int beta, bool NoNULL = false) {
        int result = MIN_VALUE;
         if (lastDepth <= 0) {
            runCount++;
            return chart.GetScore(chart.IsRedPlayChess);
            //return DfsLimit(chart, 0, alpha, beta);
        }
        //空步裁剪
        if ((!NoNULL) && chart.NullOkay()) {
            chart.NullMove();
            int value = -DfsSearch(chart, lastDepth - 3, -beta, 1 - beta, true);
            chart.BackNullMove();
            if (beta <= value) {
                return value;
            }
        }

        List<MovePoint> movePoints = chart.GetAllMovePoints(chart.IsRedPlayChess);
        for (int i = 0; i < movePoints.Count; i++) {
            int stepResult;
            MovePoint move = movePoints[i];
            chart.MoveChess(move.ChessID, move.PointKey);
            ulong newChartKey = chart.GetChartKey();
            if (mVisited.ContainsKey(newChartKey) && lastDepth - 1 <= mVisited[newChartKey]) {
                //已经计算过更深的
                stepResult = -mVisitedScore[newChartKey];
            } else if (-1 == chart.GetChessPoint(BoardTools.GetChessID(0, chart.IsRedPlayChess))) {
                //吃掉将
                stepResult = MAX_VALUE;
            }else {
                if (MIN_VALUE == result) {
                    stepResult = -DfsSearch(chart, (lastDepth - 1), -beta, -alpha, true);
                } else {
                    stepResult = -DfsSearch(chart, (lastDepth - 1), -alpha - 1, -alpha);
                    if (alpha < stepResult && stepResult < beta) {
                        stepResult = -DfsSearch(chart, (lastDepth - 1), -beta, -alpha, true);
                    }
                }
            }
            chart.BackStep();
            if (result < stepResult) {
                result = stepResult;
            }
            if (alpha < stepResult) {
                alpha = stepResult;
                if (beta <= alpha) {
                    //剪枝
                    return result;
                }
            }
        }
        ulong chartKey = chart.GetChartKey();
        if (!mVisited.ContainsKey(chartKey) || mVisited[chartKey] < lastDepth) {
            mVisited.Update(chartKey, lastDepth);
            mVisitedScore.Update(chartKey, result);
        }
        return result;
    }

    /// <summary>
    /// 最后的极限搜索，只搜索吃子和将军的情况
    /// </summary>
    /// <returns></returns>
    public static int DfsLimit(Chart chart, int curDepth, int alpha, int beta) {
        if (-1 == chart.GetChessPoint(BoardTools.GetChessID(0, chart.IsRedPlayChess))) {
            return MIN_VALUE;
        }
        int result = chart.GetScore(chart.IsRedPlayChess);
        if (curDepth == 2) {
            return result;
        }
        List<MovePoint> movePoints;
        if (chart.IsBeJiangJun()) {
            //被将军生成所有能走的
            movePoints = chart.GetAllMovePoints(chart.IsRedPlayChess);
        } else {
            if (alpha < result) {
                alpha = result;
                if (beta <= alpha) {
                    //剪枝
                    return result;
                }
            }
            //生成吃子走法，后期看能不能加上将军的走法
            movePoints = chart.GetAllMovePoints(chart.IsRedPlayChess, true);
        }
        result = MIN_VALUE;
        for (int i = 0; i < movePoints.Count; i++) {
            MovePoint move = movePoints[i];
            chart.MoveChess(move.ChessID, move.PointKey);
            int stepResult = -DfsLimit(chart, curDepth++, -beta, -alpha);
            chart.BackStep();
            if (result < stepResult) {
                result = stepResult;
            }
            if (alpha < stepResult) {
                alpha = stepResult;
                if (beta <= alpha) {
                    //剪枝
                    return result;
                }
            }
        }
        return result;
    }
}