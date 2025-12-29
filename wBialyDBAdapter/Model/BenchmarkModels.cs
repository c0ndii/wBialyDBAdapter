namespace wBialyDBAdapter.Model
{
    public class BenchmarkRequest : BaseRequest
    {
        public string EntityType { get; set; } = "Event";
        public List<int> RecordCounts { get; set; } = new List<int> { 1, 100, 2000, 4000, 8000, 20000 };
        public int Iterations { get; set; } = 10;
    }

    public class BenchmarkResult
    {
        public string Operation { get; set; } = string.Empty;
        public DatabaseType DatabaseType { get; set; }
        public int RecordCount { get; set; }
        public int Iteration { get; set; }
        public double ExecutionTimeMs { get; set; }
    }

    public class BenchmarkSummary
    {
        public string Operation { get; set; } = string.Empty;
        public DatabaseType DatabaseType { get; set; }
        public int RecordCount { get; set; }
        public double AverageTimeMs { get; set; }
        public double MinTimeMs { get; set; }
        public double MaxTimeMs { get; set; }
        public List<double> AllTimes { get; set; } = new List<double>();
    }

    public class BenchmarkResponse
    {
        public List<BenchmarkResult> DetailedResults { get; set; } = new List<BenchmarkResult>();
        public List<BenchmarkSummary> Summaries { get; set; } = new List<BenchmarkSummary>();
        public double TotalExecutionTimeMs { get; set; }
    }
}