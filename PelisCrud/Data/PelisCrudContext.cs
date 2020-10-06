using Microsoft.EntityFrameworkCore;
using PelisCrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PelisCrud.Data
{
    public class PelisCrudContext:DbContext
    {
        public PelisCrudContext(DbContextOptions<PelisCrudContext> options):base(options)
        {
        }

        public DbSet<Pelicula> Pelicula { get; set; }
    }
}
