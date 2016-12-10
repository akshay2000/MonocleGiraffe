using System;

namespace XamarinImgur.Models
{
    public class Response<T> where T : new()
    {
        public Exception Error { get; set; }
        public bool IsError { get; set; }
        public string Message { get; set; }
        //public T Content { get; set; } = new T();

        private T content;
        public T Content
        {
            get
            {
                content = content == null ? new T() : content;
                return content;
            }
            set
            {
                content = value;
            }
        }
    }
}
