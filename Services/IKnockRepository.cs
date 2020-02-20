using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Knock.API.Entities;

namespace Knock.API.Services
{
  public interface IKnockRepository
  {
    // Restaurants
    Task<IEnumerable<Restaurant>> GetRestaurantsAsync();
    Task<IEnumerable<Restaurant>> GetRestaurantsAsync(IEnumerable<Guid> restaurantIds);
    Task<Restaurant> GetRestaurantAsync(Guid restaurantId);
    void AddRestaurant(Restaurant restaurant);
    void UpdateRestaurant(Restaurant restaurant);
    void DeleteRestaurant(Restaurant restaurant);
    Task<bool> RestaurantExists(Guid restaurantId);
    Task<bool> SaveChangesAsync();

    // Reviews
    Task<IEnumerable<Review>> GetReviewsAsync(Guid restaurantId, IEnumerable<Guid> reviewIds);
    Task<IEnumerable<Review>> GetReviewsAsync();
    Task<Review> GetReviewAsync(Guid restaurantId, Guid reviewId);
    void AddReview(Guid restaurantId, Review review);
    void UpdateReview(Review review);
    void DeleteReview(Review review);

    // Users
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User> GetUserAsync(Guid userId);
    User GetUser(Guid userId);
    void AddUser(User user);
    void UpdateUser(User user);
    void DeleteUser(User user);

    // Auth
    Task<User> Authenticated(string email, string password);
    Task<bool> EmailExists(string email);

  }
}