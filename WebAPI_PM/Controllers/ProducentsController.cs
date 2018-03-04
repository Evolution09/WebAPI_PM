using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI_PM.Models;

namespace WebAPI_PM.Controllers
{
    public class ProducentsController : ApiController
    {
        private readonly MySQL_Prod m_Db = new MySQL_Prod();

        // GET: api/Producents
        public IQueryable<producent> GetProducents()
        {
            return m_Db.producents;
        }

        // GET: api/Producents/5
        [ResponseType(typeof(producent))]
        public IHttpActionResult GetProducent(string Code)
        {
            producent producent = m_Db.producents.AsEnumerable().FirstOrDefault(X => X.Code == Code);
            if (producent == null)
                return NotFound();

            return Ok(producent);
        }

        // GET: api/Producents/5
        [ResponseType(typeof(producent))]
        public IQueryable<producent> GetProducentsByCountry(string Country)
        {
            var addrIDs = m_Db.addresses.AsEnumerable().Where(X => X.Country == Country).Select(X => X.ID);
            return m_Db.producents.AsEnumerable().ToList()
                .FindAll(X => X.AddressID.HasValue && addrIDs.Contains(X.AddressID.Value)).AsQueryable();
        }

        // GET: api/Producents/5
        [ResponseType(typeof(producent))]
        public IQueryable<producent> GetProducentsByCity(string City)
        {
            var addrIDs = m_Db.addresses.AsEnumerable().Where(X => X.City == City).Select(X => X.ID);
            return m_Db.producents.AsEnumerable().ToList()
                .FindAll(X => X.AddressID.HasValue && addrIDs.Contains(X.AddressID.Value)).AsQueryable();
        }

        // PUT: api/Producents/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProducent(string Code, producent Producent)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (Code != Producent.Code)
                return BadRequest();

            m_Db.Entry(Producent).State = EntityState.Modified;

            try
            {
                m_Db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProducentExists(Code))
                    return NotFound();
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Producents
        [ResponseType(typeof(producent))]
        public IHttpActionResult PostProducent(producent Producent)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            m_Db.producents.Add(Producent);

            try
            {
                m_Db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ProducentExists(Producent.Code))
                    return Conflict();
                throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = Producent.ID }, Producent);
        }

        // DELETE: api/Producents/5
        [ResponseType(typeof(producent))]
        public IHttpActionResult DeleteProducent(string Code)
        {
            producent producent = m_Db.producents.AsEnumerable().FirstOrDefault(X => X.Code == Code);
            if (producent == null)
                return NotFound();

            m_Db.producents.Remove(producent);
            m_Db.SaveChanges();

            return Ok(producent);
        }

        protected override void Dispose(bool Disposing)
        {
            if (Disposing)
                m_Db.Dispose();
            base.Dispose(Disposing);
        }

        private bool ProducentExists(string Code)
        {
            return m_Db.producents.Count(E => E.Code == Code) > 0;
        }
    }
}