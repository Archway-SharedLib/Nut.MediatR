﻿using FluentAssertions;
using NSubstitute.Routing.Handlers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Nut.MediatR.Test.Validation
{
    public class DataAnnotationValidationBehaviorTest
    {
        [Fact]
        public async Task Handle_バリデーションエラーがない場合は何もおこらず完了する()
        {
            var executed = false;
            var behavior = new DataAnnotationValidationBehavior<TestBehaviorRequest, TestBehaviorResponse>();
            await behavior.Handle(new TestBehaviorRequest()
            {
                Value = "A"
            }, new CancellationToken(), () => {
                executed = true;
                return Task.FromResult(new TestBehaviorResponse());
            });
            executed.Should().BeTrue();
        }

        [Fact]
        public void Handle_バリデーションエラーがある場合は例外が発生して処理が継続されない()
        {
            var executed = false;
            var behavior = new DataAnnotationValidationBehavior<TestBehaviorRequest, TestBehaviorResponse>();
            Func<Task> act = () => behavior.Handle(new TestBehaviorRequest()
            {
                Value = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            }, new CancellationToken(), () => {
                executed = true;
                return Task.FromResult(new TestBehaviorResponse());
            });

            act.Should().Throw<ValidationException>();
            executed.Should().BeFalse();
        }
    }
}
