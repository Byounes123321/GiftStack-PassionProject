using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftStack.Models.ViewModels
{
    public class DetailsGift
    {
        // Items i want to be displayed
        public giftDto SelectedGift { get; set; }
        
        // Items i want to be refranced
        public IEnumerable<RecipientDto> RelatedRecipient { get; set; }
        public IEnumerable<EventDto> RelatedEvent { get; set; }
    }
}