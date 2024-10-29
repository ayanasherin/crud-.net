using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Project1.Models;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;



namespace Project1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public LoginController(IConfiguration configuration , ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        [HttpPost]
        [Route("login")]
        public string login(Login login)
        {
            MySqlConnection conn = new MySqlConnection(_configuration.GetConnectionString("DbConnection").ToString());
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM user WHERE Email = '" + login.Email + "' AND Password =  '" + login.Password + "'", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return "Login Success";
            }
            else
            {
                return "Invalid Email or Password";
            }

        }

        [HttpPut]
        //[Route("reset-password")]
        public string ResetPassword(ResetPassword resetPassword)
        {
            MySqlConnection conn = new MySqlConnection(_configuration.GetConnectionString("DbConnection").ToString());


            MySqlDataAdapter adapter = new MySqlDataAdapter(
                 "SELECT * FROM user WHERE Email = '" + resetPassword.Email + "' AND Password = '" + resetPassword.OldPassword + "'", conn);


            DataTable dt = new DataTable();
            adapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {

                conn.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "UPDATE user SET Password = '" + resetPassword.NewPassword + "' WHERE Email = '" + resetPassword.Email + "'", conn);


                int i = cmd.ExecuteNonQuery();
                conn.Close();

                if (i > 0)
                {
                    return "Password reset successfully";
                }
                else
                {
                    return "Error updating password";
                }
            }
            else
            {
                return "Invalid email or old password";
            }
        }
        [HttpPost]
       [Route("forgotpassword")]
        public IActionResult ForgotPassword([FromBody] ForgetPassword password)
        {
            if (string.IsNullOrEmpty(password.Email))
            {
                return BadRequest("The email field is required.");
            }

            MySqlConnection conn = new MySqlConnection(_configuration.GetConnectionString("DbConnection").ToString());
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM user WHERE Email = '" + password.Email + "'", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                string token = GenerateToken();

                try
                {
                    conn.Open();
                    string updateQuery = "UPDATE user SET Token = '"+token+"' WHERE Email = '"+password.Email+"'";
                    MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                   
                    cmd.ExecuteNonQuery();
                    conn.Close();
                   string resetLink = Url.Action("ResetPassword", "Password", new { token }, Request.Scheme);


                    SendResetPasswordEmail(password.Email, resetLink);

                    return Ok($"Mail Sent");
                }
                catch (Exception e)
                {
                    return StatusCode(500, e.Message);
                }
            }
            else
            {
                return NotFound("Email not found");
            }
        }

        private void SendResetPasswordEmail(string Email, string resetLink)
        {
            try
            {
                
                var mail = new MailMessage();
                var smtpServer = new SmtpClient("smtp.gmail.com");

                
                mail.From = new MailAddress("ayanasherin2001@gmail.com");  
                mail.To.Add(Email); 
                mail.Subject = "Reset your password";
                mail.Body = $"The link is : {resetLink}";

                
                smtpServer.Port = 587;
                smtpServer.Credentials = new NetworkCredential("ayanasherin2001@gmail.com", "gyhw svrn updm hfaw");  
                smtpServer.EnableSsl = true;

               
                smtpServer.Send(mail);
                Console.WriteLine("Reset password email sent successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }
        private string GenerateToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }





        [HttpPost]
        [Route("resetwithtoken")]
        public IActionResult ChangePassword(ResetWithToken reset)
        {

            if (reset.NewPassword != reset.ConfirmNewPassword)
            {
                return BadRequest("Password and ConfirmPassword do not match.");
            }

            MySqlConnection conn = new MySqlConnection(_configuration.GetConnectionString("DbConnection").ToString());
            {
                try
                {
                    conn.Open();

                    string selectQuery = "SELECT * FROM User WHERE Token ='"+reset.Token+"'";
                    MySqlCommand Cmmd = new MySqlCommand(selectQuery, conn);
                   

                    MySqlDataAdapter adapter = new MySqlDataAdapter(Cmmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {

                        string NewPassword = reset.NewPassword;

                        string updateQuery = "UPDATE user SET Password = '"+reset.NewPassword+ "', Token = NULL WHERE Token = '"+reset.Token+"'";
                        MySqlCommand Cmd = new MySqlCommand(updateQuery, conn);
                        
                        Cmd.ExecuteNonQuery();

                        return Ok("Password  changed.");
                    }
                    else
                    {
                        return BadRequest("Invalid token.");
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Error: " + ex.Message);
                }
            }
        }


       
    }
}
