// (c) gfoidl, all rights reserved

using System;
using NUnit.Framework;

namespace gfoidl.Trx2Junit.Core.Tests;

[TestFixture]
public class DummyTests
{
    [Test]
    [Ignore("For demo")]
    public void Ignored_test_for_demo_in_CI()
    {
        throw new Exception();
    }
    //-------------------------------------------------------------------------
    [Test]
    public void Test_with_unmet_assumption_for_demo_in_CI()
    {
        Assume.That(false, "unmet assumption for demo");

        throw new Exception();
    }
}
