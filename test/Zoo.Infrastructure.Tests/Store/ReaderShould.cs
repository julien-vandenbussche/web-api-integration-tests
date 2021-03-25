namespace Zoo.Infrastructure.Tests.Store
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Linq.Expressions;

    using Core.Specification;

    using FizzWare.NBuilder;

    using FluentAssertions;

    using Infrastructure.Store;

    using Internals;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class ReaderShould
    {
        [Test]
        public void AddRelationshipWhenCallGetWithSpecification()
        {
            Expression<Func<Entity, bool>> filter = entity => true;
            var entities = Builder<Entity>.CreateListOfSize(10)
                                          .Build();
            var relationship = Relationship<Entity>.Create(entity => entity.SubEntities);
            relationship.Then<SubEntity>(s => s.Values);
            var mockedContext = new Mock<IDbContext>();
            mockedContext.Setup(context => context.Set<Entity>())
                         .ReturnsDbSet(entities);
            var mockedSpecification = new Mock<ISpecification<Entity>>();
            mockedSpecification.SetupGet(specification => specification.Relationships)
                               .Returns(() => new List<Relationship<Entity>> { relationship }.ToImmutableList());
            mockedSpecification.Setup(specification => specification.ToExpression())
                               .Returns(filter);
            using var reader = new Reader(mockedContext.Object);

            var actualEntities = reader.Get(mockedSpecification.Object);

            actualEntities.Should().Equal(entities);
            mockedSpecification.Verify(specification => specification.Relationships, Times.Once);
        }

        [SetUp]
        public void BeforeEach()
        {
        }

        [Test]
        public void CallDisposeOnContextWhenCallDisposeOnReader()
        {
            var mockedContext = new Mock<IDbContext>();
            using (var reader = new Reader(mockedContext.Object))
            {
            }

            mockedContext.Verify(context => context.Dispose(), Times.Once);
        }

        [Test]
        public void CallOnlyOnceDisposeOnContextWhenCallManyTimesDisposeOnReader()
        {
            var mockedContext = new Mock<IDbContext>();
            using (var reader = new Reader(mockedContext.Object))
            {
                reader.Dispose();
            }

            mockedContext.Verify(context => context.Dispose(), Times.Once);
        }

        [Test]
        public void ReturnFilteredEntitiesWhenCallGetWithSpecification()
        {
            Expression<Func<Entity, bool>> filter = entity => entity.Id == 69;
            var entities = Builder<Entity>.CreateListOfSize(10)
                                          .TheFirst(1)
                                          .With(e => e.Id = 69)
                                          .Build();
            var mockedContext = new Mock<IDbContext>();
            mockedContext.Setup(context => context.Set<Entity>())
                         .ReturnsDbSet(entities);
            var mockedSpecification = new Mock<ISpecification<Entity>>();
            mockedSpecification.Setup(specification => specification.Relationships)
                               .Returns(() => Enumerable.Empty<Relationship<Entity>>().ToImmutableList());
            mockedSpecification.Setup(specification => specification.ToExpression())
                               .Returns(filter);
            using var reader = new Reader(mockedContext.Object);

            var actualEntities = reader.Get(mockedSpecification.Object);

            actualEntities.Should().ContainSingle();
        }

        public sealed class Entity
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public ICollection<SubEntity> SubEntities { get; set; } = new HashSet<SubEntity>();
        }

        public sealed class SubEntity
        {
            public Entity Entity { get; set; }

            public int EntityId { get; set; }

            public int Id { get; set; }

            public ICollection<SubValue> Values { get; set; } = new HashSet<SubValue>();
        }

        public sealed class SubValue
        {
            public int Id { get; set; }

            public SubEntity SubEntity { get; set; }

            public int SubValueId { get; set; }
        }
    }
}
