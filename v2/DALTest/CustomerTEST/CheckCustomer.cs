using Xunit;
using DAL;
using Persistence;
public class CustomerTest
{
    CustomerDAL cDAL = new CustomerDAL();
    // [Theory]
    // [InlineData("")]
    // [InlineData(null)]
    // [InlineData("user")]
    // public void PassingCheckUserName(string user_name)
    // {
    //     Assert.True(cDAL.CheckUserName(user_name));
    // }

    // [Theory]
    // [InlineData("", "")]
    // [InlineData("", "123")]
    // [InlineData("123", "")]
    // [InlineData(null, null)]
    // [InlineData("user", "")]
    // [InlineData("user","testunit123")]
    // public void PassingCheckPassword(string user_name, string password)
    // {
    //     Assert.True(cDAL.CheckPassword(user_name, password));
    // }

    // [Theory]
    // [InlineData("", "staff")]
    // [InlineData(null, "staff")]
    // [InlineData("user", "staff")]
    // [InlineData("admin", "staff")]
    // [InlineData("", "premium")]
    // [InlineData(null, "premium")]
    // [InlineData("user", "premium")]
    // [InlineData("preuser", "premium")]
    // public void PassingCheckPermission(string user_name, string perm)
    // {
    //     Assert.True(cDAL.CheckPermission(user_name, perm));
    // }

    // [Theory]
    // [InlineData("user")]
    // [InlineData("")]
    // [InlineData(null)]
    // public void PassingGetCustomer(string user_name)
    // {
    //     Customer customer = cDAL.GetCustomer(user_name);
    //     Assert.True(customer != null);
    //     Assert.Equal(user_name, customer.user_name);
    // }

    // [Theory]
    // [InlineData("preuser")] //already premium
    // [InlineData("admin")] //staff
    // [InlineData(null)]
    // [InlineData("")]
    // [InlineData("falsfas")]// Doesn't exist
    // public void PassingUpgradePremium_Staff(string user_name)
    // {
    //     Assert.True(cDAL.UpgradePremium_Staff(user_name));
    // }

    // [Theory]
    // [InlineData("imthetest", "firstName", "lastName", "")]
    // [InlineData("imthetest", "firstName", "", "")]
    // [InlineData("imthetest", "", "", "")]
    // [InlineData("imthetest", null, "lastName", "newgmail@gmail.com")]
    // [InlineData("imthetest", null, null, "newgmail@gmail.com")]
    // [InlineData("imthetest", null, null, null)]
    // [InlineData("imthetest", "firstName", "lastName", "newgmail@gmail.com")]
    // public void PassingUpdateCustomerInformation(string user_name, string firstName, string lastName, string gmail)
    // {
    //     Customer customer = cDAL.GetCustomer(user_name);
    //     customer.firstName = firstName;
    //     customer.lastName = lastName;
    //     customer.gmail = gmail;
    //     Assert.True(cDAL.UpdateCustomerInformation(customer));
    // }

    // [Theory]
    // [InlineData("thenewuser2", "password123", "firstName", "lastName", "thisisagmail@gmail.com", true, false, false)]
    // //[InlineData("thenewuser", "test123", "", "", "thisisagmail@gmail.com", true, false, false)]

    // public void PassingRegisterAccount(string user_name, string password, string firstName, string lastName, string gmail, bool status, bool staff, bool premium)
    // {
    //     Customer customer = new Customer();
    //     customer.user_name = user_name;
    //     customer.firstName = firstName;
    //     customer.lastName = lastName;
    //     customer.password = password;
    //     customer.gmail = gmail;
    //     customer.premium = premium;
    //     customer.staff = staff;
    //     customer.accountStatus = status;
    //     Assert.True(cDAL.RegisterAccount(customer));
    // }

    // [Theory]
    // [InlineData("")]
    // [InlineData(null)]
    // [InlineData("test@gmail.com")]
    // public void PassingCheckMail(string gmail)
    // {
    //     Assert.True(cDAL.CheckGmail(gmail));
    // }

    // [Theory]
    // [InlineData(0, null)] //non-exist id
    // [InlineData(9, "newpas123")]
    // public void PassingUpdateNewPassword(int userId, string newPass)
    // {
    //     Assert.True(cDAL.UpdateNewPassword(userId, newPass));
    // }

    // [Fact]
    // public void PassingGetMaxId()
    // {
    //     Assert.True(cDAL.GetMaxIdInCustomer() != 0);
    // }
}