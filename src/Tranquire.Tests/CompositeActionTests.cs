﻿using System;
using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Xunit;

namespace Tranquire.Tests
{
    public class CompositeActionTests
    {
        [Theory, DomainAutoData]
        public void Sut_ShouldBeAction(CompositeAction sut)
        {
            sut.Should().BeAssignableTo<IAction<Unit>>();
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(CompositeAction));
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorParameters(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(CompositeAction).GetConstructors()
                                         .Where(c => !c.GetParameters()
                                                       .Select(pi => pi.ParameterType)
                                                       .SequenceEqual(new[] { typeof(Func<CompositeAction, CompositeAction>) })
                                                )
                            );
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorWithParams(IAction<Unit>[] expected)
        {
            //act            
            var sut = new Mock<CompositeAction>(expected);
            //assert
            sut.Object.Actions.Should().Equal(expected);
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldCallActorExecute(
            CompositeAction sut,
            Mock<IActor> actor
            )
        {
            //arrange
            //act
            sut.ExecuteGivenAs(actor.Object);
            //assert
            foreach (var action in sut.Actions)
            {
                actor.Verify(a => a.Execute(action), Times.Once());
            }
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldCallActorExecute(
            CompositeAction sut,
            Mock<IActor> actor
            )
        {
            //arrange
            //act
            sut.ExecuteWhenAs(actor.Object);
            //assert
            foreach (var action in sut.Actions)
            {
                actor.Verify(a => a.Execute(action), Times.Once());
            }
        }

        [Theory, DomainAutoData]
        public void And_ShouldAddAction(
           CompositeAction sut,
           Mock<IActor> actor,
           IAction<Unit> expected
           )
        {
            //arrange
            var existingActions = sut.Actions.ToArray();
            //act
            var actual = sut.And(expected);
            //assert            
            actual.Actions.Except(existingActions).Single().Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void And_WithAbility_ShouldWrapAction(           
           IFixture fixture,
           Mock<IActor> actor,
           IAction<object, object, Unit> expected
           )
        {
            //arrange
            var sut = new Mock<CompositeAction>(fixture.CreateMany<IAction<Unit>>().ToImmutableArray()).Object;
            var existingActions = sut.Actions.ToArray();
            //act
            var actual = sut.And(expected);
            //assert            
            var actualAction = actual.Actions.Except(existingActions).Single();
            actualAction.Should()
                        .BeOfType<ActionWithAbilityToActionAdapter<object, object, Unit>>()
                        .Which
                        .Action
                        .Should()
                        .Be(expected);
        }

        [Theory, DomainAutoData]
        public void And_WithAbility_ShouldContainExistingAbility(
          IFixture fixture,
          Mock<IActor> actor,
          IAction<object, object, Unit> action
          )
        {
            //arrange
            var sut = new Mock<CompositeAction>(fixture.CreateMany<IAction<Unit>>().ToImmutableArray()).Object;
            var existingActions = sut.Actions.ToArray();
            //act
            var actual = sut.And(action);
            //assert                        
            actual.Actions.Should().Contain(existingActions);
        }

        [Theory, DomainAutoData]
        public void Sut_WithCompositeActionBuilder_ShouldHaveCorrectCompositeActions(
           IAction<Unit>[] expected
           )
        {
            //arrange
            //act
            var sut = new Mock<CompositeAction>(new Func<CompositeAction, CompositeAction>(t => expected.Aggregate(t, (tresult, tt) => tresult.And(tt))));
            //assert            
            sut.Object.Actions.Should().Equal(expected);
        }

        public class ToStringCompositeAction : CompositeAction
        {
            public ToStringCompositeAction(string name)
            {
                Name = name;
            }
            public override string Name { get; }
        }

        [Theory, DomainAutoData]
        public void ToString_ShouldReturnCorrectValue(ToStringCompositeAction sut, string expected)
        {
            //act
            var actual = sut.ToString();
            //assert
            Assert.Equal(sut.Name, actual);
        }

        public class EnumeratorCompositeAction : CompositeAction
        {
            public EnumeratorCompositeAction(IAction<Unit>[] actions):base(actions)
            {
            }

            public override string Name => "";
        }

        [Theory, DomainAutoData]
        public void GetEnumerator_ShouldReturnCorrectValue(EnumeratorCompositeAction sut)
        {
            //act            
            //assert
            Assert.Equal(sut.Actions, sut);
        }
    }
}