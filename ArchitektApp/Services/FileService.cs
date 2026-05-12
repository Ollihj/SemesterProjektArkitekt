using System.Net.Http.Json;
using ArchitektApp.Pages;

namespace ArchitektApp.Service;

// Håndterer kommunikation med server-API'et for fil-upload og download
public class FileService
{
    private readonly HttpClient _client;

    public FileService(HttpClient client)
    {
        _client = client;
    }

    // Sender en fil til serveren og returnerer det unikke filnavn hvis det lykkedes
    public async Task<(bool success, string info)> SendFile(string filename, Stream stream)
    {
        // Pakker filen ind i en multipart form request som serveren kan modtage
        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(stream), "file", filename);

        // Sender POST request til upload-endpointet
        var response = await _client.PostAsync($"{Config.ServerUrl}/api/file/upload", content);

        // Læser svaret fra serveren (filnavnet hvis success, fejlbesked hvis ikke)
        string info = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
            return (true, info.Trim('"')); // Fjerner eventuelle anførselstegn fra JSON-svar

        return (false, info);
    }

    // Returnerer URL'en der bruges til at vise et billede i en <img> tag
    public string GetImageUrl(string fileName)
    {
        return $"{Config.ServerUrl}/api/file/download/{fileName}";
    }

    // Henter en liste af alle gemte filnavne fra serveren
    public async Task<List<string>> GetAllKeys()
    {
        var keys = await _client.GetFromJsonAsync<List<string>>($"{Config.ServerUrl}/api/file/keys");
        return keys ?? new List<string>();
    }

    // Sletter en fil fra serveren baseret på filnavn
    public async Task DeleteFile(string fileName)
    {
        await _client.DeleteAsync($"{Config.ServerUrl}/api/file/{fileName}");
    }
}
