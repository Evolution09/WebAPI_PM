using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI_PM.Models;

namespace WebAPI_PM.Controllers
{
	public class ProductController : ApiController
	{
	   private readonly MySQL_Prod m_Db = new MySQL_Prod();

	    public IQueryable<product> GetProducts()
	    {
	        return m_Db.products;
	    }

        // GET: api/Products/5
        [ResponseType(typeof(product))]
	    public IHttpActionResult GetProduct(string Code)
	    {
	        var prod = m_Db.products.AsEnumerable().FirstOrDefault(X => X.Code == Code);
	        if (prod == null)
	            return NotFound();

	        return Ok(prod);
	    }

	    // PUT: api/Products/5
	    [ResponseType(typeof(void))]
	    public IHttpActionResult PutProduct(string Code, product Product)
	    {
	        if (!ModelState.IsValid)
	        {
	            return BadRequest(ModelState);
	        }

	        if (Code != Product.Code)
	        {
	            return BadRequest();
	        }

	        m_Db.Entry(Product).State = EntityState.Modified;

	        try
	        {
	            m_Db.SaveChanges();
	        }
	        catch (DbUpdateConcurrencyException)
	        {
	            if (!ProductExists(Code))
	            {
	                return NotFound();
	            }
	            throw;
	        }

	        return StatusCode(HttpStatusCode.NoContent);
	    }

	    // POST: api/Products
	    [ResponseType(typeof(product))]
	    public IHttpActionResult PostProduct(product Product)
	    {
	        if (!ModelState.IsValid)
	        {
	            return BadRequest(ModelState);
	        }

	        m_Db.products.Add(Product);
	        m_Db.SaveChanges();

	        return CreatedAtRoute("DefaultApi", new { id = Product.ID }, Product);
	    }

	    // DELETE: api/Products/5
	    [ResponseType(typeof(product))]
	    public IHttpActionResult DeleteProduct(string Code)
	    {
	        var product = m_Db.products.AsEnumerable().FirstOrDefault(X => X.Code == Code);
	        if (product == null)
	        {
	            return NotFound();
	        }

	        m_Db.products.Remove(product);
	        m_Db.SaveChanges();

	        return Ok(product);
	    }

        protected override void Dispose(bool Disposing)
        {
            m_Db.Dispose();
            base.Dispose(Disposing);
        }

	    private bool ProductExists(string Code)
	    {
	        return m_Db.products.Count(E => E.Code == Code) > 0;
	    }
    }
}

