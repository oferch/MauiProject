using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;
namespace MauiProject.Models
{
    [Table("Favorite")]

    public class Favorite
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(User))]
        public int UserID { get; set; }

        [ForeignKey(typeof(Product))]
        public int ProductID { get; set; }
    }
}
