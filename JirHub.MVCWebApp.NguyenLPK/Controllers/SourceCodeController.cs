using JirHub.Entities.NguyenLPK.Models;
using JirHub.Services.NguyenLPK;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JirHub.MVCWebApp.NguyenLPK.Controllers
{
    [Authorize]
    public class SourceCodeController : Controller
    {

        private readonly IGithubService _githubService;
        private readonly IProjectRepoService _projectRepoService;
        private readonly IWorkLinkService _workLinkService;
        public SourceCodeController(IGithubService githubService, IProjectRepoService projectRepoService, IWorkLinkService workLinkService) 
        {
            _githubService = githubService;
            _projectRepoService = projectRepoService;
            _workLinkService = workLinkService;
        }

        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Index(int groupId)
        {
            ViewBag.GroupId = groupId;
            return View(await _projectRepoService.GetProjectRepoByGroupId(groupId));
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult AddRepo(int groupId, string repoUrl, string repoType)
        {
            _projectRepoService.CreateProjectRepoAsync(new ProjectReposNguyenLpk
            {
                GroupId = groupId,
                RepoUrl = repoUrl,
                RepoType = repoType,
                RepoName = repoUrl.Split('/').Last()
            });
            return RedirectToAction("Index", new { groupId = groupId });
        }

        [HttpPost]
        public async Task<IActionResult> Sync(int groupId)
        {
            await _githubService.SyncGroupDataAsync(groupId);
            return RedirectToAction("Index", new { groupId = groupId });
        }
        public IActionResult Traceability(int groupId)
        {
   
            

            var links = _workLinkService.GetWorkLinksByGroupId(groupId);
         
                
                
            return View(links);
        }

    }
}
