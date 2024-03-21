using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.VisualBasic;

namespace Telemetry;

public static class ParkerMetrics
{
    public static int occasionsChecked = 0;
    public static int occasionsAdded = 0;

    public static readonly string OccasionMetricName = "OccasionMetric";
    public static Meter occasionMeter = new Meter(OccasionMetricName, "1.0.0");

    public static Counter<int> occasionCounter = occasionMeter.CreateCounter<int>("Occasion", description: "Counts the number of occasions");
    public static UpDownCounter<int> occasionUpDown = occasionMeter.CreateUpDownCounter<int>("OccasionUpDown", description: "An up down counter");
    public static ObservableCounter<int> occasionObservable = occasionMeter.CreateObservableCounter<int>("OccasionObservable", () => occasionsChecked);
    public static ObservableUpDownCounter<int> occassionObservableUpDown = occasionMeter.CreateObservableUpDownCounter<int>("OccasionObservableUpDown", () => occasionsChecked);
    public static ObservableGauge<int> occasionGauge = occasionMeter.CreateObservableGauge<int>("OccasionGauge", () => System.DateTime.Now.Second);
    public static Histogram<int> occasionHist = occasionMeter.CreateHistogram<int>("OccasionHist", description: "Histogram for occasions");
}
