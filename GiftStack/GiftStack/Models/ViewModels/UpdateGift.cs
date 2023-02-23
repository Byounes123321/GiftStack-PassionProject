using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GiftStack.Models;


namespace GiftStack.Models.ViewModels
{
    public class UpdateGift
    {
        //details i want to be displayed
        public giftDto SelectedGift { get; set; }
        //details i want to be referenced
        public IEnumerable<RecipientDto> Recipient { get; set; }

        public IEnumerable<EventDto> Event { get; set; }


    }
}