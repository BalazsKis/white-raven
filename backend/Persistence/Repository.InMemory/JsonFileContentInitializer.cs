using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using WhiteRaven.Repository.Contract;

namespace WhiteRaven.Repository.InMemory
{
    public class JsonFileContentInitializer : IContentInitializer
    {
        public Task LoadContent<T>(string fromPath, IRepository<T> toRepository)
        {
            if (!File.Exists(fromPath))
            {
                throw new FileNotFoundException($"The content file was not fount at '{fromPath}'");
            }

            return Task.Run(() =>
            {
                var content = File.ReadAllText(fromPath);
                var items = JsonConvert.DeserializeObject<T[]>(content);

                return toRepository.Insert(items);
            });
        }
    }
}