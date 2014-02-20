using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Security;
using Newtonsoft.Json;
using System.Text;

namespace ExternalProviderAuthentication.iOS
{
	public class AuthenticationServices
	{
		private readonly string _baseUri;

		public AuthenticationServices (string baseUri)
		{
			_baseUri = baseUri;
		}

		public string BaseUri { get { return _baseUri; } }

		public string AccessToken { get; set; }

		public async Task<IEnumerable<ExternalLoginViewModel>> GetExternalLoginProviders()
		{
			string uri = String.Format("{0}/api/Account/ExternalLogins?returnUrl=%2F&generateState=true", _baseUri);
			HttpWebRequest request = new HttpWebRequest(new Uri(uri));
			request.Method = "GET";
			try
			{
				WebResponse response = await request.GetResponseAsync();
				HttpWebResponse httpResponse = (HttpWebResponse) response;
				string result;

				using (Stream responseStream = httpResponse.GetResponseStream())
				{
					result = new StreamReader(responseStream).ReadToEnd();
				}

				List<ExternalLoginViewModel> models = JsonConvert.DeserializeObject<List<ExternalLoginViewModel>>(result);
				return models;
			}
			catch (SecurityException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Unable to get login providers", ex);
			}
		}

		public async Task RegisterExternal(
			string username)
		{
			string uri = String.Format("{0}/api/Account/RegisterExternal", BaseUri);

			RegisterExternalBindingModel model = new RegisterExternalBindingModel
			{
				UserName = username
			};
			HttpWebRequest request = new HttpWebRequest(new Uri(uri));

			request.ContentType = "application/json";
			request.Accept = "application/json";
			request.Headers.Add("Authorization", String.Format("Bearer {0}", AccessToken));
			request.Method = "POST";

			string postJson = JsonConvert.SerializeObject(model);
			byte[] bytes = Encoding.UTF8.GetBytes(postJson);
			using (Stream requestStream = await request.GetRequestStreamAsync())
			{
				requestStream.Write(bytes, 0, bytes.Length);
			}

			try
			{
				WebResponse response = await request.GetResponseAsync();
				HttpWebResponse httpResponse = (HttpWebResponse)response;
				string result;

				using (Stream responseStream = httpResponse.GetResponseStream())
				{
					result = new StreamReader(responseStream).ReadToEnd();
					Console.WriteLine(result);
				}
			}
			catch (SecurityException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Unable to register user", ex);
			}
		}
	}
}

