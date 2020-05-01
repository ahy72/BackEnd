using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    /// <summary>
    /// 接続状態を表します。
    /// </summary>
    public class ConnectionStatus
    {
        /// <summary>
        /// ポート番号を取得します。
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 接続しているマシンを取得または設定します。
        /// </summary>
        public string ConnectedMachine { get; set; }
    }
}