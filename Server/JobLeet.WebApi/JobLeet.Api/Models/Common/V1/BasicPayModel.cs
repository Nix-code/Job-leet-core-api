namespace JobLeet.WebApi.JobLeet.Core.Entities.Common.V1
{
    public class BasicPayModel{
        public decimal? MinmumPay {get; set;}
        public decimal? MaximumPay {get; set;}
        public string? Currency{get; set;}
    }
}