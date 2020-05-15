﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avatar.App.Core.Models
{
    public class ResponseModel
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }



        public ResponseModel(int statusCode, string message, object data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

        public ResponseModel(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }



        public ResponseModel()
        {
            StatusCode = 200;
            Message = string.Empty;
        }

       
    }
}
