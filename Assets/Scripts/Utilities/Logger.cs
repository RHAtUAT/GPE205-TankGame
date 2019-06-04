using UnityEngine;

namespace Assets.Scripts.Utilities
{
    /// <summary>
    /// A simple logger with color. Severity codes are modeled after RFC 7321.
    /// </summary>
    public class Logger
    {

        private readonly string id;

        public enum Severity
        {
            Debug,
            Info,
            Notice,
            Warning,
            Error
        }

        /// <summary>
        /// Creates a new Logger instance with the specified name.
        /// </summary>
        /// <param name="name"></param>
        public Logger(string name)
        {
            id = name;
        }

        /// <summary>
        /// Writes a log message.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <param name="context"></param>
        public void Log(Severity type, string message, Object context)
        {
            UnityEngine.Debug.Log(string.Format(
                "{0} {1} {2}",
                string.Format("[{0}]", type.ToString()),
                string.Format("<color=#444444FF>[{0}]</color>", id),
                message
            ), context);
        }

        /// <summary>
        /// Writes a log message with a colored severity tag.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <param name="context"></param>
        /// <param name="color"></param>
        public void Log(Severity type, string message, Object context, string color)
        {
            UnityEngine.Debug.Log(string.Format(
                "{0} {1} {2}",
                string.Format("<color={0}>[{1}]</color>", color, type.ToString()),
                string.Format("<color=#444444FF>[{0}]</color>", id),
                message
            ), context);
        }

        /// <summary>
        /// Writes a debug log message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="context"></param>
        public void Debug(string message, Object context = null)
        {
            Log(Severity.Debug, message, context, "cyan");
        }

        /// <summary>
        /// Writes an info log message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="context"></param>
        public void Info(string message, Object context = null)
        {
            Log(Severity.Info, message, context, "green");
        }

        /// <summary>
        /// Writes a notice log message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="context"></param>
        public void Notice(string message, Object context = null)
        {
            Log(Severity.Notice, message, context, "purple");
        }

        /// <summary>
        /// Writes a warning log message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="context"></param>
        public void Warning(string message, Object context = null)
        {
            Log(Severity.Warning, message, context, "orange");
        }

        /// <summary>
        /// Writes an error log message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="context"></param>
        public void Error(string message, Object context = null)
        {
            Log(Severity.Error, message, context, "red");
        }

    }
}