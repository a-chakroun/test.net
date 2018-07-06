// -----------------------------------------------------------
// <copyright file="UnhandledExceptionFilterAttribute.cs" company="Company Name">
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
//   An MVC Filter to format the response for unhandled exceptions.
// </summary>
// -----------------------------------------------------------

namespace DotNetCoreWebApiTemplate.Filters
{
    using System;
    using System.Net;
    using DotNetCoreWebApiTemplate.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

    public class UnhandledExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<UnhandledExceptionFilterAttribute> logger;

        public UnhandledExceptionFilterAttribute(ILogger<UnhandledExceptionFilterAttribute> logger)
        {
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            logger.LogWarning("Request generated an unhandled exception {Exception}", context.Exception);

            context.ExceptionHandled = true;

            context.Result = CreateErrorResult(context.Exception);
        }

        private static ObjectResult CreateErrorResult(Exception exception)
        {
            return new ObjectResult(new ErrorResponse(exception.Message))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }
    }
}
