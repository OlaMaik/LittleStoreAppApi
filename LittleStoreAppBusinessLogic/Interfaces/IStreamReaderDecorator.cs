using Microsoft.AspNetCore.Http;
using System.IO;

namespace LittleStoreAppBusinessLogic.Interfaces
{
    public interface IStreamReaderDecorator
    {
        StreamReader GetStreamReader(IFormFile file);
    }
}
