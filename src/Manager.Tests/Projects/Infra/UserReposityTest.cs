using Manager.Domain.Entities;
using Manager.Infra.Context;
using Manager.Infra.Interfaces;
using Manager.Infra.Repositories;
using Manager.Tests.Projects.Fixtures;
using Microsoft.EntityFrameworkCore;

using Moq;
using Xunit;

namespace Manager.Tests.Projects.Infra
{
    public class UserReposityTest
    {

        private readonly IUserRepository? _sut;

        private readonly Mock<ManagerContext>? _managerContextMock;
        private readonly Mock<UserRepository>? _UserRepositoryMock;
        private readonly Mock<DbSet<User>>? _dbSetMock;

        public UserReposityTest()
        {
            _managerContextMock = new Mock<ManagerContext>();
            _dbSetMock = new Mock<DbSet<User>>();
            _UserRepositoryMock = new Mock<UserRepository>();

            _sut = new UserRepository(
                context: _managerContextMock.Object
            );
        }

        [Fact(DisplayName = "Create")]
        [Trait("Category", "Repository")]
        public async Task Create_UserValid_ReturnUser()
        {

            //Arrange
            var userToCreate = UserFixture.CreateValidUser();
            _managerContextMock.Setup(m => m.Users).Returns(_dbSetMock.Object);

            //Act
            var createdUser = await _sut.Create(userToCreate);

            // Assert
            Assert.True(createdUser != null);
        }

        [Fact(DisplayName = "Update")]
        [Trait("Category", "Repository")]
        public async Task Update_UserValid_ReturnUser()
        {

            //Arrange
            var userToUpdate = UserFixture.CreateValidUser();
            _managerContextMock.Setup(m => m.Users).Returns(_dbSetMock.Object);
            _UserRepositoryMock.Setup(x => x.Remove(It.IsAny<long>())).Returns<long>(x => Task.CompletedTask);

            //Act
            var updatedUser = _sut.Update(userToUpdate);

            // Assert
            Assert.True(updatedUser.IsCompleted);
        }

        [Fact(DisplayName = "Remove")]
        [Trait("Category", "Repository")]
        public async Task Remove_UserValid_ReturnUser()
        {

            //Arrange
            var userToRemove = UserFixture.CreateValidUser();
            _managerContextMock.Setup(m => m.Users).Returns(_dbSetMock.Object);


            //Act
            var removedUser = _sut.Remove(userToRemove.Id);

            // Assert
            Assert.True(removedUser.IsCompleted);
        }


        [Fact(DisplayName = "GetByEmail")]
        [Trait("Category", "Repository")]
        public async Task GetByEmail_UserValid_ReturnUser()
        {

            //Arrange
            var userToGet = UserFixture.CreateValidUser();
            _managerContextMock.Setup(m => m.Users).Returns(_dbSetMock.Object);

            //Act
            var getUser = await _sut.GetByEmail(userToGet.Email);

            // Assert
            Assert.True(getUser != null);


        }

        [Fact(DisplayName = "SearchByEmail")]
        [Trait("Category", "Repository")]
        public async Task SearchByEmail_UserValid_ReturnUser()
        {

            //Arrange
            var userToGet = UserFixture.CreateValidUser();
            _managerContextMock.Setup(m => m.Users).Returns(_dbSetMock.Object);

            //Act
            var getUser = await _sut.SearchByEmail(userToGet.Email);

            // Assert
            Assert.True(getUser != null);
        }

        [Fact(DisplayName = "SearchByName")]
        [Trait("Category", "Repository")]
        public async Task SearchByName_UserValid_ReturnUser()
        {

            //Arrange
            var userToGet = UserFixture.CreateValidUser();
            _managerContextMock.Setup(m => m.Users).Returns(_dbSetMock.Object);

            //Act
            var getUser = _sut.SearchByName(userToGet.Name);

            // Assert
            Assert.True(getUser != null);
        }
    }

}

