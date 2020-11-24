namespace StazorPages.Models
{
    public interface IRetrievedContent
    {
        string GetContentTypeAlias();

        string Title { get; set; }

        string Url { get; set; }
    }
}
