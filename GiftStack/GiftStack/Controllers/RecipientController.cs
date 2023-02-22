using GiftStack.Models.ViewModels;
using GiftStack.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GiftStack.Controllers
{
    public class RecipientController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();


        static RecipientController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44367/api/recipientdata/");
        }
        // GET: recipient/list
        public ActionResult List()
        {
            //comunicate with our recipient data api to retrive a list of recipient 
            //curl https://localhost:44367/api/recipientdata/listRecipients

            string url = "listRecipients";
            HttpResponseMessage response = client.GetAsync(url).Result;


            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<RecipientDto> recipients = response.Content.ReadAsAsync<IEnumerable<RecipientDto>>().Result;

            Debug.WriteLine("number of recipients recived");
            Debug.WriteLine(recipients.Count());

            return View(recipients);
        }
        // GET: recipient/Details/5
        public ActionResult Details(int id)
        {
            DetailsRecipient ViewModel = new DetailsRecipient();

            //objective: communicate with our recipient data api to retrieve one Keeper
            //curl https://localhost:44324/api/RecipientData/findRecipient/{id}

            string url = "findRecipient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            RecipientDto SelectedRecipient = response.Content.ReadAsAsync<RecipientDto>().Result;
            Debug.WriteLine("Recipient received : ");
            Debug.WriteLine(SelectedRecipient.RecipientName);

            ViewModel.SelectedRecipient = SelectedRecipient;

            //show all gifts that the recipient is reciving
            HttpClient client2 = new HttpClient();
            client2.BaseAddress = new Uri("https://localhost:44367/api/giftdata/");
            string url2 = "ListGiftsForRecipient/" + id;
            HttpResponseMessage response2 = client2.GetAsync(url2).Result;
            IEnumerable<giftDto> GiftsRecived = response2.Content.ReadAsAsync<IEnumerable<giftDto>>().Result;
            
            Debug.WriteLine("the gifts recived are: ",GiftsRecived);
            
            ViewModel.GiftsRecived = GiftsRecived;


            return View(ViewModel);
        }
        // GET: Recipient/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Recipient/Create
        [HttpPost]
        public ActionResult Create(Recipient recipient)
        {
            Debug.WriteLine("the inputed recipient name is ");
            Debug.WriteLine(recipient.RecipientName);
            // add a new recipient into the system using the api
            string url = "AddRecipient";

            string jsonPayload = jss.Serialize(recipient);

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
        // GET: gift/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateRecipient ViewModel = new UpdateRecipient();

            string url = "findRecipient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RecipientDto SelectedRecipient = response.Content.ReadAsAsync<RecipientDto>().Result;
            ViewModel.SelectedRecipient = SelectedRecipient;


            return View(ViewModel);
        }

        // POST: gift/update/5
        [HttpPost]
        public ActionResult Update(int id, Recipient recipient)
        {
            // EVENTID AND RECIPANTID ARE HARD CODED THE GIFT ID IS 5
            //ADD UPDATE FUCTIONALITYS
            string url = "updateRecipient/" + id;
            string jsonpayload = jss.Serialize(recipient);
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

            string url = "findRecipient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            RecipientDto SelectedRecipient = response.Content.ReadAsAsync<RecipientDto>().Result;

            return View(SelectedRecipient);
        }

        // POST: gift/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "deleteRecipient/" + id;
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
