using System;

namespace AuthServerDemo
{
    public class AccountOptions
    {
        public static bool AllowLocalLogin = true;

        public static bool AllowRememberLogin = true;

        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

        public static bool ShowLogoutPrompt = true;

        public static bool AutomaticRedirectAfterSignOut = false;

        public static bool WindowsAuthenticationEnabled = true;

        public static readonly string[] WindowsAuthenticationSchemes = new string[] { "Negotiate", "NTLM" };

        public static readonly string WindowsAuthenticationProviderName = "Windows";

        public static readonly string WindowsAuthenticationDisplayName = "Windows";

        public static string InvalidCredentialsErrorMessage = "Invalid username or password";
    }
}
