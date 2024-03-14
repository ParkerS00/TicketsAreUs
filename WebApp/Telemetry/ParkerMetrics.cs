using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.VisualBasic;

namespace Telemetry;

public static class ParkerMetrics
{
    public static readonly string OccasionMetricName = "OccasionMetric";
    public static Meter occasionMeter = new Meter(OccasionMetricName, "1.0.0");

    public static Counter<int> occasionCounter = occasionMeter.CreateCounter<int>("Occasion", description: "Counts the number of occasions");
}
