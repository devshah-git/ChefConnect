using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ChefConnect.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChefConnect.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public readonly ChefConnectDbContext _chefConnectDbContext;

        public AdminController(ChefConnectDbContext chefConnectDbContext)
        {
            _chefConnectDbContext = chefConnectDbContext;
        }

        [HttpGet("/Admin/Home")]
        public async Task<IActionResult> AdminHome()
        {
            return View();
        }


        //Get Method to get all the reviews
        [HttpGet("/Admin/Reviews")]
        public async Task<IActionResult> GetAllPendingReviews()
        {
            var reviews = await _chefConnectDbContext.Reviews.Include(r => r.Customer).Include(r => r.ChefRecipe).ThenInclude(r => r.Chef).Where(r => r.Status == Entities.Reviews.ReviewStatus.Reported).ToListAsync();

            return View("AdminReview",reviews);
        }

        [HttpGet("/{id}/Approved")]
        public async Task<IActionResult> ApproveReview(int id)
        {
            var review = await _chefConnectDbContext.Reviews.Include(r => r.Customer).Include(r => r.ChefRecipe).ThenInclude(r => r.Chef).Where(r => r.ReviewsId == id).FirstOrDefaultAsync();

            review.Status = Entities.Reviews.ReviewStatus.Clean;
            _chefConnectDbContext.Reviews.Update(review);
            _chefConnectDbContext.SaveChanges();

            var reviews = await _chefConnectDbContext.Reviews.Include(r => r.Customer).Include(r => r.ChefRecipe).ThenInclude(r => r.Chef).Where(r => r.Status == Entities.Reviews.ReviewStatus.Reported).ToListAsync();

            return View("AdminReview", reviews);
        }

        [HttpGet("/{id}/Deleted")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _chefConnectDbContext.Reviews.Include(r => r.Customer).Include(r => r.ChefRecipe).ThenInclude(r => r.Chef).Where(r => r.ReviewsId == id).FirstOrDefaultAsync();

            _chefConnectDbContext.Reviews.Remove(review);
            _chefConnectDbContext.SaveChanges();

            var reviews = await _chefConnectDbContext.Reviews.Include(r => r.Customer).Include(r => r.ChefRecipe).ThenInclude(r => r.Chef).Where(r => r.Status == Entities.Reviews.ReviewStatus.Reported).ToListAsync();

            return View("AdminReview", reviews);
        }
    }
}

