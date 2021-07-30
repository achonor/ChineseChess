using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts {
    public class Board  : SingleInstance<Board>{
        [SerializeField]
        private List<ChessBase> AllChess = new List<ChessBase>();

        /// <summary>
        /// 是否红方下
        /// </summary>
        private bool mIsRedPlayChess = true;

        private ChessBase SelectedChess = null;
    }
}
