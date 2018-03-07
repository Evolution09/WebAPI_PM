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

        public IQueryable<product> GetProducts()
        {
            var prods = Db.products;
            prods.ForEachAsync(LoadAdditionalProductData);

            return prods;
        }

        // GET: api/Products/5
        [ResponseType(typeof(product))]
	    public IHttpActionResult GetProduct(string Code)
	    {
	        var prod = Db.products.AsEnumerable().FirstOrDefault(X => X.Code == Code);
	        if (prod == null)
	            return NotFound();

	        LoadAdditionalProductData(prod);

            return Ok(prod);
	    }

        // GET: api/Products/5
        [ResponseType(typeof(product))]
	    public IQueryable<product> GetProductsByProducentCode(string Code)
        {
            var prodIDs = Db.producents.AsEnumerable().Where(X => X.Code == Code).Select(X => X.ID);
            var products = Db.products.AsEnumerable().ToList()
                .FindAll(X => prodIDs.Contains(X.ProducentID)).AsQueryable();
            products.ForEachAsync(LoadAdditionalProductData);

            return products;
	    }

        // GET: api/Products/5
        [ResponseType(typeof(product))]
	    public IQueryable<product> GetProductsByCategoryCode(string Code)
        {
            var catIDs = Db.category_dict.AsEnumerable().Where(X => X.Code == Code).Select(X => X.ID);
            var products = Db.products.AsEnumerable().ToList()
                .FindAll(X => catIDs.Contains(X.CategoryID)).AsQueryable();
            products.ForEachAsync(LoadAdditionalProductData);

            return products;
	    }

	    // PUT: api/Products/5
	    [ResponseType(typeof(void))]
	    public IHttpActionResult PutProduct(string Code, product Product)
	    {
	        if (!ModelState.IsValid)
	            return BadRequest(ModelState);

	        if (Code != Product.Code)
	            return BadRequest();

	        Db.Entry(Product).State = EntityState.Modified;

	        try
	        {
	            Db.SaveChanges();
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
	            return BadRequest(ModelState);

	        Db.products.Add(Product);
	        Db.SaveChanges();

	        return CreatedAtRoute("DefaultApi", new { id = Product.ID }, Product);
	    }

	    // DELETE: api/Products/5
	    [ResponseType(typeof(product))]
	    public IHttpActionResult DeleteProduct(string Code)
	    {
	        var product = Db.products.AsEnumerable().FirstOrDefault(X => X.Code == Code);
	        if (product == null)
	            return NotFound();

	        Db.products.Remove(product);
	        Db.SaveChanges();

	        return Ok(product);
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
            Db.Dispose();
            base.Dispose(Disposing);
        }

	    private bool ProductExists(string Code)
	    {
	        return Db.products.Count(E => E.Code == Code) > 0;
	    }
    }
}

