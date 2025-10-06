using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace BestellApp
{
    public class GraphService
    {
        private readonly GraphServiceClient _graphClient;

        public GraphService(GraphServiceClient graphClient)
        {
            _graphClient = graphClient;
        }

        /*public async Task<List<Message>> GetGroupChatMessages(string chatId)
        {
            var messages = await _graphClient.Chats[chatId].Messages
                .Request()
                .GetAsync();

            return messages.CurrentRange.ToList();
        }*/
    }
}
