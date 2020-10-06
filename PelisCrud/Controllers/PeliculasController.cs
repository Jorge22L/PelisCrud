using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PelisCrud.Data;
using PelisCrud.Models;

namespace PelisCrud.Controllers
{
    public class PeliculasController : Controller
    {
        private readonly PelisCrudContext _context;

        public PeliculasController(PelisCrudContext context)
        {
            _context = context;
        }

        // GET: Peliculas
        public async Task<IActionResult> Index(string CadenaBusq, 
            string sortOrder, 
            string currentFilter, 
            int? pageNumber)
        {
            //Paginación
            ViewData["CurrentSort"] = sortOrder;
            //Ordenar
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : " ";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            

            if (CadenaBusq != null)
            {
                pageNumber = 1;
            }
            else
            {
                CadenaBusq = currentFilter;
            }

            ViewData["CurrentFilter"] = CadenaBusq;

            //Busqueda
            var peliculas = from p in _context.Pelicula
                            select p;

            if(!String.IsNullOrEmpty(CadenaBusq))
            {
                peliculas = peliculas.Where(p => p.Titulo.Contains(CadenaBusq));
            }
     

            switch (sortOrder)
            {
                case "name_desc":
                    peliculas = peliculas.OrderByDescending(o => o.Titulo);
                    break;

                case "Date":
                    peliculas = peliculas.OrderBy(d => d.FechaPublicacion);
                    break;

                case "date_desc":
                    peliculas = peliculas.OrderByDescending(da => da.FechaPublicacion);
                    break;

                default:
                    peliculas = peliculas.OrderBy(p => p.Titulo);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<Pelicula>.CreateAsync(peliculas.AsNoTracking(), pageNumber ?? 1, pageSize));
            //return View(await peliculas.ToListAsync());
            //return View(await _context.Pelicula.ToListAsync());
        }

        // GET: Peliculas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Pelicula
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // GET: Peliculas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Peliculas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,FechaPublicacion,Genero,Precio")] Pelicula pelicula)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pelicula);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pelicula);
        }

        // GET: Peliculas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Pelicula.FindAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }
            return View(pelicula);
        }

        // POST: Peliculas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,FechaPublicacion,Genero,Precio")] Pelicula pelicula)
        {
            if (id != pelicula.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pelicula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeliculaExists(pelicula.Id))
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
            return View(pelicula);
        }

        // GET: Peliculas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Pelicula
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // POST: Peliculas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pelicula = await _context.Pelicula.FindAsync(id);
            _context.Pelicula.Remove(pelicula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeliculaExists(int id)
        {
            return _context.Pelicula.Any(e => e.Id == id);
        }
    }
}
