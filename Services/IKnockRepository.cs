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
    void UpdateRestaurant(Restaurant restaurant);
    void DeleteRestaurant(Restaurant restaurant);
    Task<bool> RestaurantExists(Guid restaurantId);
    Task<bool> SaveChangesAsync();


  }
}