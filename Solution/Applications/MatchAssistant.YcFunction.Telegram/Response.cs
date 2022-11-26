namespace MatchAssistant.YcFunction.Telegram
{
    public class Response
    {
        public int statusCode { get; set; }
        public string body { get; set; }

        public Response(int statusCode, string body)
        {
            this.statusCode = statusCode;
            this.body = body;
        }
    }
}
