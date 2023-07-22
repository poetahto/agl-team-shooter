using System;

namespace Poetools.UI.Items
{
    /// <summary>
    /// A disposable that runs an action when disposed.
    /// <remarks>The dispose action is idempotent: you can call it several times, but it only runs once.</remarks>
    /// </summary>
    public class DisposableAction : IDisposable
    {
        private readonly Action _action;
        private bool _disposed;

        /// <summary>
        /// Creates a new instance of <see cref="DisposableAction"/>
        /// </summary>
        /// <param name="action">The action to execute, when disposed.</param>
        public DisposableAction(Action action = null)
        {
            _action = action;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!_disposed)
            {
                _action?.Invoke();
                _disposed = true;
            }
        }
    }
}
