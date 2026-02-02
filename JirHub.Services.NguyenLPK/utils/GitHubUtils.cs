using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JirHub.Services.NguyenLPK.utils
{
    public static class GitHubUtils
    {

        public static (string Owner, string Name)? GetRepoInfoFromUrl(string url)
        {
            try
            {
                var uri = new Uri(url);
                var segments = uri.AbsolutePath.Trim('/').Split('/');
                if (segments.Length >= 2) return (segments[0], segments[1]);
            }
            catch { }
            return null;
        }

    }
}
