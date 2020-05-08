using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models
{
    /// <summary>
    /// メッセージを表します。
    /// </summary>
    public class Message
    {
        /// <summary>
        /// IDを取得します。
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// メッセージ文字列を取得します。
        /// </summary>
        [Required]
        public string Text { get; set; }
    }
}