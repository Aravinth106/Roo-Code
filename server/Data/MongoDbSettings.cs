namespace RooCode.Chat.Server.Data;

public class MongoDbSettings
{
    public const string SectionName = "MongoDb";

    public string ConnectionString { get; init; } = string.Empty;
    public string DatabaseName { get; init; } = string.Empty;
    public string UsersCollectionName { get; init; } = "users";
    public string MessagesCollectionName { get; init; } = "messages";
}
