using System;

namespace Helvetica.NntpClient.Models
{
    public class Article : ArticlePartBase
    {
        public Head Headers { get; private set; }
        public Body Body { get; private set; }

        public Article(NntpResponse response)
        {
            Parse(response);
        }

        public override void Dispose()
        {
            if(Body != null)
                Body.Dispose();
            if(Headers != null)
                Headers.Dispose();
        }

        protected override void Parse(NntpResponse response)
        {
            base.Parse(response);

            Headers = new Head(response);
            Body = new Body(response);
        }
    }
}