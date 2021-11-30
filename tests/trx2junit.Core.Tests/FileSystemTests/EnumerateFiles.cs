// (c) gfoidl, all rights reserved

using System.IO;
using NUnit.Framework;

namespace gfoidl.Trx2Junit.Core.Tests.FileSystemTests;

[TestFixture]
public class EnumerateFiles
{
    [Test]
    public void Path_and_pattern_given___OK()
    {
        string path    = "./data/trx";
        string pattern = "*.trx";
        var sut        = new FileSystem();

        var actual   = sut.EnumerateFiles(path, pattern);
        var expected = Directory.EnumerateFiles("./data/trx", "*.trx", SearchOption.TopDirectoryOnly);

        CollectionAssert.AreEqual(expected, actual);
    }
}
