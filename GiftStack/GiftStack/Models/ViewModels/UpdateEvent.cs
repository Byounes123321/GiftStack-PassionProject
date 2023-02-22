using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftStack.Models.ViewModels
{
    public class UpdateEvent
    {
        public EventDto SelectedEvent { get; set; }

        public IEnumerable<giftDto> GiftInfo { get; set; }
    }
}