namespace BlobSampleApp1.Interfaces
{
    public interface IMockupServices
    {
        string CreateTempFile(string content = "sample text");
        string Randomize(string prefix = "sample");
    }
}
