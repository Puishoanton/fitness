﻿namespace Fitness.Application.Exceptions
{
    public abstract class ApiException(int statusCode, string message) : Exception(message)
    {
        public int StatusCode { get; } = statusCode;
    }
}
