namespace Authentication.Client
{
    // Add properties to this class and update the server and client AuthenticationStateProviders
    // to expose more information about the authenticated user to the client.

    //Cz: Reprezentuje u�ivatelel na klientsk� stran�

    //!! public required = znamen �e hodnota mus� b�t na�tena p�i inicializaci objektu jinak dojde k chyb� !!


    public class UserInfo
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }

    }
}
