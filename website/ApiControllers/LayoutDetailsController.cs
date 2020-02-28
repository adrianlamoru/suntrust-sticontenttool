using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using st1001.data;

namespace st1001.website.ApiControllers
{
    public class LayoutDetailsController : ApiController
    {
        private st1001Entities db = new st1001Entities();

        // GET: api/LayoutDetails
        public IQueryable<LayoutDetail> GetLayoutDetails()
        {
            return db.LayoutDetails;
        }

        // GET: api/LayoutDetails/5
        [ResponseType(typeof(LayoutDetail))]
        public IHttpActionResult GetLayoutDetail(int id)
        {
            var layoutDetail = db.LayoutDetails.Find(id);
            if (layoutDetail == null)
            {
                return NotFound();
            }

            return Ok(layoutDetail);
        }

        // PUT: api/LayoutDetails/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLayoutDetail(int id, LayoutDetail layoutDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != layoutDetail.ID)
            {
                return BadRequest();
            }

            db.Entry(layoutDetail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LayoutDetailExists(id))
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

        // POST: api/LayoutDetails
        [ResponseType(typeof(LayoutDetail))]
        public IHttpActionResult PostLayoutDetail(LayoutDetail layoutDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LayoutDetails.Add(layoutDetail);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = layoutDetail.ID }, layoutDetail);
        }

        // DELETE: api/LayoutDetails/5
        [ResponseType(typeof(LayoutDetail))]
        public IHttpActionResult DeleteLayoutDetail(int id)
        {
            LayoutDetail layoutDetail = db.LayoutDetails.Find(id);
            if (layoutDetail == null)
            {
                return NotFound();
            }

            db.LayoutDetails.Remove(layoutDetail);
            db.SaveChanges();

            return Ok(layoutDetail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LayoutDetailExists(int id)
        {
            return db.LayoutDetails.Count(e => e.ID == id) > 0;
        }
    }
}