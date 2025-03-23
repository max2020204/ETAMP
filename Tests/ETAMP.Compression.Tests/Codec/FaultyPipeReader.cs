using System.IO.Pipelines;

namespace ETAMP.Compression.Tests.Codec;

internal class FaultyPipeReader : PipeReader
{
    public override void AdvanceTo(SequencePosition consumed)
    {
        // throw new NotImplementedException();
    }

    public override void AdvanceTo(SequencePosition consumed, SequencePosition examined)
    {
        // throw new NotImplementedException();
    }

    public override void CancelPendingRead()
    {
    }

    public override void Complete(Exception? exception = null)
    {
    }

    public override ValueTask<ReadResult> ReadAsync(CancellationToken cancellationToken = default)
    {
        throw new IOException("Simulated read failure");
    }

    public override bool TryRead(out ReadResult result)
    {
        throw new IOException("Simulated TryRead failure");
    }
}