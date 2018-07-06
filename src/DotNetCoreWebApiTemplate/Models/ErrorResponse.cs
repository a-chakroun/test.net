// -----------------------------------------------------------
// <copyright file="ErrorResponse.cs" company="Company Name">
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
//   Response model used to return error messages.
// </summary>
// -----------------------------------------------------------

namespace DotNetCoreWebApiTemplate.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Newtonsoft.Json;

    public class ErrorResponse
    {
        public ErrorResponse(ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                throw new ArgumentException("ModelState must be invalid", nameof(modelState));
            }

            Errors = GetModelErrors(modelState);
        }

        [JsonConstructor]
        public ErrorResponse(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        public ErrorResponse(string error)
            : this(new[] { error })
        {
        }

        public IEnumerable<string> Errors { get; }

        private static IEnumerable<string> GetModelErrors(ModelStateDictionary modelState)
        {
            return (from entry in modelState from error in entry.Value.Errors select FormatErrorMessage(entry.Key, error.ErrorMessage)).ToList();
        }

        private static string FormatErrorMessage(string key, string errorMessage)
        {
            return string.IsNullOrEmpty(key)
                ? errorMessage
                : $"{key}: {errorMessage}";
        }
    }
}
