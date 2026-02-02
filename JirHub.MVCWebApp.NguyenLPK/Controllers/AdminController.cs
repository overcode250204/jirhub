using JirHub.Entities.NguyenLPK.Models;
using JirHub.Services.NguyenLPK;
using JirHub.Services.NguyenLPK.implements;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JirHub.MVCWebApp.NguyenLPK.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProjectRepoService _projectRepoService;
        public AdminController()
        {
            _projectRepoService ??= new ProjectRepoService();
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ProjectRepo(int groupid)
        {
            var listRepo = await _projectRepoService.GetProjectRepoByGroupId(groupid);
            ViewBag.ListRepo = listRepo;
            return View(listRepo);
        }

        public async Task<IActionResult> GetRepoById(int id)
        {
            ProjectReposNguyenLpk entity =  await _projectRepoService.GetProjectRepoById(id);

            if (entity == null) return NotFound();

            return View(entity);
        }

        public async Task<IActionResult> EditRepo(ProjectReposNguyenLpk projectReposNguyenLpk)
        {
            if (ModelState.IsValid)
            {
                await _projectRepoService.UpdateProjectRepoAsync(projectReposNguyenLpk);
                return RedirectToAction("ProjectRepo", new { groupId = projectReposNguyenLpk.GroupId });
            }
            return View(projectReposNguyenLpk);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRepo(int id)
        {
            var repo = await _projectRepoService.GetProjectRepoById(id);
            if (repo != null)
            {
                int groupId = repo.GroupId;
                await _projectRepoService.DeleteProjectRepoAsync(id);
                return RedirectToAction("ProjectRepo", new { groupid = groupId });
            }
            return RedirectToAction("Index"); // Hoặc trang lỗi
        }


    }
}
