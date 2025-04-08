namespace Nutritions.Model
{
    public class VisitorLog
    {
        public int Id { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string UrlVisited { get; set; }
        public string Referrer { get; set; }
        public DateTime VisitTime { get; set; }
    }

}
