using Microsoft.EntityFrameworkCore;
using Nav.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Domain.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Fund> Funds { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
