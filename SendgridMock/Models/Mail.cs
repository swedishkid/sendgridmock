using System;

namespace SendgridMock
{
    public class Mail
    {
        public string Id { get; set; }
        public From From { get; set; }
        public string Subject { get; set; }
        public Personalization[] Personalizations { get; set; }
        public Content[] Content { get; set; }
        public string[] Categories { get; set; }
        public DateTime ReceivedAt { get; set; }
    }
}