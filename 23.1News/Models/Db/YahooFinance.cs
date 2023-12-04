using Newtonsoft.Json;

namespace _23._1News.Models.Db
{
    public class YahooFinance
    {
        public class Rootobject
        {
            // ... (unchanged properties)

            public Quote[] quotes { get; set; }
            public News[] news { get; set; }
        }

        public class Quote
        {
            
            public string symbol { get; set; }
            public string shortname { get; set; }
            public float score { get; set; }
            public string typeDisp { get; set; }
            public string longname { get; set; }
            public string sector { get; set; }
            public string industry { get; set; }

            //public Quote[] quotes { get; set; }
            //public News[] news { get; set; }
        }

        public class News
        {
            public string title { get; set; }
            public string publisher { get; set; }
            public string link { get; set; }
            public int providerPublishTime { get; set; }
            public Thumbnail thumbnail { get; set; }
        }

        public class Thumbnail
        {
            public Resolution[] resolutions { get; set; }
        }

        public class Resolution
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public string tag { get; set; }
        }
    }
}
