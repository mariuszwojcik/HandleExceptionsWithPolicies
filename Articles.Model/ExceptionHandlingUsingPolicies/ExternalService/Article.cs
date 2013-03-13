namespace Articles.Model.ExceptionHandlingUsingPolicies.ExternalService
{
    public class Article
    {
        private readonly long _id;
        private readonly string _title;
        private readonly string _author;
        private readonly string _body;

        public Article(long id, string title, string author, string body)
        {
            _id = id;
            _title = title;
            _author = author;
            _body = body;
        }

        public long Id
        {
            get { return _id; }
        }

        public string Title
        {
            get { return _title; }
        }

        public string Author
        {
            get { return _author; }
        }

        public string Body
        {
            get { return _body; }
        }
    }
}