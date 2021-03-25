namespace Zoo.Infrastructure.Tests.Store
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FizzWare.NBuilder;

    using FluentAssertions;

    using Infrastructure.Store;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class WriterShould
    {
        [Test]
        public void AddEntityToDbSetWhenCallCreate()
        {
            var entities = new List<ReaderShould.Entity>();
            var inMemoryDbSet = new InMemoryDbSet<ReaderShould.Entity>(entities);
            var mockedContext = new Mock<IDbContext>();
            mockedContext.Setup(context => context.Set<ReaderShould.Entity>())
                         .Returns(inMemoryDbSet);
            using var writer = new Writer(mockedContext.Object);

            var newEntity = new ReaderShould.Entity();
            writer.Create(newEntity);

            entities.Should().ContainSingle();
            entities.First().Should().Be(newEntity);
        }

        [TearDown]
        public void AfterEach()
        {
        }

        [SetUp]
        public void BeforeEach()
        {
        }

        [Test]
        public void CallOnlyOnceDisposeOnContextWhenCallManyTimesDisposeOnWriter()
        {
            var mockedContext = new Mock<IDbContext>();
            using (var writer = new Writer(mockedContext.Object))
            {
                writer.Dispose();
            }

            mockedContext.Verify(context => context.Dispose(), Times.Once);
        }

        [Test]
        public async Task CallSaveChangeAsyncWhenCallSaveAsync()
        {
            var mockedContext = new Mock<IDbContext>();
            using var writer = new Writer(mockedContext.Object);

            await writer.SaveAsync();

            mockedContext.Verify(context => context.SaveChangesAsync(default));
        }

        [Test]
        public void UpdateExistingEntityWhenCallUpdate()
        {
            var entities = Builder<ReaderShould.Entity>.CreateListOfSize(1).Build().ToList();
            var inMemoryDbSet = new InMemoryDbSet<ReaderShould.Entity>(entities);
            var mockedContext = new Mock<IDbContext>();
            mockedContext.Setup(context => context.Set<ReaderShould.Entity>())
                         .Returns(inMemoryDbSet);
            using var writer = new Writer(mockedContext.Object);

            writer.Update<int, ReaderShould.Entity>(
                1,
                updatingEntity => updatingEntity.Name = "newValue");

            entities.Should().ContainSingle();
            entities.First().Name.Should().Be("newValue");
        }

        public sealed class InMemoryDbSet<TEntity> : DbSet<TEntity> where TEntity : class
        {
            private readonly List<TEntity> entities;

            public InMemoryDbSet(List<TEntity> entities)
            {
                this.entities = entities;
            }

            public override EntityEntry<TEntity> Add(TEntity entity)
            {
                this.entities.Add(entity);
                return default;
            }

            public override TEntity Find(params object[] keyValues)
            {
                return this.entities.First();
            }
        }
    }
}
