namespace Auth.ClientLayer.Helpers.Exceptions
{
    public class NotFoundException : Exception
    {
        public string Message { get; set; }
        public NotFoundException(string messsage)
        {
            this.Message = messsage;
        }
    }
}
