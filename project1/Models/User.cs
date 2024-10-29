using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project1.Models;

public partial class User
{


    public int Userid { get; set; }

    public string Prefix { get; set; } = null!;

    [Required(ErrorMessage = "First name is required")]
    public string Firstname { get; set; } = null!;



    [Required(ErrorMessage = "Last name is required")]
    public string Lastname { get; set; } = null!;


    [Required(ErrorMessage = "Address line 1 is required")]
    public string Addressline1 { get; set; } = null!;


    [Required(ErrorMessage = "Address line 2 is required")]
    public string Addressline2 { get; set; } = null!;

    [Required(ErrorMessage = "City is required")]
    public string City { get; set; } = null!;

    [Required(ErrorMessage = "State is required")]
    public string State { get; set; } = null!;

   

    [Required(ErrorMessage = "Zip Code is required")]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid Zip Code format.")]
    public string Zipcode { get; set; } = null!;

    [Required(ErrorMessage = "Country code is required")]
    [RegularExpression(@"^\+\d{1,3}$", ErrorMessage = "Invalid country code format")]
    public string Countrycode { get; set; } = null!;


    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
    public string Phone { get; set; } = null!;

    [Required(ErrorMessage = "Email is required")]
    [RegularExpression(@"^[^\s]+@([^\s]+\.)+[^\s]{2,4}$", ErrorMessage = "Invalid email format.")]

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;



    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,16}$",
        ErrorMessage = "Password must be 8 characters long, contain at least one lowercase letter, one uppercase letter, one number, and one special character.")]
    public string Password { get; set; } = null!;

    public string? BachelorDegree { get; set; }

    public decimal? BachelorGpa { get; set; }

    public string? Md { get; set; }

    public decimal? MdGpa { get; set; }

    public sbyte? LookingForInternship { get; set; }

    public string? LearnedAboutUs { get; set; }

    public IFormFile Resume { get; set; }

    public string? Token { get;  set; }
}
