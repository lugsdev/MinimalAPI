using MinimalAPI.Interfaces;

namespace MinimalAPI.Services
{
	public class SayHello : ISayHello
	{
		public string BoasVindas() 
		{
			return "Boas Vindas!";
		}
	}
}
