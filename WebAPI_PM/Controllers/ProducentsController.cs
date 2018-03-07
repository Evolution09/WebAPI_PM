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

        // GET: api/Producents
        public IQueryable<producent> GetProducents()
        {
            var producents = Db.producents;
            producents.ForEachAsync(LoadAdditionalProducentsData);

            return producents;
        }

        // GET: api/Producents/5
        [ResponseType(typeof(producent))]
        public IHttpActionResult GetProducent(string Code)
        {
            producent producent = Db.producents.AsEnumerable().FirstOrDefault(X => X.Code == Code);
            if (producent == null)
                return NotFound();

            LoadAdditionalProducentsData(producent);

            return Ok(producent);
        }

        // GET: api/Producents/5
        [ResponseType(typeof(producent))]
        public IQueryable<producent> GetProducentsByCountry(string Country)
        {
            var addrIDs = Db.addresses.AsEnumerable().Where(X => X.Country == Country).Select(X => X.ID);
            var producents = Db.producents.AsEnumerable().ToList()
                .FindAll(X => X.AddressID.HasValue && addrIDs.Contains(X.AddressID.Value)).AsQueryable();
            producents.ForEachAsync(LoadAdditionalProducentsData);

            return producents;
        }

        // GET: api/Producents/5
        [ResponseType(typeof(producent))]
        public IQueryable<producent> GetProducentsByCity(string City)
        {
            var addrIDs = Db.addresses.AsEnumerable().Where(X => X.City == City).Select(X => X.ID);
            var producents = Db.producents.AsEnumerable().ToList()
                .FindAll(X => X.AddressID.HasValue && addrIDs.Contains(X.AddressID.Value)).AsQueryable();
            producents.ForEachAsync(LoadAdditionalProducentsData);

            return producents;
        }

        // PUT: api/Producents/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProducent(string Code, producent Producent)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (Code != Producent.Code)
                return BadRequest();

            Db.Entry(Producent).State = EntityState.Modified;

            try
            {
                Db.SaveChanges();
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

            Db.producents.Add(Producent);

            try
            {
                Db.SaveChanges();
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
            producent producent = Db.producents.AsEnumerable().FirstOrDefault(X => X.Code == Code);
            if (producent == null)
                return NotFound();

            Db.producents.Remove(producent);
            Db.SaveChanges();

            return Ok(producent);
        }

        private void LoadAdditionalProducentsData(producent Producent)
        {
            if (Producent.AddressID != null)
                Producent.address = Db.addresses.FirstOrDefault(A => A.ID == Producent.AddressID.Value);
        }

        protected override void Dispose(bool Disposing)
        {
            if (Disposing)
                Db.Dispose();
            base.Dispose(Disposing);
        }

        private bool ProducentExists(string Code)
        {
            return Db.producents.Count(E => E.Code == Code) > 0;
        }
    }
}