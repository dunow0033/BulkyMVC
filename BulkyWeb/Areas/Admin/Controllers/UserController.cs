﻿using Microsoft.AspNetCore.Mvc;
using Bulky.DataAccess.Data;
using Bulky.Models;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Bulky.DataAccess.Repository;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        
        public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {

            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

		public IActionResult RoleManagement(string userId)
		{
            string RoleID = _db.UserRoles.FirstOrDefault(u => u.UserId == userId).RoleId;

            RoleManagementVM RoleVM = new RoleManagementVM() {
                ApplicationUser = _db.ApplicationUsers.Include(u => u.Company).FirstOrDefault(u => u.Id == userId),

                RoleList = _db.Roles.Select(i => new SelectListItem {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = _db.Companies.Select(i => new SelectListItem {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),

            };

            RoleVM.ApplicationUser.Role = _db.Roles.FirstOrDefault(u => u.Id == RoleID).Name;

			return View(RoleVM);
		}

        [HttpPost]
		public IActionResult RoleManagement(RoleManagementVM roleManagementVM)
		{
			string RoleID = _db.UserRoles.FirstOrDefault(u => u.UserId == roleManagementVM.ApplicationUser.Id).RoleId;
            string oldRole = _db.Roles.FirstOrDefault(u => u.Id == RoleID).Name;

            if (!(roleManagementVM.ApplicationUser.Role == oldRole))
            {
                ApplicationUser applicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == roleManagementVM.ApplicationUser.Id);
                if (roleManagementVM.ApplicationUser.Role == SD.Role_Company)
                {
                    applicationUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;
                }
                if (oldRole == SD.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }
                _db.SaveChanges();

                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleManagementVM.ApplicationUser.Role).GetAwaiter().GetResult();
            }

			return RedirectToAction("Index");
		}

		#region API CALLS

		[HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> objUserList = _db.ApplicationUsers.Include(u => u.Company).ToList();

            var userRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach(var user in objUserList) {

                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

                if(user.Company == null) {
                    user.Company = new() { Name = "" };
                }
            }

            return Json(new { data = objUserList });
        }

		[HttpPost]
		public IActionResult LockUnlock([FromBody]string id)
		{
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);

            if(objFromDb == null) {
                return Json(new { success = false, message = "Error while Locking/Unlocking" }); 
            }

            if(objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }

            _db.SaveChanges();

			return Json(new { success = true, message = "Operation Successful" });
		}

        //[HttpDelete]
        //public IActionResult Delete(int? id)
        //{
        //    var CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
        //    if (CompanyToBeDeleted != null)
        //    {
        //        return Json(new { success = true, message = "Error while deleting" });
        //    }

        //    _unitOfWork.Company.Remove(CompanyToBeDeleted);
        //    _unitOfWork.Save();
        //    return Json(new { success = true, message = "Delete Successful" });
        //}

        #endregion
    }
}
