using System;
using Knock.API.Entities;
using Knock.API.DbContexts;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Knock.API.ResourceParameters;

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

    private IQueryable<Restaurant> SortRestaurant(string sort, 
                            IQueryable<Restaurant> collection)
    {

      if(!string.IsNullOrWhiteSpace(sort))
      {
        switch(sort.Trim())
        {
          case "name":
            collection = collection.OrderBy(c => c.Name);
            break;
          case "name_desc":
            collection = collection.OrderByDescending(c => c.Name);
            break;
          case "date_asc":
            collection = collection.OrderBy(c => c.DateCreated);
            break;
          case "rating_asc":
            collection = collection.OrderBy(c => c.AverageRating);
            break;
          case "rating_desc":
            collection = collection.OrderByDescending(c => c.AverageRating);
            break;
          default:
            collection = collection.OrderByDescending(c => c.DateCreated);
            break;
        }
      }
    
      return collection;
    }
    public async Task<IEnumerable<Restaurant>> GetRestaurantsAsync(RestaurantResourceParameters restaurantParams)
    {
      if(restaurantParams == null)
      {
        throw new ArgumentNullException(nameof(restaurantParams));
      }

      // check if both is null
      if(string.IsNullOrWhiteSpace(restaurantParams.MainCategory)
            && string.IsNullOrWhiteSpace(restaurantParams.SearchQuery)
            && string.IsNullOrWhiteSpace(restaurantParams.Sort))
      {
        return await GetRestaurantsAsync();
      }

      var collection = _context.Restaurants as IQueryable<Restaurant>;

      if(!string.IsNullOrWhiteSpace(restaurantParams.SearchQuery))
      {
        var searchQuery = restaurantParams.SearchQuery.Trim();
        collection = collection.Where(r => r.Name.IndexOf(searchQuery, StringComparison.CurrentCultureIgnoreCase) >= 0
                                        || r.Address.IndexOf(searchQuery, StringComparison.CurrentCultureIgnoreCase) >= 0);

        Console.WriteLine(collection.ToList()[0]);
      }

      // sort query
      collection = SortRestaurant(restaurantParams.Sort, collection);
      return await collection.AsNoTracking().ToListAsync();
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

      // add restaurant id
      restaurant.Id = Guid.NewGuid();

      foreach(var review in restaurant.Reviews)
      {
        review.Id = Guid.NewGuid();
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

    public async Task<IEnumerable<Review>> GetReviewsAsync(Guid restaurantId, IEnumerable<Guid> reviewIds)
    {
      if(restaurantId == null)
      {
        throw new ArgumentNullException(nameof(restaurantId));
      }

      if(reviewIds == null)
      {
        throw new ArgumentNullException(nameof(reviewIds));
      }

      return await _context.Reviews
                    .Where(r => r.RestaurantId == restaurantId && reviewIds.Contains(r.Id))
                    .ToListAsync();

    }

    public async Task<Review> GetReviewAsync(Guid restaurantId, Guid reviewId)
    {
      if(reviewId == Guid.Empty)
      {
        throw new ArgumentNullException(nameof(reviewId));
      }

      if(restaurantId == Guid.Empty)
      {
        throw new ArgumentNullException(nameof(restaurantId));
      }

      return await _context.Reviews
                    .Where(r => r.Id == reviewId && r.RestaurantId == restaurantId)
                    .FirstOrDefaultAsync();
    }

    public void AddReview(Guid restaurantId, Review review)
    {

      if(restaurantId == Guid.Empty)
      {
        throw new ArgumentNullException(nameof(restaurantId));
      }

      if(review == null)
      {
        throw new ArgumentNullException(nameof(review));
      }

      // add restaurant id
      review.RestaurantId = restaurantId;      
      _context.Reviews.Add(review);
    }

    public void UpdateReview(Review review)
    {
      // No implementation
    }

    public void DeleteReview(Review review)
    {
      _context.Reviews.Remove(review);
    }

    public async Task GetAverageRatingAsync(Guid restaurantId)
    {
      // get average of all ratings related to certain restaurant
      var rating = await _context.Reviews
                              .Where(r => r.RestaurantId == restaurantId)
                              .Select(r => r.Rating)
                              .ToListAsync();
      // get ratings count
      int ratingsCount = rating.Count;
      int ratingsSum = 0;

      foreach(byte r in rating)
      {
        ratingsSum += r;
      }

      // get average
      double ratingsAverage = (double) ratingsSum / ratingsCount;

      // modify average rating
      var restaurant = await GetRestaurantAsync(restaurantId);
      restaurant.AverageRating = Math.Round(ratingsAverage, 2);
      
      // save changes
      await SaveChangesAsync();
                              
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
      return await _context.Users.ToListAsync();
    }

    public  async Task<User> GetUserAsync(Guid userId)
    {
      if(userId == Guid.Empty)
      {
        throw new ArgumentNullException(nameof(userId));
      }

      // get user data
      return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public User GetUser(Guid userId)
    {
      if(userId == Guid.Empty)
      {
        throw new ArgumentNullException(nameof(userId));
      }

      // get user data
      return _context.Users.FirstOrDefault(u => u.Id == userId);
    }

    public void AddUser(User user)
    {
      // check if user if null
      if(user == null)
      {
        throw new ArgumentNullException(nameof(user));
      }

      // generate user id
      user.Id = Guid.NewGuid();
      
      // add user to db
      _context.Users.Add(user);
    }

    public async Task<bool> EmailExists(string email)
    {
      if(string.IsNullOrWhiteSpace(email))
      {
        throw new ArgumentNullException(nameof(email));
      }

      return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public void UpdateUser(User user)
    {
      throw new NotImplementedException();
    }

    public void DeleteUser(User user)
    {
      throw new NotImplementedException();
    }

    // Auth
    public async Task<User> Authenticated(string email, string password)
    {
      // email and password exist
      if(string.IsNullOrWhiteSpace(email))
      {
        throw new ArgumentNullException(nameof(email));
      }

      if(string.IsNullOrWhiteSpace(password))
      {
        throw new ArgumentNullException(nameof(password));
      }
      
      return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
  }
}