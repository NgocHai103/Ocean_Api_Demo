using Nest;
using Sea.Share.ElasticSearch.ElasticDtos;
using Serilog;

namespace Sea.Share.ElasticSearch.Services.Implements;

public class ElasticSearchService<TElasticDto, Tkey>(IElasticClient _elasticClient) : IElasticSearchService<TElasticDto, Tkey> where TElasticDto : ElasticEtoBase<Tkey>
{
    public async ValueTask<TElasticDto> GetByIdAsync(string index, Tkey id)
    {
        try
        {
            var response = await _elasticClient.GetAsync<TElasticDto>(id?.ToString(), g => g.Index(index));

            if (!response.IsValid || response.Source == null)
            {
                Log.Warning("Document not found in Elasticsearch: {Index} - {Id}", index, id);

                return default;
            }

            return response.Source;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetByIdAsync-ElasticSearchService-Exception: {Index} - {Id}", index, id);
            throw;
        }
    }

    public async ValueTask<DocumentResultEto<TElasticDto>> SearchAsync(string index, Func<SearchDescriptor<TElasticDto>, ISearchRequest> query, int pageIndex = 1, int pageSize = 10)
    {
        try
        {
            // Calculate the starting point for pagination
            var from = (pageIndex - 1) * pageSize;

            var response = await _elasticClient.SearchAsync<TElasticDto>(s => query(s.Index(index).From(from).Size(pageSize)));

            if (!response.IsValid)
            {
                Log.Warning("Search failed in Elasticsearch: {Index}", index);
                return default;
            }

            return new DocumentResultEto<TElasticDto>(response.Documents, (int)response.Total, pageIndex, pageSize);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "SearchAsync-ElasticSearchService-Exception: {Index}", index);
            throw;
        }
    }

    public async ValueTask<bool> IndexAsync(string index, TElasticDto document)
    {
        try
        {
            var response = await _elasticClient.IndexAsync(document, i => i.Index(index).Id(document.Id?.ToString()));

            if (!response.IsValid)
            {
                Log.Warning("Failed to index document: {Index} - {Id}", index, document.Id);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "IndexAsync-ElasticSearchService-Exception: {Index} - {Id}", index, document.Id);
            throw;
        }
    }

    public async ValueTask<bool> UpdateAsync(string index, TElasticDto document)
    {
        try
        {
            var response = await _elasticClient.UpdateAsync<TElasticDto>(document.Id?.ToString(), u => u.Index(index).Doc(document));

            if (!response.IsValid)
            {
                Log.Warning("Failed to update document: {Index} - {Id}", index, document.Id);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "UpdateAsync-ElasticSearchService-Exception: {Index} - {Id}", index, document.Id);
            throw;
        }
    }

    public async ValueTask<bool> DeleteAsync(string index, Tkey id)
    {
        try
        {
            var response = await _elasticClient.DeleteAsync<TElasticDto>(id?.ToString(), d => d.Index(index));

            if (!response.IsValid)
            {
                Log.Warning("Failed to delete document: {Index} - {Id}", index, id);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "DeleteAsync-ElasticSearchService-Exception: {Index} - {Id}", index, id);
            throw;
        }
    }

    public async ValueTask<bool> BulkIndexAsync(string index, IList<TElasticDto> documents)
    {
        try
        {
            var bulkDescriptor = new BulkDescriptor();

            _ = Parallel.For(0, documents.Count, i => bulkDescriptor.Index<TElasticDto>(op => op
                    .Index(index)
                    .Document(documents[i])
                    .Id(documents[i].Id.ToString())));

            _ = await _elasticClient.BulkAsync(bulkDescriptor);

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "IndexAsync-ElasticSearchService-Exception: {Index} - {Id}", index);
            throw;
        }
    }
}
