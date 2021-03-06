﻿using MediatR;
using Nut.MediatR.ServiceLike;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLikeSample.Sample.Void
{
    [AsService("/void")]
    public class VoidRequest : IRequest<Unit>
    {
        public string Id { get; set; }
    }
}
