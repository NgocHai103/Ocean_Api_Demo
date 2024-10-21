using Nest;
using Sea.Share.ElasticSearch.ElasticDtos;

namespace Sea.Share.ElasticSearch.Services;

public interface IElasticSearchService<TElasticDto, Tkey> where TElasticDto : ElasticEtoBase<Tkey>
{
    /// <summary>
    /// Get a document by its ID from the specified index.
    /// </summary>
    /// <param name="index">The index to search.</param>
    /// <param name="id">The ID of the document.</param>
    /// <returns>The document of type TElasticDto.</returns>
    ValueTask<TElasticDto> GetByIdAsync(string index, Tkey id);

    /// <summary>
    /// Search for documents in the specified index.
    /// </summary>
    /// <param name="index">The index to search.</param>
    /// <param name="query">The search query to execute.</param>
    /// <returns>A collection of documents of type TElasticDto.</returns>
    ValueTask<DocumentResultEto<TElasticDto>> SearchAsync(string index, Func<SearchDescriptor<TElasticDto>, ISearchRequest> query, int pageIndex, int pageSize);

    /// <summary>
    /// Index a document with a specified ID in the specified index.
    /// </summary>
    /// <param name="index">The index to insert the document into.</param>
    /// <param name="document">The document to be indexed.</param>
    /// <returns>True if the operation was successful, false otherwise.</returns>
    ValueTask<bool> IndexAsync(string index, TElasticDto document);

    /// <summary>
    /// Update a document by its ID in the specified index.
    /// </summary>
    /// <param name="index">The index where the document resides.</param>
    /// <param name="document">The document with updated data.</param>
    /// <returns>True if the operation was successful, false otherwise.</returns>
    ValueTask<bool> UpdateAsync(string index, TElasticDto document);

    /// <summary>
    /// Delete a document by its ID from the specified index.
    /// </summary>
    /// <param name="index">The index where the document resides.</param>
    /// <param name="id">The ID of the document to be deleted.</param>
    /// <returns>True if the document was successfully deleted, false otherwise.</returns>
    ValueTask<bool> DeleteAsync(string index, Tkey id);

    /// <summary>
    /// Index a list of documents in the specified index.
    /// </summary>
    /// <param name="index">The index to insert the document into.</param>
    /// <param name="document">The document to be indexed.</param>
    /// <returns>True if the operation was successful, false otherwise.</returns>
    ValueTask<bool> BulkIndexAsync(string index, IList<TElasticDto> documents);
}
