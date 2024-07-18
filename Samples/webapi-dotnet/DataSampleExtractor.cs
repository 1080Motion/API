using WebApiSample.DataTransferObjects;

namespace WebApiSample;

internal static class DataSampleExtractor
{
    private const int BytesPerSample = 5 * sizeof(float);

    /// <summary>
    /// Converts a byte array encoded as a base64 string returned from the API into a list of samples 
    /// </summary>
    /// <param name="base64data">Base64 encoded byte array to decode and deserialize into a list of samples</param>
    public static PublicDataSample[]? DeserializeList(string? base64data)
    {
        if (base64data == null)
            return null;
        byte[] bytes = Convert.FromBase64String(base64data);
        
        return DeserializeList(bytes);
    }

    /// <summary>
    /// Converts a byte array returned from the API into a list of samples 
    /// </summary>
    /// <param name="bytes">Byte array holding the serialized list of samples</param>
    public static PublicDataSample[]? DeserializeList(byte[]? bytes)
    {
        if (bytes is null || bytes.Length == 0)
            return Array.Empty<PublicDataSample>();
        if (bytes.Length % BytesPerSample != 0)
            throw new ArgumentException("Incorrect length of byte array");

        int numSamples = bytes.Length / BytesPerSample;
        using var reader = new BinaryReader(new MemoryStream(bytes));
        return DeserializeList(reader, numSamples);
    }
    
    private static PublicDataSample[] DeserializeList(BinaryReader reader, int numSamples)
    {
        var samples = new PublicDataSample[numSamples];
        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] = DeserializeSingleSample(reader);
        }

        return samples;
    }
    
    private static PublicDataSample DeserializeSingleSample(BinaryReader reader)
    {
        var sample = new PublicDataSample
        {
            TimeSinceStart = reader.ReadSingle(),
            Position = reader.ReadSingle(),
            Speed = reader.ReadSingle(),
            Acceleration = reader.ReadSingle(),
            Force = reader.ReadSingle(),
        };
        return sample;
    }
}