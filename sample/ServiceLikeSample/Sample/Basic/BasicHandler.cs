﻿using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceLikeSample.Sample.Basic
{
    public class BasicHandler : IRequestHandler<BasicRequest, BasicResult>
    {
        private readonly ILogger<BasicHandler> logger;

        public BasicHandler(ILogger<BasicHandler> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<BasicResult> Handle(BasicRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation(request.Id);
            return Task.FromResult(new BasicResult() { Name = "Test" });
        }
    }
}
