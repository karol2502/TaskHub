namespace TaskHub.Common.Exceptions;

public sealed class NotFoundException(string resourceType, string resourceIdentifier)
    : Exception($"{resourceType} with id: {resourceIdentifier} does not exist");
    