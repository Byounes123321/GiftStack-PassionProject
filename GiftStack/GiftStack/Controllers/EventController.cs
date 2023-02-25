using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GiftStack.Models;
using GiftStack.Models.ViewModels;

namespace GiftStack.Controllers
{
    public class EventController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();


        static EventController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44367/api/EventData/");
        }
        // GET: Event/list
        public ActionResult List()
        {
            //comunicate with our event data api to retrive a list of events 
            //curl https://localhost:44367/api/eventdata/listevents

            string url = "listevents";
            HttpResponseMessage response = client.GetAsync(url).Result;


            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<EventDto> events = response.Content.ReadAsAsync<IEnumerable<EventDto>>().Result;

            Debug.WriteLine("number of events recived");
            Debug.WriteLine(events.Count());

            return View(events);
        }

        // GET: eventdata/Details/5
        public ActionResult Details(int id)
        {

            //comunicate with our event data api to retrive one event
            //curl https://localhost:44367/api/eventdata/findevent/{id}

            DetailsEvent ViewModel = new DetailsEvent();

            string url = "findevent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;


            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            EventDto SelectedEvent = response.Content.ReadAsAsync<EventDto>().Result;

            Debug.WriteLine("event recived");
            Debug.WriteLine(SelectedEvent.EventName);

            ViewModel.SelectedEvent = SelectedEvent;
            
            //Set secconed api call to get the list of gifts for the selected event and do same process to retrive data
            HttpClient client2 = new HttpClient();
            client2.BaseAddress = new Uri("https://localhost:44367/api/GiftData/");

            string EventUrl = "ListGiftsForEvent/" + id;
            HttpResponseMessage EventResponse = client2.GetAsync(EventUrl).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(EventResponse.StatusCode);
            IEnumerable<giftDto> RelatedGifts = EventResponse.Content.ReadAsAsync<IEnumerable<giftDto>>().Result;

            ViewModel.RelatedGifts = RelatedGifts;
            
            return View(ViewModel);
        }
        public ActionResult Error()
        {
            return View();
        }

        // GET: RecipientData/New
        public ActionResult New()
        {
            return View();
        }

        // POST: RecipientData/Create
        [HttpPost]
        public ActionResult Create(Event Event)
        {
            Debug.WriteLine("the inputed event name is ");
            Debug.WriteLine(Event.EventName);
            // add a new event into the system using the api
            string url = "AddEvent";
            string jsonPayload = jss.Serialize(Event);

            Debug.WriteLine("the Json payload is: ");
            Debug.WriteLine(jsonPayload);

            HttpContent content = new StringContent(jsonPayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: event/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateEvent ViewModel = new UpdateEvent();

            string url = "findevent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            EventDto SelectedEvent = response.Content.ReadAsAsync<EventDto>().Result;
            ViewModel.SelectedEvent = SelectedEvent;


            return View(ViewModel);
        }

        // POST: event/update/5
        [HttpPost]
        public ActionResult Update(int id, Event Event)
        {
            //update an event in the system using the api
            Debug.WriteLine("event date:");
            Debug.WriteLine(Event.EventDate);
            string url = "updateEvent/" + id;
            string jsonpayload = jss.Serialize(Event);
            Debug.WriteLine("Jason Data: ");
            Debug.WriteLine(jsonpayload);

          

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: RecipientData/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            //communicate with our event data api to retrive one event
            string url = "findevent/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            EventDto SelectedEvent = response.Content.ReadAsAsync<EventDto>().Result;

            return View(SelectedEvent);
        }

        // POST: event/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //delete an event in the system using the api
            string url = "deleteEvent/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
