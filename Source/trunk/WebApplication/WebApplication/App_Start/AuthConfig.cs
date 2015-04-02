using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using Sioux.TaskManagement.DBContext;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;

namespace Sioux.TaskManagement
{
	public static class AuthConfig
	{
		public static void RegisterAuth()
		{
			// To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
			// you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

			//OAuthWebSecurity.RegisterMicrosoftClient(
			//    clientId: "",
			//    clientSecret: "");

			//OAuthWebSecurity.RegisterTwitterClient(
			//    consumerKey: "",
			//    consumerSecret: "");

			OAuthWebSecurity.RegisterFacebookClient(
				appId: "434121943408391",
				appSecret: "03b399e0bbeb1d8b7e4af7bbf690f3e7");

		//	OAuthWebSecurity.RegisterGoogleClient();

			OAuthWebSecurity.RegisterClient(new GoogleCustomClient(), "Google", null);

			InitializeMembership initMembership = new InitializeMembership();
			initMembership.Init();
		}
	}

	public class GoogleCustomClient : OpenIdClient
	{
		#region Constructors and Destructors

		public GoogleCustomClient()
			: base("google", WellKnownProviders.Google) { }

		#endregion

		#region Methods

		/// <summary>
		/// Gets the extra data obtained from the response message when authentication is successful.
		/// </summary>
		/// <param name="response">
		/// The response message. 
		/// </param>
		/// <returns>A dictionary of profile data; or null if no data is available.</returns>
		protected override Dictionary<string, string> GetExtraData(IAuthenticationResponse response)
		{
			FetchResponse fetchResponse = response.GetExtension<FetchResponse>();
			if (fetchResponse != null)
			{
				var extraData = new Dictionary<string, string>();
				extraData.Add("email", fetchResponse.GetAttributeValue(WellKnownAttributes.Contact.Email));
				extraData.Add("country", fetchResponse.GetAttributeValue(WellKnownAttributes.Contact.HomeAddress.Country));
				extraData.Add("firstName", fetchResponse.GetAttributeValue(WellKnownAttributes.Name.First));
				extraData.Add("lastName", fetchResponse.GetAttributeValue(WellKnownAttributes.Name.Last));
				extraData.Add("picture", fetchResponse.GetAttributeValue(WellKnownAttributes.Contact.Phone.Mobile));
				return extraData;
			}

			return null;
		}

		/// <summary
		/// Called just before the authentication request is sent to service provider.
		/// </summary>
		/// <param name="request">
		/// The request. 
		/// </param>
		protected override void OnBeforeSendingAuthenticationRequest(IAuthenticationRequest request)
		{
			// Attribute Exchange extensions
			var fetchRequest = new FetchRequest();
			fetchRequest.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
			fetchRequest.Attributes.AddRequired(WellKnownAttributes.Contact.HomeAddress.Country);
			fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.First);
			fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.Last);
			fetchRequest.Attributes.AddRequired(WellKnownAttributes.Contact.Phone.Mobile);
			request.AddExtension(fetchRequest);
		}

		#endregion
	}
}
