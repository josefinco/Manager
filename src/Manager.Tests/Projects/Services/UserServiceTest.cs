using AutoMapper;
using Bogus;
using Bogus.DataSets;
using EscNet.Cryptography.Interfaces;
using FluentAssertions;
using Manager.Core.Exceptions;
using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Manager.Services.DTO;
using Manager.Services.Interfaces;
using Manager.Services.Services;
using Manager.Tests.Configuration;
using Manager.Tests.Projects.Fixtures;
using Moq;
using Xunit;

namespace Manager.Tests.Projects.Services
{
    public class UserServiceTest
    {
        private readonly IUserService? _sut;

        private readonly IMapper? _mapper;

        private readonly Mock<IUserRepository>? _userRepositoryMock;

        private readonly Mock<IRijndaelCryptography>? _rijndaelCryptographyMock;

        public UserServiceTest()
        {
            _mapper = AutoMapperConfiguration.Configure(); ;
            _userRepositoryMock = new Mock<IUserRepository>();
            _rijndaelCryptographyMock = new Mock<IRijndaelCryptography>();


            _sut = new UserService(
                mapper: _mapper,
                userRepository: _userRepositoryMock.Object,
                rijndaelCryptography: _rijndaelCryptographyMock.Object
                );
        }

        #region Create
        [Fact(DisplayName = "Create a Valid User")]
        [Trait("Category", "Services")]
        // NOMEDOMETODO_CONDICAO_RESULTADOESPERADO
        public async Task Create_WhenUserIsValid_ReturnUserDTO()
        {
            // Arrange
            var userToCreate = UserFixture.CreateValidUserDTO();

            var encryptedPassword = "123456489";
            var userCreated = _mapper.Map<User>(userToCreate);
            userCreated.ChangePassword(encryptedPassword);

            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _rijndaelCryptographyMock.Setup(x => x.Encrypt(It.IsAny<string>()))
                .Returns(encryptedPassword);

            _userRepositoryMock.Setup(x => x.Create(It.IsAny<User>()))
                .ReturnsAsync(() => userCreated);

            // ACT
            var result = await _sut.Create(userToCreate);

            // Assert
            result.Should()
                .BeEquivalentTo(_mapper.Map<UserDTO>(userCreated));
        }

        [Fact(DisplayName = "Create When User Exists")]
        [Trait("Category", "Services")]
        public void Create_WhenUserExists_ThrowsNewDomainException()
        {
            // Arrange
            var userToCreate = UserFixture.CreateValidUserDTO();
            var userExists = UserFixture.CreateValidUser();

            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>()))
                .ReturnsAsync(() => userExists);

            // Act
            Func<Task<UserDTO>> act = async () =>
            {
                return await _sut.Create(userToCreate);
            };

