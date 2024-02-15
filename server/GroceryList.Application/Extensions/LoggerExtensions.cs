using Microsoft.Extensions.Logging;

namespace GroceryList.Application.Extensions;
public static class LoggingExtensions
{
    public static void LogInformation(this ILogger logger, string message, params object[] args)
    {
        const string logPrefix = "[API]"; // Customize the log prefix as needed
        var formattedMessage = $"{logPrefix} {message}";

        logger.LogInformation(formattedMessage, args);
    }

    public static void LogError(this ILogger logger, Exception exception, string message, params object[] args)
    {
        // Include the original error message
        var formattedMessage = string.Format(message, args);

        // Add stack trace information
        var stackTrace = new System.Diagnostics.StackTrace(exception, true);
        var stackTraceString = stackTrace.ToString();

        // Append stack trace to the original message
        var logMessage = $"{formattedMessage}\nStackTrace:\n{stackTraceString}";

        // Log the message along with the stack trace
        logger.LogError(exception, logMessage);
    }
}
