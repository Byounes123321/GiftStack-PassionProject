using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftStack.Models.ViewModels
{
    public class UpdateEvent
    {
        // Items i want to be displayed
        public EventDto SelectedEvent { get; set; }

        // Items i want to be referenced
        public IEnumerable<giftDto> GiftInfo { get; set; }
    }
}