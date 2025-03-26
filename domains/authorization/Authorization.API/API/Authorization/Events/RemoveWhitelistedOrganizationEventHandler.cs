using System;
using System.Threading.Tasks;
using API.Authorization._Features_.Internal;
using EnergyOrigin.Domain.ValueObjects;
using EnergyOrigin.IntegrationEvents.Events.OrganizationWhitelisted;
using MassTransit;
using MediatR;

namespace API.Authorization.Events;

public class RemoveWhitelistedOrganizationEventHandler(IMediator mediator) : IConsumer<OrganizationRemovedFromWhitelist>
{
    public async Task Consume(ConsumeContext<OrganizationRemovedFromWhitelist> context)
    {
        var tin = Tin.Create(context.Message.Tin);
        await mediator.Send(new RemoveWhitelistOrganizationCommand(tin), context.CancellationToken);
    }
}
public class RemoveWhitelistedOrganizationEventHandlerDefinition
    : ConsumerDefinition<RemoveWhitelistedOrganizationEventHandler>
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<RemoveWhitelistedOrganizationEventHandler> consumerConfigurator,
        IRegistrationContext context
    )
    {
        endpointConfigurator.UseMessageRetry(r =>
            r.Incremental(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(15)));
    }
}
