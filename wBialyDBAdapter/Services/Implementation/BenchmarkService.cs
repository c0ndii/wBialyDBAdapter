using System.Diagnostics;
using MongoDB.Bson;
using wBialyDBAdapter.Model;

namespace wBialyDBAdapter.Services.Implementation
{
    public class BenchmarkService : IBenchmarkService
    {
        private readonly IQueryService<UnifiedEventModel> _eventService;
        private readonly IQueryService<UnifiedGastroModel> _gastroService;
        private readonly Dictionary<string, List<string>> _insertedIds;

        public BenchmarkService(
            IQueryService<UnifiedEventModel> eventService,
            IQueryService<UnifiedGastroModel> gastroService)
        {
            _eventService = eventService;
            _gastroService = gastroService;
            _insertedIds = new Dictionary<string, List<string>>();
        }

        public async Task<BenchmarkResponse> RunBenchmarksAsync(
            BenchmarkRequest request,
            CancellationToken cancellationToken = default)
        {
            var response = new BenchmarkResponse();
            var totalStopwatch = Stopwatch.StartNew();

            var dbType = request.DatabaseType;

            foreach (var recordCount in request.RecordCounts)
            {
                var key = $"{dbType}_{recordCount}";
                _insertedIds[key] = new List<string>();

                await RunInsertBenchmark(request, dbType, recordCount, response, cancellationToken);

                await RunSelectManyBenchmark(request, dbType, recordCount, response, cancellationToken);

                if (_insertedIds[key].Any())
                {
                    await RunSelectByIdBenchmark(request, dbType, recordCount, response, cancellationToken);
                }

                if (_insertedIds[key].Any())
                {
                    await RunUpdateBenchmark(request, dbType, recordCount, response, cancellationToken);
                }

                if (_insertedIds[key].Any())
                {
                    await RunDeleteBenchmark(request, dbType, recordCount, response, cancellationToken);
                }

                _insertedIds[key].Clear();
            }


            totalStopwatch.Stop();
            response.TotalExecutionTimeMs = totalStopwatch.Elapsed.TotalMilliseconds;

            CalculateSummaries(response);

            return response;
        }

        private async Task RunInsertBenchmark(
            BenchmarkRequest request,
            DatabaseType dbType,
            int recordCount,
            BenchmarkResponse response,
            CancellationToken cancellationToken)
        {
            var key = $"{dbType}_{recordCount}";

            for (int i = 0; i < request.Iterations; i++)
            {
                var stopwatch = Stopwatch.StartNew();
                var iterationIds = new List<string>();

                for (int j = 0; j < recordCount; j++)
                {
                    if (request.EntityType == "Event")
                    {
                        var eventData = GenerateEventData(j, dbType);
                        var eventRequest = new PostRequest<UnifiedEventModel>
                        {
                            DatabaseType = dbType,
                            Data = eventData
                        };

                        var result = await _eventService.AddAsync(eventRequest, cancellationToken);

                        if (result.Data != null && result.Data.Any())
                        {
                            var addedId = result.Data.First().Id;
                            if (!string.IsNullOrEmpty(addedId))
                            {
                                iterationIds.Add(addedId);
                            }
                        }
                    }
                    else
                    {
                        var gastroData = GenerateGastroData(j, dbType);
                        var gastroRequest = new PostRequest<UnifiedGastroModel>
                        {
                            DatabaseType = dbType,
                            Data = gastroData
                        };

                        var result = await _gastroService.AddAsync(gastroRequest, cancellationToken);

                        if (result.Data != null && result.Data.Any())
                        {
                            var addedId = result.Data.First().Id;
                            if (!string.IsNullOrEmpty(addedId))
                            {
                                iterationIds.Add(addedId);
                            }
                        }
                    }
                }

                stopwatch.Stop();

                _insertedIds[key].AddRange(iterationIds);

                response.DetailedResults.Add(new BenchmarkResult
                {
                    Operation = "INSERT",
                    DatabaseType = dbType,
                    RecordCount = recordCount,
                    Iteration = i + 1,
                    ExecutionTimeMs = stopwatch.Elapsed.TotalMilliseconds
                });
            }
        }

