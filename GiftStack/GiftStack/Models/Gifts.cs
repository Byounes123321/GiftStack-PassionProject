using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.EnterpriseServices.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftStack.Models
{
    public class Gift
    {
        [Key]
        public int GiftId { get; set; }
        //primary ket
        public string GiftName { get; set; }
        //Name of the gift
        public string GiftLocation { get; set; }
        //Where the user can get the gift - can be physical location or URL (IsWellFormedUriString() --using System.Runtime.dll)
        public bool IsGiftPurchaced { get; set; }
        //Check weather gift has been purchaced

        [ForeignKey("Recipient")]
        public int RecipientId { get; set; }
        public virtual Recipient Recipient { get; set; }
        //Foreign key from recipient table, recipients can have many gifts

        [ForeignKey("Event")]
        public int EventId { get; set; }
        public virtual Event Event { get; set; } 
        //Foreign key from Event table, Events can have many gifts

    }

    public class giftDto
    {
        public int GiftId { get; set; }
        //primary ket
        public string GiftName { get; set; }
        //Name of the gift
        public string GiftLocation { get; set; }
        //Where the user can get the gift - can be physical location or URL (IsWellFormedUriString() --using System.Runtime.dll)
        public bool IsGiftPurchaced { get; set; }
        //Check weather gift has been purchaced
        public string RecipientName { get; set; }
        public string EventName { get; set; }


    }
}