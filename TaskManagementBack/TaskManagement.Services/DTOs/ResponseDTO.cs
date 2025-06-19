using System.Collections.Generic;

namespace TaskManagement.Services.DTOs
{
    public class ResponseDTO<T> where T : class
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ResponseDTO(bool Result, string Message, T Data)
        {
            this.Result = Result;
            this.Message = Message;
            this.Data = Data;
        }
    }

    public class ResponseDTO
    {
        public bool Result { get; set; }
        public string Message { get; set; }

        public ResponseDTO(bool Result, string Message)
        {
            this.Result = Result;
            this.Message = Message;
        }
    }
}
