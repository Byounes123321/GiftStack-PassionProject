using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace GiftStack.Models.ViewModels
{
    public class NewGift
    {
        public giftDto Addgift { get; set; }
        public IEnumerable<RecipientDto> Recipients { get; set; }
        public IEnumerable<EventDto> Events { get; set; }


    }
}