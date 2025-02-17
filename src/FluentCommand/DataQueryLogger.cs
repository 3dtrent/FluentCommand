using System.Data;

using FluentCommand.Internal;

namespace FluentCommand;

/// <summary>
/// A class to log queries to string delegate
/// </summary>
/// <seealso cref="FluentCommand.IDataQueryLogger" />
public class DataQueryLogger : IDataQueryLogger
{
    private readonly Action<string> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataQueryLogger"/> class.
    /// </summary>
    public DataQueryLogger() : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataQueryLogger"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public DataQueryLogger(Action<string> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Log the current specified <paramref name="command" />
    /// </summary>
    /// <param name="command">The command to log.</param>
    /// <param name="duration">The execution duration.</param>
    /// <param name="exception">The exception thrown when executing the command.</param>
    /// <param name="state">The state used to control logging.</param>
    /// <exception cref="System.ArgumentNullException">command</exception>
    public virtual void LogCommand(IDbCommand command, TimeSpan duration, Exception exception = null, object state = null)
    {
        if (_logger == null)
            return;

        if (command is null)
            throw new ArgumentNullException(nameof(command));

        var output = FormatCommand(command, duration, exception);

        _logger(output);
    }

    /// <summary>
    /// Formats the command for logging.
    /// </summary>
    /// <param name="command">The command to log.</param>
    /// <param name="duration">The execution duration.</param>
    /// <param name="exception">The exception thrown when executing the command.</param>
    /// <returns>The command formatted as a string</returns>
    /// <exception cref="System.ArgumentNullException">command</exception>
    public static string FormatCommand(IDbCommand command, TimeSpan duration, Exception exception)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var elapsed = duration.TotalMilliseconds;
        var commandType = command.CommandType;
        var commandTimeout = command.CommandTimeout;
        var resultText = exception == null ? "Executed" : "Error Executing";

        var buffer = StringBuilderCache.Acquire();

        buffer
            .Append(resultText)
            .Append(" DbCommand (")
            .Append(elapsed)
            .Append("ms) [CommandType='")
            .Append(commandType)
            .Append("', CommandTimeout='")
            .Append(commandTimeout)
            .Append("']")
            .AppendLine()
            .AppendLine(command.CommandText);

        foreach (IDataParameter parameter in command.Parameters)
        {
            int precision = 0;
            int scale = 0;
            int size = 0;

            if (parameter is IDbDataParameter dataParameter)
            {
                precision = dataParameter.Precision;
                scale = dataParameter.Scale;
                size = dataParameter.Size;
            }

            buffer
                .Append("-- ")
                .Append(parameter.ParameterName)
                .Append(": ")
                .Append(parameter.Direction)
                .Append(" ")
                .Append(parameter.DbType)
                .Append("(Size=")
                .Append(size)
                .Append("; Precision=")
                .Append(precision)
                .Append("; Scale=")
                .Append(scale)
                .Append(") [")
                .Append(parameter.Value)
                .Append("]")
                .AppendLine();
        }

        var output = StringBuilderCache.ToString(buffer);
        return output;
    }
}
