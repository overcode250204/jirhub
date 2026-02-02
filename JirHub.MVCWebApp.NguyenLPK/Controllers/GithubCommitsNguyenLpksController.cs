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
    public class GithubCommitsNguyenLpksController : Controller
    {
        private readonly IGithubCommitService _githubCommitService; 

        public GithubCommitsNguyenLpksController(IGithubCommitService githubCommitService)
        {
            _githubCommitService = githubCommitService;
        }

        // GET: GithubCommitsNguyenLpks
        public async Task<IActionResult> Index(int repoId, string repoName)
        {
            var result = await _githubCommitService.SearchAsync(repoId, repoName);
            return View(result);
        }

        // GET: GithubCommitsNguyenLpks/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var githubCommitsNguyenLpk = await _githubCommitService.GetCommitByIdAsync(id);

            if (githubCommitsNguyenLpk == null)
            {
                return NotFound();
            }

            return View(githubCommitsNguyenLpk);
        }

        //// GET: GithubCommitsNguyenLpks/Create
        //public IActionResult Create()
        //{
        //    ViewData["MappedMemberId"] = new SelectList(_context.GroupMembers, "MemberId", "MemberId");
        //    ViewData["RepoId"] = new SelectList(_context.ProjectReposNguyenLpks, "RepoId", "RepoId");
        //    return View();
        //}

        //// POST: GithubCommitsNguyenLpks/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("CommitId,CommitHash,RepoId,Message,Additions,Deletions,CommittedDate,AuthorGithubUsername,MappedMemberId,LinkedIssueKey")] GithubCommitsNguyenLpk githubCommitsNguyenLpk)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(githubCommitsNguyenLpk);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["MappedMemberId"] = new SelectList(_context.GroupMembers, "MemberId", "MemberId", githubCommitsNguyenLpk.MappedMemberId);
        //    ViewData["RepoId"] = new SelectList(_context.ProjectReposNguyenLpks, "RepoId", "RepoId", githubCommitsNguyenLpk.RepoId);
        //    return View(githubCommitsNguyenLpk);
        //}

        //// GET: GithubCommitsNguyenLpks/Edit/5
        //public async Task<IActionResult> Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var githubCommitsNguyenLpk = await _context.GithubCommitsNguyenLpks.FindAsync(id);
        //    if (githubCommitsNguyenLpk == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["MappedMemberId"] = new SelectList(_context.GroupMembers, "MemberId", "MemberId", githubCommitsNguyenLpk.MappedMemberId);
        //    ViewData["RepoId"] = new SelectList(_context.ProjectReposNguyenLpks, "RepoId", "RepoId", githubCommitsNguyenLpk.RepoId);
        //    return View(githubCommitsNguyenLpk);
        //}

        //// POST: GithubCommitsNguyenLpks/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(long id, [Bind("CommitId,CommitHash,RepoId,Message,Additions,Deletions,CommittedDate,AuthorGithubUsername,MappedMemberId,LinkedIssueKey")] GithubCommitsNguyenLpk githubCommitsNguyenLpk)
        //{
        //    if (id != githubCommitsNguyenLpk.CommitId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(githubCommitsNguyenLpk);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!GithubCommitsNguyenLpkExists(githubCommitsNguyenLpk.CommitId))
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
        //    ViewData["MappedMemberId"] = new SelectList(_context.GroupMembers, "MemberId", "MemberId", githubCommitsNguyenLpk.MappedMemberId);
        //    ViewData["RepoId"] = new SelectList(_context.ProjectReposNguyenLpks, "RepoId", "RepoId", githubCommitsNguyenLpk.RepoId);
        //    return View(githubCommitsNguyenLpk);
        //}

        //// GET: GithubCommitsNguyenLpks/Delete/5
        //public async Task<IActionResult> Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var githubCommitsNguyenLpk = await _context.GithubCommitsNguyenLpks
        //        .Include(g => g.MappedMember)
        //        .Include(g => g.Repo)
        //        .FirstOrDefaultAsync(m => m.CommitId == id);
        //    if (githubCommitsNguyenLpk == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(githubCommitsNguyenLpk);
        //}

        //// POST: GithubCommitsNguyenLpks/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(long id)
        //{
        //    var githubCommitsNguyenLpk = await _context.GithubCommitsNguyenLpks.FindAsync(id);
        //    if (githubCommitsNguyenLpk != null)
        //    {
        //        _context.GithubCommitsNguyenLpks.Remove(githubCommitsNguyenLpk);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool GithubCommitsNguyenLpkExists(long id)
        //{
        //    return _context.GithubCommitsNguyenLpks.Any(e => e.CommitId == id);
        //}
    }
}
