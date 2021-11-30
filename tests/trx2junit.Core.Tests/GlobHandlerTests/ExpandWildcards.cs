// (c) gfoidl, all rights reserved

using System.IO;
using System.Linq;
using gfoidl.Trx2Junit.Core.Abstractions;
using Moq;
using NUnit.Framework;

namespace gfoidl.Trx2Junit.Core.Tests.GlobHandlerTests;

[TestFixture]
public class ExpandWildcards
{
    private Mock<IFileSystem> _fileSystemMock;
    //-------------------------------------------------------------------------
    [SetUp]
    public void SetUp()
    {
        _fileSystemMock = new Mock<IFileSystem>();
    }
    //---------------------------------------------------------------------
    private GlobHandler CreateSut() => new(_fileSystemMock.Object);
    //-------------------------------------------------------------------------
    [Test]
    public void WorkerOptions_without_wildcards___nothing_changed()
    {
        string[] inputFiles = { "a.txt", "b.txt" };
        var options         = new WorkerOptions(inputFiles);

        GlobHandler sut = this.CreateSut();

        sut.ExpandWildcards(options);

        CollectionAssert.AreEqual(inputFiles, options.InputFiles);
    }
    //-------------------------------------------------------------------------
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
    //-------------------------------------------------------------------------
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
    //-------------------------------------------------------------------------
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
    //-------------------------------------------------------------------------
    [Test]
    public void Integration_Test()
    {
        string[] inputFiles = { "data/*.trx" };

        var options = new WorkerOptions(inputFiles);
        var sut     = new GlobHandler(new FileSystem());

        sut.ExpandWildcards(options);

        string[] expected = Directory.EnumerateFiles("data", "*.trx", SearchOption.TopDirectoryOnly).ToArray();

        CollectionAssert.AreEqual(expected, options.InputFiles);
    }
}
