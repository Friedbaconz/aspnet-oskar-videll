using Application.Abstractions.Identity;
using Application.Users.Abstractions;
using Application.Users.Inputs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Presentation.WebApp.Models.Auth;

namespace Presentation.WebApp.Controllers;

[Route("Auth")]
public class AuthController(IRegisterUserAccountService registerUserAccount, ISignInUserService signInUserService, IidentityService identityService) : Controller
{
    private const string RegisterEmailSessionKey = "RegisterEmail";

    [HttpGet("SignIn")]
    public IActionResult SignIn()
    {
        return View(new SignInForm());
    }

    [HttpPost("SignIn")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(SignInForm form, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return View(form);

        var input = new SignInInput(form.Email, form.Password, form.RememberMe);

        var result = await signInUserService.ExecuteAsync(input, ct);
        if (!result.Success)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred during sign-in.";
            return View(result);
        }

        return RedirectToAction("My", "User");
    }

    [HttpPost("SignOut")]
    [ValidateAntiForgeryToken]
    public new async Task<IActionResult> SignOut()
    {
        await identityService.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }


    [HttpGet("SignUp")]
    public IActionResult SignUp()
    {
        return View(new RegisterEmailForm());
    }

    public async Task<IActionResult> SignUp(RegisterEmailForm form, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return View(form);
        HttpContext.Session.SetString(RegisterEmailSessionKey, form.Email);
        return RedirectToAction(nameof(RegisterPassword));
    }

    [HttpGet("RegisterPassword")]
    public IActionResult RegisterPassword() 
    {
        var email = HttpContext.Session.GetString(RegisterEmailSessionKey);
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction(nameof(SignUp));
        }

        return View();
    }

    [HttpPost("RegisterPassword")]
    public async Task<IActionResult> RegisterPassword(RegisterPasswordForm form, CancellationToken ct = default)
    {
        var email = HttpContext.Session.GetString(RegisterEmailSessionKey);
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction(nameof(SignUp));
        }

        if (!ModelState.IsValid)
            return View(form);

        var input = new RegisterUserAccountInput(email, form.Password);

        var result = await registerUserAccount.ExecuteAsync(input, ct);
        if(!result.Success)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred during registration.";
            return View(form);
        }

        var signInInput = new SignInInput(email, form.Password, false);

        var signInResult = await signInUserService.ExecuteAsync(signInInput, ct);
        if (!signInResult.Success)
        {
            ViewData["ErrorMessage"] = signInResult.ErrorMessage ?? "An error occurred during sign-in after registration.";
            return View(form);
        }

        HttpContext.Session.Remove(RegisterEmailSessionKey);
        return RedirectToAction("My", "User");
    }

}
