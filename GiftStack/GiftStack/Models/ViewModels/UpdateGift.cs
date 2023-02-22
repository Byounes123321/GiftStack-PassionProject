using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GiftStack.Models;


namespace GiftStack.Models.ViewModels
{
    public class UpdateGift
    {
        public giftDto SelectedGift { get; set; }

        public IEnumerable<RecipientDto> Recipient { get; set; }

        public IEnumerable<EventDto> Event { get; set; }


    }
}