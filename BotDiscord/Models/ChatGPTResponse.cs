using OpenAI.GPT3.ObjectModels.ResponseModels;
using OpenAI.GPT3.ObjectModels.SharedModels;

namespace BotDiscord.Models
{
    public sealed class ChatGPTResponse
    {
        public List<ChoiceResponse>? Choices { get; set; }

        public int CreatedAt { get; set; }

        public Error? Error { get; set; }

        public string Id { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public bool Successful { get; set; } = false;
    }
}