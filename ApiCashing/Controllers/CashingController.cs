using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class CsvRecord
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Value { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class CachingController : ControllerBase
{
    private readonly string _csvFilePath = "data\\data.csv"; // Path to your CSV file

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CsvRecord>>> GetAllRecords()
    {
        var records = await ReadCsvFileAsync();

        Response.Headers[HeaderNames.CacheControl] = "public,max-age=600";
        Response.Headers[HeaderNames.Vary] = "Accept-Encoding";

        return Ok(records);
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any, VaryByHeader = "Accept-Encoding")]
    public async Task<ActionResult<CsvRecord>> GetRecordById(int id)
    {
        var records = await ReadCsvFileAsync();
        var record = records.FirstOrDefault(r => r.Id == id);
        if (record == null)
        {
            return NotFound();
        }
        return Ok(record);
    }

    private async Task<List<CsvRecord>> ReadCsvFileAsync()
    {
        var records = new List<CsvRecord>();
        using (var reader = new StreamReader(_csvFilePath))
        {
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                var values = line.Split(',');
                if (values[0] == "Id") continue; // Skip the header row

                var record = new CsvRecord
                {
                    Id = int.Parse(values[0]),
                    Name = values[1],
                    Value = int.Parse(values[2])
                };

                records.Add(record);
            }
        }
        return records;
    }
}
