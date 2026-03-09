using JirHub.Entities.NguyenLPK.Models;
using JirHub.Services.NguyenLPK;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace JirHub.MVCWebApp.NguyenLPK.Controllers
{
    [Authorize]
    public class ProjectReposNguyenLpksController : Controller
    {
        private readonly IProjectRepoService _projectRepoService;
        private readonly IGithubService _githubService;
        private readonly IClassGroupService _classGroupService;


        public ProjectReposNguyenLpksController(IProjectRepoService projectRepoService, IGithubService githubService, IClassGroupService classGroupService)
        {
            _projectRepoService = projectRepoService;
            _githubService = githubService;
            _classGroupService = classGroupService;
        }

        // GET: ProjectReposNguyenLpks
        //public async Task<IActionResult> Index()
        //{

        //    return View(await _projectRepoService.GetAllAsync());
        //}

        public async Task<IActionResult> Index(string repoName, string repoType, string groupName)
        {
            var result = await _projectRepoService.SearchProjectRepo(repoName, repoType, groupName);

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<int> leaderGroupIds = new List<int>();
            List<int> allowedGroupIds = new List<int>();
            if (int.TryParse(userIdStr, out int userId))
            {
                leaderGroupIds = await _projectRepoService.GetLeaderGroupIdsAsync(userId);

                if (User.IsInRole("LECTURER")) allowedGroupIds = await _projectRepoService.GetLecturerGroupIdsAsync(userId);
                else if (User.IsInRole("STUDENT")) allowedGroupIds = await _projectRepoService.GetStudentGroupIdsAsync(userId);
            }
            ViewBag.LeaderGroupIds = leaderGroupIds;

            if (User.IsInRole("LECTURER") || User.IsInRole("STUDENT"))
            {
                result = result.Where(r => allowedGroupIds.Contains(r.GroupId)).ToList();
            }

            ViewData["CurrentFilterName"]     = repoName;
            ViewData["CurrentFilterGroup"]    = groupName;
            ViewData["CurrentFilterRepoType"] = repoType;

            return View(result);
        }

        public async Task<IActionResult> SyncGroup(int groupId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<int> leaderGroupIds = new List<int>();
            if (int.TryParse(userIdStr, out int userId))
            {
                leaderGroupIds = await _projectRepoService.GetLeaderGroupIdsAsync(userId);
            }

            if (!User.IsInRole("ADMIN") && !leaderGroupIds.Contains(groupId))
            {
                return Forbid();
            }

            try
            {
                bool isSuccess = await _githubService.SyncGroupDataAsync(groupId);

                if (isSuccess)
                    TempData["SuccessMessage"] = "Đồng bộ dữ liệu GitHub thành công!";
                else
                    TempData["ErrorMessage"] = "Đồng bộ thất bại. Vui lòng kiểm tra Token hoặc Mạng.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi hệ thống: " + ex.Message;
            }


            return RedirectToAction(nameof(Index));
        }


        // GET: ProjectReposNguyenLpks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var repo = await _projectRepoService.GetProjectRepoById(id);
            if (repo == null) return NotFound();

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<int> allowedGroupIds = new List<int>();
            List<int> leaderGroupIds = new List<int>();

            if (int.TryParse(userIdStr, out int userId))
            {
                leaderGroupIds = await _projectRepoService.GetLeaderGroupIdsAsync(userId);

                if (User.IsInRole("LECTURER")) allowedGroupIds = await _projectRepoService.GetLecturerGroupIdsAsync(userId);
                else if (User.IsInRole("STUDENT")) allowedGroupIds = await _projectRepoService.GetStudentGroupIdsAsync(userId);
            }

            
            if ((User.IsInRole("LECTURER") || User.IsInRole("STUDENT")) && !allowedGroupIds.Contains(repo.GroupId))
            {
                return Forbid();
            }

            if (!User.IsInRole("ADMIN") && !repo.IsActive && !leaderGroupIds.Contains(repo.GroupId))
            {
                return Forbid();
            }

            return View(repo);
        }


        // GET: ProjectReposNguyenLpks/Create
        public async Task<IActionResult> Create()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<int> leaderGroupIds = new List<int>();
            if (int.TryParse(userIdStr, out int userId))
            {
                leaderGroupIds = await _projectRepoService.GetLeaderGroupIdsAsync(userId);
            }

            var allGroups = await _classGroupService.GetAllClassGroupAsync();
            if (!User.IsInRole("ADMIN"))
            {
                allGroups = allGroups.Where(g => leaderGroupIds.Contains(g.GroupId)).ToList();
            }

            ViewData["GroupId"] = new SelectList(allGroups, "GroupId", "GroupName");
            return View();
        }

        // POST: ProjectReposNguyenLpks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectReposNguyenLpk projectReposNguyenLpk)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<int> leaderGroupIds = new List<int>();
            if (int.TryParse(userIdStr, out int userId))
            {
                leaderGroupIds = await _projectRepoService.GetLeaderGroupIdsAsync(userId);
            }

            if (!User.IsInRole("ADMIN") && !leaderGroupIds.Contains(projectReposNguyenLpk.GroupId))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                //_context.Add(projectReposNguyenLpk);
                //await _context.SaveChangesAsync();
                var result = await _projectRepoService.CreateProjectRepoAsync(projectReposNguyenLpk);

                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                } 


                  
            }

            var classGroups = await _classGroupService.GetAllClassGroupAsync();
            ViewData["GroupId"] = new SelectList(classGroups, "GroupId", "GroupName", projectReposNguyenLpk.GroupId);
            return View(projectReposNguyenLpk);
        }


        // GET: ProjectReposNguyenLpks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectReposNguyenLpk = await _projectRepoService.GetProjectRepoById(id);
            if (projectReposNguyenLpk == null)
            {
                return NotFound();
            }

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<int> leaderGroupIds = new List<int>();
            if (int.TryParse(userIdStr, out int userId))
            {
                leaderGroupIds = await _projectRepoService.GetLeaderGroupIdsAsync(userId);
            }

            if (!User.IsInRole("ADMIN") && !leaderGroupIds.Contains(projectReposNguyenLpk.GroupId))
            {
                return Forbid();
            }

            ViewData["GroupId"] = new SelectList(await _classGroupService.GetAllClassGroupAsync(), "GroupId", "GroupName", projectReposNguyenLpk.GroupId);
            return View(projectReposNguyenLpk);
        }

        // POST: ProjectReposNguyenLpks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProjectReposNguyenLpk projectReposNguyenLpk)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<int> leaderGroupIds = new List<int>();
            if (int.TryParse(userIdStr, out int userId))
            {
                leaderGroupIds = await _projectRepoService.GetLeaderGroupIdsAsync(userId);
            }

            if (!User.IsInRole("ADMIN") && !leaderGroupIds.Contains(projectReposNguyenLpk.GroupId))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _projectRepoService.UpdateProjectRepoAsync(projectReposNguyenLpk);
                    if (result > 0)
                    {
                        TempData["SuccessMessage"] = "Cập nhật repo thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError("", "Không tìm thấy repo để cập nhật.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi lưu: " + ex.Message);
                }
            }

            ViewData["GroupId"] = new SelectList(await _classGroupService.GetAllClassGroupAsync(), "GroupId", "GroupName", projectReposNguyenLpk.GroupId);
            return View(projectReposNguyenLpk);
        }


        // GET: ProjectReposNguyenLpks/Delete/5 — hiển thị trang confirm trước khi xóa
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var repo = await _projectRepoService.GetProjectRepoById(id);
            if (repo == null) return NotFound();

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<int> leaderGroupIds = new List<int>();
            if (int.TryParse(userIdStr, out int userId))
            {
                leaderGroupIds = await _projectRepoService.GetLeaderGroupIdsAsync(userId);
            }

            if (!User.IsInRole("ADMIN") && !leaderGroupIds.Contains(repo.GroupId))
            {
                return Forbid();
            }

            return View(repo);
        }

        // POST: ProjectReposNguyenLpks/Delete/5 — thực sự xóa sau khi user confirm
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repo = await _projectRepoService.GetProjectRepoById(id);
            if (repo == null) return NotFound();

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<int> leaderGroupIds = new List<int>();
            if (int.TryParse(userIdStr, out int userId))
            {
                leaderGroupIds = await _projectRepoService.GetLeaderGroupIdsAsync(userId);
            }

            if (!User.IsInRole("ADMIN") && !leaderGroupIds.Contains(repo.GroupId))
            {
                return Forbid();
            }

            try
            {
                var result = await _projectRepoService.DeleteProjectRepoAsync(id);
                TempData[result ? "SuccessMessage" : "ErrorMessage"] =
                    result ? "Đã xóa repository thành công!" : "Không tìm thấy repository để xóa.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi xóa: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }





    }
}