        private async Task RunSelectManyBenchmark(
            BenchmarkRequest request,
            DatabaseType dbType,
            int recordCount,
            BenchmarkResponse response,
            CancellationToken cancellationToken)
        {
            for (int i = 0; i < request.Iterations; i++)
            {
                var stopwatch = Stopwatch.StartNew();

                if (request.EntityType == "Event")
                {
                    var eventRequest = new EndpointRequest
                    {
                        DatabaseType = dbType,
                        PageSize = recordCount
                    };
                    await _eventService.GetManyAsync(eventRequest, cancellationToken);
                }
                else
                {
                    var gastroRequest = new EndpointRequest
                    {
                        DatabaseType = dbType,
                        PageSize = recordCount
                    };
                    await _gastroService.GetManyAsync(gastroRequest, cancellationToken);
                }

                stopwatch.Stop();

                response.DetailedResults.Add(new BenchmarkResult
                {
                    Operation = "SELECT_MANY",
                    DatabaseType = dbType,
                    RecordCount = recordCount,
                    Iteration = i + 1,
                    ExecutionTimeMs = stopwatch.Elapsed.TotalMilliseconds
                });
            }
        }

        private async Task RunSelectByIdBenchmark(
            BenchmarkRequest request,
            DatabaseType dbType,
            int recordCount,
            BenchmarkResponse response,
            CancellationToken cancellationToken)
        {
            var key = $"{dbType}_{recordCount}";
            var availableIds = _insertedIds[key];

            if (!availableIds.Any())
                return;

            var random = new Random();

            for (int i = 0; i < request.Iterations; i++)
            {
                var stopwatch = Stopwatch.StartNew();

                var randomId = availableIds[random.Next(0, availableIds.Count)];

                if (request.EntityType == "Event")
                {
                    var eventRequest = new BaseRequest { DatabaseType = dbType };
                    await _eventService.GetByKeyAsync(eventRequest, randomId, cancellationToken);
                }
                else
                {
                    var gastroRequest = new BaseRequest { DatabaseType = dbType };
                    await _gastroService.GetByKeyAsync(gastroRequest, randomId, cancellationToken);
                }

                stopwatch.Stop();

                response.DetailedResults.Add(new BenchmarkResult
                {
                    Operation = "SELECT_BY_ID",
                    DatabaseType = dbType,
                    RecordCount = recordCount,
                    Iteration = i + 1,
                    ExecutionTimeMs = stopwatch.Elapsed.TotalMilliseconds
                });
            }
        }

        private async Task RunUpdateBenchmark(
            BenchmarkRequest request,
            DatabaseType dbType,
            int recordCount,
            BenchmarkResponse response,
            CancellationToken cancellationToken)
        {
            var key = $"{dbType}_{recordCount}";
            var availableIds = _insertedIds[key];

            if (!availableIds.Any())
                return;

            var random = new Random();

            for (int i = 0; i < request.Iterations; i++)
            {
                var stopwatch = Stopwatch.StartNew();

                var randomId = availableIds[random.Next(0, availableIds.Count)];

                if (request.EntityType == "Event")
                {
                    var eventData = GenerateEventData(i, dbType, isUpdate: true);
                    eventData.Id = randomId;

                    var eventRequest = new PostRequest<UnifiedEventModel>
                    {
                        DatabaseType = dbType,
                        Data = eventData
                    };
                    await _eventService.UpdateAsync(eventRequest, randomId, cancellationToken);
                }
                else
                {
                    var gastroData = GenerateGastroData(i, dbType, isUpdate: true);
                    gastroData.Id = randomId;

                    var gastroRequest = new PostRequest<UnifiedGastroModel>
                    {
                        DatabaseType = dbType,
                        Data = gastroData
                    };
                    await _gastroService.UpdateAsync(gastroRequest, randomId, cancellationToken);
                }

                stopwatch.Stop();

                response.DetailedResults.Add(new BenchmarkResult
                {
                    Operation = "UPDATE",
                    DatabaseType = dbType,
                    RecordCount = recordCount,
                    Iteration = i + 1,
                    ExecutionTimeMs = stopwatch.Elapsed.TotalMilliseconds
                });
            }
        }

