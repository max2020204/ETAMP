namespace ETAMP.Compression.Tests.Codec;

public class FaultyStream : Stream
{
    private Exception _exception;

    // Остальные методы Stream должны быть переопределены, но они не используются в тесте
    public override bool CanRead => true;
    public override bool CanSeek => false;
    public override bool CanWrite => false;
    public override long Length => throw new NotSupportedException();
    public override long Position { get; set; }

    public void SetException(Exception exception)
    {
        _exception = exception;
    }

    public override async Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
    {
        if (_exception != null) throw _exception;

        await base.CopyToAsync(destination, bufferSize, cancellationToken);
    }

    public override void Flush()
    {
        throw new NotSupportedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }
}