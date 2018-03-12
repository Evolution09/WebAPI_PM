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
        private MySQL_Prod m_Db;
        private MySQL_Prod Db
        {
            get
            {
                m_Db = new MySQL_Prod();
                m_Db.Configuration.ProxyCreationEnabled = false;
                return m_Db;
            }
        }

        [Route("address/")]
        public IQueryable<address> GetAddresses()
        {
            return Db.addresses;
        }

        [ResponseType(typeof(void))]
        [Route("address/{Id}")]
        public IHttpActionResult PutAddress(int Id, address Address)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (Id != Address.ID)
                return BadRequest();

            Db.Entry(Address).State = EntityState.Modified;

            try
            {
                Db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(Id))
                    return NotFound();
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(address))]
        [Route("address/")]
        public IHttpActionResult PostAddress(address Address)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Db.addresses.Add(Address);

            try
            {
                Db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AddressExists(Address.ID))
                    return Conflict();
                throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = Address.ID }, Address);
        }

        [ResponseType(typeof(address))]
        [Route("address/{Id}")]
        public IHttpActionResult DeleteAddress(int Id)
        {
            address address = Db.addresses.Find(Id);
            if (address == null)
                return NotFound();

            Db.addresses.Remove(address);
            Db.SaveChanges();

            return Ok(address);
        }

        protected override void Dispose(bool Disposing)
        {
            if (Disposing)
                Db.Dispose();
            base.Dispose(Disposing);
        }

        private bool AddressExists(int Id)
        {
            return Db.addresses.Count(E => E.ID == Id) > 0;
        }
    }
}