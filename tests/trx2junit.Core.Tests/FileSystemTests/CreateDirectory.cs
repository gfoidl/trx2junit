// (c) gfoidl, all rights reserved

using System;
using System.IO;
using NUnit.Framework;

namespace gfoidl.Trx2Junit.Core.Tests.FileSystemTests;

[TestFixture]
public class CreateDirectory
{
    [Test]
    public void Path_given___OK()
    {
        string path = $"./{Guid.NewGuid()}";
        var sut     = new FileSystem();

        sut.CreateDirectory(path);

        DirectoryAssert.Exists(path);

        try
        {
            Directory.Delete(path);
        }
        catch { }
    }
}
