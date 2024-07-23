using _3raPracticaProgramada_OscarNaranjoZuniga.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace _3raPracticaProgramada_OscarNaranjoZuniga.Controllers
{
    public class ComentariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComentariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Comentarios
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Comentarios.Include(c => c.Usuario);
            ViewBag.ComentariosCantidad = _context.Comentarios.Count();
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Comentarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comentarios = await _context.Comentarios
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.ComentarioId == id);
            if (comentarios == null)
            {
                return NotFound();
            }

            return View(comentarios);
        }

        // GET: Comentarios/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId");
            return View();
        }

        // POST: Comentarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId, Comentario")] Comentarios comentarios)
        {
            _context.Add(comentarios);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Comentarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comentarios = await _context.Comentarios
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.ComentarioId == id);
            if (comentarios == null)
            {
                return NotFound();
            }

            return View(comentarios);
        }

        // POST: Comentarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comentarios = await _context.Comentarios.FindAsync(id);
            if (comentarios != null)
            {
                _context.Comentarios.Remove(comentarios);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComentariosExists(int id)
        {
            return _context.Comentarios.Any(e => e.ComentarioId == id);
        }
    }
}
