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

        [Route("producent/")]
        public IQueryable<producent> GetProducents()
        {
            var producents = Db.producents;
            producents.ForEachAsync(LoadAdditionalProducentsData);

            return producents;
        }

        [ResponseType(typeof(producent))]
        [Route("producent/{code}")]
        public IHttpActionResult GetProducent(string Code)
        {
            producent producent = Db.producents.AsEnumerable().FirstOrDefault(X => X.Code == Code);
            if (producent == null)
                return NotFound();

            LoadAdditionalProducentsData(producent);

            return Ok(producent);
        }

        [ResponseType(typeof(product))]
        [Route("producent/{code}")]
        public IQueryable<product> GetProductsByProducentCode(string Code)
        {
            var prodIDs = Db.producents.AsEnumerable().Where(X => X.Code == Code).Select(X => X.ID);
            var products = Db.products.AsEnumerable().ToList()
                .FindAll(X => prodIDs.Contains(X.ProducentID)).AsQueryable();
            products.ForEachAsync(LoadAdditionalProductData);

            return products;
        }

        [ResponseType(typeof(void))]
        [Route("producent/{code}")]
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

        [ResponseType(typeof(producent))]
        [Route("producent/")]
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

        [ResponseType(typeof(producent))]
        [Route("producent/{code}")]
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

        private void LoadAdditionalProductData(product Product)
        {
            Product.category_dict = Db.category_dict.FirstOrDefault(C => C.ID == Product.CategoryID);
            Product.producent = Db.producents.FirstOrDefault(Pr => Pr.ID == Product.ProducentID);
            if (Product.producent?.AddressID != null)
                Product.producent.address = Db.addresses.FirstOrDefault(A => A.ID == Product.producent.AddressID.Value);
            Product.vat_dict = Db.vat_dict.FirstOrDefault(V => V.ID == Product.VATID);
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