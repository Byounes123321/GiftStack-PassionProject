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

namespace GiftStack.Controllers
{
    public class GiftDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// gets the list of all gifts from the database
        /// </summary>
        /// <returns>
        /// A list of all gifts from the database 
        /// </returns>
        // GET: api/GiftData/ListGifts
        [HttpGet]
        public IEnumerable<giftDto> ListGifts()
        {
            List<Gift> gifts = db.gifts.ToList();
            List<giftDto> giftDtos = new List<giftDto>();

            gifts.ForEach(a => giftDtos.Add(new giftDto()
            {
                GiftId =a.GiftId,
                GiftName = a.GiftName,
                GiftLocation = a.GiftLocation,
                IsGiftPurchaced = a.IsGiftPurchaced,
                RecipientName = a.Recipient.RecipientName,
                EventName = a.Event.EventName
            }));

            return giftDtos;
        }
        /// <summary>
        /// Gathers information about all gifts related to a specific recipient ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all gifts in the database, including their associated event that is linked to the recipient id
        /// </returns>
        /// <param name="id">recipeint ID.</param>
        /// <example>
        /// GET: api/GiftData/ListGiftsForRecipient/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(giftDto))]
        public IHttpActionResult ListGiftsForRecipient(int id)
        {
            List<Gift> gifts = db.gifts.Where(a => a.RecipientId == id).ToList();
            List<giftDto> giftDtos = new List<giftDto>();

            gifts.ForEach(g => giftDtos.Add(new giftDto()
            {
                GiftId = g.GiftId,
                GiftName = g.GiftName,
                GiftLocation = g.GiftLocation,
                IsGiftPurchaced = g.IsGiftPurchaced,
                RecipientName =g.Recipient.RecipientName,
                EventName = g.Event.EventName
            }));

            return Ok(giftDtos);
        }
        /// <summary>
        /// Finds a specific gift in the database by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// One gift that matches the id that it was passed
        /// </returns>
        // GET: api/GiftData/FindGift/5
        [ResponseType(typeof(Gift))]
        [HttpGet]
        public IHttpActionResult FindGift(int id)
        {
            Gift Gift = db.gifts.Find(id);
            giftDto giftDto = new giftDto()
            {
                GiftId = Gift.GiftId,
                GiftName = Gift.GiftName,
                GiftLocation = Gift.GiftLocation,
                IsGiftPurchaced = Gift.IsGiftPurchaced,
                RecipientName = Gift.Recipient.RecipientName,
                EventName = Gift.Event.EventName
            };

            if (Gift == null)
            {
                return NotFound();
            }

            return Ok(giftDto);
        }
        /// <summary>
        /// Gathers information about all gifts related to a particular event ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all gifts in the database, including their associated recipient matched with a particular event ID
        /// </returns>
        /// <param name="id">Species ID.</param>
        /// <example>
        /// GET: api/GiftData/ListGiftsForEvent/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(giftDto))]
        public IHttpActionResult ListGiftsForEvent(int id)
        {
            List<Gift> gifts = db.gifts.Where(a => a.EventId == id).ToList();
            List<giftDto> giftDtos = new List<giftDto>();

            gifts.ForEach(g => giftDtos.Add(new giftDto()
            {
                GiftId = g.GiftId,
                GiftName = g.GiftName,
                GiftLocation = g.GiftLocation,
                IsGiftPurchaced = g.IsGiftPurchaced,
                RecipientName = g.Recipient.RecipientName
            }));

            return Ok(giftDtos);
        }
        /// <summary>
        /// Updates one gift in the database with data provided in a post form
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gift"></param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        // POST: api/GiftData/UpdateGift/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateGift(int id, Gift gift)
        {
            Debug.WriteLine("I have reached the update gift method");
            if (!ModelState.IsValid) 
            {
                Debug.WriteLine("Modle State is invalid");
                return BadRequest(ModelState);
            }

            if (id != gift.GiftId)
            {
                Debug.WriteLine("id missmatch");
                Debug.WriteLine("GET param "+id);
                Debug.WriteLine("POST param " +gift.GiftId);

                return BadRequest();
            }

            db.Entry(gift).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GiftExists(id))
                {
                    Debug.WriteLine("gift not found");

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
            /// <summary>
            /// Adds a gift to the database using post data
            /// </summary>
            /// <param name="gift"></param>
            /// <returns>
            /// HEADER: 200 (OK)
            /// </returns>
            // POST: api/GiftData/AddGift
            [ResponseType(typeof(Gift))]
            [HttpPost]
            public IHttpActionResult AddGift(Gift gift)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.gifts.Add(gift);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = gift.GiftId }, gift);
            }

        /// <summary>
        /// Deletes a gift in the database with a matching id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// </returns>
        // POST: api/GiftData/DeleteGift/5
        [ResponseType(typeof(Gift))]
        [HttpPost]
        public IHttpActionResult DeleteGift(int id)
        {
            Gift gift = db.gifts.Find(id);
            if (gift == null)
            {
                return NotFound();
            }

            db.gifts.Remove(gift);
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

        private bool GiftExists(int id)
        {
            return db.gifts.Count(e => e.GiftId == id) > 0;
        }
    }
}