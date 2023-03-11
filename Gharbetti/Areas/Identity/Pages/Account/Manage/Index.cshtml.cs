// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Gharbetti.Data;
using Gharbetti.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Gharbetti.Areas.Identity.Pages.Account.RegisterModel;

namespace Gharbetti.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _db;


        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;

        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            public string FirstName { get; set; }

            public string? MiddleName { get; set; }
            [Required]
            public string LastName { get; set; }
            [Required]
            public string MobileNumber { get; set; }


            [Required]
            [DataType(DataType.Date)]
            public DateTime Dob { get; set; }

            public string? Identification { get; set; }

            public string? PhotoId { get; set; }

            public byte Status { get; set; }

            [Required]
            public string AddressLine1 { get; set; }

            public string? AddressLine2 { get; set; }

            public string? AddressLine3 { get; set; }

            [Required]
            public string City { get; set; }

            [Required]
            public string PostalCode { get; set; }

            public string? County { get; set; }

            [Required]
            public string Country { get; set; }


            [BindProperty]
            [Required(ErrorMessage = "Please select a file.")]
            [DataType(DataType.Upload)]
            public IFormFile IdentificationFile { get; set; }


            [BindProperty]
            [Required(ErrorMessage = "Please select a file.")]
            [DataType(DataType.Upload)]
            [MaxFileSize(5 * 1024 * 1024)]
            [AllowedExtensions(new string[] { ".jpg", ".png" })]
            public IFormFile PhotoFile { get; set; }

            [ValidateNever]
            public IEnumerable<SelectListItem> CountryList { get; set; }


            [ValidateNever]
            public IEnumerable<SelectListItem> RoomList { get; set; }

            public int RoomId { get; set; }
            public string StayLength { get; set; }

            public bool IsUploadPhoto { get; set; }
            public bool IsUploadDocument { get; set; }

        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var userData = await _db.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == user.Id);

            Username = userName;

            var roomList = await (from r in _db.Rooms
                                  join hr in _db.HouseRooms on r.Id equals hr.RoomId
                                  join h in _db.Houses on hr.HouseId equals h.Id
                                  select new HouseRoomViewModel
                                  {
                                      Id = r.Id,
                                      HouseId = h.Id,
                                      RoomId = r.Id,
                                      HouseName = h.Name,
                                      RoomName = r.RoomNo
                                  }).ToListAsync();


          
            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                MobileNumber = userData.MobileNumber,
                RoomId = userData.RoomId,
                StayLength = userData.StayLength,
                AddressLine1 = userData.AddressLine1,
                AddressLine2 = userData.AddressLine2,
                AddressLine3 = userData.AddressLine3,
                City = userData.City,
                Country = userData.Country,
                County = userData.County,
                PostalCode = userData.PostalCode,
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                MiddleName = userData.MiddleName,
                Dob = userData.Dob,
                IsUploadDocument = false,
                IsUploadPhoto = false,
                Identification = userData.Identification,
                PhotoId = userData.PhotoId,
                CountryList = GetCountryList().Select(x => new SelectListItem
                {
                    Text = x,
                    Value = x
                }),
                RoomList = roomList.Select(x => new SelectListItem
                {
                    Text = x.HouseName + "-> " + x.RoomName,
                    Value = x.Id.ToString()
                })

            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public static List<string> GetCountryList()
        {
            List<string> cultureList = new List<string>();

            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo culture in cultures)
            {
                RegionInfo region = new RegionInfo(culture.LCID);

                if (!(cultureList.Contains(region.EnglishName)))
                {
                    cultureList.Add(region.EnglishName);
                }
            }
            return cultureList;
        }

        public class MaxFileSizeAttribute : ValidationAttribute
        {
            private readonly int _maxFileSize;
            public MaxFileSizeAttribute(int maxFileSize)
            {
                _maxFileSize = maxFileSize;
            }

            protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
            {
                var file = value as IFormFile;
                if (file != null)
                {
                    if (file.Length > _maxFileSize)
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }

                return ValidationResult.Success;
            }

            public string GetErrorMessage()
            {
                return $"Maximum allowed file size is {_maxFileSize} bytes.";
            }
        }

        public class AllowedExtensionsAttribute : ValidationAttribute
        {
            private readonly string[] _extensions;
            public AllowedExtensionsAttribute(string[] extensions)
            {
                _extensions = extensions;
            }

            protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
            {
                var file = value as IFormFile;
                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName);
                    if (!_extensions.Contains(extension.ToLower()))
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }

                return ValidationResult.Success;
            }

            public string GetErrorMessage()
            {
                return $"This photo extension is not allowed!";
            }
        }
    }

  
}
