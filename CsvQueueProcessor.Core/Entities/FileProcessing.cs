using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvQueueProcessor.Core.Entities
{
    public class FileProcessing
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public int? StatusCode { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}
