namespace Auth.ClientLayer.Helpers.Exceptions
{
    public class BadRequestException : Exception
    {
        public string Message { get; set; }
        public BadRequestException(string messsage)
        {
            this.Message = messsage;
        }
    }
}
