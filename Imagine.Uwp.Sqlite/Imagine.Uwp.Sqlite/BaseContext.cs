// ******************************************************************
// Copyright (c) 2017 by Nguyen Pham. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Diagnostics;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Platform.WinRT;
using System.IO;
using Windows.Storage;
using System;

namespace Imagine.Uwp.Sqlite
{
    public class BaseContext
    {
        protected static SQLitePlatformWinRT SqlitePlatform = new SQLitePlatformWinRT();
        protected static string ConnectionString = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "data.sqlite"));
        protected static SQLiteConnectionWithLock Connection = new SQLiteConnectionWithLock(SqlitePlatform, new SQLiteConnectionString(ConnectionString, false));

        protected static SQLiteAsyncConnection AsyncConnection = new SQLiteAsyncConnection(GetConnection);

        private static SQLiteConnectionWithLock GetConnection()
        {
            //TODO: comment out below line to write SQL query in output
            //Connection.TraceListener = new DebugTraceListener();
            return Connection;
        }

        public static void OpenConnection(int userId, Action completed = null)
        {
            ConnectionString = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, string.Format("{0}_data.sqlite", userId)));
            Connection = new SQLiteConnectionWithLock(SqlitePlatform, new SQLiteConnectionString(ConnectionString, false));
            AsyncConnection = new SQLiteAsyncConnection(GetConnection);

            completed?.Invoke();
        }
    }

    public class DebugTraceListener : ITraceListener
    {
        public void Receive(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
