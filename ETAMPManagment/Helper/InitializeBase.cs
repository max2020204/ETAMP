namespace ETAMPManagment.Helper;

/// <summary>
/// The InitializeBase class provides a base implementation with initialization checks for derived classes.
/// </summary>
public class InitializeBase
{
    /// <summary>
    /// Indicates if the initialization for a derived class has been completed.
    /// </summary>
    protected bool _init = false;

    /// <summary>
    /// Checks if the initialization for the derived class has been completed.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the 'Initialize' method has not been called or a value has not been assigned.
    /// </exception>
    protected void CheckInitialization()
    {
        if (!_init)
            throw new InvalidOperationException(
                "The 'Initialize' method has not been called or a value has not been assigned.");
    } 
}