using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI_PM.Models;

namespace WebAPI_PM.Controllers
{
    public class VatController : ApiController
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

        // GET: api/Vat
        public IQueryable<vat_dict> GetVat()
        {
            return Db.vat_dict;
        }

        // GET: api/Vat/5
        [ResponseType(typeof(vat_dict))]
        public IHttpActionResult GetVat(string Code)
        {
            vat_dict vatDict = Db.vat_dict.AsEnumerable().FirstOrDefault(X => X.Code == Code);
            if (vatDict == null)
                return NotFound();

            return Ok(vatDict);
        }

        // PUT: api/Vat/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutVat(string Code, vat_dict VatDict)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (Code != VatDict.Code)
                return BadRequest();

            Db.Entry(VatDict).State = EntityState.Modified;

            try
            {
                Db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VatExists(VatDict.Code))
                    return NotFound();
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Vat
        [ResponseType(typeof(vat_dict))]
        public IHttpActionResult PostVat(vat_dict VatDict)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Db.vat_dict.Add(VatDict);

            try
            {
                Db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (VatExists(VatDict.Code))
                    return Conflict();
                throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = VatDict.ID }, VatDict);
        }

        // DELETE: api/Vat/5
        [ResponseType(typeof(vat_dict))]
        public IHttpActionResult DeleteVat(string Code)
        {
            vat_dict vatDict = Db.vat_dict.AsEnumerable().FirstOrDefault(X => X.Code == Code);
            if (vatDict == null)
                return NotFound();

            Db.vat_dict.Remove(vatDict);
            Db.SaveChanges();

            return Ok(vatDict);
        }

        protected override void Dispose(bool Disposing)
        {
            if (Disposing)
                Db.Dispose();
            base.Dispose(Disposing);
        }

        private bool VatExists(string Code)
        {
            return Db.vat_dict.Count(E => E.Code == Code) > 0;
        }
    }
}