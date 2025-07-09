using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
namespace FIAP.TechChallenge.UserHub.Domain.Interfaces.Elastic
{
    public interface IElasticClient<T>
    {
        Task<IReadOnlyCollection<T>> Get(int page, int size, IndexName index);

        Task<bool> Create(T log, IndexName index);

        Task<IReadOnlyCollection<T>> Search(IndexName index, Func<QueryDescriptor<T>, QueryDescriptor<T>> query, int page = 0, int size = 10);

        Task Update(T document, string indexName);
    }
}
