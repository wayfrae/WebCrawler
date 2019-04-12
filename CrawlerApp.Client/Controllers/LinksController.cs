using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrawlerApp.Client.Models;

namespace CrawlerApp.Client.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LinksController : ControllerBase
    {
        private readonly CrawlerContext _context;

        public LinksController(CrawlerContext context)
        {
            _context = context;
        }

        // GET: /Links
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Link>>> GetLinks()
        {
            return await _context.Links.ToListAsync();
        }

        // GET: /Links/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Link>> GetLink(int id)
        {
            var link = await _context.Links.FindAsync(id);

            if (link == null)
            {
                return NotFound();
            }

            return link;
        }

        

        // GET: /Links/ToCrawl
        [HttpGet("tocrawl")]
        public async Task<ActionResult<IEnumerable<Link>>> GetLinksToCrawl()
        {
            var links = _context.Links.Where(x => x.IsCrawled == false);

            if (links == null)
            {
                return NotFound();
            }

            return await links.ToListAsync();
        }

        // GET: /Links/ToCrawl/Count
        [HttpGet("tocrawl/count")]
        public int GetToCrawlCount()
        {
            var links = _context.Links.Where(x => x.IsCrawled == false).Count();

            return links;
        }

        // GET: /Links/HaveCrawled
        [HttpGet("havecrawled")]
        public async Task<ActionResult<IEnumerable<Link>>> GetLinksHaveCrawled()
        {
            var links = _context.Links.Where(x => x.IsCrawled == true);

            if (links == null)
            {
                return NotFound();
            }

            return await links.ToListAsync();
        }

        // GET: /Links/ToCrawl/Count
        [HttpGet("havecrawled/count")]
        public int GetHaveCrawledCount()
        {
            var links = _context.Links.Where(x => x.IsCrawled == true).Count();
            return links;
        }

        // PUT: /Links/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLink(int id, Link link)
        {
            if (id != link.ID)
            {
                return BadRequest();
            }

            _context.Entry(link).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LinkExists(id))
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

        // POST: /Links
        [HttpPost]
        public async Task<ActionResult<Link>> PostLink(Link link)
        {
            _context.Links.Add(link);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLink", new { id = link.ID }, link);
        }

        // DELETE: /Links/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Link>> DeleteLink(int id)
        {
            var link = await _context.Links.FindAsync(id);
            if (link == null)
            {
                return NotFound();
            }

            _context.Links.Remove(link);
            await _context.SaveChangesAsync();

            return link;
        }

        private bool LinkExists(int id)
        {
            return _context.Links.Any(e => e.ID == id);
        }
    }
}
