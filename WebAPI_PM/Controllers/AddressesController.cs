using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI_PM.Models;

namespace WebAPI_PM.Controllers
{
    public class AddressesController : ApiController
    {
        private readonly MySQL_Prod m_Db = new MySQL_Prod();

        // GET: api/Addresses
        public IQueryable<address> GetAddresses()
        {
            return m_Db.addresses;
        }

        // PUT: api/Addresses/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAddress(int Id, address Address)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (Id != Address.ID)
                return BadRequest();

            m_Db.Entry(Address).State = EntityState.Modified;

            try
            {
                m_Db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(Id))
                    return NotFound();
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Addresses
        [ResponseType(typeof(address))]
        public IHttpActionResult PostAddress(address Address)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            m_Db.addresses.Add(Address);

            try
            {
                m_Db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AddressExists(Address.ID))
                    return Conflict();
                throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = Address.ID }, Address);
        }

        // DELETE: api/Addresses/5
        [ResponseType(typeof(address))]
        public IHttpActionResult DeletAaddress(int Id)
        {
            address address = m_Db.addresses.Find(Id);
            if (address == null)
                return NotFound();

            m_Db.addresses.Remove(address);
            m_Db.SaveChanges();

            return Ok(address);
        }

        protected override void Dispose(bool Disposing)
        {
            if (Disposing)
                m_Db.Dispose();
            base.Dispose(Disposing);
        }

        private bool AddressExists(int Id)
        {
            return m_Db.addresses.Count(E => E.ID == Id) > 0;
        }
    }
}