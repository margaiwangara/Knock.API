using System;
using Knock.API.Entities;
using Knock.API.DbContexts;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
      return await _context.Restaurants.ToListAsync<Restaurant>();
    }

    public async Task<IEnumerable<Restaurant>> GetRestaurantsAsync(IEnumerable<Guid> restaurantIds)
    {
      if(restaurantIds == null)
      {
        throw new ArgumentNullException(nameof(restaurantIds));
      }

      return await _context.Restaurants
                    .Where(r => restaurantIds.Contains(r.Id))
                    .ToListAsync();
    }

    public async Task<Restaurant> GetRestaurantAsync(Guid restaurantId)
    {
      if(restaurantId == Guid.Empty)
      {
        throw new ArgumentNullException(nameof(restaurantId));
      }

      return await _context.Restaurants.FirstOrDefaultAsync(a => a.Id == restaurantId);
    }

    public void AddRestaurant(Restaurant restaurant)
    {
      if(restaurant == null)
      {
        throw new ArgumentNullException(nameof(restaurant));
      }

      _context.Restaurants.Add(restaurant);
    }

    public void UpdateRestaurant(Restaurant restaurant)
    {
      // no implementation
    }

    public void DeleteRestaurant(Restaurant restaurant)
    {
      if(restaurant == null)
      {
        throw new ArgumentNullException(nameof(restaurant));
      }

      _context.Restaurants.Remove(restaurant);

    }

    public async Task<bool> RestaurantExists(Guid restaurantId)
    {
      return await _context.Restaurants.AnyAsync(r => r.Id == restaurantId);
    }

    public async Task<bool> SaveChangesAsync()
    {
      return (await _context.SaveChangesAsync() >= 0);
    }

    public async Task<IEnumerable<Review>> GetReviewsAsync()
    {
      return await _context.Reviews.ToListAsync<Review>();
    }

    public async Task<Review> GetReviewAsync(Guid reviewId)
    {
      if(reviewId == Guid.Empty)
      {
        throw new ArgumentNullException(nameof(reviewId));
      }

      return await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
    }

    public void AddReview(Review review)
    {
      if(review == null)
      {
        throw new ArgumentNullException(nameof(review));
      }

      _context.Reviews.Add(review);
    }

    public void UpdateReview(Review review)
    {
      throw new NotImplementedException();
    }

    public void DeleteReview(Review review)
    {
      throw new NotImplementedException();
    }
  }
}