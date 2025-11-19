using Qdrant.Client;
using Qdrant.Client.Grpc;
using System;
using static Qdrant.Client.Grpc.Conditions;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace RoshedTehran.Services
{
    public class QdrantService
    {
        private readonly QdrantClient _client;
        private const string CollectionName = "Entities";

        public QdrantService()
        {
            // Qdrant runs on port 6333 by default
            _client = new QdrantClient("localhost", 6334);
        }

        /// <summary>
        /// Creates the collection if it doesn't exist yet.
        /// </summary>
        ///
        //set it to 768 for paraphrase-multilingual-mpnet-base-v2
        public async Task InitializeAsync(int vectorSize)
        {
            var exists = await _client.CollectionExistsAsync(CollectionName);

            if (!exists)
            {
                await _client.CreateCollectionAsync(CollectionName, new VectorParams
                {
                    Size = (uint)vectorSize,
                    Distance = Distance.Cosine
                });
            }
        }

        /// <summary>
        /// Saves a note embedding into Qdrant.
        /// </summary>
        public async Task SaveEntityAsync(Guid id, string TitleTag, float[] vector)
        {
            UpdateResult Result = await _client.UpsertAsync(collectionName: CollectionName, points: new List<PointStruct>{
                 new PointStruct{
                    Id = id,
                    Vectors = vector,
                    Payload = { ["TitleTag"] = TitleTag, ["id"] = id.ToString() }}});

        }

        /// <summary>
        /// Searches for the most similar note given a query vector.
        /// </summary>
        public async Task<IEnumerable<ScoredPoint>> SearchAsync(float[] queryVector, int limit = 5)
        {
            return await _client.SearchAsync(CollectionName, queryVector, limit: (uint)limit);
        }

        public async Task DeleteEntity(Guid id)
        {
            await _client.DeleteAsync(collectionName: "notes", filter: MatchKeyword("id", id.ToString()));

        }
    }
}