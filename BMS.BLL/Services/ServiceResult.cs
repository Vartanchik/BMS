namespace BMS.BLL.Services
{
    public class ServiceResult
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; }

        /// <summary>
        /// Successful result
        /// </summary>
        public ServiceResult()
        {
            Succeeded = true;
        }

        /// <summary>
        /// Unsuccessful result
        /// </summary>
        /// <param name="error"></param>
        public ServiceResult(string error)
        {
            Succeeded = false;
            Error = error;
        }
    }
}
