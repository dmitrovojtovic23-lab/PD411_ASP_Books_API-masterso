namespace PD411_Books.API.Settings
{
    public static class StaticFilesSettings
    {
        public static string StorageDir => "Storage";

        public static string BooksDir => "Books";
        public static string AuthorsDir => "Authors";
        public static string ShareDir => "Share";

        public static string ShareUrl => "/image";
        public static string BookUrl => "/image/book";
        public static string AuthorUrl => "/image/author";
    }
}
