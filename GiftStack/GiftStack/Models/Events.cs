using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GiftStack.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        //Primary Key
        public string EventName { get; set; }
        //The name of the event
        public DateTime EventDate { get; set; }
        //The event date
        public string EventColor { get; set; }
        //The color coding of the event

        //an event has many gifts
        public ICollection<Gift> Gifts { get; set; }

    }
    public class EventDto
    {
        public string EventColor { get; set; }
        public DateTime EventDate { get; set; }
        public string EventName { get; set; }
        public int EventId { get; set; }
    }

}