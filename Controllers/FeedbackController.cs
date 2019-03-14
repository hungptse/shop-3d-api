using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAPI.Entities;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private WebsiteShoppingContext _context = new WebsiteShoppingContext();

        // GET: api/Feedback
        [HttpGet]
        public IEnumerable<Feedback> GetFeedback()
        {
            return _context.Feedback.Include(f => f.Acc);
        }

        // GET: api/Feedback/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeedback([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feedback = await _context.Feedback.FindAsync(id);

            if (feedback == null)
            {
                return NotFound();
            }

            return Ok(feedback);
        }

        // PUT: api/Feedback/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedback([FromRoute] int id, [FromBody] Feedback feedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != feedback.Id)
            {
                return BadRequest();
            }

            _context.Entry(feedback).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Feedback
        [HttpPost]
        public async Task<IActionResult> PostFeedback([FromBody] Dictionary<string, string> body)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rate = body.GetValueOrDefault("rate");
            var comment = body.GetValueOrDefault("comment");
            var uid = body.GetValueOrDefault("uid");
            var pro = body.GetValueOrDefault("pro");
            Feedback feedback = new Feedback { isApprove = false, Rate = int.Parse(rate), ProId = int.Parse(pro), UserId = uid, Comment = comment, PostedTime = DateTime.Now };
            _context.Feedback.Add(feedback);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFeedback", new { id = feedback.Id }, feedback);
        }

        // DELETE: api/Feedback/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feedback = await _context.Feedback.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            _context.Feedback.Remove(feedback);
            await _context.SaveChangesAsync();

            return Ok(feedback);
        }

        private bool FeedbackExists(int id)
        {
            return _context.Feedback.Any(e => e.Id == id);
        }
    }
}