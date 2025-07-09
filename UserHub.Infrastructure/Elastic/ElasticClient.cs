using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elastic.Transport;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Elastic;





namespace FFIAP.TechChallenge.UserHub.Infrastructure.ElasticSearch
{
    public class ElasticClient<T> : IElasticClient<T>
    {
        private readonly ElasticsearchClient _client;

        public ElasticClient(IElasticSettings settings)
        {
            this._client = new ElasticsearchClient(settings.CloudId, new ApiKey(settings.ApiKey));
        }

        public async Task<bool> Create(T log, IndexName index)
        {
            var response = await _client.IndexAsync<T>(log, index);

            return response.IsValidResponse;
        }

        public async Task<IReadOnlyCollection<T>> Get(int page, int size, IndexName index)
        {
            var response = await _client.SearchAsync<T>(s => s.Index(index)
                                                              .From(page)
                                                              .Size(size));
            return response.Documents;
        }

        public async Task<IReadOnlyCollection<T>> Search(
            IndexName index,
            Func<QueryDescriptor<T>, QueryDescriptor<T>> queryDescriptorAction, // Renomeado para clareza
            int page = 0,
            int size = 10)
        {
            var response = await _client.SearchAsync<T>(s => s
                .Index(index)
                .From(page)
                .Size(size)
                .Query(q => queryDescriptorAction(q))
            );

            if (response != null && !response.IsValidResponse) // Mudando para IsValid (ou IsValidResponse se for o caso na sua versão)
            {
                Console.WriteLine($"Erro na busca: {response.DebugInformation}");
                return new List<T>();
            }

            return response.Documents;
        }

        public async Task Update(T document, string indexName)
        {
            var indexResponse =
            await _client.IndexAsync(document, idx => idx.Index(indexName));

            if (!indexResponse.IsValidResponse)
            {
                throw new Exception(indexResponse.DebugInformation);
            }
            
        }




    }
}

