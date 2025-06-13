using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.Exceptions;
using Minio.DataModel.Args;
using Microsoft.Extensions.WebEncoders.Testing;



[ApiController]
public class ListBucket : ControllerBase
{
    private readonly IMinioClient _minioClient;

    public ListBucket(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    [HttpGet("bucketurl/{bucketID}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUrl(string bucketID)
    {
        Console.WriteLine($"Getting URL for bucket: {bucketID!} {_minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket("test-bucket")).Result}");
        return Ok(await _minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket("test-bucket"))
            .ConfigureAwait(false));
    }

}