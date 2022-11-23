namespace MatchAssistant.Domain.Core.Entities
{
    public class Response
    {
        public object Payload { get; }

        public Response(object payload = null)
        {
            Payload = payload;
        }
    }
}
