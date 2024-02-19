﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhereToDataAccess.Entities;
using WhereToServices.DTOs;

namespace WhereToServices.Interfaces
{
    public interface IFeedbackService
    {
        Task<List<SasFilePathResponseModel>> CreateFeedback(FeedbackDto tourFeedback);
        Task UploadPhotoToBlob(string token,UploadPhotoUsingSasModel content);

        Task<FeddbackResponseModel> GetFeedbackAsync(int feedbackId);
    }
}
