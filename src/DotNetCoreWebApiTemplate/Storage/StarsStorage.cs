// -----------------------------------------------------------
// <copyright file="StarsStorage.cs" company="Company Name">
// Copyright YEAR COPYRIGHT HOLDER
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall
// be included in all copies or substantial portions of the
// Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Stars database storage.
// </summary>
// -----------------------------------------------------------

namespace DotNetCoreWebApiTemplate.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DotNetCoreWebApiTemplate.Models;

    public class StarsStorage
    {
        private readonly DbConfig dbConfig;

        private readonly IList<Star> stars = new List<Star>
        {
            new Star
            {
                Id = Guid.Parse("b96a31ed-b13b-4823-88e6-051b9a34755d"),
                Name = "Sirius",
            },
            new Star
            {
                Id = Guid.Parse("e094bef2-c6e1-41db-a2cb-ef863e15159a"),
                Name = "Vega",
            },
            new Star
            {
                Id = Guid.Parse("32af4461-ca5f-4652-b1a7-8d70302d76ca"),
                Name = "Polaris",
            },
        };

        public StarsStorage(DbConfig dbConfig)
        {
            this.dbConfig = dbConfig;
        }

        public async Task Init()
        {
            // Initialize database here;
            await Task.FromResult(dbConfig.ConnectionString);
        }

        public async Task<Guid> AddAsync(Star star)
        {
            if (star.Id != Guid.Empty)
            {
                throw new ArgumentException();
            }

            star.Id = Guid.NewGuid();

            stars.Add(star);

            return await Task.FromResult(star.Id);
        }

        public async Task<IEnumerable<Star>> GetAllAsync()
        {
            return await Task.FromResult(stars);
        }

        public async Task<Star> GetAsync(Guid id)
        {
            var star = stars.FirstOrDefault(s => s.Id == id);

            if (star == null)
            {
                throw new KeyNotFoundException();
            }

            return await Task.FromResult(star);
        }

        public async Task UpdateAsync(Star star)
        {
            var storedStar = await GetAsync(star.Id);

            storedStar.Name = star.Name;
        }

        public async Task DeleteAsync(Guid id)
        {
            stars.Remove(await GetAsync(id));
        }
    }
}
