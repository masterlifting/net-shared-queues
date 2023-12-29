﻿using Net.Shared.Exceptions;

namespace Net.Shared.Queues.Abstractions.Models.Exceptions;

public sealed class QueuesException : NetSharedException
{
    public QueuesException(string message) : base(message)
    {
    }

    public QueuesException(Exception exception) : base(exception)
    {
    }
}