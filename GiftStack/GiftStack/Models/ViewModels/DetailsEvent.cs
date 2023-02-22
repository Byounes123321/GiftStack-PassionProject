using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftStack.Models.ViewModels
{
    public class DetailsEvent
    {
        // Items i want to be displayed
        public EventDto SelectedEvent { get; set; }
        // Items i want to be refranced
        public IEnumerable<giftDto> RelatedGifts { get; set; }
    }
}