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
        private readonly MySQL_Prod m_Db = new MySQL_Prod();

        // GET: api/Category
        public IQueryable<category_dict> GetCategory()
        {
            return m_Db.category_dict;
        }

        // GET: api/Category/5
        [ResponseType(typeof(category_dict))]
        public IHttpActionResult GetCategory(string Code)
        {
            category_dict categoryDict = m_Db.category_dict.AsEnumerable().FirstOrDefault(X => X.Code == Code);
            if (categoryDict == null)
                return NotFound();

            return Ok(categoryDict);
        }

        // PUT: api/Category/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCategory(string Code, category_dict Category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (Code != Category.Code)
                return BadRequest();

            m_Db.Entry(Category).State = EntityState.Modified;

            try
            {
                m_Db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(Code))
                    return NotFound();
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Category
        [ResponseType(typeof(category_dict))]
        public IHttpActionResult PostCategory(category_dict Category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            m_Db.category_dict.Add(Category);

            try
            {
                m_Db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CategoryExists(Category.Code))
                    return Conflict();
                throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = Category.ID }, Category);
        }

        // DELETE: api/Category/5
        [ResponseType(typeof(category_dict))]
        public IHttpActionResult DeleteCategory(string Code)
        {
            category_dict categoryDict = m_Db.category_dict.AsEnumerable().FirstOrDefault(X => X.Code == Code);
            if (categoryDict == null)
                return NotFound();

            m_Db.category_dict.Remove(categoryDict);
            m_Db.SaveChanges();

            return Ok(categoryDict);
        }

        protected override void Dispose(bool Disposing)
        {
            if (Disposing)
                m_Db.Dispose();
            base.Dispose(Disposing);
        }

        private bool CategoryExists(string Code)
        {
            return m_Db.category_dict.Count(E => E.Code == Code) > 0;
        }
    }
}