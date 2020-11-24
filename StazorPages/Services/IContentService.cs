using System;
using System.Threading.Tasks;
using StazorPages.Models;

namespace StazorPages.Services
{
    public interface IContentService
    {
        Task<IRetrievedContent> GetContentByUrlSlug(string path);

        Task<IRetrievedContent> GetContentById(Guid id);
    }
}
