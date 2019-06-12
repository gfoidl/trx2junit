using System.Linq;
using Moq;
using NUnit.Framework;

namespace trx2junit.Tests.Internal.GlobHandlerTests
{
    [TestFixture]
    public class ExpandWildcards
    {
        private Mock<IFileSystem> _fileSystemMock;
        //---------------------------------------------------------------------
        [SetUp]
        public void SetUp()
        {
            _fileSystemMock = new Mock<IFileSystem>();
        }
        //---------------------------------------------------------------------
        private GlobHandler CreateSut() => new GlobHandler(_fileSystemMock.Object);
        //---------------------------------------------------------------------
        [Test]
        public void WorkerOptions_without_wildcards___nothing_changed()
        {
            string[] inputFiles = { "a.txt", "b.txt" };
            var options         = new WorkerOptions(inputFiles);

            GlobHandler sut = this.CreateSut();

            sut.ExpandWildcards(options);

            CollectionAssert.AreEqual(inputFiles, options.InputFiles);
        }
        //---------------------------------------------------------------------
        [Test]
        public void WorkerOptions_with_wildcards___got_expanded()
        {
            string[] inputFiles = { "*.txt" };
            string[] expected   = { "a.txt", "b.txt" };

            var options = new WorkerOptions(inputFiles);

            _fileSystemMock
                .Setup(f => f.EnumerateFiles("", "*.txt"))
                .Returns(expected)
                .Verifiable();

            GlobHandler sut = this.CreateSut();

            sut.ExpandWildcards(options);

            CollectionAssert.AreEqual(expected, options.InputFiles);
        }
        //---------------------------------------------------------------------
        [Test]
        public void WorkerOptions_with_wildcards_in_path___got_expanded()
        {
            string[] inputFiles = { "foo/*.txt" };
            string[] expected   = { "a.txt", "b.txt" };

            var options = new WorkerOptions(inputFiles);

            _fileSystemMock
                .Setup(f => f.EnumerateFiles("foo", "*.txt"))
                .Returns(expected)
                .Verifiable();

            GlobHandler sut = this.CreateSut();

            sut.ExpandWildcards(options);

            CollectionAssert.AreEqual(expected, options.InputFiles);
        }
        //---------------------------------------------------------------------
        [Test]
        public void WorkerOptions_with_wildcards_and_expanded_files___OK()
        {
            string[] inputFiles = { "foo.txt", "*.txt" };
            string[] expected   = { "foo.txt", "a.txt", "b.txt" };

            var options = new WorkerOptions(inputFiles);

            _fileSystemMock
                .Setup(f => f.EnumerateFiles("", "*.txt"))
                .Returns(expected.Skip(1))
                .Verifiable();

            GlobHandler sut = this.CreateSut();

            sut.ExpandWildcards(options);

            CollectionAssert.AreEqual(expected, options.InputFiles);
        }
    }
}
