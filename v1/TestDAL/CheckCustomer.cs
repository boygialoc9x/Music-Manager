using Xunit;
using DAL;
using Persistence;
public class CustomerTest
{
    CustomerDAL cDAL = new CustomerDAL();
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("user")]
    public void PassingCheckUserName(string user_name)
    {
        Assert.True(cDAL.CheckUserName(user_name));
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("", "123")]
    [InlineData("123", "")]
    [InlineData(null, null)]
    [InlineData("user", "")]
    [InlineData("user","testunit123")]
    public void PassingCheckPassword(string user_name, string password)
    {
        Assert.True(cDAL.CheckPassword(user_name, password));
    }

    [Theory]
    [InlineData("user")]
    [InlineData("")]
    [InlineData(null)]
    public void PassingGetCustomer(string user_name)
    {
        Customer customer = cDAL.GetCustomer(user_name);
        Assert.True(customer != null);
        Assert.Equal(user_name, customer.user_name);
    }

    [Theory]
    [InlineData("preuser")] //already premium
    [InlineData("admin")] //staff
    [InlineData(null)]
    [InlineData("")]
    [InlineData("falsfas")]// Doesn't exist
    public void PassingUpgradePremium_Staff(string user_name)
    {
        Assert.True(cDAL.UpgradePremium_Staff(user_name, true));
    }


    [Theory]
    [InlineData("thenewuser2", "password123", "firstName", "lastName", "thisisagmail@gmail.com", true, false, false, Persistence.Enum.Gender.Male)]
    [InlineData("thenewuser", "test123", "", "", "thisisagmail@gmail.com", true, false, false, Persistence.Enum.Gender.Male)]

    public void PassingRegisterAccount(string user_name, string password, string firstName, string lastName, string gmail, bool status, bool staff, bool premium, Persistence.Enum.Gender gender)
    {
        Customer customer = new Customer();
        customer.user_name = user_name;
        customer.firstName = firstName;
        customer.lastName = lastName;
        customer.password = password;
        customer.gmail = gmail;
        customer.premium = premium;
        customer.staff = staff;
        customer.accountStatus = status;
        customer.gender = gender;
        Assert.True(cDAL.RegisterAccount(customer));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("test@gmail.com")]
    public void PassingCheckMail(string gmail)
    {
        Assert.True(cDAL.CheckGmail(gmail));
    }

    [Fact]
    public void PassingGetMaxId()
    {
        Assert.True(cDAL.GetMaxIdInCustomer() != 0);
    }
}