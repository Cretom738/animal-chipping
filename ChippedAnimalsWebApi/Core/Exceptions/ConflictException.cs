﻿namespace Core.Exceptions
{
    public abstract class ConflictException : Exception
    {
        protected ConflictException(string message) : base(message)
        {
        }
    }
}
