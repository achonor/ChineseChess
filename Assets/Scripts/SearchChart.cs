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

    public static sbyte[] ChessCheckOrder = new sbyte[] { 7, 8, 9, 10, 5, 6, 3, 4, 1, 2, 11, 12, 13, 14, 15, 0 };
    public class Step {
        public sbyte chessID;
        public Vector2Byte point;

        public byte searchDepth;
        /// <summary>
        /// 往后搜索searchDepth步的最优分数
        /// </summary>
        public int mostScore = -100000;

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
        runCount = 0;
        mVisited.Clear();
        Step result = null;
        Thread thread = new Thread(()=> {
            try {
                result = DfsSearch(chart, lastDepth, true, int.MinValue, int.MaxValue);
            } catch (Exception ex) {
                Debug.LogError(ex.ToString());
            }
        });
        thread.Start();
        thread.IsBackground = true;
        // 等待搜索完成
        Achonor.Scheduler.CreateScheduler("WaitSearchFinished", 1.0f, 0, 0.1f, () => {
            if (!thread.IsAlive) {
                Debug.Log(runCount);
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
                Record record0 = chart.Records[0];
                //if (record0.AChessID == 22 && record0.APoint == 19 && record0.BPoint == 27 && chessID == 7) {
                //    record0.AChessID |= 0;
                //}
                Chart newChart = chart;
                newChart.MoveChess(chessID, movePoints[k]);
                string newChartKey = newChart.GetChartKey();
                Step step;
                if (!UpdateVisited(newChartKey, lastDepth)) {
                    //已经计算过更深的
                    newChart.BackStep();
                    continue;
                } else {
                    if (lastDepth <= 0) {
                        //直接计算分数，不能往下搜索了
                        step = new Step();
                        step.SetValue(chessID, movePoints[k], lastDepth, newChart.GetScore(!newChart.IsRedPlayChess));
                    } else {
                        step = DfsSearch(newChart, (byte)(lastDepth - 1), !isMax, alpha, beta);
                        step.mostScore *= -1;
                    }
                }
                newChart.BackStep();

                if (result.mostScore < step.mostScore) {
                    result.SetValue(chessID, movePoints[k], lastDepth, step.mostScore);
                    //newChart.PrintStep();
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