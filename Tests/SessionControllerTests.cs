using ApI.Controllers;
using ApI.Data;
using ApI.Managers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace Tests
{
    public class SessionControllerTests
    {
        [Fact]
        public async void Verify_StartSession_Returns_SessionId_When_Successful()
        {
            var sessionResult = new SessionResult
            {
                Value = Guid.NewGuid(),
                Status = StatusType.Success
            };
            var mockedSessionManager = new Mock<ISessionManager>();
            mockedSessionManager.Setup(p => p.StartSession(It.IsAny<SessionInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sessionResult);
            SessionController sc = new(mockedSessionManager.Object);
            var result = await sc.StartSession(new SessionInput(), new CancellationToken());
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(((ObjectResult)result.Result).StatusCode, (int)HttpStatusCode.OK);
            Assert.Equal(((ObjectResult)result.Result).Value, sessionResult.Value);
        }

        [Fact]
        public async void Verify_StartSession_Returns_Notfound()
        {
            var sessionResult = new SessionResult
            {
                Message = "Test Message",
                Status = StatusType.NotFound
            };
            var mockedSessionManager = new Mock<ISessionManager>();
            mockedSessionManager.Setup(p => p.StartSession(It.IsAny<SessionInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sessionResult);
            SessionController sc = new(mockedSessionManager.Object);
            var result = await sc.StartSession(new SessionInput(), new CancellationToken());
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(((ObjectResult)result.Result).StatusCode, (int)HttpStatusCode.NotFound);
            Assert.Equal(((ObjectResult)result.Result).Value, sessionResult.Message);
        }

        [Fact]
        public async void Verify_StartSession_Returns_Conflict()
        {
            var sessionResult = new SessionResult
            {
                Message = "Test Message",
                Status = StatusType.Conflict
            };
            var mockedSessionManager = new Mock<ISessionManager>();
            mockedSessionManager.Setup(p => p.StartSession(It.IsAny<SessionInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sessionResult);
            SessionController sc = new(mockedSessionManager.Object);
            var result = await sc.StartSession(new SessionInput(), new CancellationToken());
            Assert.NotNull(result);
            Assert.IsType<ConflictObjectResult>(result.Result);
            Assert.Equal(((ObjectResult)result.Result).StatusCode, (int)HttpStatusCode.Conflict);
            Assert.Equal(((ObjectResult)result.Result).Value, sessionResult.Message);
        }

        [Fact]
        public async void Verify_StartSession_Returns_DependencyFailure()
        {
            var sessionResult = new SessionResult
            {
                Message = "Test Message",
                Status = StatusType.DependencyFail
            };
            var mockedSessionManager = new Mock<ISessionManager>();
            mockedSessionManager.Setup(p => p.StartSession(It.IsAny<SessionInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sessionResult);
            SessionController sc = new(mockedSessionManager.Object);
            var result = await sc.StartSession(new SessionInput(), new CancellationToken());
            Assert.NotNull(result);
            Assert.IsType<StatusCodeResult>(result.Result);
            Assert.Equal(((StatusCodeResult)result.Result).StatusCode, (int)HttpStatusCode.FailedDependency);
        }

        [Fact]
        public async void Verify_StartSession_Returns_InternalServerError()
        {
            var sessionResult = new SessionResult
            {
                Message = "Test Message",
                Status = StatusType.Error
            };
            var mockedSessionManager = new Mock<ISessionManager>();
            mockedSessionManager.Setup(p => p.StartSession(It.IsAny<SessionInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sessionResult);
            SessionController sc = new(mockedSessionManager.Object);
            var result = await sc.StartSession(new SessionInput(), new CancellationToken());
            Assert.NotNull(result);
            Assert.IsType<StatusCodeResult>(result.Result);
            Assert.Equal(((StatusCodeResult)result.Result).StatusCode, (int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async void Verify_StopSession_Returns_OK_When_Successfull()
        {
            var sessionResult = new SessionResult
            {
                Status = StatusType.Success
            };
            var mockedSessionManager = new Mock<ISessionManager>();
            mockedSessionManager.Setup(p => p.StopSession(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sessionResult);
            SessionController sc = new(mockedSessionManager.Object);
            var result = await sc.StopSession(Guid.NewGuid(), new CancellationToken());
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
            Assert.Equal(((OkResult)result).StatusCode, (int)HttpStatusCode.OK);
        }

        [Fact]
        public async void Verify_StopSession_Returns_Notfound()
        {
            var sessionResult = new SessionResult
            {
                Message = "Test Message",
                Status = StatusType.NotFound
            };
            var mockedSessionManager = new Mock<ISessionManager>();
            mockedSessionManager.Setup(p => p.StopSession(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sessionResult);
            SessionController sc = new(mockedSessionManager.Object);
            var result = await sc.StopSession(Guid.NewGuid(), new CancellationToken());
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, (int)HttpStatusCode.NotFound);
            Assert.Equal(((ObjectResult)result).Value, sessionResult.Message);
        }

        [Fact]
        public async void Verify_StopSession_Returns_Conflict()
        {
            var sessionResult = new SessionResult
            {
                Message = "Test Message",
                Status = StatusType.Conflict
            };
            var mockedSessionManager = new Mock<ISessionManager>();
            mockedSessionManager.Setup(p => p.StopSession(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sessionResult);
            SessionController sc = new(mockedSessionManager.Object);
            var result = await sc.StopSession(Guid.NewGuid(), new CancellationToken());
            Assert.NotNull(result);
            Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal(((ObjectResult)result).StatusCode, (int)HttpStatusCode.Conflict);
            Assert.Equal(((ObjectResult)result).Value, sessionResult.Message);
        }

        [Fact]
        public async void Verify_StopSession_Returns_DependencyFailure()
        {
            var sessionResult = new SessionResult
            {
                Message = "Test Message",
                Status = StatusType.DependencyFail
            };
            var mockedSessionManager = new Mock<ISessionManager>();
            mockedSessionManager.Setup(p => p.StopSession(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sessionResult);
            SessionController sc = new(mockedSessionManager.Object);
            var result = await sc.StopSession(Guid.NewGuid(), new CancellationToken());
            Assert.NotNull(result);
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(((StatusCodeResult)result).StatusCode, (int)HttpStatusCode.FailedDependency);
        }

        [Fact]
        public async void Verify_StopSession_Returns_InternalServerError()
        {
            var sessionResult = new SessionResult
            {
                Message = "Test Message",
                Status = StatusType.Error
            };
            var mockedSessionManager = new Mock<ISessionManager>();
            mockedSessionManager.Setup(p => p.StopSession(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sessionResult);
            SessionController sc = new(mockedSessionManager.Object);
            var result = await sc.StopSession(Guid.NewGuid(), new CancellationToken());
            Assert.NotNull(result);
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(((StatusCodeResult)result).StatusCode, (int)HttpStatusCode.InternalServerError);
        }
    }
}