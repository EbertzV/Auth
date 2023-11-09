using Auth.Entities;
using Auth.Services;

var authService = new AuthenticationService("https://authapp-test.azurewebsites.net/");
var user = new User("vitor", "caralhinhosvoadores");

var result = await authService.TryAuthenticate(user);
Console.WriteLine(result.StatusCode);
Console.WriteLine(result.Token);

var sessionService = new EvaluationService("https://authapp-test.azurewebsites.net/");
var sessResult = await sessionService.EvaluateSessionFromToken(result.Token);
Console.WriteLine(sessResult);