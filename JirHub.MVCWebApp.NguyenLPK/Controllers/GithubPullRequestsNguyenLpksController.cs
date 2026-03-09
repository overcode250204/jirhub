using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JirHub.Entities.NguyenLPK.Models;
using JirHub.Services.NguyenLPK;

namespace JirHub.MVCWebApp.NguyenLPK.Controllers
{
    public class GithubPullRequestsNguyenLpksController : Controller
    {
        private readonly IGithubPrService _githubPrService;

        public GithubPullRequestsNguyenLpksController(IGithubPrService githubPrService)
        {
            _githubPrService = githubPrService;
        }

        // GET: GithubPullRequestsNguyenLpks
        public async Task<IActionResult> Index(int repoId, string repoName)
        {
            List<GithubPullRequestsNguyenLpk> prs = await _githubPrService.SearchAsync(repoId, repoName);
            ViewData["RepoName"] = repoName;
            ViewData["RepoId"]   = repoId;
            return View(prs);
        }

        // GET: GithubPullRequestsNguyenLpks/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var githubPullRequestsNguyenLpk = await _githubPrService.GetPrByIdAsync(id);
            if (githubPullRequestsNguyenLpk == null)
            {
                return NotFound();
            }

            return View(githubPullRequestsNguyenLpk);
        }

        //// GET: GithubPullRequestsNguyenLpks/Create
        //public IActionResult Create()
        //{
        //    ViewData["MappedMemberId"] = new SelectList(_context.GroupMembers, "MemberId", "MemberId");
        //    ViewData["RepoId"] = new SelectList(_context.ProjectReposNguyenLpks, "RepoId", "RepoId");
        //    return View();
        //}

        //// POST: GithubPullRequestsNguyenLpks/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("PrId,RepoId,PrNumber,Title,State,IsMerged,AuthorGithubUsername,MappedMemberId,Additions,Deletions,ChangedFiles,CreatedAt,UpdatedAt,MergedAt,ClosedAt,LinkedIssueKey")] GithubPullRequestsNguyenLpk githubPullRequestsNguyenLpk)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(githubPullRequestsNguyenLpk);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["MappedMemberId"] = new SelectList(_context.GroupMembers, "MemberId", "MemberId", githubPullRequestsNguyenLpk.MappedMemberId);
        //    ViewData["RepoId"] = new SelectList(_context.ProjectReposNguyenLpks, "RepoId", "RepoId", githubPullRequestsNguyenLpk.RepoId);
        //    return View(githubPullRequestsNguyenLpk);
        //}

        //// GET: GithubPullRequestsNguyenLpks/Edit/5
        //public async Task<IActionResult> Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var githubPullRequestsNguyenLpk = await _context.GithubPullRequestsNguyenLpks.FindAsync(id);
        //    if (githubPullRequestsNguyenLpk == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["MappedMemberId"] = new SelectList(_context.GroupMembers, "MemberId", "MemberId", githubPullRequestsNguyenLpk.MappedMemberId);
        //    ViewData["RepoId"] = new SelectList(_context.ProjectReposNguyenLpks, "RepoId", "RepoId", githubPullRequestsNguyenLpk.RepoId);
        //    return View(githubPullRequestsNguyenLpk);
        //}

        //// POST: GithubPullRequestsNguyenLpks/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(long id, [Bind("PrId,RepoId,PrNumber,Title,State,IsMerged,AuthorGithubUsername,MappedMemberId,Additions,Deletions,ChangedFiles,CreatedAt,UpdatedAt,MergedAt,ClosedAt,LinkedIssueKey")] GithubPullRequestsNguyenLpk githubPullRequestsNguyenLpk)
        //{
        //    if (id != githubPullRequestsNguyenLpk.PrId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(githubPullRequestsNguyenLpk);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!GithubPullRequestsNguyenLpkExists(githubPullRequestsNguyenLpk.PrId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["MappedMemberId"] = new SelectList(_context.GroupMembers, "MemberId", "MemberId", githubPullRequestsNguyenLpk.MappedMemberId);
        //    ViewData["RepoId"] = new SelectList(_context.ProjectReposNguyenLpks, "RepoId", "RepoId", githubPullRequestsNguyenLpk.RepoId);
        //    return View(githubPullRequestsNguyenLpk);
        //}

        //// GET: GithubPullRequestsNguyenLpks/Delete/5
        //public async Task<IActionResult> Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var githubPullRequestsNguyenLpk = await _context.GithubPullRequestsNguyenLpks
        //        .Include(g => g.MappedMember)
        //        .Include(g => g.Repo)
        //        .FirstOrDefaultAsync(m => m.PrId == id);
        //    if (githubPullRequestsNguyenLpk == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(githubPullRequestsNguyenLpk);
        //}

        //// POST: GithubPullRequestsNguyenLpks/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(long id)
        //{
        //    var githubPullRequestsNguyenLpk = await _context.GithubPullRequestsNguyenLpks.FindAsync(id);
        //    if (githubPullRequestsNguyenLpk != null)
        //    {
        //        _context.GithubPullRequestsNguyenLpks.Remove(githubPullRequestsNguyenLpk);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool GithubPullRequestsNguyenLpkExists(long id)
        //{
        //    return _context.GithubPullRequestsNguyenLpks.Any(e => e.PrId == id);
        //}
    }
}
