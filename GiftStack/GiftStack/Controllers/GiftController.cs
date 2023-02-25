using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GiftStack.Models;
using GiftStack.Models.ViewModels;
using Newtonsoft.Json;

namespace GiftStack.Controllers
{
    public class GiftController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

    
        static GiftController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44367/api/giftdata/");
        }
        // GET: gift/list
        public ActionResult List()
        {
            //comunicate with our gift data api to retrive a list of gifts 
            //curl https://localhost:44367/api/giftdata/listgifts
      
            string url = "listgifts";
            HttpResponseMessage response = client.GetAsync(url).Result;


            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<giftDto> gifts = response.Content.ReadAsAsync<IEnumerable<giftDto>>().Result;

            Debug.WriteLine("number of gifts recived");
            Debug.WriteLine(gifts.Count());

            return View(gifts);
        }

        // GET: giftdata/Details/5
        public ActionResult Details(int id)
        {

            //comunicate with our gift data api to retrive one gift
            //curl https://localhost:44367/api/giftdata/findgift/{id}
       
            string url = "findgift/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;


            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            giftDto SelectedGift = response.Content.ReadAsAsync<giftDto>().Result;

            Debug.WriteLine("gift recived");
            Debug.WriteLine(SelectedGift.GiftName);

           
            //Show events and recipients that are associated with this gift
            HttpClient apiClient2 = new HttpClient();
            apiClient2.BaseAddress = new Uri("https://localhost:44367/api/");

            // find the recipients associated with this gift

            int giftId = SelectedGift.GiftId;

            string recUrl = "RecipientData/listRecipientForGift/" + giftId;
            HttpResponseMessage RecResponse = apiClient2.GetAsync(recUrl).Result;
            
            Debug.WriteLine("The recipient response code is ");
            Debug.WriteLine(RecResponse.StatusCode);
            
            IEnumerable< RecipientDto> RelatedRecipent = RecResponse.Content.ReadAsAsync< IEnumerable< RecipientDto>>().Result;

            Debug.WriteLine("Recipient recived");
            Debug.WriteLine(RelatedRecipent.Count());


            //find events associated with this gift
            
            string eveUrl = "EventData/ListEventForGift/" + giftId;
            HttpResponseMessage eveResponse = apiClient2.GetAsync(eveUrl).Result;
            
            Debug.WriteLine("The event response code is ");
            Debug.WriteLine(eveResponse.StatusCode);
            
            IEnumerable< EventDto> RelatedEvent = eveResponse.Content.ReadAsAsync<IEnumerable< EventDto>>().Result;

            Debug.WriteLine("Event recived");
            Debug.WriteLine(RelatedEvent.Count());

            // add the related recipients and events to the view model
            DetailsGift viewModel = new DetailsGift();
   
            viewModel.RelatedRecipient = RelatedRecipent;
            viewModel.RelatedEvent = RelatedEvent;
            viewModel.SelectedGift = SelectedGift;

            return View(viewModel);
        }
        public ActionResult Error()
        {
            return View();
        }

        // GET: RecipientData/New
        public ActionResult New()
        {
            //this page needs two streams of information 
            // All recipeints (API/listRecipients)
            // All Events (API/listEvents)
            HttpClient apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri("https://localhost:44367/api/");

            string url = "RecipientData/listRecipients";
            
            HttpResponseMessage response = apiClient.GetAsync(url).Result;

            IEnumerable<RecipientDto> recipientInfo = response.Content.ReadAsAsync<IEnumerable<RecipientDto>>().Result;

            //Debug.WriteLine("response:", json);

            string url2 = "EventData/listEvents";
            HttpResponseMessage response2 = apiClient.GetAsync(url2).Result;

            IEnumerable< EventDto> eventInfo = response2.Content.ReadAsAsync<IEnumerable< EventDto>>().Result;

            //add the information to the view model
            NewGift viewModel = new NewGift();
            viewModel.Recipients = recipientInfo;
            viewModel.Events = eventInfo;
            //Debug.WriteLine("number of recipients recived");
            //Debug.WriteLine(recipient.Count());
            //Debug.WriteLine("number of events recived");
            //Debug.WriteLine(events.Count());\
            Debug.WriteLine("viewmodel");
            Debug.WriteLine(viewModel);
            return View(viewModel);
        }
 

        
        // POST: RecipientData/Create
        [HttpPost]
        public ActionResult Create(Gift gift)
        {
            Debug.WriteLine("the inputed gift name is ");
            Debug.WriteLine(gift.GiftName);
            // add a new gift into the system using the api
            
            string url = "AddGift";
    
            string jsonPayload = jss.Serialize(gift);

            Debug.WriteLine("the Json payload is: ");
            Debug.WriteLine(jsonPayload);

            HttpContent content = new StringContent(jsonPayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response =  client.PostAsync(url,content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");

            }else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: gift/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateGift ViewModel = new UpdateGift();

            string url = "findgift/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            giftDto SelectedGift = response.Content.ReadAsAsync<giftDto>().Result;

            HttpClient apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri("https://localhost:44367/api/");

            //find the recipients associated with this gift

            string RecUrl = "RecipientData/listRecipients";
            HttpResponseMessage RecResponse = apiClient.GetAsync(RecUrl).Result;
            IEnumerable<RecipientDto> recipientInfo = RecResponse.Content.ReadAsAsync<IEnumerable<RecipientDto>>().Result;

            //find events associated with this gift
            string EveUrl = "EventData/listEvents";
            HttpResponseMessage response2 = apiClient.GetAsync(EveUrl).Result;

            IEnumerable<EventDto> eventInfo = response2.Content.ReadAsAsync<IEnumerable<EventDto>>().Result;

            //add recipeints and events to viewmodel
            ViewModel.SelectedGift = SelectedGift;
            ViewModel.Event = eventInfo;
            ViewModel.Recipient = recipientInfo;


            return View(ViewModel);
        }

        // POST: gift/update/5
        [HttpPost]
        public ActionResult Update(int id, Gift gift)
        {
            Debug.WriteLine("the inputed gift name is ");
            Debug.WriteLine(gift.GiftName);
            
            string url = "updateGift/" + id;
            string jsonpayload = jss.Serialize(gift);
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
            
            string url = "findgift/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            giftDto SelectedGift = response.Content.ReadAsAsync<giftDto>().Result;
           
            return View(SelectedGift);
        }

        // POST: gift/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "deleteGift/" + id;
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
