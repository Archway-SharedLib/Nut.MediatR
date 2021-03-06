﻿using MediatR;
using Microsoft.Extensions.Logging;
using Nut.MediatR.ServiceLike;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLikeSample.Sample.Filter
{
    public class ExceptionFilter : IMediatorServiceFilter
    {
        public async Task<object> HandleAsync(RequestContext context, object parameter, Func<object, Task<object>> next)
        {
            try
            {
                return await next(parameter);
            }
            catch(Exception ex)
            {
                var logger = context.ServiceFactory.GetInstance<ILogger<ExceptionFilter>>();
                logger.LogError(ex, "Error");
                throw ex;
            }
        }
    }
}
