using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftStack.Models.ViewModels
{
    public class DetailsRecipient
    {

        // Items i want to be displayed
        public RecipientDto SelectedRecipient { get; set; }
        
        // Items i want to be refranced
        public IEnumerable<giftDto> GiftsRecived { get; set; }
    }
}