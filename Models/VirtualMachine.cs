using System;

namespace BackEnd.Controllers
{
    /// <summary>
    /// 仮想マシンを表します。
    /// </summary>
    public class VirtualMachine
    {
        /// <summary>
        /// IDを取得します。
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// 仮想マシン名を取得します。
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// ポート番号を取得します。
        /// </summary>
        public int Port { get; }
        /// <summary>
        /// 稼働状況を取得または設定します。
        /// </summary>
        public OperationStatus Operation { get; set; }
        /// <summary>
        /// 接続しているマシンを取得または設定します。
        /// </summary>
        public string ConnectedMachine { get; set; }

        /// <summary>
        /// 稼働状況を表す文字列を取得します。
        /// </summary>
        public string OperationStr
        {
            get { return Operation == OperationStatus.Work ? "稼働" : "停止"; }
        }
        /// <summary>
        /// 接続しているマシンを表す文字列を取得します。
        /// </summary>
        public string ConnectedMachineStr
        {
            get { return String.IsNullOrEmpty(ConnectedMachine) ? "接続なし" : ConnectedMachine; }
        }

        /// <summary>
        /// 仮想マシンの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">仮想マシン名</param>
        /// <param name="port">ポート番号</param>
        public VirtualMachine(int id, string name, int port)
        {
            Id = id;
            Name = name;
            Port = port;
            Operation = OperationStatus.Stop;
        }

        /// <summary>
        /// 仮想マシン状態を表す文字列を取得します。
        /// </summary>
        /// <returns>仮想マシン状態を表す文字列</returns>
        public string ToStatusString()
        {
            var items = new string[]
            {
                Id.ToString(),
                ((int)(Operation)).ToString(),
                ConnectedMachine
            };

            return String.Join("\t", items);
        }
    }

    /// <summary>
    /// 仮想マシンの稼働状況を指定します。
    /// </summary>
    public enum OperationStatus
    {
        /// <summary>
        /// 稼働
        /// </summary>
        Work,
        /// <summary>
        /// 停止
        /// </summary>
        Stop,
    }
}