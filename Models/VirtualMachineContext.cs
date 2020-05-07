using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Models
{
	public class VirtualMachineContext : DbContext
	{
		public DbSet<VirtualMachine> VirtualMachines { get; set; }
		public DbSet<RefreshTime> RefreshTime { get; set; }

		public VirtualMachineContext(DbContextOptions<VirtualMachineContext> options)
			: base(options)
		{
		}

		public async Task<IEnumerable<VirtualMachine>> GetNewVirtualMachines()
		{
			var machines = await VirtualMachines.ToListAsync();
			var connections = GetConnectionStatus()?.ToDictionary(target => target.Port) ?? new Dictionary<int, ConnectionStatus>();

			foreach (var machine in machines)
			{
				if (connections.TryGetValue(machine.Port, out ConnectionStatus connection) == false)
				{
					machine.Operation = OperationStatus.Stop;
					machine.ConnectedMachine = null;
					continue;
				}

				machine.ConnectedMachine = connection.ConnectedMachine;
			}

			return machines;
		}

		/// <summary>
		/// 仮想マシン状態を取得します。
		/// </summary>
		/// <returns>仮想マシン状態の列挙子</returns>
		public static IEnumerable<ConnectionStatus> GetConnectionStatus()
		{
			var commandOutputLines = GetCommandOutputLines("netstat", "-a");
			if (commandOutputLines == null)
			{
				return null;
			}

			var connections = new Dictionary<int, ConnectionStatus>();
			// 以下のような行が含まれていれば、稼働状態と判断
			// "TCP         [::]:13393             servername:0                 LISTENING"
			foreach (var line in commandOutputLines)
			{
				if (line.Contains("TCP") == false || line.Contains("LISTENING") == false)
				{
					continue;
				}

				var items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				if (items.Length < 2)
				{
					continue;
				}

				// 2つ目の項目からポート番号を取り出す
				string portStr = items[1].Split(':').LastOrDefault();
				int port = 0;
				if (String.IsNullOrEmpty(portStr) == true || Int32.TryParse(portStr, out port) == false)
				{
					continue;
				}

				if (connections.ContainsKey(port) == true)
				{
					continue;
				}

				connections.Add(port, new ConnectionStatus() { Port = port });
			}

			// 以下のような行が含まれていれば、接続ありと判断
			// "TCP         172.16.2.196:13389     clientname:3651         ESTABLISHED"
			foreach (var line in commandOutputLines)
			{
				if (line.Contains("TCP") == false || line.Contains("ESTABLISHED") == false)
				{
					continue;
				}

				// 1行を項目に分割
				var items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				if (items.Length < 2)
				{
					continue;
				}

				// 2つ目の項目からポート番号を取り出す
				string portStr = items[1].Split(':').LastOrDefault();
				int port = 0;
				if (String.IsNullOrEmpty(portStr) == true || Int32.TryParse(portStr, out port) == false)
				{
					continue;
				}

				// 該当するポート番号があれば、接続しているPC名の取得を試みる
				if (connections.TryGetValue(port, out ConnectionStatus connection) == true)
				{
					if (items.Length < 3)
					{
						connection.ConnectedMachine = "不明";
						continue;
					}

					// 3つ目の項目からPC名を取り出す
					string connectedMachine = items[2].Split(':').FirstOrDefault();
					if (String.IsNullOrEmpty(portStr) == true)
					{
						connection.ConnectedMachine = "不明";
						continue;
					}

					connection.ConnectedMachine = connectedMachine;
				}
			}

			return connections.Values;
		}

		/// <summary>
		/// コマンドを実行し、その結果を返します。
		/// </summary>
		/// <param name="commandName">コマンド</param>
		/// <param name="arguments">コマンドライン引数</param>
		/// <returns>結果の文字列 失敗した場合、null</returns>
		private static List<string> GetCommandOutputLines(string commandName, string arguments)
		{
            // TODO 仮修正
            return null;

			var result = new List<string>();
			var lockObject = new object();

			try
			{
				using (var process = new Process())
				{
					process.StartInfo = new ProcessStartInfo(commandName, arguments)
					{
						RedirectStandardInput = true,
						RedirectStandardError = true,
						RedirectStandardOutput = true,
						UseShellExecute = false,
						CreateNoWindow = true
					};

					var writeRedirect = (DataReceivedEventHandler)((sender, e) =>
					{
						if (String.IsNullOrEmpty(e.Data) == false)
						{
							lock (lockObject)
							{
								result.Add(e.Data);
							}
						}
					});

					process.ErrorDataReceived += writeRedirect;
					process.OutputDataReceived += writeRedirect;

					process.Start();
					process.BeginOutputReadLine();
					process.BeginErrorReadLine();
					process.WaitForExit();

					if (process.ExitCode != 0)
					{
						// 処理に失敗したらnull
						return null;
					}
				}
			}
			catch
			{
				return null;
			}

			return result;
		}
	}
}