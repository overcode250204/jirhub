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

namespace JirHub.MVCWebApp.NguyenLPK.Controllers
{
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
            ViewData["CurrentFilterName"] = repoName;
            ViewData["CurrentFilterGroup"] = groupName;
            return View(result);
        }

        public async Task<IActionResult> SyncGroup(int groupId)
        {
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
            if (id == null)
            {
                return NotFound();
            }

            //var projectReposNguyenLpk = await _context.ProjectReposNguyenLpks
            //    .Include(p => p.Group)
            //    .FirstOrDefaultAsync(m => m.RepoId == id);

            var projectReposNguyenLpk = await _projectRepoService.GetPrsByRepoId(id);
            if (projectReposNguyenLpk == null)
            {
                return NotFound();
            }

            return View(projectReposNguyenLpk);
        }


        // GET: ProjectReposNguyenLpks/Create
        public async Task<IActionResult> Create()
        {

            ViewData["GroupId"] = new SelectList(await _classGroupService.GetAllClassGroupAsync(), "GroupId", "GroupName");
            return View();
        }

        // POST: ProjectReposNguyenLpks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectReposNguyenLpk projectReposNguyenLpk)
        {
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


            return View(projectReposNguyenLpk);
        }

        // POST: ProjectReposNguyenLpks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProjectReposNguyenLpk projectReposNguyenLpk)
        {
            //if (id != projectReposNguyenLpk.RepoId)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _projectRepoService.UpdateProjectRepoAsync(projectReposNguyenLpk);
                    if (result > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    //if (!ProjectReposNguyenLpkExists(projectReposNguyenLpk.RepoId))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                    Console.WriteLine(ex.ToString());
                    throw new Exception(ex.Message);
                }
                
            }
            ViewBag.Groups = new SelectList(
                await _projectRepoService.GetAllAsync(),
                "GroupId",
                "GroupName",
                projectReposNguyenLpk.GroupId
            );

            return View(projectReposNguyenLpk);
        }

        // GET: ProjectReposNguyenLpks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var projectReposNguyenLpk = await _context.ProjectReposNguyenLpks
            //    .Include(p => p.Group)
            //    .FirstOrDefaultAsync(m => m.RepoId == id);
            var projectReposNguyenLpk = await _projectRepoService.DeleteProjectRepoAsync(id);
            if (projectReposNguyenLpk)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Delete), new {id = id});
        }

        //    // POST: ProjectReposNguyenLpks/Delete/5
        //    [HttpPost, ActionName("Delete")]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> DeleteConfirmed(int id)
        //    {
        //        var projectReposNguyenLpk = await _context.ProjectReposNguyenLpks.FindAsync(id);
        //        if (projectReposNguyenLpk != null)
        //        {
        //            _context.ProjectReposNguyenLpks.Remove(projectReposNguyenLpk);
        //        }

        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    private bool ProjectReposNguyenLpkExists(int id)
        //    {
        //        return _context.ProjectReposNguyenLpks.Any(e => e.RepoId == id);
        //    }




    }
}
