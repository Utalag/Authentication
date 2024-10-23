namespace Authentication.Client
{
    // Add properties to this class and update the server and client AuthenticationStateProviders
    // to expose more information about the authenticated user to the client.

    //Cz: Reprezentuje uživatelel na klientské stranì

    //!! public required = znamen že hodnota musí bát naètena pøi inicializaci objektu jinak dojde k chybì !!


    public class UserInfo
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }

    }
}
