using System;
using Infrastructure.Users.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UserService;

namespace UserServiceTest
{
    [TestClass]
    public class UserManagerTest
    {
        /// <summary>
        ///     Test subject.
        /// </summary>
        private UserManager UserManager { get; set; }

        /// <summary>
        ///     Mock representation of repository.
        /// </summary>
        private Mock<IUserRepository> MockRepository { get; set; }
                
        [TestMethod]
        public void AddUserReturnsValidId()
        {
            // Arrange
            MockRepository = new Mock<IUserRepository>();

            MockRepository.Setup(r => r.)
        }
    }
}
