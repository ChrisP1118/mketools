using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Middleware.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ExceptionMiddleware(IHostingEnvironment hostingEnvironment, RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in middleware");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode = 500;
            ErrorDetails errorDetails = null;
            bool isDev = _hostingEnvironment.IsDevelopment();

            switch (exception)
            {
                case ValidationException ex:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    errorDetails = new ValidationErrorDetails(ex, isDev);
                    break;
                case ForbiddenException ex:
                    statusCode = (int)HttpStatusCode.Forbidden;
                    errorDetails = new ErrorDetails(ex, isDev);
                    break;
                case IdentityException ex:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    errorDetails = new IdentityErrorDetails(ex, isDev);
                    break;
                case DbUpdateConcurrencyException ex:
                    statusCode = (int)HttpStatusCode.Conflict;
                    errorDetails = new ErrorDetails(exception, isDev);
                    break;
                case InvalidRequestException ex:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    errorDetails = new ErrorDetails(exception, isDev);
                    break;
                default:
                    errorDetails = new ErrorDetails(exception, isDev);
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(errorDetails));
        }
    }
}
