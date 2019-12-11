using CsvHelper.Configuration;
using r2bw.Data;

namespace r2bw.Services
{
    public class UserCsvMap : ClassMap<User>
    {
        public UserCsvMap()
        {
            Map(u => u.FirstName).Name("FirstName");
            Map(u => u.LastName).Name("LastName");
            Map(u => u.Email).Name("Email");
            Map(u => u.UserName).Name("UserName");
            Map(u => u.DateOfBirth).Name("DateOfBirth");
            Map(u => u.WaiverSignedOn).Name("WaiverSignedOn");
            Map(u => u.EmailConfirmedText).Name("EmailConfirmed");
            Map(u => u.Active).Name("Active");
            Map(u => u.Sex).Name("ApparelSex");
            Map(u => u.Size).Name("ApparelSize");
            Map(u => u.ShoeSize).Name("ShoeSize");
            Map(u => u.Street1).Name("Street1");
            Map(u => u.Street2).Name("Street2");
            Map(u => u.City).Name("City");
            Map(u => u.State).Name("State");
            Map(u => u.Zip).Name("Zip");
        }
    }
}