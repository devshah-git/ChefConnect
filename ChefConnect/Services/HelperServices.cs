using System;
using System.Net;
using MailKit.Net.Smtp;
using System.Text.RegularExpressions;
using ChefConnect.Data;
using MailKit.Security;
using MimeKit;

namespace ChefConnect.Services
{
	public class HelperServices
	{
        public bool IsPhoneNumberValid(string phone)
		{
			string pattern = "^([2-9]{1}[0-9]{2})(([2-9]{1})(1[0,2-9]{1}|[0,2-9]{1}[0-9]{1}))([0-9]{4})$";
            Regex regex = new Regex(pattern);

            if (regex.IsMatch(phone))
            {
                return true;
            }
            else
            {
                return false;
            }

		}

        //Create the helper method to make the call to chatbot api
      



		public bool IsPostalCodeValid(string postalcode)
		{
			string pattern = "^[A-Za-z]\\d[A-Za-z][ -]?\\d[A-Za-z]\\d$";

            Regex regex = new Regex(pattern);

            if (regex.IsMatch(postalcode))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		public bool IsValidAge(DateTime? dateOfBirth)
		{
			DateTime today = DateTime.Now;
			DateTime addEighteenYears = Convert.ToDateTime(dateOfBirth).AddYears(18);

            if (today >= addEighteenYears)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public async void SendEmailAsync(string email, string mailSubject, string mailMessage)
		{
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Team ChefConnect", "teamchefconnect@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = mailSubject;
            message.Body = new TextPart("plain") { Text = mailMessage };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate("teamchefconnect@gmail.com", "gbkv fqra kkga jpet");
                client.Send(message);
                client.Disconnect(true);
            }

        }
	}
}

