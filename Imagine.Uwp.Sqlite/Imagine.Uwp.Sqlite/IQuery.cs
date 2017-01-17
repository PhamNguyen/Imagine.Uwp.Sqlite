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

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Imagine.Uwp.Sqlite
{
    public interface IQuery<T>
    {
        Task<IEnumerable<T>> SelectAsync(string query);

        Task<IEnumerable<T>> QueryAsync(string query);

        Task<IEnumerable<T>> SelectAsync(string format, params object[] parametters);

        Task<IEnumerable<T>> SelectAsync(int page, int size, string query = "");

        Task<T> FirstOrDefault(string query);
        
        Task<int> CommitAsync(T input);

        Task<int> InsertAsync(T input);

        Task<int> InsertAllAsync(IEnumerable<T> input);

        Task<int> UpdateAsync(T input);

        Task<int> DeleteAsync(T input);

        Task<int> Clear();
    }
}
