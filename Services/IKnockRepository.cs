using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Knock.API.Entities;

namespace Knock.API.Services
{
  public interface IKnockRepository
  {
    Task<IEnumerable<Restaurant>> GetRestaurantsAsync();
    Task<Restaurant> GetRestaurantAsync(Guid restaurantId);
    void AddRestaurant(Restaurant restaurant);
    // Task<Restaurant> UpdateRestaurantAsync(Restaurant restaurant);
    // Task<Restaurant> DeleteRestaurantAsync(Restaurant restaurant);
    // Task<bool> RestaurantExists(Guid restaurantId);
    Task<bool> SaveChangesAsync();


  }
}