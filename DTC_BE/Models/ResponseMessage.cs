namespace DTC_BE.Models
{
    public class ResponseMessage
    {
        public int? Code { get; set; }
        public string Title { get; set; }
        public bool IsError { get; set; }
        public object ObjData { get; set; }
        public string Data { get; set; }

        public ResponseMessage()
        {
            Title = string.Empty;
            IsError = false;
            ObjData = null;
            Data = string.Empty;
            Code = 0;
        }

        public ResponseMessage(string title, bool error, object data, string strData, int? code)
        {
            Title = title;
            IsError = error;
            ObjData = data;
            Data = strData;
            Code = code;
        }
    }
}
