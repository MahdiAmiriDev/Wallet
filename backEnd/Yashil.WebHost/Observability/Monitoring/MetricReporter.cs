using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Yashil.WebHost.Observability.Monitoring;

public class MetricReporter
{
    private readonly Counter<int> _requestCounter;
    private readonly Histogram<double> _responseTimeHistogram;
    private readonly Histogram<double> _databaseResponseTimeHistogram;
    private readonly ObservableGauge<int> _inprogressRequest;

    public string MetricName { get; }

    public MetricReporter(string meterName = "Anosito")
    {
        var meter = new Meter(meterName);


        MetricName = meterName;

        _requestCounter = meter.CreateCounter<int>(
            name: "yashil_total_request",
            unit: "hits",
            description: "the total number of requests");

        _responseTimeHistogram = meter.CreateHistogram<double>(
            name: "yashil_request_duration_seconds",
            unit: "double",
            description: "The duration in seconds between the response to a request.");

        _databaseResponseTimeHistogram = meter.CreateHistogram<double>(
            name: "yashil_database_query_duration_seconds",
            unit: "double",
            description: "");
    }

    public void RegisterRequest(string? path)
        => _requestCounter.Add(1, new KeyValuePair<string, object?>("path", path));

    public void RegisterResponseTime(
        int statusCode,
        string httpMethod,
        string? controller,
        string? action,
        string? path,
        string instance,
        TimeSpan elapsed)
    {
        var keyValuePairs = new KeyValuePair<string, object?>[6]
        {
            new KeyValuePair<string, object?>("path", path),
            new KeyValuePair<string, object?>("statusCode", statusCode),
            new KeyValuePair<string, object?>("httpMethod", httpMethod),
            new KeyValuePair<string, object?>("controller", controller),
            new KeyValuePair<string, object?>("action", action),
            new KeyValuePair<string, object?>("instance", instance)
        };
        var tags = new TagList(keyValuePairs);
        _responseTimeHistogram.Record(elapsed.Seconds, tags);
    }

    public void RegisterQueryResponseTime(QueryType queryType, string query, QueryStatus queryStatus, TimeSpan elapsed)
    {
        var keyValuePairs = new KeyValuePair<string, object?>[3]
        {
            new KeyValuePair<string, object?>("queryType", queryType.ToString()),
            new KeyValuePair<string, object?>("query", query),
            new KeyValuePair<string, object?>("queryStatus", queryStatus.ToString())
        };
        var tags = new TagList(keyValuePairs);
        _responseTimeHistogram.Record(elapsed.TotalMilliseconds, tags);
    }

}

public enum QueryStatus
{
    SUCCEED, FAILED
}

public enum QueryType
{
    Procedure, Query
}