using System;
using Android.Content;

namespace Schedulee.Droid.Controls
{
	public interface IDataTemplate
	{
		Func<Context, object> LoadTemplate { get; set; }
	}
}