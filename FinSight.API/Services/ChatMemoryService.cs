namespace FinSight.API.Services
{
    public class ChatMemoryService
    {
        private static readonly Dictionary<string, List<string>> _memory = new();

        public void AddMessage(string sessionId, string message)
        {
            if (!_memory.ContainsKey(sessionId))
                _memory[sessionId] = new List<string>();

            _memory[sessionId].Add(message);
        }

        public string GetHistory(string sessionId)
        {
            if (!_memory.ContainsKey(sessionId))
                return "";

            return string.Join("\n", _memory[sessionId].TakeLast(5));
        }
    }
}