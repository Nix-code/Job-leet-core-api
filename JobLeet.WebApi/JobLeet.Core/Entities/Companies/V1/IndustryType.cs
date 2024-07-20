﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace JobLeet.WebApi.JobLeet.Core.Entities.Companies.V1
{
    public class IndustryType : BaseEntity
    {
        public IndustryCategory IndustryCategory { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum IndustryCategory
    {
        [Display(Name="Technology")]
        Technology = 1,
        [Display(Name = "Healthcare")]
        Healthcare = 2,
        [Display(Name = "Finance")]
        Finance = 3,
        [Display(Name= "Manufacturing")]
        Manufacturing = 4,
        [Display(Name="Others")]
        Others = 5
    }
}
