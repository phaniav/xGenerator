using System;
using System.Collections.Generic;

namespace ExperienceGenerator.Models.Exm
{
    public class ExmSpecificCampaign
    {
        public string Name { get; set; }

        public List<string> IncludeLists { get; set; }

        public DateTime? Date { get; set; }

        public int DaysAgo { get; set; }

        public DateTime DateEffective
        {
            get
            {
                return Date.HasValue
                    ? Date.Value.ToUniversalTime()
                    : DateTime.UtcNow.AddDays(-1*DaysAgo);
            }
        }

        public ExmEventPercentages Events { get; set; }
    }
}