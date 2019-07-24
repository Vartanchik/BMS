namespace BMS.WebAPI.Models
{
    public class ResponseModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Info { get; set; }

        public ResponseModel()
        {
        }

        public ResponseModel(int statusCode, string message, string info = null)
        {
            StatusCode = statusCode;
            Message = message;
            Info = info;
        }
    }
}
