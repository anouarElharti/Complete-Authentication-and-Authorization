using System.Security.Cryptography;

namespace AngularAuthApi.Helpers
{
    public static class EmailBody
    {
        public static string EmailStringBody(string email, string emailToken) {
            return $@"<html>
                    <head></head>
                    <body style=""margin: 0;padding: 0;font-family:Arial,sans - serif; "">
                        <div style=""width:400px;margin:0 auto;padding:30px;height:auto;"">
                            <div>
                                <h1>Reset Your Password</h1>
                                <hr/>
                                <p>
                                    You're receiving this e-mail because you requested a password reset for your account!
                                </p>
                                <p>
                                    Please click on the button below to choose a new password.
                                </p>
                                <a href=""http://localhost:4200/reset?email={email}&code={emailToken}"" target=""_blank"" style=""background:#0d6efc;color:white;border-radius=4px;display:block;margin:0;width:50%;text-align:center;text-decoration:none;"">Reset Password</a>
                                <p>
                                    Best Regards,
                                </p>
                            </div>
                        </div>
                    </body>
                    </html>
                ";
        }
    }
}
