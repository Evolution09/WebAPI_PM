using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI_PM.Models;

namespace WebAPI_PM.Controllers
{
    public class CategoryController : ApiController
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

        [Route("category/")]
        public IQueryable<category_dict> GetCategory()
        {
            return Db.category_dict;
        }

        [ResponseType(typeof(category_dict))]
        [Route("category/{code}")]
        public IHttpActionResult GetCategory(string Code)
        {
            category_dict categoryDict = Db.category_dict.AsEnumerable().FirstOrDefault(X => X.Code == Code);
            if (categoryDict == null)
                return NotFound();

            return Ok(categoryDict);
        }

        [ResponseType(typeof(product))]
        [Route("category/{code}")]
        public IQueryable<product> GetProductsByCategoryCode(string Code)
        {
            var catIDs = Db.category_dict.AsEnumerable().Where(X => X.Code == Code).Select(X => X.ID);
            var products = Db.products.AsEnumerable().ToList()
                .FindAll(X => catIDs.Contains(X.CategoryID)).AsQueryable();
            products.ForEachAsync(LoadAdditionalProductData);

            return products;
        }

        [ResponseType(typeof(void))]
        [Route("category/{code}")]
        public IHttpActionResult PutCategory(string Code, category_dict Category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (Code != Category.Code)
                return BadRequest();

            Db.Entry(Category).State = EntityState.Modified;

            try
            {
                Db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(Code))
                    return NotFound();
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(category_dict))]
        [Route("category/")]
        public IHttpActionResult PostCategory(category_dict Category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Db.category_dict.Add(Category);

            try
            {
                Db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CategoryExists(Category.Code))
                    return Conflict();
                throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = Category.ID }, Category);
        }

        [ResponseType(typeof(category_dict))]
        [Route("category/{code}")]
        public IHttpActionResult DeleteCategory(string Code)
        {
            category_dict categoryDict = Db.category_dict.AsEnumerable().FirstOrDefault(X => X.Code == Code);
            if (categoryDict == null)
                return NotFound();

            Db.category_dict.Remove(categoryDict);
            Db.SaveChanges();

            return Ok(categoryDict);
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

        private bool CategoryExists(string Code)
        {
            return Db.category_dict.Count(E => E.Code == Code) > 0;
        }
    }
}