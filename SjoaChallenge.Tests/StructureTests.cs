using AutoFixture;
using FluentAssertions;

namespace SjoaChallenge.Tests
{
    [TestFixture]
    public class StructureTests
    {
        private Structure _sut;
        private IFixture _fixture;
        private const int repeatCount = 3;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior(repeatCount));            

            _sut = _fixture.Create<Structure>();
        }

        [Test]
        public void OnInitialize_ShouldNotBeNull()
        {
            _sut.Should().NotBeNull();
        }

        [Test]
        public void OnGetAllDescendants_ShouldHaveCorrectCount()
        {
            var descendants = _sut.GetAllDescendants(); //first three: 3 + children of children: 3*3  + parent: 1
            descendants.Count().Should().Be(13);
        }


        [Test]
        public void OnGetParent_ShouldReturnCorrectParent()
        {
            var child = _sut.Children.First().Children.First();
            var parent = _sut.GetParent(child);

            parent.Should().Be(_sut.Children.First());            
        }

        [Test]
        public void OnGetParent_IfIsTopNode_ShouldReturNull()
        {
            var child = _sut;
            var parent = _sut.GetParent(child);

            parent.Should().Be(null);
        }
    }
}
