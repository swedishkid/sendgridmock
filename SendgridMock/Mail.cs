namespace SendgridMock
{
    public class Mail
    {
        public From From { get; set; }
        public string Subject { get; set; }
        public Personalization[] Personalizations { get; set; }
        public Content[] Content { get; set; }
        public string[] Categories { get; set; }
    }
}