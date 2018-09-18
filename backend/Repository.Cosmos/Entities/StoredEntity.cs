using Newtonsoft.Json;

namespace WhiteRaven.Repository.Cosmos.Entities
{
    public class StoredEntity<T>
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public T Entity { get; set; }
    }
}