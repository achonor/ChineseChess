using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


/// <summary>
/// 搜索最优走法
/// </summary>
public class SearchChart {
    public const sbyte SByte_0 = (sbyte)0;
    public const sbyte SByte_1 = (sbyte)1;
    public const sbyte SByte_16 = (sbyte)16;

    public class Step {
        public sbyte chessID;
        public Vector2Byte point;

        public byte searchDepth;
        /// <summary>
        /// 往后搜索searchDepth步的最优分数
        /// </summary>
        public int mostScore = int.MinValue;

        public void SetValue(sbyte _chessID, Vector2Byte _point, byte _searchDepth, int _mostScore) {
            this.chessID = _chessID;
            this.point = _point;
            this.searchDepth = _searchDepth;
            this.mostScore = _mostScore;
        }
    }
    /// <summary>
    /// 搜索过的状态标记,byte表示搜索深度
    /// </summary>
    private static Dictionary<string, byte> mVisited = new Dictionary<string, byte>();

    private static Dictionary<string, int> mVisitedScore = new Dictionary<string, int>();

    public static bool UpdateVisited(string chartKey, byte depth) {
        lock (mVisited) {
            if (mVisited.ContainsKey(chartKey) && depth <= mVisited[chartKey]) {
                return false;
            }
            Achonor.Function.Update(mVisited, chartKey, depth);
        }
        return true;
    }
    private static int runCount = 0;
    public static void Search(Chart chart, byte lastDepth, Action<Step> callback) {
        if (true) {
            runCount = 0;
            Step result1 =  DfsSearch(chart, lastDepth, true, int.MinValue, int.MaxValue);
            Debug.Log(runCount);
            Achonor.Function.CallCallback(callback, result1);
            return;
        }

        Step result = new Step();
        mVisited.Clear();
        int threadCount = 1;
        for (sbyte i = 0; i < 16; i++) {
            sbyte chessID = (sbyte)(i | (chart.IsRedPlayChess ? SByte_0 : SByte_16));
            List<Vector2Byte> movePoints = chart.GetMovePoints(chessID);
            for (int k = 0; k < movePoints.Count; k++) {
                Chart newChart = Chart.Clone(chart);
                newChart.MoveChess(chessID, movePoints[k]);
                Step newStep = new Step();
                newStep.SetValue(chessID, movePoints[k], lastDepth, int.MinValue);

                threadCount++;
                new Thread(() => {
                    SearchChart search = new SearchChart();
                    Step step = DfsSearch(newChart, (byte)(lastDepth - 1), false, int.MinValue, int.MaxValue);
                    step.mostScore *= -1;
                    if (result.mostScore < step.mostScore) {
                        result = newStep;
                        result.mostScore = step.mostScore;
                    }
                    threadCount--;
                }).Start();
            }
        }
        threadCount--;
        // 等待搜索完成
        Achonor.Scheduler.CreateScheduler("WaitSearchFinished", 1.0f, 0, 0.1f, () => {
            if (0 == threadCount) {
                Achonor.Function.CallCallback(callback, result);
                Achonor.Scheduler.Stop("WaitSearchFinished");
            }
        });
    }


    public static Step DfsSearch(Chart chart, byte lastDepth, bool isMax, int alpha, int beta) {
        Step result = new Step();
        result.searchDepth = lastDepth;
        if (null == chart.GetChessPoint((sbyte)(chart.IsRedPlayChess ? 0 : 16))) {
            return result;
        }
        
        for (sbyte i = 0; i < 16; i++) {
            sbyte chessID = (sbyte)(i | (chart.IsRedPlayChess ? SByte_0 : SByte_16));
            List<Vector2Byte> movePoints = chart.GetMovePoints(chessID);
            for (int k = 0; k < movePoints.Count; k++) {
                runCount++;
                Chart newChart = Chart.Clone(chart);
                newChart.MoveChess(chessID, movePoints[k]);
                string newChartKey = newChart.GetChartKey();
                Step step;
                if (!UpdateVisited(newChartKey, lastDepth)) {
                    //已经计算过更深的
                    continue;
                } else {
                    if (lastDepth <= 0) {
                        //直接计算分数，不能往下搜索了
                        step = new Step();
                        step.SetValue(chessID, movePoints[k], lastDepth, newChart.GetScore(chart.IsRedPlayChess));
                    } else {
                        step = DfsSearch(newChart, (byte)(lastDepth - 1), !isMax, alpha, beta);
                        step.mostScore *= -1;
                    }
                }
                if (result.mostScore < step.mostScore) {
                    result.SetValue(chessID, movePoints[k], lastDepth, step.mostScore);
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
        }
        return result;
    }
}