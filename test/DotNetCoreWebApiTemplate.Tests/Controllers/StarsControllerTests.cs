// -----------------------------------------------------------
// <copyright file="StarsControllerTests.cs" company="Company Name">
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
//   Unit tests for StarsController.
// </summary>
// -----------------------------------------------------------

namespace DotNetCoreWebApiTemplate.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using DotNetCoreWebApiTemplate.Models;
    using Shouldly;
    using Xunit;

    public class StarsControllerTests : IClassFixture<TestFixture>
    {
        private const string StarsEndpoint = "/api/stars";
        private readonly HttpClient httpClient;
        private readonly Guid siriusStarId = Guid.Parse("b96a31ed-b13b-4823-88e6-051b9a34755d");
        private readonly Guid polarisStarId = Guid.Parse("32af4461-ca5f-4652-b1a7-8d70302d76ca");
        private readonly Guid vegaStarId = Guid.Parse("e094bef2-c6e1-41db-a2cb-ef863e15159a");
        private readonly Guid unknownStarId = Guid.Parse("764a98cf-22c3-4d86-adc4-4d81a9102c19");

        public StarsControllerTests(TestFixture fixture)
        {
            httpClient = fixture.HttpClient;
        }

        [Fact]
        public async Task GetAll_should_return_all_stars()
        {
            var response = await httpClient.GetAsync(StarsEndpoint);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var stars = await response.Content.ReadAsJsonAsync<IList<Star>>();

            stars.Count.ShouldBe(3);
            stars.First().Id.ShouldBe(siriusStarId);
        }

        [Fact]
        public async Task Get_should_return_a_single_star_when_a_valid_id_is_supplied()
        {
            var response = await httpClient.GetAsync($"{StarsEndpoint}/{siriusStarId}");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var star = await response.Content.ReadAsJsonAsync<Star>();

            star.ShouldBe(new Star
            {
                Id = siriusStarId,
                Name = "Sirius",
            });
        }

        [Fact]
        public async Task Get_should_return_a_404_not_found_when_an_unknown_id_is_supplied()
        {
            var response = await httpClient.GetAsync($"{StarsEndpoint}/{unknownStarId}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData(1)]
        [InlineData("123")]
        [InlineData("abc")]
        public async Task Get_should_return_400_bad_request_when_an_invalid_format_id_is_supplied(object invalidStarId)
        {
            var response = await httpClient.GetAsync($"{StarsEndpoint}/{invalidStarId}");

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var errorResponse = await response.Content.ReadAsJsonAsync<ErrorResponse>();

            errorResponse.Errors.ShouldContain($"id: The value '{invalidStarId}' is not valid.");
        }

        [Fact]
        public async Task Add_should_create_a_new_star_and_return_its_unique_id()
        {
            var newStar = new Star
            {
                Name = "NewStar",
            };

            var response = await httpClient.PostAsJsonAsync(StarsEndpoint, newStar);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            var star = await response.Content.ReadAsJsonAsync<Star>();

            star.Id.ShouldNotBe(Guid.Empty);

            // Check the created star has been stored.
            response = await httpClient.GetAsync($"{StarsEndpoint}/{star.Id}");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var storedStar = await response.Content.ReadAsJsonAsync<Star>();

            storedStar.ShouldBe(star);
        }

        [Fact]
        public async Task Add_should_return_400_bad_request_when_an_id_is_supplied()
        {
            var newStar = new Star
            {
                Id = siriusStarId,
                Name = "NewStar",
            };

            var response = await httpClient.PostAsJsonAsync(StarsEndpoint, newStar);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var errorResponse = await response.Content.ReadAsJsonAsync<ErrorResponse>();

            errorResponse.Errors.ShouldContain("Star ID must be empty to create.");
        }

        [Fact]
        public async Task Update_should_update_a_stored_star_when_a_valid_id_is_supplied()
        {
            var existingStar = new Star
            {
                Id = polarisStarId,
                Name = "NewStarName",
            };

            var response = await httpClient.PutAsJsonAsync($"{StarsEndpoint}/{polarisStarId}", existingStar);

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

            // Check the updated star has been stored.
            response = await httpClient.GetAsync($"{StarsEndpoint}/{polarisStarId}");

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var star = await response.Content.ReadAsJsonAsync<Star>();

            star.ShouldBe(existingStar);
        }

        [Fact]
        public async Task Update_should_return_404_not_found_when_an_invalid_id_is_supplied()
        {
            var invalidStar = new Star
            {
                Id = unknownStarId,
                Name = "NewStarName",
            };

            var response = await httpClient.PutAsJsonAsync($"{StarsEndpoint}/{unknownStarId}", invalidStar);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_should_return_400_bad_request_when_the_URL_id_does_not_match_model_id()
        {
            var existingStar = new Star
            {
                Id = unknownStarId,
                Name = "NewStarName",
            };

            var response = await httpClient.PutAsJsonAsync($"{StarsEndpoint}/{polarisStarId}", existingStar);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errorResponse = await response.Content.ReadAsJsonAsync<ErrorResponse>();
            errorResponse.Errors.ShouldContain("Star ID in URL must match ID in model.");
        }

        [Fact]
        public async Task Update_should_return_400_bad_request_when_the_star_name_is_missing()
        {
            var existingStar = new Star
            {
                Id = polarisStarId,
                Name = null,
            };

            var response = await httpClient.PutAsJsonAsync($"{StarsEndpoint}/{polarisStarId}", existingStar);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errorResponse = await response.Content.ReadAsJsonAsync<ErrorResponse>();
            errorResponse.Errors.ShouldContain("Name: The Name field is required.");
        }

        [Fact]
        public async Task Update_should_return_400_bad_request_when_the_star_model_is_missing()
        {
            var response = await httpClient.PutAsJsonAsync($"{StarsEndpoint}/{polarisStarId}", default(Star));

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var errorResponse = await response.Content.ReadAsJsonAsync<ErrorResponse>();
            errorResponse.Errors.ShouldContain("A non-empty request body is required.");
        }

        [Fact]
        public async Task Delete_should_remove_a_stored_star_when_a_valid_id_is_supplied()
        {
            var response = await httpClient.DeleteAsync($"{StarsEndpoint}/{vegaStarId}");

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

            // Check the star has been deleted.
            response = await httpClient.GetAsync($"{StarsEndpoint}/{vegaStarId}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_should_return_204_no_content_even_when_the_star_is_not_found_to_be_idempotence()
        {
            var response = await httpClient.DeleteAsync($"{StarsEndpoint}/{unknownStarId}");

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }
    }
}