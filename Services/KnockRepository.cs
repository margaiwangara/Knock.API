using System;
using Knock.API.Entities;
using Knock.API.DbContexts;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Knock.API.Services
{
  public class KnockRepository : IKnockRepository
  {
    private readonly KnockContext _context;
    public KnockRepository(KnockContext context)
    {
      _context = context ?? 
              throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Restaurant>> GetRestaurantsAsync()
    {
      return await _context.Restaurants.ToListAsync();
    }
  }
}