using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Project1.Models;

namespace Project1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public RegistrationController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        [Route("registration")]
        public  async Task<string> registration(User user, IFormFile resume)
        {
            if (resume == null || resume.Length == 0)
            {
                return "Resume file is required.";
            }

            byte[] resumeData;
            using (var memoryStream = new MemoryStream())
            {
                await resume.CopyToAsync(memoryStream);
                resumeData = memoryStream.ToArray();
            }

            MySqlConnection conn = new MySqlConnection(_configuration.GetConnectionString("DbConnection").ToString());
            MySqlCommand cmd = new MySqlCommand("INSERT INTO User(Prefix,Firstname,Lastname,AddressLine1,AddressLine2,City,State,Zipcode,CountryCode,Phone,Email,Role,Password,BachelorDegree,BachelorGpa,Md,MdGpa,LookingForInternship,LearnedAboutUs,Resume)VALUES('" + user.Prefix+ "','"+user.Firstname+ "','"+user.Lastname+"','"+user.Addressline1+ "','"+user.Addressline2+"','"+user.City+"','"+user.State+"','"+user.Zipcode+"','"+user.Countrycode+"','"+user.Phone+"','"+user.Email+ "','"+user.Role+ "','"+user.Password+ "','"+user.BachelorDegree+"','"+user.BachelorGpa+"','"+user.Md+"','"+user.MdGpa+"','"+user.LookingForInternship+"','"+user.LearnedAboutUs+ "','"+resumeData+"')", conn);
            await conn.OpenAsync();
            int i = cmd.ExecuteNonQuery();
            conn.Close();
            if (i > 0) 
            {
                return "Data Inserted Successfuly";

            }
            else
            {
                return "Error";
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetRegistration()

        {
            return await _context.Users.ToListAsync();
        }
    }
}
