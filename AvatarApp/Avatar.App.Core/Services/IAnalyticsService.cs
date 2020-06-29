using System;
using System.Collections.Generic;
using System.Text;
using Avatar.App.Core.Models;

namespace Avatar.App.Core.Services
{
    public interface IAnalyticsService
    {
        GeneralStatisticsDto GetGeneralStatistics();
    }
}
