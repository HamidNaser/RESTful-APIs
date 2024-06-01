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
public class CashingController : ControllerBase
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

    [HttpPost]
    public async Task<ActionResult<CsvRecord>> AddRecord([FromBody] CsvRecord newRecord)
    {
        if (newRecord == null)
        {
            return BadRequest();
        }

        var records = await ReadCsvFileAsync();
        newRecord.Id = records.Max(r => r.Id) + 1;
        records.Add(newRecord);
        await WriteCsvFileAsync(records);

        return CreatedAtAction(nameof(GetRecordById), new { id = newRecord.Id }, newRecord);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecord(int id, [FromBody] CsvRecord updatedRecord)
    {
        if (updatedRecord == null || updatedRecord.Id != id)
        {
            return BadRequest();
        }

        var records = await ReadCsvFileAsync();
        var existingRecord = records.FirstOrDefault(r => r.Id == id);
        if (existingRecord == null)
        {
            return NotFound();
        }

        existingRecord.Name = updatedRecord.Name;
        existingRecord.Value = updatedRecord.Value;
        await WriteCsvFileAsync(records);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecord(int id)
    {
        var records = await ReadCsvFileAsync();
        var record = records.FirstOrDefault(r => r.Id == id);
        if (record == null)
        {
            return NotFound();
        }

        records.Remove(record);
        await WriteCsvFileAsync(records);

        return NoContent();
    }

    [HttpPut("batch")]
    public async Task<IActionResult> UpdateBatchRecords([FromBody] IEnumerable<CsvRecord> updatedRecords)
    {
        if (updatedRecords == null || !updatedRecords.Any())
        {
            return BadRequest();
        }

        var records = await ReadCsvFileAsync();

        foreach (var updatedRecord in updatedRecords)
        {
            var existingRecord = records.FirstOrDefault(r => r.Id == updatedRecord.Id);
            if (existingRecord != null)
            {
                existingRecord.Name = updatedRecord.Name;
                existingRecord.Value = updatedRecord.Value;
            }
        }

        await WriteCsvFileAsync(records);

        return NoContent();
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

    private async Task WriteCsvFileAsync(IEnumerable<CsvRecord> records)
    {
        using (var writer = new StreamWriter(_csvFilePath))
        {
            // Write the header
            await writer.WriteLineAsync("Id,Name,Value");

            // Write each record
            foreach (var record in records)
            {
                var line = $"{record.Id},{record.Name},{record.Value}";
                await writer.WriteLineAsync(line);
            }
        }
    }
}
