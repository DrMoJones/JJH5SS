using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using H5SS.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using H5SS.Codes;
using Microsoft.AspNetCore.DataProtection;
using H5SS.Areas.Identity.Codes;

namespace H5SS.Controllers
{
    public class WishlistsController : Controller
    {
        private readonly H5SSContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly CryptoEx _cryptoEx;
        private readonly IDataProtector _dataProtector;
        private UserManager<IdentityUser> userManager;

        public WishlistsController(
            H5SSContext context, 
            IServiceProvider serviceProvider, 
            CryptoEx cryptoEx,
            IDataProtectionProvider dataProtector)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            _dataProtector = dataProtector.CreateProtector("MinNoegle1");
            _cryptoEx = cryptoEx;
            userManager = _serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        }

        // GET: Wishlists
        public async Task<IActionResult> Index()
        {
            var wishlist = await _context.Wishlists.Where(s => s.Email == userManager.GetUserName(this.User)).ToListAsync();
            foreach (var item in wishlist)
            {
                item.Wish = _cryptoEx.Decrypt(item.Wish, _dataProtector);
                item.Note = _cryptoEx.Decrypt(item.Note, _dataProtector);
            }

            return View(wishlist);
        }

        // GET: Wishlists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishlist = await _context.Wishlists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wishlist == null)
            {
                return NotFound();
            }

            return View(wishlist);
        }

        // GET BY NAME
        public async Task<IActionResult> IndexByName()
        {
            var wishlist = await _context.Wishlists
                .Where(s => s.Email == "dwad")
                .ToListAsync();
            Console.WriteLine("EMAIL");

            return View();
        }

        // GET: Wishlists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Wishlists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Wish,Note")] Wishlist wishlist)
        {
            if (ModelState.IsValid)
            {
                var UserManager = _serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                wishlist.Email = UserManager.GetUserName(this.User);
                wishlist.Wish = _cryptoEx.Encrypt(wishlist.Wish, _dataProtector);
                wishlist.Note = _cryptoEx.Encrypt(wishlist.Note, _dataProtector);
                _context.Add(wishlist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wishlist);
        }

        // GET: Wishlists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishlist = await _context.Wishlists.FindAsync(id);
            if (wishlist == null)
            {
                return NotFound();
            }
            return View(wishlist);
        }

        // POST: Wishlists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Wish,Note")] Wishlist wishlist)
        {
            if (id != wishlist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wishlist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WishlistExists(wishlist.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(wishlist);
        }

        // GET: Wishlists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishlist = await _context.Wishlists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wishlist == null)
            {
                return NotFound();
            }

            return View(wishlist);
        }

        // POST: Wishlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wishlist = await _context.Wishlists.FindAsync(id);
            _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WishlistExists(int id)
        {
            return _context.Wishlists.Any(e => e.Id == id);
        }
    }
}
