namespace Core.Exceptions
{
    public class AnalyticsStartDateLaterOrEqualToEndDateException : BadRequestException
    {
        public AnalyticsStartDateLaterOrEqualToEndDateException(DateTime startDate, DateTime endDate) 
            : base($"Start date {startDate} is later or equal to end date {endDate}")
        {
        }
    }
}
