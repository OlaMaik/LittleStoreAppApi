using LittleStoreAppBusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace LittleStoreAppBusinessLogic.Services
{
    public class StreamReaderDecorator : IStreamReaderDecorator
    {
        public StreamReader GetStreamReader(IFormFile file)
        {
            return new StreamReader(file.OpenReadStream());
        }
    }
}
