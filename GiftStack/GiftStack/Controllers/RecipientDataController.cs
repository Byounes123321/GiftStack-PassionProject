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
    public class RecipientDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// Returns all recipients in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all recipents in the database, including their associated gifts.
        /// </returns>
        /// <example>
        /// GET: /api/recipientData/listRecipients
        /// </example>

        [HttpGet]
        [ResponseType(typeof(RecipientDto))]
        public IHttpActionResult ListRecipients()
        {
            List<Recipient> recipients = db.recipients.ToList();
            List<RecipientDto> recipientDtos = new List<RecipientDto>();

            recipients.ForEach(R => recipientDtos.Add(new RecipientDto()
            {
                RecipientId = R.RecipientId,
                RecipientName = R.RecipientName
              
            }));

            return Ok(recipientDtos);
        }

        /// <summary>
        /// Returns all recipients in the system associated with a particular gift.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all recipients in the database are reciving a gift
        /// </returns>
        /// <param name="id">Gift Primary Key</param>
        /// <example>
        /// GET: /api/RecipientData/ListRecipientsForGift/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(RecipientDto))]
        public IHttpActionResult ListRecipientForGift(int id)
        {
            List<Recipient> recipients = db.recipients.Where(
                r => r.Gifts.Any(
                    g => g.GiftId == id)
                ).ToList();
            List<RecipientDto> recipientDtos = new List<RecipientDto>();

            recipients.ForEach(r => recipientDtos.Add(new RecipientDto()
            {
                RecipientId = r.RecipientId,
                RecipientName = r.RecipientName
            }));

            return Ok(recipientDtos);
        }

        /// <summary>
        /// Returns recipients in the system not reciving any gifts.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all recpients in the database not reciving gifts
        /// </returns>
        /// <param name="id">gift Primary Key</param>
        /// <example>
        /// GET: /api/RecipientData/ListRecipientsNotRecivingGift/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(RecipientDto))]
        public IHttpActionResult ListRecipientsNotRecivingGift(int id)
        {
            List<Recipient> recipients = db.recipients.Where(
                r => !r.Gifts.Any(
                    g => g.GiftId == id)
                ).ToList();
            List<RecipientDto> recipientDtos = new List<RecipientDto>();

            recipients.ForEach( r => recipientDtos.Add(new RecipientDto()
            {
                RecipientId = r.RecipientId,
                RecipientName = r.RecipientName
            }));

            return Ok(recipientDtos);
        }
        /// <summary>
        /// Returns all recipients in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An recipent in the system matching up to the recipient ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the recipient</param>
        /// <example>
        /// GET: /api/RecipientData/FindRecipient/2
        /// </example>
        [ResponseType(typeof(RecipientDto))]
        [HttpGet]
        public IHttpActionResult FindRecipient(int id)
        {
            Recipient recipient = db.recipients.Find(id);

            if (recipient == null)
            {
                return NotFound();
            }
            RecipientDto recipientDto = new RecipientDto()
            {
                RecipientId = recipient.RecipientId,
                RecipientName = recipient.RecipientName
            };
           

            return Ok(recipientDto);
        }

        /// <summary>
        /// Updates a particular recipient in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the recipient ID primary key</param>
        /// <param name="Recipient">JSON FORM DATA of an recipient</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: /api/RecipientData/UpdateRecipient/1
        /// FORM DATA: Keeper JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateRecipient(int id, Recipient Recipient)
            //TRY MAYBE DOING ADD AND DELETE FIRST?? IDK WHY THIS   ^^ IS NULL 
        {
            Debug.WriteLine("I have reached the update recipient method");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Modle State is invalid");
                return BadRequest(ModelState);
            }
            Debug.WriteLine("RECIPIENT DATA: ",Recipient);

            if (id != Recipient.RecipientId)
            {
                Debug.WriteLine("id missmatch");
                Debug.WriteLine("GET param " + id);
                Debug.WriteLine("POST param " + Recipient.RecipientId);
            
                return BadRequest();
            }

            db.Entry(Recipient).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipientExists(id))
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
        /// Adds an recipient to the system
        /// </summary>
        /// <param name="Recipient">JSON FORM DATA of an recipient</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: recipient ID, recipient name
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: /api/RecipientData/AddRecipient
        /// FORM DATA: recipient JSON Object
        /// </example>
        [ResponseType(typeof(Recipient))]
        [HttpPost]
        public IHttpActionResult AddRecipient(Recipient recipient)
        {
            Debug.WriteLine(recipient);
            Debug.WriteLine("entered the add keeper");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("model state is not valid");
                return BadRequest(ModelState);
            }

            db.recipients.Add(recipient);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = recipient.RecipientId }, recipient);
        }
        /// <summary>
        /// Deletes an recipient from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the recipient</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/recipientData/deleteRecipient/4
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Recipient))]
        [HttpPost]
        public IHttpActionResult DeleteRecipient(int id)
        {
            Recipient recipient = db.recipients.Find(id);
            if (recipient == null)
            {
                return NotFound();
            }

            db.recipients.Remove(recipient);
            db.SaveChanges();

            return Ok();
        }











        private bool RecipientExists(int id)
        {
            return db.recipients.Count(e => e.RecipientId == id) > 0;
        }







    }
}