        private async Task RunDeleteBenchmark(
            BenchmarkRequest request,
            DatabaseType dbType,
            int recordCount,
            BenchmarkResponse response,
            CancellationToken cancellationToken)
        {
            var key = $"{dbType}_{recordCount}";
            var availableIds = _insertedIds[key];

            if (!availableIds.Any())
                return;

            var random = new Random();

            for (int i = 0; i < request.Iterations && i < availableIds.Count; i++)
            {
                var stopwatch = Stopwatch.StartNew();

                var idToDelete = availableIds[i];

                if (request.EntityType == "Event")
                {
                    var eventRequest = new BaseRequest { DatabaseType = dbType };
                    await _eventService.DeleteAsync(eventRequest, idToDelete, cancellationToken);
                }
                else
                {
                    var gastroRequest = new BaseRequest { DatabaseType = dbType };
                    await _gastroService.DeleteAsync(gastroRequest, idToDelete, cancellationToken);
                }

                stopwatch.Stop();

                response.DetailedResults.Add(new BenchmarkResult
                {
                    Operation = "DELETE",
                    DatabaseType = dbType,
                    RecordCount = recordCount,
                    Iteration = i + 1,
                    ExecutionTimeMs = stopwatch.Elapsed.TotalMilliseconds
                });
            }
        }

        private void CalculateSummaries(BenchmarkResponse response)
        {
            var grouped = response.DetailedResults
                .GroupBy(r => new { r.Operation, r.DatabaseType, r.RecordCount });

            foreach (var group in grouped)
            {
                var times = group.Select(g => g.ExecutionTimeMs).ToList();

                response.Summaries.Add(new BenchmarkSummary
                {
                    Operation = group.Key.Operation,
                    DatabaseType = group.Key.DatabaseType,
                    RecordCount = group.Key.RecordCount,
                    AverageTimeMs = times.Average(),
                    MinTimeMs = times.Min(),
                    MaxTimeMs = times.Max(),
                    AllTimes = times
                });
            }
        }

        private UnifiedEventModel GenerateEventData(int index, DatabaseType dbType, bool isUpdate = false)
        {
            var model = new UnifiedEventModel
            {
                Title = isUpdate ? $"Updated Event {index}" : $"Benchmark Event {index}",
                Description = $"Description for event {index}",
                EventDate = DateTime.UtcNow.AddDays(index),
                AddDate = DateTime.UtcNow,
                Place = $"Place {index}",
                Author = $"Author {index}",
                Link = $"https://example.com/event/{index}",
                Tags = new List<UnifiedTagModel>()
            };

            if (dbType == DatabaseType.NoSQL)
            {
                model.Tags.Add(new UnifiedTagModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = $"Tag{index % 10}"
                });
            }
            else
            {
                model.Tags.Add(new UnifiedTagModel
                {
                    Id = (index % 10 + 1).ToString(),
                    Name = $"Tag{index % 10}"
                });
            }

            return model;
        }

        private UnifiedGastroModel GenerateGastroData(int index, DatabaseType dbType, bool isUpdate = false)
        {
            var model = new UnifiedGastroModel
            {
                Title = isUpdate ? $"Updated Gastro {index}" : $"Benchmark Gastro {index}",
                Description = $"Description for gastro {index}",
                Day = DateTime.UtcNow.AddDays(index),
                AddDate = DateTime.UtcNow,
                Place = $"Place {index}",
                Author = $"Author {index}",
                Link = $"https://example.com/gastro/{index}",
                Tags = new List<UnifiedTagModel>()
            };

            if (dbType == DatabaseType.NoSQL)
            {
                model.Tags.Add(new UnifiedTagModel
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = $"Tag{index % 10}"
                });
            }
            else
            {
                model.Tags.Add(new UnifiedTagModel
                {
                    Id = (index % 10 + 1).ToString(),
                    Name = $"Tag{index % 10}"
                });
            }

            return model;
        }
    }
}