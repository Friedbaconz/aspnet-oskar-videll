

namespace Application.Users.Inputs;

public record SignInInput
(
    string Email,
    string Password,
    bool RememberMe
);