            // Act
            act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("Já existe um usuário cadastrado com o email informado.");
        }

        [Fact(DisplayName = "Create When User is Invalid")]
        [Trait("Category", "Services")]
        public void Create_WhenUserIsInvalid_ThrowsNewDomainException()
        {
            // Arrange
            var userToCreate = UserFixture.CreateInvalidUserDTO();

            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            // Act
            Func<Task<UserDTO>> act = async () =>
            {
                return await _sut.Create(userToCreate);
            };

            // Act
            act.Should()
                .ThrowAsync<DomainException>();
        }

        #endregion

        #region Update

        [Fact(DisplayName = "Update Valid User")]
        [Trait("Category", "Services")]
        public async Task Update_WhenUserIsValid_ReturnsUserDTO()
        {
            // Arrange
            var oldUser = UserFixture.CreateValidUser();
            var userToUpdate = UserFixture.CreateValidUserDTO();
            var userUpdated = _mapper.Map<User>(userToUpdate);

            var encryptedPassword = "123456789";

            _userRepositoryMock.Setup(x => x.Get(It.IsAny<long>()))
                .ReturnsAsync(() => oldUser);

            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>()))
                .ReturnsAsync(() => oldUser);

            _rijndaelCryptographyMock.Setup(x => x.Encrypt(It.IsAny<string>()))
                .Returns(encryptedPassword);

            _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>()))
                .ReturnsAsync(() => userUpdated);

            // Act
            var result = await _sut.Update(userToUpdate);

            // Assert
            result.Should()
                .BeEquivalentTo(_mapper.Map<UserDTO>(userUpdated));
        }


        [Fact(DisplayName = "Update When User Not Exists")]
        [Trait("Category", "Services")]
        public void Update_WhenUserNotExists_ThrowsNewDomainException()
        {
            // Arrange
            var userToUpdate = UserFixture.CreateValidUserDTO();

            _userRepositoryMock.Setup(x => x.Get(It.IsAny<long>()))
                .ReturnsAsync(() => null);

            // Act
            Func<Task<UserDTO>> act = async () =>
            {
                return await _sut.Update(userToUpdate);
            };

            // Act
            act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("Não existe nenhum usuário com o id informado!");
        }

        [Fact(DisplayName = "Update When User is Invalid")]
        [Trait("Category", "Services")]
        public void Update_WhenUserIsInvalid_ThrowsNewDomainException()
        {
            // Arrange
            var oldUser = UserFixture.CreateValidUser();
            var userToUpdate = UserFixture.CreateInvalidUserDTO();

            _userRepositoryMock.Setup(x => x.Get(It.IsAny<long>()))
                .ReturnsAsync(() => oldUser);

            // Act
            Func<Task<UserDTO>> act = async () =>
            {
                return await _sut.Update(userToUpdate);
            };

            // Act
            act.Should()
                .ThrowAsync<DomainException>();
        }

        #endregion

        #region Remove

        [Fact(DisplayName = "Remove User")]
        [Trait("Category", "Services")]
        public async Task Remove_WhenUserExists_RemoveUser()
        {

            // Arrange
            var user = UserFixture.CreateValidUserDTO();
            var userCreated = _mapper.Map<User>(user);

            _userRepositoryMock.Setup(x => x.Get(It.IsAny<long>()))
                .ReturnsAsync(() => userCreated);

            _userRepositoryMock.Setup(x => x.Remove(It.IsAny<long>())).Returns<long>(x => Task.CompletedTask);

            var _sutTeste = new UserService(
                mapper: _mapper,
                userRepository: _userRepositoryMock.Object,
                rijndaelCryptography: _rijndaelCryptographyMock.Object
                );

            // ACT
            var exception = await Record.ExceptionAsync(() => _sutTeste.Remove(userCreated.Id));


            // Assert
            Assert.True(exception == null);
        }

        [Fact(DisplayName = "Remove User When User Not Exists")]
        [Trait("Category", "Services")]
        public void Remove_WhenUserNotExists_ThrowsNewDomainException()
        {
            // Arrange
            var userId = new Randomizer().Int(0, 1000);

            _userRepositoryMock.Setup(x => x.Remove(It.IsAny<int>()))
                .Verifiable();

            // Act
            Func<Task> act = async () =>
            {
                await _sut.Remove(userId);
            };

            // Act
            act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("O Usuário não existe");
        }

        #endregion

        #region Get

        [Fact(DisplayName = "Get By Id")]
        [Trait("Category", "Services")]
        public async Task GetById_WhenUserExists_ReturnsUserDTO()
        {
            // Arrange
            var userId = new Randomizer().Int(0, 1000);
            var userFound = UserFixture.CreateValidUser();

            _userRepositoryMock.Setup(x => x.Get(userId))
                .ReturnsAsync(() => userFound);

            // Act
            var result = await _sut.Get(userId);

            // Assert
            result.Should()
                .BeEquivalentTo(_mapper.Map<UserDTO>(userFound));
        }

        [Fact(DisplayName = "Get By Id When User Not Exists")]
        [Trait("Category", "Services")]
        public async Task GetById_WhenUserNotExists_ReturnsNull()
        {
            // Arrange
            var userId = new Randomizer().Int(0, 1000);

            _userRepositoryMock.Setup(x => x.Get(userId))
                .ReturnsAsync(() => null);

            // Act
            var exeception = await Record.ExceptionAsync(() => _sut.Get(userId));

            // Assert

            Assert.True(exeception.Message == "O Usuário não existe");
        }

        [Fact(DisplayName = "Get By Email")]
        [Trait("Category", "Services")]
        public async Task GetByEmail_WhenUserExists_ReturnsUserDTO()
        {
            // Arrange
            var userEmail = new Internet().Email();
            var userFound = UserFixture.CreateValidUser();

            _userRepositoryMock.Setup(x => x.GetByEmail(userEmail))
                .ReturnsAsync(() => userFound);

            // Act
            var result = await _sut.GetByEmail(userEmail);

            // Assert
            result.Should()
                .BeEquivalentTo(_mapper.Map<UserDTO>(userFound));
        }

        [Fact(DisplayName = "Get By Email When User Not Exists")]
        [Trait("Category", "Services")]
        public async Task GetByEmail_WhenUserNotExists_ReturnsNull()
        {
            // Arrange
            var userEmail = new Internet().Email();

            _userRepositoryMock.Setup(x => x.GetByEmail(userEmail))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetByEmail(userEmail);

            // Assert
            result.Should()
                .Be(null);
        }

        [Fact(DisplayName = "Get All Users")]
        [Trait("Category", "Services")]
        public async Task GetAllUsers_WhenUsersExists_ReturnsAListOfUserDTO()
        {
            // Arrange
            var usersFound = UserFixture.CreateListValidUser();

            _userRepositoryMock.Setup(x => x.Get())
                .ReturnsAsync(() => usersFound);

            // Act
            var result = await _sut.Get();

            // Assert
            result.Should()
                .BeEquivalentTo(_mapper.Map<List<UserDTO>>(usersFound));
        }

        [Fact(DisplayName = "Get All Users When None User Found")]
        [Trait("Category", "Services")]
        public async Task GetAllUsers_WhenNoneUserFound_ReturnsEmptyList()
        {
            // Arrange

            _userRepositoryMock.Setup(x => x.Get())
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.Get();

            // Assert
            result.Should()
                .BeEmpty();
        }

        #endregion

        #region Search

        [Fact(DisplayName = "Search By Name")]
        [Trait("Category", "Services")]
        public async Task SearchByName_WhenAnyUserFound_ReturnsAListOfUserDTO()
        {
            // Arrange
            var nameToSearch = new Name().FirstName();
            var usersFound = UserFixture.CreateListValidUser();

            _userRepositoryMock.Setup(x => x.SearchByName(nameToSearch))
                .ReturnsAsync(() => usersFound);

            // Act
            var result = await _sut.SearchByName(nameToSearch);

            // Assert
            result.Should()
                .BeEquivalentTo(_mapper.Map<List<UserDTO>>(usersFound));
        }

        [Fact(DisplayName = "Search By Name When None User Found")]
        [Trait("Category", "Services")]
        public async Task SearchByName_WhenNoneUserFound_ReturnsEmptyList()
        {
            // Arrange
            var nameToSearch = new Name().FirstName();

            _userRepositoryMock.Setup(x => x.SearchByName(nameToSearch))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.SearchByName(nameToSearch);

            // Assert
            result.Should()
                .BeEmpty();
        }

        [Fact(DisplayName = "Search By Email")]
        [Trait("Category", "Services")]
        public async Task SearchByEmail_WhenAnyUserFound_ReturnsAListOfUserDTO()
        {
            // Arrange
            var emailSoSearch = new Internet().Email();
            var usersFound = UserFixture.CreateListValidUser();

            _userRepositoryMock.Setup(x => x.SearchByEmail(emailSoSearch))
                .ReturnsAsync(() => usersFound);

            // Act
            var result = await _sut.SearchByEmail(emailSoSearch);

            // Assert
            result.Should()
                .BeEquivalentTo(_mapper.Map<List<UserDTO>>(usersFound));
        }

        [Fact(DisplayName = "Search By Email When None User Found")]
        [Trait("Category", "Services")]
        public async Task SearchByEmail_WhenNoneUserFound_ReturnsEmptyList()
        {
            // Arrange
            var emailSoSearch = new Internet().Email();

            _userRepositoryMock.Setup(x => x.SearchByEmail(emailSoSearch))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.SearchByEmail(emailSoSearch);

            // Assert
            result.Should()
                .BeEmpty();
        }

        #endregion

    }
}
