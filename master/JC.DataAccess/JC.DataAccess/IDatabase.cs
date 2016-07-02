using System;
using System.Collections.Generic;
using System.Data;

namespace JC.DataAccess
{
    /// <summary>
    /// Defines the contract adhered to by classes representing data stores.
    /// </summary>
    public interface IDatabase : IDisposable
    {
        /// <summary>
        /// Rolls back a transaction previously begun by calling one of the
        /// <see cref="BeginTransaction"/> methods.
        /// </summary>
        /// <exception cref="InvalidOperationException"> if no transaction is
        /// currently in progress.</exception>
        void Abort();

        /// <summary>
        /// Starts a transaction with an isolation level of 
        /// <see cref="IsolationLevel.ReadCommitted"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"> if a transaction is
        /// already in progress.</exception>
        void BeginTransaction();

        /// <summary>
        /// Starts a transaction with a specified isolation level.
        /// </summary>
        /// <exception cref="InvalidOperationException"> if a transaction
        /// is already in progress.</exception>
        void BeginTransaction(IsolationLevel isolationLevel);

        /// <summary>
        /// Commits a transaction previously begun by calling one of the
        /// <see cref="BeginTransaction"/> methods.
        /// </summary>
        /// <exception cref="InvalidOperationException"> if no transaction
        /// is in progress.</exception>
        void Commit();

        /// <summary>
        /// Executes a non-row-returning statement with no arguments.
        /// </summary>
        /// <returns>
        /// The number of rows affected, or -1 if a rowcount is not available.
        /// </returns>
        /// <exception cref="ArgumentNullException"> if <paramref name="command"/>
        /// is null or <see cref="string.Empty"/>.</exception>
        int Execute(string command, CommandType commandType);

        /// <summary>
        /// Executes a non-row-returning statement with parameters specified by
        /// an object whose properties will determine the command's parameters.
        /// </summary>
        /// <returns>
        /// The number of rows affected, or -1 if a rowcount is not available.
        /// </returns>
        /// <exception cref="ArgumentNullException"> if <paramref name="command"/>
        /// is null or <see cref="string.Empty"/>.</exception>
        int Execute(string command, CommandType commandType, object parameters);

        /// <summary>
        /// Executes a command and returns object-mapped rows.
        /// </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="command"/>
        /// is null or <see cref="string.Empty"/>.</exception>
        IEnumerable<T> Query<T>(string command, CommandType commandType);

        /// <summary>
        /// Executes a command with specified parameters and returns 
        /// object-mapped rows.
        /// </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="command"/>
        /// is null or <see cref="string.Empty"/>.</exception>
        IEnumerable<T> Query<T>(string command, CommandType commandType, object parameters);

        /// <summary>
        /// Executes a command with specified parameters and returns 
        /// rows object-mapped according to a specified function.
        /// </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="command"/>
        /// is null or <see cref="string.Empty"/>, or if <paramref name="mapper"/>
        /// is null.</exception>
        IEnumerable<T> Query<T>(string command, CommandType commandType, object parameters, Func<IDataReader, T> mapper);

        /// <summary>
        /// Executes a command and returns the first column of the first row
        /// of the resultset, cast to a specified type.
        /// </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="command"/>
        /// is null or <see cref="string.Empty"/>.
        T QueryScalar<T>(string command, CommandType commandType);

        /// <summary>
        /// Executes a command with specified parameters and returns the 
        /// first column of the first row of the resultset, cast to a 
        /// specified type.
        /// </summary>
        /// <exception cref="ArgumentNullException">if <paramref name="command"/>
        /// is null or <see cref="string.Empty"/>.
        T QueryScalar<T>(string command, CommandType commandType, object parameters);

        /// <summary>
        /// Gets a value indicating if the implementation supports transactional
        /// commands.
        /// </summary>
        bool SupportsTransactions { get; }
    }
}
