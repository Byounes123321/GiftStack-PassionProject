using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GiftStack.Models;
using System.Diagnostics;
using GiftStack.Migrations;
using Event = GiftStack.Models.Event;

namespace GiftStack.Controllers
{
    public class EventDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// Returns all events in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all events in the database, including their associated gifts.
        /// </returns>
        /// <example>
        /// GET: /api/EventData/listEvents
        /// </example>

        [HttpGet]
        [ResponseType(typeof(EventDto))]
        public IHttpActionResult ListEvents()
        {
            List<Event> events = db.events.ToList();
            List<EventDto> eventDtos = new List<EventDto>();

            events.ForEach(e => eventDtos.Add(new EventDto()
            {
                EventId = e.EventId,
                EventName = e.EventName,
                EventDate = e.EventDate,
                EventColor = e.EventColor

            }));

            return Ok(eventDtos);
        }


        /// <summary>
        /// Returns all events in the system associated with a particular gift.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all events in the database are reciving a gift
        /// </returns>
        /// <param name="id">Gift Primary Key</param>
        /// <example>
        /// GET: /api/EventData/ListEventForGift/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(EventDto))]
        public IHttpActionResult ListEventForGift(int id)
        {
            List<Event> events = db.events.Where(
                e => e.Gifts.Any(
                    g => g.GiftId == id)
                ).ToList();
            List<EventDto> eventDtos = new List<EventDto>();

            events.ForEach(e => eventDtos.Add(new EventDto()
            {
                EventId = e.EventId,
                EventName = e.EventName,
                EventDate = e.EventDate,
                EventColor = e.EventColor
            }));

            return Ok(eventDtos);
        }


        /// <summary>
        /// Returns all events in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An event in the system matching up to the event ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the event</param>
        /// <example>
        /// GET: /api/EventData/FindEvent/2
        /// </example>
        [ResponseType(typeof(EventDto))]
        [HttpGet]
        public IHttpActionResult FindEvent(int id)
        {
            Event events = db.events.Find(id);
            EventDto eventDto = new EventDto()
            {
                EventId = events.EventId,
                EventColor = events.EventColor,
                EventDate = events.EventDate,
                EventName = events.EventName
            };
            if (events == null)
            {
                return NotFound();
            }

            return Ok(eventDto);
        }

        /// <summary>
        /// Updates a particular event in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the event ID primary key</param>
        /// <param name="Event">JSON FORM DATA of an event</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: /api/EventData/UpdateEvent/1
        /// FORM DATA: Keeper JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateEvent(int id, Event Event)

        {
            Debug.WriteLine("I have reached the update event method");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Modle State is invalid");
                return BadRequest(ModelState);
            }
            Debug.WriteLine("EVENT DATA: ", Event.EventColor);

            if (id != Event.EventId)
            {
                Debug.WriteLine("id missmatch");
                Debug.WriteLine("GET param " + id);
                Debug.WriteLine("POST param " + Event.EventId);

                return BadRequest();
            }

            db.Entry(Event).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    Debug.WriteLine("event not found");

                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            Debug.WriteLine("none of the conditions triggerd");
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: /api/EventData/AddEvent
        [ResponseType(typeof(Event))]
        [HttpPost]
        public IHttpActionResult AddEvent(Event Event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.events.Add(Event);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Event.EventId }, Event);
        }


        // POST: api/EventData/DeleteEvent/5
        [Route("api/EventData/DeleteEvent/{id}")]
        [ResponseType(typeof(Event))]
        [HttpPost]
        public IHttpActionResult DeleteEvent(int id)
        {
            Debug.WriteLine("Reached delete event function");
            Event Event = db.events.Find(id);
            if(Event == null)
            {
                Debug.WriteLine("The id " + id + " does not exist");
            return NotFound();
            }
            Debug.WriteLine("idk whats going on here is the event" + Event);
            db.events.Remove(Event);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventExists(int id)
        {
            return db.events.Count(e => e.EventId == id) > 0;
        }
    }      
}