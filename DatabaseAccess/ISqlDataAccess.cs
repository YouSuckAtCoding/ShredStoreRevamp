
namespace DatabaseAccess
{
    public interface ISqlDataAccess
    {
        Task<IEnumerable<T>> LoadData<T, U>(string StoredProcedure, U parameters, string connectionId = "Default", CancellationToken token = default);
        Task SaveData<T>(string StoredProcedure, T parameters, string connectionId = "Default", CancellationToken token = default);
    }
}