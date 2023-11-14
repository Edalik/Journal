namespace Journal.Models.Abstractions;

interface ILoginModel
{
    string Ip { get; set; }
    string Login { get; set; }
    string Password { get; set; }
}
