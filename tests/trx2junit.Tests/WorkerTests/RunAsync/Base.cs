using Moq;
using NUnit.Framework;

namespace trx2junit.Tests.WorkerTests.RunAsync
{
    [TestFixture]
    public abstract class Base
    {
        protected Mock<IGlobHandler> _globHandlerMock;
        //---------------------------------------------------------------------
        [SetUp]
        public void BaseSetUp()
        {
            _globHandlerMock = new Mock<IGlobHandler>();

            _globHandlerMock
                .Setup(g => g.ExpandWildcards(It.IsAny<WorkerOptions>()))
                .Verifiable();
        }
        //---------------------------------------------------------------------
        [TearDown]
        public void BaseTearDown()
        {
            _globHandlerMock.Verify();
        }
        //---------------------------------------------------------------------
        protected Worker CreateSut()
        {
            return new Worker(globHandler: _globHandlerMock.Object);
        }
    }
}
