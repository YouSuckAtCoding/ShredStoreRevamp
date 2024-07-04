
namespace DatabaseAccess
{
    public interface ISqlDataAccess
    {
        Task<IEnumerable<T>> LoadData<T, U>(string StoredProcedure, U parameters, string connectionId = ConfigConstants.ConnectionId, CancellationToken token = default);
        Task SaveData<T>(string StoredProcedure, T parameters, string connectionId = ConfigConstants.ConnectionId, CancellationToken token = default);
    }
}