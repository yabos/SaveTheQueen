using UnityEngine;

namespace Aniz.Resource.Unit
{
	public class ScriptableResource : IResource
	{
		public override eResourceType Type
		{
			get { return eResourceType.Scriptable; }
		}
		public ScriptableResource(Object obj, bool isAssetBundle) : base(obj, isAssetBundle)
		{

		}

		protected override bool InitUpdate()
		{
			return true;
		}

		public override void UnLoad(bool unloadAllLoadedObjects)
		{

		}

	}
}