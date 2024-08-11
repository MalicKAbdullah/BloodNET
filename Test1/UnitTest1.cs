//using BloodNET_Web.Controllers;
//using BloodNET_Web.Models;
//using BloodNET_Web.Models.Interfaces;
//using Castle.Core.Resource;
//using Moq;

//namespace Test1
//{
//    public class Tests
//    {
//        private Mock<IBloodDonors> _mockBloodDonorsRepository;


//        [SetUp]
//        public void Setup()
//        {
//            _mockBloodDonorsRepository = new Mock<IBloodDonors>();
//        }

//        [Test]
//        public void GetCustomerById_ReturnsCustomer()
//        {
//            // Arrange
//            var bloodDonor = new BloodDonors
//            {
//                DonorId = "123",
//                Name = "John Doe",
//                DOB = new DateTime(1985, 6, 15),
//                Gender = "Male",
//                Weight = 70.0f,
//                WeightUnit = "kg",
//                BloodGroup = "O+",
//                Email = "john.doe@example.com",
//                Address = "123 Main St",
//                City = "Somewhere",
//                Country = "Country",
//                MedicalHistory = "No known allergies",
//                DonorStatus = 1,
//                ImgUrl = "http://example.com/image.jpg",
//                PhoneNumber = "+1234567890"
//            };
//            _mockBloodDonorsRepository.Setup(repo => repo.GetDonorById("123")).Returns(bloodDonor);

//            // Act
//            var result = _mockBloodDonorsRepository.GetDonorById("123");

//            // Assert
//            Assert.AreEqual(customer, result);
//        }

//        [Test]
//        public void SaveCustomer_WithValidCustomer_CallsSaveCustomerOnRepository()
//        {
//            // Arrange
//            var customer = new Customer { Id = 1, Name = "John Doe", Email = "john.doe@example.com" };

//            // Act
//            _customerService.SaveCustomer(customer);

//            // Assert
//            _mockCustomerRepository.Verify(repo => repo.SaveCustomer(customer), Times.Once);
//        }

//        [Test]
//        public void SaveCustomer_WithEmptyName_ThrowsArgumentException()
//        {
//            // Arrange
//            var customer = new Customer { Id = 1, Name = "", Email = "john.doe@example.com" };

//            // Act & Assert
//            var ex = Assert.Throws<ArgumentException>(() => _customerService.SaveCustomer(customer));
//            Assert.AreEqual("Customer name cannot be empty", ex.Message);
//        }

//        [Test]
//        public void SaveCustomer_WithEmptyEmail_ThrowsArgumentException()
//        {
//            // Arrange
//            var customer = new Customer { Id = 1, Name = "John Doe", Email = "" };

//            // Act & Assert
//            var ex = Assert.Throws<ArgumentException>(() => _customerService.SaveCustomer(customer));
//            Assert.AreEqual("Customer email cannot be empty", ex.Message);
//        }
//    }
//}