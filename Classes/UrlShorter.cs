using System.Text;

namespace WebUrlCutterApp.Classes
{
    public class UrlShorter
    {

        public static string GetShortLinkGuid(string fullUrl)
        {
            StringBuilder builder = new StringBuilder();
            Enumerable
               .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(12)
                .ToList().ForEach(e => builder.Append(e));
            Console.WriteLine(fullUrl);            
            return builder.ToString();
        }

    }
}
