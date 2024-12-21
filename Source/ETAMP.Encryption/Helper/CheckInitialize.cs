namespace ETAMP.Encryption.Helper;

/// <summary>
///     The InitializeWrapper class provides a base implementation with initialization checks for derived classes.
/// </summary>
public class CheckInitialize
{
    /// <summary>
    ///     Indicates if the initialization for a derived class has been completed.
    /// </summary>
    protected bool Init = false;

    /// <summary>
    ///     Checks if the initialization for the derived class has been completed.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the 'InitializeWrapper' method has not been called or a value has not been assigned.
    /// </exception>
    protected void CheckInitialization()
    {
        if (!Init)
            throw new InvalidOperationException(
                "The 'InitializeWrapper' method has not been called or a value has not been assigned.");
    }
}