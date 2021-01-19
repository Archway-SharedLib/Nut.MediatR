﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Nut.MediatR.ServiceLike
{
    public class DefaultMediatorClient : IMediatorClient
    {
        private readonly IMediator mediator;
        private readonly RequestRegistry registry;
        private readonly ServiceFactory factory;

        public DefaultMediatorClient(IMediator mediator, RequestRegistry registry, ServiceFactory factory)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task<TResult?> SendAsync<TResult>(string path, object request) where TResult : class
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var mediatorRequest = registry.GetRequest(path);
            if(mediatorRequest is null)
            {
                throw new InvalidOperationException(SR.MediatorRequestNotFound(path));
            }
            var value = TranslateType(request, mediatorRequest.RequestType);

            var context = new RequestContext(mediatorRequest.Path, mediatorRequest.RequestType, typeof(TResult), factory);

            var result = await ExecuteAsync(new Queue<Type>(mediatorRequest.Filters), value, mediator, context).ConfigureAwait(false);
            
            return TranslateType(result, typeof(TResult)) as TResult;
        }

        private object? TranslateType(object? value, Type type)
        {
            if (value is null or Unit) return null;
            var json = JsonSerializer.Serialize(value, value.GetType(), new JsonSerializerOptions()
            {
            });
            return JsonSerializer.Deserialize(json, type);
        }

        private async Task<object?> ExecuteAsync(Queue<Type> filterTypes, object? parameter, IMediator mediator, RequestContext context)
        {
            if (filterTypes.TryDequeue(out Type filterType))
            {
                var filter = filterType.Activate<IMediatorServiceFilter>();
                return await filter.HandleAsync(context, parameter, async (newParam) =>
                {
                    return await ExecuteAsync(filterTypes, newParam, mediator, context).ConfigureAwait(false);
                }).ConfigureAwait(false);
            }
            return await mediator.Send(parameter!).ConfigureAwait(false);
        }
    }
}
