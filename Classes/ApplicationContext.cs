namespace WebUrlCutterApp.Classes
{

    public class ApplicationContext //можно было унаследовать от DbContext EntityFramework, но в ТЗ задачи с БД не стояло
    {
        public List<Link> Links { get; set; }

        public ApplicationContext()// в случае с DbContext - тут бы создавалась БД если не была создана раньше 
        {
            Links = new List<Link>();
        }

        public string ShortHostName { get; set; }

        public Link? CreateShortLink(string fullLink, int lifeMinutes = 2)
        {
            Link? link; 
            if (Links.Count == 0) link = null;
            else
            link = Links.Where(x => x.FullUrl.ToUpper() == fullLink.ToUpper()).FirstOrDefault();
            //нет такой ссылки в списке
            if (link == null)
            {
                string id = UrlShorter.GetShortLinkGuid(fullLink);
                string shorturl = ShortHostName + id;

                link = new Link
                {
                    FullUrl = fullLink,
                    Id = id,
                    ShortUrl = shorturl,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddMinutes(lifeMinutes)
                };

                Links.Add(link);
                return link;
            }
            else
            {
                Console.WriteLine(link.ShortUrl);

                //есть такой адрес и ссылка просрочена
                if (link.EndTime <= DateTime.Now)
                {
                    return null;
                }
                else
                {
                    return link;
                }
            }
        }


    }
}
