using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.Exceptions;
using Minio.DataModel.Args;
[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IMinioClient _minioClient;

    public FileController(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    [HttpPost("putobject")]
    public async Task<IActionResult> PutObjectToTestBucket(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var bucketName = "test-bucket";
        try
        {
            using (var stream = file.OpenReadStream())
            {
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(file.FileName)
                    .WithStreamData(stream)
                    .WithObjectSize(file.Length);
                var result = await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
                Console.WriteLine(result.ToString());
            }

            return Ok($"File '{file.FileName}' uploaded to bucket '{bucketName}'.");
        }
        catch (MinioException e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}