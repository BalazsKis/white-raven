using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WhiteRaven.Repository.Contract.Exceptions;
using WhiteRaven.Shared.Basics;

namespace WhiteRaven.Web.Api
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;


        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }


        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            if (ex is KeyNotFoundException)
            {
                return SetResult(
                    context,
                    HttpStatusCode.NotFound,
                    "Resource not found",
                    ex.ToMessageWithInnerExceptions());
            }

            if (ex is UnauthorizedAccessException)
            {
                return SetResult(
                    context,
                    HttpStatusCode.Forbidden,
                    "Authentication failure or insufficient rights",
                    ex.ToMessageWithInnerExceptions());
            }

            if (ex is ArgumentException)
            {
                return SetResult(
                    context,
                    HttpStatusCode.UnprocessableEntity,
                    "The request content was empty or insufficient",
                    ex.ToMessageWithInnerExceptions());
            }

            if (ex is CreateFailedException)
            {
                return SetResult(
                    context,
                    HttpStatusCode.InternalServerError,
                    "Entity creation failed",
                    ex.ToMessageWithInnerExceptions());
            }

            if (ex is ReadFailedException)
            {
                return SetResult(
                    context,
                    HttpStatusCode.InternalServerError,
                    "Entity read failed",
                    ex.ToMessageWithInnerExceptions());
            }

            if (ex is UpdateFailedException)
            {
                return SetResult(
                    context,
                    HttpStatusCode.InternalServerError,
                    "Entity update failed",
                    ex.ToMessageWithInnerExceptions());
            }

            if (ex is DeleteFailedException)
            {
                return SetResult(
                    context,
                    HttpStatusCode.InternalServerError,
                    "Entity deletion failed",
                    ex.ToMessageWithInnerExceptions());
            }

            return SetResult(
                context,
                HttpStatusCode.InternalServerError,
                "Unexpected error while processing request",
                ex.ToShortStringWithInnerExceptions());
        }

        private static Task SetResult(HttpContext context, HttpStatusCode statusCode, string title, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(JsonApi.ErrorObject(statusCode, title, message));
        }
    }
}