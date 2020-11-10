using System;
using Xunit;
using FluentAssertions;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Nut.MediatR.Test.PerRequest
{
    public class WithBehaviorsAttributeTest
    {
        [Fact]
        public void ctor_�p�����[�^�[��null�̏ꍇ�͗�O����������()
        {
            Action act = () => new WithBehaviorsAttribute(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ctor_�p�����[�^�[�̒���null���܂܂�Ă���ꍇ�͗�O����������()
        {
            Action act = () => new WithBehaviorsAttribute(typeof(TestBehavior1<,>), null, typeof(TestBehavior2<,>));
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ctor_�p�����[�^�[�̒���IPipelineBehavior�ȊO�̌^���܂܂�Ă���ꍇ�͗�O����������()
        {
            Action act = () => new WithBehaviorsAttribute(typeof(TestBehavior1<,>), typeof(string), typeof(TestBehavior2<,>));
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void BehaviorTypes_�R���X�g���N�^�Őݒ肳�ꂽ����Type���擾�ł���()
        {
            var attr = new WithBehaviorsAttribute(typeof(TestBehavior2<,>), typeof(TestBehavior1<,>));
            attr.BehaviorTypes.Should().HaveCount(2).And.ContainInOrder(typeof(TestBehavior2<,>), typeof(TestBehavior1<,>));
        }
    }
}
