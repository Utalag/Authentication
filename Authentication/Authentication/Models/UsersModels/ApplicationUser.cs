using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace Authentication.Models.UsersModels
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;           //Jon
        public string LastName { get; set; } = string.Empty;            //Doe
        public string FullName => $"{FirstName} {LastName}";            //Jon Doe
        public string ProfilePicture { get; set; } = string.Empty;      
        public string NickName { get; set; } = string.Empty;            //Deadpoll
        public string Title { get; set; } = string.Empty;               //Mr.


    }

}
