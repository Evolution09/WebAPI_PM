using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebAPI_PM.Controllers
{
    public class ProductsController : ApiController
    {
        private ProductsContext db = new ProductsContext();

        // GET: api/Products
        public IQueryable<Product> GetProducts()
        {
            return db.Products;
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(string Code)
        {
            Product product = db.Products.AsEnumerable().Where(X => X.Code == Code).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(string Code, Product Product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (Code != Product.Code)
            {
                return BadRequest();
            }

            db.Entry(Product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Code))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product Product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(Product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Product.ID }, Product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(string Code)
        {
            Product product = db.Products.AsEnumerable().Where(X => X.Code == Code).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool Disposing)
        {
            if (Disposing)
            {
                db.Dispose();
            }
            base.Dispose(Disposing);
        }

        private bool ProductExists(string Code)
        {
            return db.Products.Count(e => e.Code == Code) > 0;
        }
    }
}