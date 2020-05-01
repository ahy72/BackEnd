using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    /// <summary>
    /// 仮想マシンを表します。
    /// </summary>
    public class VirtualMachine
    {
        /// <summary>
        /// IDを取得します。
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 仮想マシン名を取得します。
        /// </summary>
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
        /// <summary>
        /// ポート番号を取得します。
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 稼働状況を取得または設定します。
        /// </summary>
        public OperationStatus Operation { get; set; }
        /// <summary>
        /// 接続しているマシンを取得または設定します。
        /// </summary>
        [MaxLength(32)]
        public string ConnectedMachine { get; set; }

        /// <summary>
        /// 稼働状況を表す文字列を取得します。
        /// </summary>
        [NotMapped]
        public string OperationStr => Operation == OperationStatus.Work ? "稼働" : "停止";
        /// <summary>
        /// 接続しているマシンを表す文字列を取得します。
        /// </summary>
        [NotMapped]
        public string ConnectedMachineStr => String.IsNullOrEmpty(ConnectedMachine) ? "接続なし" : ConnectedMachine;

        /// <summary>
        /// 仮想マシンの新しいインスタンスを初期化します。
        /// </summary>
        public VirtualMachine()
        {
        }

        /// <summary>
        /// 仮想マシンの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="org">元になるエンティティ</param>
        public VirtualMachine(VirtualMachine org)
        {
            Id = org.Id;
            Name = org.Name;
            Port = org.Port;
            Operation = org.Operation;
            ConnectedMachine = org.ConnectedMachine;
        }

        public VirtualMachine Clone() => new VirtualMachine(this);
    }

    /// <summary>
    /// 仮想マシンの稼働状況を指定します。
    /// </summary>
    public enum OperationStatus : int
    {
        /// <summary>
        /// 稼働
        /// </summary>
        Work = 0,
        /// <summary>
        /// 停止
        /// </summary>
        Stop = 1,
    }
}