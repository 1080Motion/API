using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

using WebApiSample.DataTransferObjects;

namespace WebApiSample;

public class CsvApiClient
{
    private readonly HttpClient _httpClient;

    public CsvApiClient(Uri apiHost, string apiKey)
    {
        // Setup the HttpClient that is pre-configured with the address of the public API and
        // make it send the API key with every request.
        // This means we only need to specify the relative path when we make HTTP requests
        _httpClient = new HttpClient()
        {
            BaseAddress = apiHost,
        };
        _httpClient.DefaultRequestHeaders.Add("X-1080-API-Key", apiKey);
    }
    
    private async Task<Guid> FindSetWithLinearTrainingData()
    {
        int numDays = 100;
        var sessions = await _httpClient.GetFromJsonAsync<SessionInfo[]>($"Session?maxAgeDays={numDays}");
        if (IsEmpty(sessions))
            throw new InvalidOperationException($"No sessions found in the last {numDays} days");

        var exerciseLibrary = await _httpClient.GetFromJsonAsync<PublicExerciseType[]>("ExerciseType");

        var linearExerciseTypes = exerciseLibrary!
                                 .Where(et => et.PresentationType == ExercisePresentationType.Linear)
                                 .ToList();
        
        foreach (SessionInfo session in sessions)
        {
            PublicSession? details = await _httpClient.GetFromJsonAsync<PublicSession>($"Session/{session.Id}");
            if (IsEmpty(details?.Exercises) || details.Exercises.All(e => IsEmpty(e.Sets)))
                continue;
            
            foreach (var exercise in details.Exercises)
            {
                if (IsEmpty(exercise.Sets))
                    continue;
                if (!linearExerciseTypes.Any(et => et.Id == exercise.ExerciseTypeId))
                    continue;
                return exercise.Sets.First().Id;
            }
        }

        throw new InvalidOperationException($"No sessions within the last {numDays} had any sets");
    }

    public async Task DownloadSetSummaryToCsv()
    {
        Guid setId = await FindSetWithLinearTrainingData();
        await DownloadSetTrainingDataToCsv(setId, $"set-summary-{setId}.csv");
    }
    
    public async Task DownloadRepSamplesToCsv()
    {
        Guid setId = await FindSetWithLinearTrainingData();
        await DownloadSamplesToCsv(setId);
    }

    private async Task DownloadSetTrainingDataToCsv(Guid setId, string filename)
    {
        var set = await _httpClient.GetFromJsonAsync<PublicSetData>($"TrainingData/Set/{setId}?includeSamples=false");
        if (set is null || IsEmpty(set.MotionGroups))
        {
            Console.WriteLine("No training data found for session {0}", setId);
            return;
        }

        await using var writer = new StreamWriter(filename);
        await writer.WriteLineAsync("Set;RepNumber;RepId;RepTime;TopSpeed;TotalTime;Distance");
        int repNumber = 0;
        foreach (var rep in set.MotionGroups)
        {
            if (IsEmpty(rep.Motions))
                continue;
            repNumber++;
            // Linear sprints only have one motion per motion group
            var motion = rep.Motions[0];
            await writer.WriteLineAsync($"{setId};{repNumber};{rep.Id:D};{rep.Created:s};{motion.TopSpeed};{motion.TotalTime};{motion.TotalDistance}");
        }

        Console.WriteLine("Training data saved to {0}", filename);
    }
    
    public async Task DownloadSamplesToCsv(Guid setId)
    {
        var set = await _httpClient.GetFromJsonAsync<PublicSetData>($"TrainingData/Set/{setId}?includeSamples=true");
        if (set is null || IsEmpty(set.MotionGroups))
        {
            Console.WriteLine("No training data found for session {0}", setId);
            return;
        }
        
        int repNumber = 0;
        foreach (var rep in set.MotionGroups)
        {
            if (IsEmpty(rep.Motions))
                continue;
            
            repNumber++;
            
            string filename = $"set-{setId}-rep-{repNumber}.csv";
            await using var writer = new StreamWriter(filename);
            // Linear sprints only have one motion per motion group
            var motion = rep.Motions[0];
            var samples = DataSampleExtractor.DeserializeList(motion.SampleData);
            if (samples is null)
            {
                Console.WriteLine("No samples found for rep {0} in set {1}", repNumber, setId);
                continue;
            }
            await writer.WriteLineAsync("Time;Position;Speed;Force");
            foreach (var sample in samples)
            {
                await writer.WriteLineAsync($"{sample.TimeSinceStart};{sample.Position};{sample.Speed};{sample.Force}");
            }
            
            Console.WriteLine("Data samples saved to {0}", filename);
        }
    }
    
    private static bool IsEmpty<T>([NotNullWhen(false)] ICollection<T>? collection) => collection is null || collection.Count == 0;
}