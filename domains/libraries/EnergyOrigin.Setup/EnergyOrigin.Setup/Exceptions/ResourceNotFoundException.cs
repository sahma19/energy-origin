namespace EnergyOrigin.Setup.Exceptions;

public class ResourceNotFoundException(string id) : NotFoundException($"Resource with id {id} could not be found.");
