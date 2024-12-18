﻿using CsvQueueProcessor.Core.Entities;
using CsvQueueProcessor.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvQueueProcessor.Core.Services
{
    public class FileProcessingService : IFileProcessingService
    {
        private readonly IFileProcessingRepository _fileProcessingRepository;

        public FileProcessingService(IFileProcessingRepository fileProcessingRepository)
        {
            _fileProcessingRepository = fileProcessingRepository;
        }

        public async Task<IEnumerable<FileProcessing>> GetAllFileProcessingStatuses()
        {
            try
            {
                return await _fileProcessingRepository.GetAllFileProcessingStatuses();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> InsertFileProcessingStatus(FileProcessing fileProcessing)
        {
            try
            {
                return await _fileProcessingRepository.InsertFileProcessingStatus(fileProcessing);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
