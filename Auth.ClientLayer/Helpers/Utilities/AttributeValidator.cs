namespace Auth.ClientLayer.Helpers.Utilities
{
    public static class AttributeValidator
    {
        public static bool isValidDate(DateTime date)
        {
            string dateString = date.ToString();

            bool isValidDate = false;

            if(DateTime.TryParse(dateString, out _))
            {
                isValidDate = true;
            }

            return isValidDate;
        }
    }
}
