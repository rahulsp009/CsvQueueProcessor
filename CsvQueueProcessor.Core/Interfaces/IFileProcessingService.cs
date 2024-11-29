using CsvQueueProcessor.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvQueueProcessor.Core.Interfaces
{
    public interface IFileProcessingService
    {
        Task<int> InsertFileProcessingStatus(FileProcessing fileProcessing);
        Task<IEnumerable<FileProcessing>> GetAllFileProcessingStatuses();
    }
}
