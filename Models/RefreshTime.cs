using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    /// <summary>
    /// 更新時間を表します。
    /// </summary>
    public class RefreshTime
    {
        /// <summary>
        /// IDを取得します。
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 更新時間を取得します。
        /// </summary>
        [Required]
        public DateTime Time { get; set; }
    }
}