using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace uTools {
	[AddComponentMenu("uTools/Tween/Tween Alpha(uTools)")]
	public class uTweenAlpha : uTweenValue {

		public bool includeChilds = false;

		private Text mText;
		private Light mLight;
		private Material mMat;
		private Image mImage;
		private SpriteRenderer mSpriteRender;

		private Text [] mTextList;
		private Light [] mLightList;
		private List<Material> mMatList = new List<Material>();
		private Image [] mImageList;
		private SpriteRenderer [] mSpriteRenderList;

		float mAlpha = 0f;

		public float alpha {
			get {
				return mAlpha;
			}
			set {
				SetAlpha(transform, value);
				mAlpha = value;
			}
		}

        void Awake() {
            var _transform = transform;

			if (includeChilds)
			{
				mTextList = _transform.GetComponentsInChildren<Text>();
				mLightList = _transform.GetComponentsInChildren<Light>();
				mImageList = _transform.GetComponentsInChildren<Image>();
				mSpriteRenderList = _transform.GetComponentsInChildren<SpriteRenderer>();
				for (int i = 0; i < _transform.childCount; ++i)
				{
					Transform child = _transform.GetChild(i);
					if (child.GetComponent<Renderer>() != null)
						mMatList.Add( child.GetComponent<Renderer>().material );
				}
			}
			else
			{
				mText = _transform.GetComponent<Text>();
				mLight = _transform.GetComponent<Light>();
				mImage = _transform.GetComponent<Image>();
				mSpriteRender = _transform.GetComponent<SpriteRenderer>();
				if (_transform.GetComponent<Renderer>() != null)
					mMat = _transform.GetComponent<Renderer>().material;
			}

		}

        protected override void ValueUpdate (float value, bool isFinished)
		{
			alpha = value;
		}

		public void BegineToReset()
		{
			factor = 0f;
			alpha = from;
		}

		void SetAlpha(Transform _transform, float _alpha) {
			Color c = Color.white;

			if (includeChilds)
			{
				for (int i = 0; i < mTextList.Length; ++i)
				{
					c = mTextList[i].color;
					c.a = _alpha;
					mTextList[i].color = c;
				}

				for (int i = 0; i < mLightList.Length; ++i)
				{
					c = mLightList[i].color;
					c.a = _alpha;
					mLightList[i].color = c;
				}

				for (int i = 0; i < mImageList.Length; ++i)
				{
					c = mImageList[i].color;
					c.a = _alpha;
					mImageList[i].color = c;
				}

				for (int i = 0; i < mSpriteRenderList.Length; ++i)
				{
					c = mSpriteRenderList[i].color;
					c.a = _alpha;
					mSpriteRenderList[i].color = c;
				}

				for (int i = 0; i < mMatList.Count; ++i)
				{
					c = mMatList[i].color;
					c.a = _alpha;
					mMatList[i].color = c;
				}
			}
			else
			{
				if (mText != null)
				{
					c = mText.color;
					c.a = _alpha;
					mText.color = c;
				}
				if (mLight != null)
				{
					c = mLight.color;
					c.a = _alpha;
					mLight.color = c;
				}
				if (mImage != null)
				{
					c = mImage.color;
					c.a = _alpha;
					mImage.color = c;
				}
				if (mSpriteRender != null)
				{
					c = mSpriteRender.color;
					c.a = _alpha;
					mSpriteRender.color = c;
				}
				if (mMat != null)
				{
					c = mMat.color;
					c.a = _alpha;
					mMat.color = c;
				}
			}

		}

	}

}
