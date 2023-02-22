using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace GiftStack.Models
{
    public class Recipient
    {
        [Key]
        public int RecipientId { get; set; }
        //The primary key
        public string RecipientName { get; set; }
        //The name of the Recipient

        public ICollection<Gift> Gifts { get; set; }


    }
    public class RecipientDto
    {
        public int RecipientId { get; set; }
        //primary ket
        public string RecipientName { get; set; }
        //Name of the recipient 


    }
}