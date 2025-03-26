namespace EnergyOrigin.IntegrationEvents.Events.OrganizationWhitelisted;

public record OrganizationRemovedFromWhitelist : IntegrationEvent
{
    public string Tin { get; }

    public OrganizationRemovedFromWhitelist(Guid id, string traceId, DateTimeOffset created, string tin)
        : base(id, traceId, created)
    {
        Tin = tin;
    }
}
