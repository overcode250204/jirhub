//using JirHub.Services.NguyenLPK;
//using JirHub.Services.NguyenLPK.implements;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;

//namespace JirHub.MVCWebApp.NguyenLPK.Controllers
//{
//    public class GithubManagerController : Controller
//    {
//        private readonly IGithubService _githubService;
//        private readonly IProjectRepoService _projectRepoService;

//        public GithubManagerController() 
//        { 
//            _githubService ??= new GithubService();
//            _projectRepoService ??= new ProjectRepoService();
//        }
//        public async Task<IActionResult> Index(int groupId)
        
//        {
//            if (groupId == 0) groupId = 1;
//            var repos = await _projectRepoService.GetProjectRepoByGroupId(groupId);

//            return View(repos);
//        }
//        public async Task<IActionResult> CommitHistory(int repoId)
//        {
//            var commits = await _projectRepoService.GetCommitsByRepoId(repoId);
//            //ViewBag.RepoId = repoId;
//            return View(commits);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Sync(int repoId)
//        {

//            bool result = await _githubService.SyncCommitsForRepo(repoId);

//            if (result)
//                TempData["Message"] = "Đồng bộ thành công!";
//            else
//                TempData["Error"] = "Đồng bộ thất bại!";

//            return RedirectToAction("Index");
//        }

//        [HttpPost]
//        public async Task<IActionResult> SyncPullRequests(int repoId)
//        {

//            bool result = await _githubService.SyncPullRequestsForRepo(repoId, tempToken);

//            if (result) TempData["Message"] = "Đồng bộ PR & Review thành công!";
//            else TempData["Error"] = "Đồng bộ PR thất bại!";

//            return RedirectToAction("Index");
//        }

//        public async Task<IActionResult> PullRequestHistory(int repoId)
//        {
//            var prs = await _projectRepoService.GetPrsByRepoId(repoId);
//            ViewBag.RepoId = repoId;
//            return View(prs);
//        }
//    }
//}
